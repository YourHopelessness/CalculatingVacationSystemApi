using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.BL.Utils;
using CalculationVacationSystem.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Text.Json.Serialization;

namespace CalculationVacationSystem.WebApi
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        /// <summary>
        /// Application Configuration
        /// </summary>
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DAL.Context.BaseDbContext>(opt =>
            {
                if (_env.IsDevelopment())
                    opt.UseNpgsql(Configuration["Database:ConnectionString"]);
                else
                    opt.UseSqlServer(Configuration["Database:ConnectionString"]);
                opt.UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()));
            });
            services.AddLogging();
            services.AddScoped<IEmployeesServiceInterface, EmployeeService>();
            services.AddScoped<IAuthData, AuthService>();
            services.AddScoped<IRequestHandler, RequestService>();
            services.AddTransient<IJwtUtils, JwtTokenGenerator>();
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://192.168.0.2:4200", "https://192.168.0.2:5001") //TODO move to appsettings
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                    });
            });
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            services.AddControllers()
                    .AddJsonOptions(x =>
                            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "CalculationVacationSystem.WebApi",
                    Version = "v1",
                    Description = "Application thats calculate vacations of employees",
                    Contact = new OpenApiContact
                    {
                        Name = "Darya",
                        Url = new Uri("https://github.com/YourHopelessness"),
                    }

                });
                var filePath = Path.Combine(AppContext.BaseDirectory, "CalculationVacationSystem.WebApi.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CalculationVacationSystem.WebApi v1"));

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
