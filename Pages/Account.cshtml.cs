using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;

[Authorize]
public class AccountModel(GraphServiceClient graphServiceClient) : PageModel {
  private readonly GraphServiceClient _graph = graphServiceClient;

  public string Username { get; private set; } = "unknown";
  public string PreferredUsername { get; private set; } = "";
  public string Name { get; private set; } = "";
  public string OID { get; private set; } = "";
  public Dictionary<string, string> GraphData { get; private set; } = [];

  public async Task<IActionResult> OnGet() {
    Username = User.Identity?.Name ?? "unknown";

    foreach (var claim in User.Claims) {
      if (claim.Type.Contains("objectidentifier") || claim.Type.Contains("oid")) {
        OID = claim.Value;
      }
      if (claim.Type == "name") {
        Name = claim.Value;
      }
    }

    try {
      // Fetch user details from Graph API using the /me endpoint
      // See: https://learn.microsoft.com/en-us/graph/api/user-get
      var graphDetails = await _graph.Me.Request().GetAsync();

      GraphData.Add("UPN", graphDetails.UserPrincipalName);
      GraphData.Add("Given Name", graphDetails.GivenName);
      GraphData.Add("Display Name", graphDetails.DisplayName);
      GraphData.Add("Office", graphDetails.OfficeLocation);
      GraphData.Add("Mobile", graphDetails.MobilePhone);
      GraphData.Add("Other Phone", graphDetails.BusinessPhones.Any() ? graphDetails.BusinessPhones.First() : "");
      GraphData.Add("Job Title", graphDetails.JobTitle);
    } catch (Exception) {
      // HACK! Cookie seems to get out of sync with the token cache when hotreloading the page.
      // Frankly this is hideous, but yeah whatever
      foreach (var cookie in Request.Cookies.Keys) {
        Response.Cookies.Delete(cookie);
      }
      return Redirect("/Account");
    }

    return Page();
  }
}

