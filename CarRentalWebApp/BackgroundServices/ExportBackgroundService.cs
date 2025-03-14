using CarRentalWebApp.Models;
using CarRentalWebApp.Repository;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

public class ExportBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ExportBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ExportBookings();
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Run daily
        }
    }

    private async Task ExportBookings()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var bookingRepo = scope.ServiceProvider.GetRequiredService<IBookingRepo>();
            var branchRepository = scope.ServiceProvider.GetRequiredService<IBranchRepository>();
            var vehicleBranchRepository = scope.ServiceProvider.GetRequiredService<IVehicleBranchRepository>();
            var vehicleRepo = scope.ServiceProvider.GetRequiredService<IVehicleRepo>();

            var bookings = await bookingRepo.GetAllBookings();
            var branches = await branchRepository.GetBranches();
            var vehicleBranches = await vehicleBranchRepository.GetVehicleBranches();
            var vehicles = await vehicleRepo.GetVehicles();

            var exportData = from booking in bookings
                where booking.EndTime != null  
                // Join to get the pickup branch
                join pickupBranch in branches on booking.PickupBranchId equals pickupBranch.BranchId
                // Left join for the drop-off branch
                join dropoffBranch in branches on booking.DropoffBranchId equals dropoffBranch.BranchId into dropoffBranchGroup
                from dropoff in dropoffBranchGroup.DefaultIfEmpty()
                // Join the VehicleBranch to get rate daily based on the pickup branch and vehicle
                join vb in vehicleBranches on new { booking.VehicleId, BranchId = booking.PickupBranchId }
                    equals new { vb.VehicleId, vb.BranchId } into vbGroup
                from vehicleBranch in vbGroup.DefaultIfEmpty()  // left join in case no matching record
                // Finally join the Vehicles table to get make/model/year
                join vehicle in vehicles on booking.VehicleId equals vehicle.VehicleId
                select new
                {
                    pickUpCity = pickupBranch.City,
                    dropOffCity = dropoff != null ? dropoff.City : "",
                    vehicleMake = vehicle.Make,
                    vehicleModel = vehicle.Model,
                    vehicleYear = vehicle.Year,
                    pickup_date = booking.StartTime,
                    dropoff_date = booking.EndTime,
                    vehicle_id = booking.VehicleId,
                    // If VehicleBranch is missing, default the rate to 0 or null
                    rateDaily = vehicleBranch != null ? vehicleBranch.Rate : 0
                };


            // Create the CSV header as required.
            var csv = new StringBuilder();
            csv.AppendLine("pickUp.city,rate.daily,vehicle.make,vehicle.model,vehicle.year,dropOff.city,pickup_date,dropoff_date,vehicle_id");

            // Append each record as a CSV line.
            foreach (var record in exportData)
            {
                csv.AppendLine($"{record.pickUpCity},{record.rateDaily},{record.vehicleMake},{record.vehicleModel},{record.vehicleYear},{record.dropOffCity},{record.pickup_date},{record.dropoff_date},{record.vehicle_id}");
            }

            var rootPath = Directory.GetCurrentDirectory();
            var preProcessFolderPath = Path.Combine(rootPath, "ML", "data");

            // Ensure the Pre-process/data folder exists
            if (!Directory.Exists(preProcessFolderPath))
            {
                Directory.CreateDirectory(preProcessFolderPath);
            }

            var filePath = Path.Combine(preProcessFolderPath, "NewCarRental.csv");
            await System.IO.File.WriteAllTextAsync(filePath, csv.ToString());

            //RunPythonScripts(filePath);
            RunStreamlit();
        }
    }

    // private void RunPythonScripts(string inputFilePath)
    // {
    //     // Get the current working directory of the application
    //     var currentDir = Directory.GetCurrentDirectory();

    //     // Path to the Python executable within the bundled virtual environment
    //     var pythonExePath = Path.Combine(currentDir, "ML", "venv", "Scripts", "python.exe");

    //     // Paths to your Python scripts (adjust the relative paths as needed)
    //     var preprocessingUtilsPath = Path.Combine(currentDir, "ML", "preprocessing_utils.py");
    //     var preprocessDataPath = Path.Combine(currentDir, "ML", "preprocess_data.py");

    //     // Run preprocessing_utils.py first
    //     var startInfo = new ProcessStartInfo
    //     {
    //         FileName = pythonExePath,
    //         Arguments = $"\"{preprocessingUtilsPath}\"",  // Run the preprocessing_utils.py script
    //         RedirectStandardOutput = true,
    //         RedirectStandardError = true,
    //         UseShellExecute = false,
    //         CreateNoWindow = true
    //     };

    //     using (var process = new Process { StartInfo = startInfo })
    //     {
    //         process.OutputDataReceived += (sender, args) =>
    //         {
    //             if (!string.IsNullOrEmpty(args.Data))
    //             {
    //                 Console.WriteLine("Output: " + args.Data);
    //             }
    //         };
    //         process.ErrorDataReceived += (sender, args) =>
    //         {
    //             if (!string.IsNullOrEmpty(args.Data))
    //             {
    //                 Console.WriteLine("Error: " + args.Data);
    //             }
    //         };

    //         try
    //         {
    //             process.Start();
    //             process.BeginOutputReadLine();
    //             process.BeginErrorReadLine();
    //             process.WaitForExit();
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"Error running preprocessing_utils.py: {ex.Message}");
    //         }
    //     }

    //     // Now run preprocess_data.py with the CSV file path as an argument
    //     startInfo.Arguments = $"\"{preprocessDataPath}\" \"{inputFilePath}\"";
    //     using (var process2 = new Process { StartInfo = startInfo })
    //     {
    //         process2.OutputDataReceived += (sender, args) =>
    //         {
    //             if (!string.IsNullOrEmpty(args.Data))
    //             {
    //                 Console.WriteLine("Output: " + args.Data);
    //             }
    //         };
    //         process2.ErrorDataReceived += (sender, args) =>
    //         {
    //             if (!string.IsNullOrEmpty(args.Data))
    //             {
    //                 Console.WriteLine("Error: " + args.Data);
    //             }
    //         };

    //         try
    //         {
    //             process2.Start();
    //             process2.BeginOutputReadLine();
    //             process2.BeginErrorReadLine();
    //             process2.WaitForExit();
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"Error running preprocess_data.py: {ex.Message}");
    //         }
    //     }
    //     RunStreamlit();
    // }

    private Process _streamlitProcess;

    private void RunStreamlit()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var mlFolderPath = Path.Combine(currentDir, "ML");
        var pythonExePath = Path.Combine(mlFolderPath, "venv", "Scripts", "python.exe");
        var appPath = Path.Combine(mlFolderPath, "src", "app.py");

        var streamlitInfo = new ProcessStartInfo
        {
            FileName = pythonExePath,
            Arguments = $"-m streamlit run \"{appPath}\"",
            WorkingDirectory = mlFolderPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _streamlitProcess = new Process
        {
            StartInfo = streamlitInfo,
            EnableRaisingEvents = true
        };

        _streamlitProcess.OutputDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                Console.WriteLine("Running Streamlit app Output: " + args.Data);
            }
        };

        _streamlitProcess.ErrorDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                Console.WriteLine("Running Streamlit app Error: " + args.Data);
            }
        };

        try
        {
            _streamlitProcess.Start();
            _streamlitProcess.BeginOutputReadLine();
            _streamlitProcess.BeginErrorReadLine();
            // Do not call WaitForExit() to prevent blocking the calling thread.
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during Running Streamlit app: " + ex.Message);
        }
    }


}