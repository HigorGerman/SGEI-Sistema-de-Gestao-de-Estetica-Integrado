using Microsoft.EntityFrameworkCore;
using SGEI.Data;
using SGEI.Models;

namespace SGEI.Services;

public class ProcedimentoService
{
    private readonly AppDbContext _context;

    public ProcedimentoService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca todos os serviços cadastrados no banco.
    /// </summary>
    public async Task<List<Procedimento>> ListarTodosAsync()
    {
        return await _context.Procedimentos.ToListAsync();
    }

    /// <summary>
    /// Salva um novo procedimento ou atualiza um existente de forma segura.
    /// Resolve o erro de conflito de rastreamento (tracking) do Entity Framework.
    /// </summary>
    public async Task<bool> SalvarProcedimentoAsync(Procedimento procedimento)
    {
        try
        {
            if (procedimento.Id == 0)
            {
                // Se o ID é 0, adicionamos um novo registro
                _context.Procedimentos.Add(procedimento);
            }
            else
            {
                // Se o ID já existe, buscamos a instância oficial que o EF está rastreando
                var existente = await _context.Procedimentos.FindAsync(procedimento.Id);
                
                if (existente != null)
                {
                    // Copia os valores que vieram da tela (procedimento) 
                    // para a entidade oficial do banco (existente)
                    _context.Entry(existente).CurrentValues.SetValues(procedimento);
                }
                else
                {
                    // Caso a entidade não esteja na memória, o Update resolve
                    _context.Procedimentos.Update(procedimento);
                }
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Log para você visualizar o erro exato no terminal do Rider
            Console.WriteLine($"Erro ao salvar procedimento: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Remove um serviço pelo ID.
    /// </summary>
    public async Task<bool> ExcluirProcedimentoAsync(int id)
    {
        try
        {
            var proc = await _context.Procedimentos.FindAsync(id);
            if (proc == null) return false;

            _context.Procedimentos.Remove(proc);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao excluir procedimento: {ex.Message}");
            return false;
        }
    }
}