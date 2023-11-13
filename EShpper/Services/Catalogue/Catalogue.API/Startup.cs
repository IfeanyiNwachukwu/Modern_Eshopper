using Catalogue.API.Extensions;
using Catalogue.Application.Handlers;
using Catalogue.Core.Repositories;
using Catalogue.Infrastructure.Data.SeedData;
using Catalogue.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
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
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalogue.API", Version = "v1" }); });
            //DI
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(CreateProductHandler).GetTypeInfo().Assembly);
           // services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddScoped<ICatalogueContext, CatalogueContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();
            services.AddScoped<ITypesRepository, ProductRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalogue.API v1"));
                app.UseDeveloperExceptionPage();
              
            }

            app.UseRouting();
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




    //// Contains Gateway and Identity Server code
    //public class Startup
    //{
    //    public IConfiguration Configuration;

    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddApiVersioning(options => options.ReportApiVersions = true)
    //            .AddVersionedApiExplorer(
    //            options =>
    //            {
    //                options.GroupNameFormat = "'v'VVV";
    //                options.SubstituteApiVersionInUrl = true;
    //            });
    //        services.AddMvcCore()
    //            .AddCors(options =>
    //            {
    //                options.AddPolicy("CorsPolicy", policy =>
    //                {
    //                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    //                });
    //            }).AddApiExplorer();

    //        services.AddHealthChecks()
    //            .AddMongoDb(Configuration["DatabaseSettings:ConnectionString"], "Catalogue  Mongo Db Health Check",
    //                HealthStatus.Degraded);
    //        // services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Catalog.API", Version = "v1"}); });

    //        services.AddSwaggerDocumentation();
    //        //DI
    //        services.AddAutoMapper(typeof(Startup));
    //        services.AddMediatR(typeof(CreateProductHandler).GetTypeInfo().Assembly);
    //        services.AddScoped<ICatalogueContext, CatalogueContext>();
    //        services.AddScoped<IProductRepository, ProductRepository>();
    //        services.AddScoped<IBrandRepository, ProductRepository>();
    //        services.AddScoped<ITypesRepository, ProductRepository>();

    //        //Identity Server changes
    //        var userPolicy = new AuthorizationPolicyBuilder()
    //            .RequireAuthenticatedUser()
    //            .Build();

    //        services.AddControllers(config =>
    //        {
    //            config.Filters.Add(new AuthorizeFilter(userPolicy));
    //        });


    //        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //                .AddJwtBearer(options =>
    //                {
    //                    options.Authority = Configuration.GetValue<string>("AuthN:Authority");
    //                    options.Audience = Configuration.GetValue<string>("AuthN:ApiName");
    //                });
    //        services.AddAuthorization(options =>
    //        {
    //            options.AddPolicy("CanRead", policy => policy.RequireClaim("scope", "catalogueapi.read"));
    //        });
    //    }

    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    //    {
    //        var nginxPath = "/catalogue";

    //        // if (env.IsEnvironment("Local"))
    //        // {
    //        //     app.UseDeveloperExceptionPage();  
    //        //     app.UseSwagger();
    //        //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
    //        // }

    //        // if (env.IsDevelopment())
    //        // {
    //        app.UseDeveloperExceptionPage();
    //        app.UseForwardedHeaders(new ForwardedHeadersOptions
    //        {
    //            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    //        });


    //        app.UseSwaggerDocumentation(nginxPath, Configuration, provider);
    //        // app.UseSwagger();
    //        // app.UseSwaggerUI(options =>
    //        // {
    //        //     foreach (var description in provider.ApiVersionDescriptions)
    //        //     {
    //        //         options.SwaggerEndpoint($"{nginxPath}/swagger/{description.GroupName}/swagger.json",
    //        //             $"Catalog API {description.GroupName.ToUpperInvariant()}");
    //        //         options.RoutePrefix = string.Empty;
    //        //     }
    //        //
    //        //     options.DocumentTitle = "Catalog API Documentation";
    //        //
    //        // });
    //        //  }

    //        app.UseHttpsRedirection();
    //        app.UseRouting();
    //        app.UseCors("CorsPolicy");
    //        app.UseAuthentication();
    //        app.UseStaticFiles();
    //        app.UseAuthorization();
    //        app.UseEndpoints(endpoints =>
    //        {
    //            endpoints.MapControllers();
    //            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
    //            {
    //                Predicate = _ => true,
    //                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    //            });
    //        });
    //    }

    //}


}
