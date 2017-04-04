using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using IRSI.Services.Website.ApiClients;
using IRSI.Services.Website.Configuration;

namespace IRSI.Services.Website
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            //TODO: Add Application Insights Telemetry

            services.Configure<SOSApiClientOptions>(x =>
            {
                x.ApiUrl = Configuration["SOSApi:Url"];
            });

            services.Configure<AVTApiClientOptions>(x =>
            {
                x.ApiUrl = Configuration["AVTApi:Url"];
            });

            services.Configure<StoreApiClientOptions>(x =>
            {
                x.ApiUrl = Configuration["StoresApi:Url"];
            });

            services.Configure<TeamSalesApiClientOptions>(x =>
            {
                x.ApiUrl = Configuration["TeamSalesApi:Url"];
            });

            services.Configure<AVTOptions>(x =>
            {
                x.AVTTarget = decimal.Parse(Configuration["AVT:Target"]);
            });

            services.AddAuthorization(p =>
            {
                p.AddPolicy("UseSOSService", pa =>
                {
                    pa.RequireAuthenticatedUser()
                    .RequireClaim("UseSOSService", "true");
                });

                p.AddPolicy("UseAVTService", pa =>
                {
                    pa.RequireAuthenticatedUser()
                    .RequireClaim("UseAVTService", "true");
                });

                p.AddPolicy("UseTeamSalesService", pa =>
                {
                    pa.RequireAuthenticatedUser()
                    .RequireClaim("UseTeamSalesService", "true");
                });
            });

            // Add framework services.
            services.AddMvc();
            services.AddTransient<SOSApiClient>();
            services.AddTransient<AVTApiClient>();
            services.AddTransient<StoresApiClient>();
            services.AddTransient<TeamSalesApiClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var oidcOptions = new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",
                AutomaticAuthenticate = false,

                Authority = Configuration["IdentityServer:Authority"],

                ClientId = Configuration["IdentityServer:ClientId"],
                ClientSecret = Configuration["IdentityServer:ClientSecret"],

                PostLogoutRedirectUri = Configuration["IdentityServer:RedirectUrl"],

                ResponseType = OpenIdConnectResponseType.CodeIdToken,

                SaveTokens = true,
                GetClaimsFromUserInfoEndpoint = true,

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                }
            };

            if (env.IsDevelopment())
            {
                oidcOptions.RequireHttpsMetadata = false;
            }

            oidcOptions.Scope.Clear();
            oidcOptions.Scope.Add("openid");
            oidcOptions.Scope.Add("profile");
            oidcOptions.Scope.Add("email");
            oidcOptions.Scope.Add("roles");
            oidcOptions.Scope.Add("sos_api");
            oidcOptions.Scope.Add("team_api");
            oidcOptions.Scope.Add("irsi_identity");

            app.UseOpenIdConnectAuthentication(oidcOptions);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
