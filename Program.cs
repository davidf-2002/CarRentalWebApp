using CarRentalWebApp.Data;
using CarRentalWebApp.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Set database path
var folder = Environment.SpecialFolder.MyDocuments;
var path = Environment.GetFolderPath(folder);
var DbPath = Path.Join(path, "CarRental.db");

// Configure DBContext to use SQLite
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlite($"Data Source={DbPath}"));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IVehicleRepo, VehicleRepo>();
    builder.Services.AddScoped<IBookingRepo, BookingRepo>();
    builder.Services.AddScoped<IBranchRepository, BranchRepository>();
    builder.Services.AddScoped<IVehicleBranchRepository, VehicleBranchRepository>();
}

// Register the BookingService
builder.Services.AddScoped<BookingService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
