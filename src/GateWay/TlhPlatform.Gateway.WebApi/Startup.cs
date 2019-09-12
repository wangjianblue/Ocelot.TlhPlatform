using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using App.Metrics;
using Exceptionless;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using TlhPlatform.Gateway.Application;
using TlhPlatform.Infrastructure.Extents;

namespace TlhPlatform.Gateway.WebApi
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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // ensure not change any return Claims from Authorization Server
            var identityServerOptions = new IdentityServerOptions();
            Configuration.Bind("IdentityServerOptions", identityServerOptions);
            services.AddMvcCore().AddAuthorization().AddJsonFormatters();
            //services.AddAuthentication(option =>
            //    {
            //        option.DefaultScheme = identityServerOptions.IdentityScheme;
            //        option.DefaultChallengeScheme = identityServerOptions.IdentityScheme;
            //        option.DefaultAuthenticateScheme = identityServerOptions.IdentityScheme;
            //    })
            //    .AddIdentityServerAuthentication("orderKey", options =>
            //    {
            //        options.ApiSecret = "tlh";
            //        options.Authority = $"http://{identityServerOptions.ServerIP}:{identityServerOptions.ServerPort}";
            //        options.ApiName = identityServerOptions.ResourceName;
            //        options.RequireHttpsMetadata = Convert.ToBoolean(identityServerOptions.UseHttps);
            //        options.SupportedTokens = SupportedTokens.Both;
            //    });
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("orderKey", cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;    
                    cfg.Audience = $"http://{identityServerOptions.ServerIP}:{identityServerOptions.ServerPort}";
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                  
                    };
                });
            services.AddOcelot(Configuration).AddConsul().AddPolly().AddConfigStoredInConsul();
            // Swagger
            services.AddMvc();
            

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"{Configuration["Swagger:DocName"]}", new OpenApiInfo
                {
                    Title = Configuration["Swagger:Title"],
                    Version = Configuration["Swagger:Version"]
                });
            });

            #region AppMetrics

            bool isOpenMetrics = Convert.ToBoolean(Configuration["AppMetrics:IsOpen"]);
            if (isOpenMetrics)
            {
                string database = Configuration["AppMetrics:DatabaseName"];
                string connStr = Configuration["AppMetrics:ConnectionString"];
                string app = Configuration["AppMetrics:App"];
                string env = Configuration["AppMetrics:Env"];
                string username = Configuration["AppMetrics:UserName"];
                string password = Configuration["AppMetrics:Password"];

                var uri = new Uri(connStr);
                var metrics = AppMetrics.CreateDefaultBuilder().Configuration.Configure(options =>
                {
                    options.AddAppTag(app);
                    options.AddEnvTag(env);
                }).Report.ToInfluxDb(options =>
                {
                    options.InfluxDb.BaseUri = uri;
                    options.InfluxDb.Database = database;
                    options.InfluxDb.UserName = username;
                    options.InfluxDb.Password = password;
                    options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                    options.HttpPolicy.FailuresBeforeBackoff = 5;
                    options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                    options.FlushInterval = TimeSpan.FromSeconds(5);
                }).Build();

                services.AddMetrics(metrics);
                services.AddMetricsReportingHostedService();
                //services.AddMetricsReportScheduler();
                services.AddMetricsTrackingMiddleware();
                services.AddMetricsEndpoints();
            }

            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var configuration = new OcelotPipelineConfiguration
            {
                PreErrorResponderMiddleware = async (ctx, next) =>
                {
                    var token = ctx.HttpContext.Request.Headers["token"].FirstOrDefault();//这里可以进行接收的客户端token解析转发
                    ctx.HttpContext.Request.Headers.Add("X-Hello", token);
                    await next.Invoke();
                }
            };
            app.UseAuthentication();//使用授权中间件
            app.UseExceptionless(Configuration["ExceptionKeyLess:ApiKey"]);
          
            // AppMetrics
            bool isOpenMetrics = Convert.ToBoolean(Configuration["AppMetrics:IsOpen"]);
            if (isOpenMetrics)
            {
                app.UseMetricsAllEndpoints();
                app.UseMetricsAllMiddleware();
            }
              var apiList = Configuration["Swagger:ServiceDocNames"].Split(',').ToList();

            app.UseMvc()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    apiList.ForEach(apiItem =>
                    {
                        options.SwaggerEndpoint($"/doc/{apiItem}/swagger.json", apiItem);
                    });
                });
         
            app.UseOcelot().Wait();
        }
    }
}
