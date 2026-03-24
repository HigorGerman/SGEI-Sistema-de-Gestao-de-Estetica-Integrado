using Microsoft.EntityFrameworkCore;
using SGEI.Data;
using SGEI.Models;

namespace SGEI.Services;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente?> FazerLoginAsync(string email, string senhaDigitada)
    {
        // 1. Busca o usuário apenas pelo E-mail e se está Ativo
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email == email && c.Ativo);

        // 2. Verifica se achou o cliente e se a senha bate com o Hash maluco do banco
        // Usando o caminho completo para o Rider não se perder
        if (cliente != null && BCrypt.Net.BCrypt.Verify(senhaDigitada, cliente.Senha))
        {
            return cliente; // Senha correta!
        }

        return null; // Senha errada ou usuário não existe
    }

    public async Task<string> CadastrarClienteAsync(Cliente novoCliente)
    {
        var existe = await _context.Clientes.AnyAsync(c => c.Email == novoCliente.Email);
        if (existe) return "Este e-mail já está cadastrado.";

        novoCliente.Role = "User";
        novoCliente.Ativo = true;
        novoCliente.DataCadastro = DateTime.Now;

        // Criptografando a senha com Hash antes de salvar no banco
        // Usando o caminho completo para o Rider não se perder
        novoCliente.Senha = BCrypt.Net.BCrypt.HashPassword(novoCliente.Senha);

        _context.Clientes.Add(novoCliente);
        await _context.SaveChangesAsync();
        
        return "Sucesso";
    }
}