using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleAspNetCore2ApplicationNETFramework.Data;
using SampleAspNetCore2ApplicationNETFramework.Services;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace SampleAspNetCore2ApplicationNETFramework
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();

            // RT: TODO make parameters come from appsettings.json
            services.AddAuthentication()
                .AddSaml2(options => 
                {
                    options.SPOptions.EntityId = new EntityId("https://localhost:44342/Saml2"); // ME! This Project

                    // base path of the SAML2 endpoints. Default is /Saml2
                    options.SPOptions.ModulePath = "/Saml2";

                    // This could be overridden by  IdentityProvider option  RelayStateUsedAsReturnUrl = true
                    //
                    // This works, but can't seem to see the SAML2 XML => can't get to the asdfjklasdlfjk
                    options.SPOptions.ReturnUrl = new Uri("/RightsTracker/HelloV2", UriKind.Relative);
                    //
                    // This throws an error
                    //options.SPOptions.ReturnUrl = new Uri("/Saml2/Acs", UriKind.Relative);
                    //
                    // This goes around in a pointless loop
                    //options.SPOptions.ReturnUrl = new Uri("/Saml2/SignIn", UriKind.Relative);

                    // RT: TODO appsettings here will be an array of IdentityProviders:
                    // #1 a StupIdp Tenant we use for internal testing
                    // #2 EBU
                    // #3 additional clients can be added
                    options.IdentityProviders.Add(
                        new IdentityProvider(
                            new EntityId("https://localhost:44300/b4bb4f76-1292-4504-8a79-a6df3d5bf707/Metadata"), // StubIdP
                            options.SPOptions)
                        {
                            LoadMetadata = true,

                            // IdP_Init == they start the login from their end
                            AllowUnsolicitedAuthnResponse = true,

                            // They can say where they want to land if this is true
                            RelayStateUsedAsReturnUrl = false
                        });

                    // Doesn't matter what pfx cert is used; It is just used for encryption. Can be any self signed cert.
                    options.SPOptions.ServiceCertificates.Add(new X509Certificate2("Sustainsys.Saml2.Tests.pfx"));
                })
                .AddSaml2("saml2V2", "saml2V2", options =>
                {
                    options.SPOptions.EntityId = new EntityId("https://localhost:44342/Saml2V2"); // ME! This Project

                    // base path of the SAML2 endpoints. Default is /Saml2
                    options.SPOptions.ModulePath = "/Saml2V2";

                    options.IdentityProviders.Add(
                        new IdentityProvider(
                            new EntityId("https://localhost:44300/e7388e4e-592d-4b3a-a02f-9432179dc292/Metadata"), // StubIdP
                            options.SPOptions)
                        {
                            LoadMetadata = true,

                            // IdP_Init == they start the login from their end
                            AllowUnsolicitedAuthnResponse = false,

                            // They can say where they want to land if this is true
                            RelayStateUsedAsReturnUrl = false
                        });

                    // Doesn't matter what pfx cert is used; It is just used for encryption. Can be any self signed cert.
                    options.SPOptions.ServiceCertificates.Add(new X509Certificate2("Sustainsys.Saml2.Tests.pfx"));
                })
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
