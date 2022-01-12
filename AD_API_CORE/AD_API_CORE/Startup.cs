using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AD_Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AD_Entities.HelperClasses;
using AD_Infrastructure;
using AD_Repository;
using AD_API_CORE.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AD_Repository_XUnit;

namespace AD_API_CORE
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;
        readonly string _myAllowedSpecificOrigins = "AllowFromAll";
        private string _UseSwaggerUi = "0";
        public Startup(IConfiguration configuration, IWebHostEnvironment HostingEnvironment)
        {
            //Configuration = configuration;
            _configuration = configuration;
            _hostingEnvironment = HostingEnvironment;
            _UseSwaggerUi = _configuration["Config:UseSwaggerUi"];
        }

       // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Config>(_configuration.GetSection("Config"));
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = Int64.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
                x.ValueLengthLimit = int.MaxValue;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = Int64.MaxValue;

            });



            services.AddDbContext<Alaska_DemoContext>(options => options
             //.UseLazyLoadingProxies(false)
             .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
             .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),

              sqlServerOptions => sqlServerOptions.CommandTimeout(60).EnableRetryOnFailure()));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Config:JwtKey"]))
        };


    });

            services.AddCors(options =>
            {
                options.AddPolicy(name: _myAllowedSpecificOrigins,
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        );
            });
            services.AddControllers().AddNewtonsoftJson();

            if (_UseSwaggerUi.Trim() == "1")
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AD_API_CORE", Version = "v1" });
                });
            }

            //Singleton objects are the same for every object and every request.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(_configuration);
            services.AddSingleton<ConfigWrapper>();
            //Scoped objects are the same within a request, but different across different requests.
            //services.AddScoped<RVideo_Test>();
            services.AddScoped<SessionManager>();
            services.AddScoped<IAccounts, RAccounts>();
            //services.AddScoped<RAccountsTest>();
            //Transient objects are always different; a new instance is provided to every controller and every service.
            //services.AddTransient<IEmailSender, AuthMessageSender>();
            SessionManager.SessionConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                if (_UseSwaggerUi.Trim() == "1")
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AD_API_CORE v1"));
                }
                System.Diagnostics.Debug.AutoFlush = true;
            }
            app.UseSimpleExceptionHandler();

            app.Use(async (context, next) =>
            {
                context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = int.MaxValue;
                await next.Invoke();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
