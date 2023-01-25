# Asp.NETCore + Svelte + VITE

This repo contains a .NET 5.0 + Svelte.js sample code with SpaCliMiddleware (VS2022).
 
---

# Features

- **ASP.NET .NET 6.x**
  - Web API
- **Svelte 3.x**
- **VITE 4.x**

# Prerequisites:
 * nodejs > 8
 * VS2022 or VS Code
 * dotnet core - [NET Core SDK 5.0 (or later)](https://www.microsoft.com/net/download/core) for Windows, Mac, or Linux

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
                        npmScript: env.IsDevelopment() ? "dev" : "",
                        port: /*default(int)*/ 8018, // Allow vite to find own port
                        regex: "VITE",
                        forceKill: true, // kill anything running on port
                        useProxy: true, // proxy node requests back through our aspnet server
                        runner: ScriptRunnerType.Npm
                    );
            });
```
