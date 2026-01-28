using LibraryProject.Presentation.Web.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryProject.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //        .SetBasePath(AppContext.BaseDirectory)
            //        .AddJsonFile("appsettings.json", optional: false);

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationStateProvider>();

            // Add services to the container.
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();




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
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
