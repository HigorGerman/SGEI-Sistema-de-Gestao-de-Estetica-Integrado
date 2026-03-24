using SGEI.Models;

namespace SGEI.Services;

public class UserSession
{
    public Cliente? UsuarioLogado { get; set; }
    
    public void Login(Cliente cliente) => UsuarioLogado = cliente;
    
    public void Logout() => UsuarioLogado = null;
    
    public bool EstaLogado => UsuarioLogado != null;
}