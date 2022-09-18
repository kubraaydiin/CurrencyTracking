using CurrencyTracking.Domain;
using CurrencyTracking.Domain.Abstracts;
using CurrencyTracking.Repository;
using CurrencyTracking.Repository.Abstracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Alachisoft.NCache.EntityFrameworkCore;
using System.Configuration;

namespace CurrencyTracking.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddDbContext<PostgreSqlContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<PostgreSqlContext>(options =>
            {
                NCacheConfiguration.Configure(Configuration["CacheId"], DependencyType.Other);
                NCacheConfiguration.ConfigureLogger();

                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();
            services.AddScoped<ICurrencyOperations, CurrencyOperations>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Currency Tracking API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency Tracking API v1.0");
            });
        }
    }
}
