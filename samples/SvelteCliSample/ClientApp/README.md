# svelte app - asp.net core middleware
- If you are using `dotnet run`, make sure navigate to applicationUrl defined in lunchSettings.json (port 5087 in this example)
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
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();

                // Note: only use spacliproxy in development. 
                // Production should use "UseSpaStaticFiles()"
                endpoints.MapToSpaCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "ClientApp" },
                    npmScript: env.IsDevelopment() ? "autobuild" : null,
                    port: 35729,
                    regex: "LiveReload enabled",
                    forceKill: true,
                    useProxy: false
                );
            });
```