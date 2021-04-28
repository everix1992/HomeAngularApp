using HomeAngularApp.Formatters;
using HomeAngularApp.Services.AdventOfCode.Impl;
using HomeAngularApp.Services.AdventOfCode.Intf;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeAngularApp
{
    public class Startup
    {
        private const string FrontEndCorsPolicyName = "Angular Front-End";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: Need to update this to support other methods of hosting the front-end
            services.AddCors(options =>
            {
                options.AddPolicy(FrontEndCorsPolicyName, builder =>
                {
                    // TODO: Should we be more strict with headers and/or methods?
                    builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddControllers(options =>
            {
                options.InputFormatters.Add(new PlainTextInputFormatter());
            });

            services.AddTransient<IAdventOfCodeSolution, ReportRepair>();
            services.AddTransient<IAdventOfCodeSolution, ReportRepairPart2>();
            services.AddTransient<IAdventOfCodeSolution, PasswordPhilosophy>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(FrontEndCorsPolicyName);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                if (env.IsDevelopment())
                {
                    endpoints.MapGet("/", async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        await context.Response.WriteAsync(string.Empty);
                    });
                }
            });
        }
    }
}