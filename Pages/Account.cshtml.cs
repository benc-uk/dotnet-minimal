using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;

[Authorize]
public class AccountModel : PageModel {
  private readonly GraphServiceClient _graphServiceClient;

  public string username { get; private set; } = "unknown";
  public string preferredUsername { get; private set; } = "";
  public string name { get; private set; } = "";
  public string oid { get; private set; } = "";
  public Dictionary<string, string> graphData = new Dictionary<string, string>();

  public AccountModel(ILogger<AccountModel> logger, GraphServiceClient graphServiceClient) {
    _graphServiceClient = graphServiceClient;
  }

  public async Task<IActionResult> OnGet() {
    username = User.Identity?.Name ?? "unknown";

    foreach (Claim claim in User.Claims) {
      if (claim.Type.Contains("objectidentifier") || claim.Type.Contains("oid")) {
        oid = claim.Value;
      }
      if (claim.Type == "name") {
        name = claim.Value;
      }
    }

    try {
      // Fetch user details from Graph API
      var graphDetails = await _graphServiceClient.Me
      .Request()
      .GetAsync();

      graphData.Add("UPN", graphDetails.UserPrincipalName);
      graphData.Add("Given Name", graphDetails.GivenName);
      graphData.Add("Display Name", graphDetails.DisplayName);
      graphData.Add("Office", graphDetails.OfficeLocation);
      graphData.Add("Mobile", graphDetails.MobilePhone);
      graphData.Add("Other Phone", graphDetails.BusinessPhones.Count() > 0 ? graphDetails.BusinessPhones.First() : "");
      graphData.Add("Job Title", graphDetails.JobTitle);
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

