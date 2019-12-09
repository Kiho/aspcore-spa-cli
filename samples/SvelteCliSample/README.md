# Asp.NETCore + Svelte + Rollup

This repo contains an aspnetcore 3.0 + Svelte.js sample code with SpaCliMiddleware (VS2019).
 
---

# Features

- **ASP.NET Core 3.x**
  - Web API
- **Svelte 3.x**
- **Rollup 1.x**

# Prerequisites:
 * nodejs > 8
 * VS2019 or VS Code
 * dotnet core - [NET Core SDK 3.0 (or later)](https://www.microsoft.com/net/download/core) for Windows, Mac, or Linux

# Use Middleware
- If you are using `dotnet run`, make sure navigate to applicationUrl defined in lunchSettings.json (port 5087 in this example)
- Install SpaCliMiddleware from nuget in new project.
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
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
                    npmScript: env.IsDevelopment() ? "autobuild" : "",
                    port: 35729,
                    regex: "LiveReload enabled",
                    forceKill: true,
                    useProxy: false
                );
            });
```
