using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpaCliMiddleware;

namespace SvelteCliSample
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
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/public";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSpaStaticFiles();

            app.UseFileServer();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();

                // Note: only use spacliproxy in development. 
                // Production should use "UseSpaStaticFiles()" and the webpack dist
                endpoints.MapToSpaCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "ClientApp" },
                    npmScript: env.IsDevelopment() ? "autobuild" : null,
                    port: 5000,
                    regex: "Your application is ready",
                    forceKill: true
                    );
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();

            //    var autoBuild = new ScriptArgs("autobuild", 35729, "LiveReload enabled");
            //    // cli will not be invoked if we don't pass npmScript
            //    endpoints.MapToSpaCliProxy(
            //        "{*path}",
            //        new SpaOptions { SourcePath = "ClientApp" },
            //        npmScript: env.IsDevelopment() ? "serve" : null,
            //        port: 8080,
            //        regex: "Your application is ready",
            //        forceKill: true,
            //        autoBuild: autoBuild
            //        );
            //});
        }
    }
}
