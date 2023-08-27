using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductRepairDealerUI.Data;
using Microsoft.AspNetCore.Authentication.Cookies; // Add this namespace for cookie authentication
using Microsoft.AspNetCore.Authentication; // Add this namespace for authentication related types
using ProductRepairDataAccess.Interfaces;
using ProductRepairDataAccess.Services;
using ProductRepairDataAccess.DataAccess;

namespace ProductRepairDealerUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddHttpContextAccessor(); // Add IHttpContextAccessor for accessing HttpContext
            builder.Services.AddScoped<IAccountService, AccountService>(); // Register the AccountService for DI
            builder.Services.AddScoped<IDataAccess, DataAccess>(); //Register DataAccess for DI
            builder.Services.AddScoped<IItemDataAccess, ItemDataAccess>();
            builder.Services.AddScoped<ICaseDataAccess, CaseDataAccess>();
            builder.Services.AddScoped<IConfigurationSettings, ConfigurationSettings>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
