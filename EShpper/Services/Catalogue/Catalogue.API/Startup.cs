using Catalogue.Application.Handlers;
using Catalogue.Core.Repositories;
using Catalogue.Infrastructure.Data.SeedData;
using Catalogue.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalogue.API
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();
        
            services.AddHealthChecks()
            .AddMongoDb(Configuration["DatabaseSettings:ConnectionString"], "Catalogue  Mongo Db Health Check",
              HealthStatus.Degraded);
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" }); });
         
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(CreateProductHandler).GetTypeInfo().Assembly);
            services.AddScoped<ICatalogueContext, CatalogueContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();
            services.AddScoped<ITypesRepository, ProductRepository>();





        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            // app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
