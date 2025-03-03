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
            var filePath = Path.Combine(rootPath, "BookingsExport.csv");
            await System.IO.File.WriteAllTextAsync(filePath, csv.ToString());

            // RunPythonScript(filePath);
        }
    }

    // private void RunPythonScript(string filePath)
    // {
    //     var psi = new ProcessStartInfo
    //     {
    //         FileName = "python",
    //         Arguments = $"path/to/script.py {filePath}",
    //         RedirectStandardOutput = true,
    //         RedirectStandardError = true,
    //         UseShellExecute = false,
    //         CreateNoWindow = true
    //     };

    //     using (var process = Process.Start(psi))
    //     {
    //         process.WaitForExit();
    //         var output = process.StandardOutput.ReadToEnd();
    //         var error = process.StandardError.ReadToEnd();

    //         if (!string.IsNullOrEmpty(error))
    //         {
    //             throw new Exception($"Python script error: {error}");
    //         }

    //         Console.WriteLine(output);
    //     }
    // }
}