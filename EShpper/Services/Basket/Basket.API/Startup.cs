﻿using Basket.API.Swagger;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Basket.API
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
            //Redis Settings
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });
            services.AddMediatR(typeof(CreateShoppingCartCommandHandler).GetTypeInfo().Assembly);
            services.AddScoped<IBasketRepository, BasketRepository>();
           // services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<DiscountGrpcService>();
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
                (o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });
            services.AddHealthChecks()
                .AddRedis(Configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ct, cfg) =>
                {
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);
                });
            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
    /// Contains Identit and API Gatewayy code
    //public class Startup
    //{
    //    public IConfiguration Configuration;

    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddApiVersioning();
    //        services.AddCors(options =>
    //        {
    //            options.AddPolicy("CorsPolicy", policy =>
    //            {
    //                //TODO read the same from settings for prod deployment
    //                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    //            });
    //        }).AddVersionedApiExplorer(
    //            options =>
    //            {
    //                options.GroupNameFormat = "'v'VVV";
    //                options.SubstituteApiVersionInUrl = true;
    //            });
    //        //services.AddCors(options =>
    //        //{
    //        //    options.AddPolicy("CorsPolicy", policy =>
    //        //    {
    //        //        //TODO read the same from settings for prod development
    //        //        policy.AllowAnyHeader()
    //        //        .AllowAnyMethod()
    //        //        .AllowAnyOrigin();
    //        //    });
    //        //}).AddVersionedApiExplorer(options =>
    //        //{
    //        //    options.GroupNameFormat = "'v'VVV";
    //        //    options.SubstituteApiVersionInUrl = true;
    //        //});
    //        //services.AddVersionedApiExplorer(options =>
    //        //{
    //        //    options.GroupNameFormat = "'v'VVV";
    //        //    options.SubstituteApiVersionInUrl = true;
    //        //    services.AddApiVersioning();
    //        //    services.AddCors(options =>
    //        //    {
    //        //        options.AddPolicy("CorsPolicy", policy =>
    //        //        {
    //        //            //TODO read the same from settings for prod deployment
    //        //            policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    //        //        });
    //        //    });
    //        //});
    //        //Redis Settings
    //        services.AddStackExchangeRedisCache(options =>
    //        {
    //            options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
    //        });
    //        services.AddMediatR(typeof(CreateShoppingCartCommandHandler).GetTypeInfo().Assembly);
    //        services.AddScoped<IBasketRepository, BasketRepository>();
    //        //services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
    //        services.AddAutoMapper(typeof(Startup));
    //        services.AddScoped<DiscountGrpcService>();
    //        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    //  (o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));

    //        //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    //        //services.AddSwaggerGen(options =>
    //        //{
    //        //    options.OperationFilter<SwaggerDefaultValues>();
    //        //});
    //        services.AddSwaggerGen(c =>
    //        {
    //            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
    //        });
    //        services.AddHealthChecks()
    //            .AddRedis(Configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);
    //        services.AddMassTransit(config =>
    //        {
    //            config.UsingRabbitMq((ct, cfg) =>
    //            {
    //                cfg.Host(Configuration["EventBusSettings:HostAddress"]);
    //            });
    //        });
    //        services.AddMassTransitHostedService();
    //        //services.AddMassTransitHostedService();
    //        // Identity Server changes
    //        var userPolicy = new AuthorizationPolicyBuilder()
    //            .RequireAuthenticatedUser()
    //            .Build();

    //        services.AddControllers(config =>
    //        {
    //            config.Filters.Add(new AuthorizeFilter(userPolicy));
    //        });

    //        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //            .AddJwtBearer(options =>
    //            {
    //                options.Authority = "https://ifeanyi.eshopping.com:44344";
    //                options.Audience = "Basket";
    //            });
    //    }

    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    //    {

    //        var nginxPath = "/basket";
    //        if (env.IsEnvironment("Local"))
    //        {
    //            app.UseDeveloperExceptionPage();
    //            app.UseSwagger();
    //            app.UseSwaggerUI();
    //            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
    //        }
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //            app.UseForwardedHeaders(new ForwardedHeadersOptions
    //            {
    //                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    //            });
    //            app.UseSwagger();
    //            app.UseSwaggerUI();
    //            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));

    //            app.UseSwaggerUI(options =>
    //            {
    //                foreach (var description in provider.ApiVersionDescriptions)
    //                {
    //                    options.SwaggerEndpoint($"{nginxPath}/swagger/{description.GroupName}/swagger.json",
    //                    $"Basket API {description.GroupName.ToUpperInvariant()}");
    //                    options.RoutePrefix = string.Empty;
    //                }
    //                options.DocumentTitle = "Basket API Documentation";
    //            });

    //            //app.UseSwaggerUI(options =>
    //            //{

    //            //    foreach (var description in provider.ApiVersionDescriptions)
    //            //    {
    //            //        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    //            //    }
    //            //});
    //        }


    //        app.UseHttpsRedirection();
    //        app.UseRouting();
    //        app.UseCors("CorsPolicy");
    //        app.UseAuthentication();
    //        app.UseStaticFiles();
    //        app.UseAuthorization();


    //        app.UseEndpoints(endpoints =>
    //        {
    //            endpoints.MapControllers();
    //            endpoints.MapHealthChecks("/health", new HealthCheckOptions
    //            {
    //                Predicate = _ => true,
    //                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    //            });
    //        });
    //    }

    //}


}
