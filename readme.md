# Super Minimal .NET 8 Web App + API, with Auth and Graph API

This is a very small example to show a working web app in .NET 8 with the new minimal hosting model. It adds sign-in auth with `Microsoft.Identity.Web` library and Graph API support. It doesn't represent best practice, and uses the least amount of code possible to get up & running.

There is a tiny REST API included `/api/books`

The entire Program.cs is shown below in all it's super minimal glory

```cs
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddMicrosoftIdentityUI();
builder.Services.AddAuthentication("OpenIdConnect")
    .AddMicrosoftIdentityWebApp(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi(["User.Read"])
    .AddMicrosoftGraph()
    .AddInMemoryTokenCaches();

var app = builder.Build();
app.UseStaticFiles();
app.UseAuthorization();
app.MapRazorPages();

BookAPI.AddRoutes(app); // A simple REST API for books

app.Run();
```

# Pre-reqs

- .NET 8 - https://dotnet.microsoft.com/download/dotnet/8.0
- A registered app in Entra ID with a secret - https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app
  - Add a "web application" for signin
  - Set the redirect URL to be `http://localhost:5001/signin-oidc`

# Quick Start

- Copy `appsettings.Development.json.sample` to `appsettings.Development.json` and update any references to `__CHANGEME__` in the file.
- Run `dotnet watch` or `dotnet run`
- Go to http://localhost:5001
- Click on "Account" to sign-in and have various details about the user shown
