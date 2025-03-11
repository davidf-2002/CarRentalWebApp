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
                            join vehicle in vehicles on booking.VehicleId equals vehicle.VehicleId
                            join pickupBranch in branches on booking.PickupBranchId equals pickupBranch.BranchId
                            join dropoffBranch in branches on booking.DropoffBranchId equals dropoffBranch.BranchId into dropoffBranchGroup
                            from dropoffBranch in dropoffBranchGroup.DefaultIfEmpty()
                            select new
                            {
                                booking.BookingId,
                                booking.CustomerName,
                                Vehicle = vehicle.Make + " " + vehicle.Model,
                                PickupBranch = pickupBranch.Name,
                                DropoffBranch = dropoffBranch?.Name,
                                booking.StartTime,
                                booking.EndTime
                            };

            var csv = new StringBuilder();
            csv.AppendLine("BookingId,CustomerName,Vehicle,PickupBranch,DropoffBranch,StartTime,EndTime");

            foreach (var record in exportData)
            {
                csv.AppendLine($"{record.BookingId},{record.CustomerName},{record.Vehicle},{record.PickupBranch},{record.DropoffBranch},{record.StartTime},{record.EndTime}");
            }

            var rootPath = Directory.GetCurrentDirectory();
            var preProcessFolderPath = Path.Combine(rootPath, "Pre-process", "data");

            // Ensure the Pre-process/data folder exists
            if (!Directory.Exists(preProcessFolderPath))
            {
                Directory.CreateDirectory(preProcessFolderPath);
            }

            var filePath = Path.Combine(preProcessFolderPath, "NewCarRental.csv");
            await System.IO.File.WriteAllTextAsync(filePath, csv.ToString());


            RunPythonScripts(filePath);
        }
    }

    private void RunPythonScripts(string inputFilePath)
    {
        // Get the current working directory of the application
        var currentDir = Directory.GetCurrentDirectory();

        // Path to the Python executable within the bundled virtual environment
        var pythonExePath = Path.Combine(currentDir, "venv", "Scripts", "python.exe");

        // Paths to your Python scripts (adjust the relative paths as needed)
        var preprocessingUtilsPath = Path.Combine(currentDir, "Pre-process", "preprocessing_utils.py");
        var preprocessDataPath = Path.Combine(currentDir, "Pre-process", "preprocess_data.py");

        // Run preprocessing_utils.py first
        var startInfo = new ProcessStartInfo
        {
            FileName = pythonExePath,
            Arguments = $"\"{preprocessingUtilsPath}\"",  // Run the preprocessing_utils.py script
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.WriteLine("Output: " + args.Data);
                }
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.WriteLine("Error: " + args.Data);
                }
            };

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running preprocessing_utils.py: {ex.Message}");
            }
        }

        // Now run preprocess_data.py with the CSV file path as an argument
        startInfo.Arguments = $"\"{preprocessDataPath}\" \"{inputFilePath}\"";
        using (var process2 = new Process { StartInfo = startInfo })
        {
            process2.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.WriteLine("Output: " + args.Data);
                }
            };
            process2.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.WriteLine("Error: " + args.Data);
                }
            };

            try
            {
                process2.Start();
                process2.BeginOutputReadLine();
                process2.BeginErrorReadLine();
                process2.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running preprocess_data.py: {ex.Message}");
            }
        }
    }

}