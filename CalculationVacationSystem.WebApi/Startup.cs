using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.BL.Utils;
using CalculationVacationSystem.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace CalculationVacationSystem.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DAL.Context.BaseDbContext>(opt =>
            {
                opt.UseNpgsql(Configuration["Database:ConnectionString"]);
                opt.UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()));
            });
            services.AddScoped<IEmployeesServiceInterface, EmloyeeService>();
            services.AddScoped<IJwtUtils, JwtTokenGenerator>();
            services.AddScoped<IAuthData, AuthService>();
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                              builder =>
                              {
                                  builder.WithOrigins("https://192.168.0.2:4200", "https://192.168.0.2:5001")
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CalculationVacationSystem.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CalculationVacationSystem.WebApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();

            /*app.UseAuthentication();
            app.UseAuthorization();*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
