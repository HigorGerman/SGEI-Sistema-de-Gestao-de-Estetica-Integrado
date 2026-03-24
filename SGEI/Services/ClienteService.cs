using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SGEI.Data;
using SGEI.Models;
using BCrypt.Net; // Adicione este using

namespace SGEI.Services;

public class ClienteService
{
    private readonly AppDbContext _context;

    public ClienteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegistrarClienteAsync(Cliente cliente)
    {
        var existe = await _context.Clientes.AnyAsync(c => c.Email == cliente.Email);
        if (existe) return false;

        // Hash 
        cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Cliente?> ValidarLoginAsync(string email, string senhaFornecida)
    {
        var usuario = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email == email);

        if (usuario == null) return null;

        // VERIFICA O HASH: Compara a senha digitada com o hash do banco
        bool senhaValida = BCrypt.Net.BCrypt.Verify(senhaFornecida, usuario.Senha);

        return senhaValida ? usuario : null;
    }
    
}