using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DGTickets.Frontend.AuthenticationProviders;

public class AuthenticationProviderTest : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await Task.Delay(3000);
        var user = new ClaimsIdentity(authenticationType: "test");
        var admin = new ClaimsIdentity(new List<Claim>
    {
        new Claim("FirstName", "DG"),
        new Claim("LastName", "Tickets"),
        new Claim(ClaimTypes.Name, "dgtickets@yopmail.com"),
        new Claim(ClaimTypes.Role, "Admin")
    },
        authenticationType: "test");

        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
    }
}