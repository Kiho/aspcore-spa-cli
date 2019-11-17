# SpaCliMiddleware - Supporting Vue Cli plus Svelte

[![](https://img.shields.io/nuget/v/SpaCliMiddleware.svg)](https://www.nuget.org/packages/SpaCliMiddleware/)

This is a stand-alone module to add Vue, Svelte and Other SPA support to AspNet Core.

This project forked from here: [https://github.com/EEParker/aspnetcore-vueclimiddleware](https://github.com/EEParker/aspnetcore-vueclimiddleware)
and added support for other SPA frameworks.

## Svelte
- Find the example here: [https://github.com/Kiho/aspcore-spa-cli/tree/master/samples/SvelteCliSample](https://github.com/Kiho/aspcore-spa-cli/tree/master/samples/SvelteCliSample)
- If you are using `dotnet run`, make sure navigate to applicationUrl defined in lunchSettings.json (port 5087 in SvelteCliSample)
- Install SpaCliMiddleware from nuget in new project.
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/public";
            });
        }
```
```csharp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ....
            app.UseSpaStaticFiles();
            ....
// To use with EndpointRouting
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                var autoBuild = new ScriptArgs("autobuild", 35729, "LiveReload enabled");
                // cli will not be invoked if we don't pass npmScript
                endpoints.MapToSpaCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "ClientApp" },
                    npmScript: env.IsDevelopment() ? "serve" : null,
                    port: 8080,
                    regex: "Your application is ready",
                    forceKill: true,
                    autoBuild: autoBuild
                    );
            });
```

## Credits
- https://github.com/EEParker/aspnetcore-vueclimiddleware
- https://github.com/lukeed/svelte-demo
- https://github.com/MaximBalaganskiy/AureliaDotnetTemplate