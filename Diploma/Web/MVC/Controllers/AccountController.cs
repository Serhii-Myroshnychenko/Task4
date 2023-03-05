using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IIdentityParser<ApplicationUser> _identityParser;

    public AccountController(
        ILogger<AccountController> logger,
        IIdentityParser<ApplicationUser> identityParser)
    {
        _logger = logger;
        _identityParser = identityParser;
    }

    public IActionResult SignIn()
    {
        var user = _identityParser.Parse(User);
        var id = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        // "Catalog" because UrlHelper doesn't support nameof() for controllers
        // https://github.com/aspnet/Mvc/issues/5853
        return RedirectToAction(nameof(CatalogController.Index), "Catalog");
    }

    public async Task<IActionResult> Signout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        // "Catalog" because UrlHelper doesn't support nameof() for controllers
        // https://github.com/aspnet/Mvc/issues/5853
        var homeUrl = Url.Action(nameof(CatalogController.Index), "Catalog");
        return new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = homeUrl });
    }
}
