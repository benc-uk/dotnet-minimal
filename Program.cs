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
