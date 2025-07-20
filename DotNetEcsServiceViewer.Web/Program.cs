using Amazon.ECS;
using DotNetEcsServiceViewer.Web.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DotNetEcsServiceViewer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IAmazonECS, AmazonECSClient>();
            builder.Services.AddScoped<EcsInfoControlService>();
            builder.Services.AddScoped<MessageService>();

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
