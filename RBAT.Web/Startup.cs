using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.LayoutRenderers;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic;
using RBAT.Web.Extensions;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace RBAT.Web
{

    public class Startup {

        const int FORM_ENTRIES_LIMIT = 5000000;

        public Startup( IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services ) {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            if ( env.IsDevelopment() ) {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } else {
                app.UseExceptionHandler( "/Home/Error" );
            }

            app.UseRewriter(new RewriteOptions()
                .AddRedirectToProxiedHttps()
                .AddRedirect("(.*)/$", "$1")  // remove trailing slash
            );

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc( routes => {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}" );
            } );

          //  Applies any pending migrations for the context to the database.Do not use update - database
            using (var ctx = new RBATContext())
                {
                    ctx.Database.Migrate();
                }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var init = scope.ServiceProvider.GetService<RBATDbInitializer>();
                init.Seed(new RBATContext());
            }


            //register $currentdir}
            LayoutRenderer.Register("currentdir", (logEvent) => Directory.GetCurrentDirectory());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services ) {
            RBATContext.ConnectionString = Configuration.GetConnectionString( "DefaultConnection" );            

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.ConfigureIdentity();

            services.Configure<FormOptions>(x => x.ValueCountLimit = FORM_ENTRIES_LIMIT);

            services.ConfigureCookie();

            // Add application services.
            services.ConfigureApplicationService();
            //Register class in startup.cs for DI purposes
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

          //  services.AddTransient<IEmailSender, AuthMessageSender>();

            services.AddMvc().AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );           
        }

     
    }
}
