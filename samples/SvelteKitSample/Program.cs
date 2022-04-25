using Jering;
using Microsoft.AspNetCore.SpaServices;
using SpaCliMiddleware;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorPages();
// Add Jering Node services
builder.Services.ConfigureNodejsService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

IHostEnvironment? hostEnvironment = app.Services.GetService<IHostEnvironment>();

app.UseHttpsRedirection();
app.UseStaticFiles();
// app.UseResponseCompression();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

if (hostEnvironment != null)
{
    if (!hostEnvironment.IsDevelopment())
    {
        // Add 
        app.UseNodejsService(hostEnvironment);
        app.UseMiddleware<NodejsMiddleware>();
    }

    app.UseEndpoints(endpoints => {
        // endpoints.MapControllers();
        
        if (hostEnvironment.IsDevelopment())
        {
            // Note: only use spacliproxy in development. 
            // Production should use "UseSpaStaticFiles()"
            endpoints.MapToSpaCliProxy(
                "{*path}",
                new SpaOptions { SourcePath = "Scripts" },
                npmScript: "dev",
                port: /*default(int)*/ 8019, // Allow vite to find own port
                regex: "SvelteKit v",
                forceKill: true, // kill anything running on our webpack port
                useProxy: true, // proxy webpack requests back through our aspnet server
                runner: ScriptRunnerType.Npm
            );
        }
    });
}

app.Run();
