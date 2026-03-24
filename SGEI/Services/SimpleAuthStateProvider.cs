using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using SGEI.Models;

namespace SGEI.Services;

public class SimpleAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    private ClaimsPrincipal _currentUser;

    public SimpleAuthStateProvider()
    {
        _currentUser = _anonymous;
    }

    public void MarcarComoLogado(Cliente cliente)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, cliente.Nome),
            new Claim(ClaimTypes.Email, cliente.Email),
            new Claim(ClaimTypes.Role, cliente.Role ?? "User"),
            new Claim("Id", cliente.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "CustomAuth");
        _currentUser = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public void MarcarComoDeslogado()
    {
        _currentUser = _anonymous;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentUser));
    }
}