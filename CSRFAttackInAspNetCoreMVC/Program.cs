using CSRFAttackInAspNetCoreMVC.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Antiforgery;

namespace CSRFAttackInAspNetCoreMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Anti-Forgery token services
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN"; // Customize as needed
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Add this line if using authentication
            app.UseAuthorization();

            // Apply Anti-Forgery Token Middleware
            app.Use(next => context =>
            {
                if (
                    string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(context.Request.Method, "PUT", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(context.Request.Method, "DELETE", StringComparison.OrdinalIgnoreCase))
                {
                    var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
                    antiforgery.ValidateRequestAsync(context).Wait();
                }
                return next(context);
            });

            app.MapRazorPages();
            app.MapControllers(); // Use this instead of UseEndpoints for mapping controllers

            app.Run();
        }
    }
}
