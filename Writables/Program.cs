using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Writables.Configurations;
using Writables.Localization;

namespace Writables
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            //builder.Services.Configure<ServerSettings>
            //    (
            //    builder.Configuration.GetSection(nameof(ServerSettings))
            //    );

            builder.Services.ConfigureWritable<ServerSettings>
                (
                builder.Configuration.GetSection(nameof(ServerSettings))
                );

            builder.Services.AddJsonLocalization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            var summaries = new[]
            {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

            app.MapGet("/server", (
                HttpContext httpContext,
                [FromServices] IWritableOptions<ServerSettings> serverOptions,
                [FromServices] IStringLocalizer L
                ) =>
            {
                serverOptions.Update(opt =>
                {
                    opt.Language = (opt.Language == "en") ? "ar" : "en";
                    opt.License = DateTime.Now.ToString();
                });

                return $"Code: {serverOptions.Value.Code}\nLanguage: {serverOptions.Value.Language}\nHi: {L["Hi"]}\nLicense: {serverOptions.Value.License}";
            });

            app.Run();
        }
    }
}