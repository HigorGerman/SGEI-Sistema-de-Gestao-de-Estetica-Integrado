using Microsoft.EntityFrameworkCore;
using SGEI.Data;
using SGEI.Models;

namespace SGEI.Services;

public class ConfiguracaoService
{
    private readonly AppDbContext _context;

    public ConfiguracaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ConfiguracaoClinica> ObterConfiguracaoAsync()
    {
        // Certifique-se de que 'ConfiguracoesClinica' está no seu AppDbContext
        var config = await _context.ConfiguracoesClinica.FirstOrDefaultAsync();
        return config ?? new ConfiguracaoClinica();
    }

    public async Task SalvarConfiguracaoAsync(ConfiguracaoClinica config)
    {
        var existente = await _context.ConfiguracoesClinica.FirstOrDefaultAsync();
        if (existente == null) {
            _context.ConfiguracoesClinica.Add(config);
        } else {
            // Nomes corrigidos conforme seu print
            existente.HoraAbertura = config.HoraAbertura; 
            existente.HoraFechamento = config.HoraFechamento;
            existente.DiasFuncionamento = config.DiasFuncionamento;
        }
        await _context.SaveChangesAsync();
    }
}