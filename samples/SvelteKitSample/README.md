# Asp.NETCore + Svelte-Kit

This repo contains a .NET 6.0 + Svelte-Kit sample code with SpaCliMiddleware (VS2022) in development.

Use [svelte-adapter-aspcore](https://github.com/kiho/svelte-adapter-aspcore) in production, it requires [Jering.Javascript.NodeJS](https://github.com/JeringTech/Javascript.NodeJS).

---

# Features

- **ASP.NET .NET 6.x**
  - Web API
- **Svelte-Kit 1.x.x**
  - svelte-adapter-aspcore
  - Jering.Javascript.NodeJS

# Prerequisites:
 * nodejs > 16
 * VS2022 or VS Code
 * dotnet core - [NET Core SDK 6.0 (or later)](https://www.microsoft.com/net/download/core) for Windows, Mac, or Linux

# Use Middleware
- If you are using `dotnet run`, make sure navigate to applicationUrl defined in lunchSettings.json (port 5004 in this example)
- Install SpaCliMiddleware from nuget in new project.
