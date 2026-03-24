using Microsoft.EntityFrameworkCore;
using SGEI.Data;
using SGEI.Models;

namespace SGEI.Services;

public class AgendamentoService
{
    private readonly AppDbContext _context;

    public AgendamentoService(AppDbContext context)
    {
        _context = context;
    }

   
    private string TraduzirDia(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => "Segunda",
        DayOfWeek.Tuesday => "Terça",
        DayOfWeek.Wednesday => "Quarta",
        DayOfWeek.Thursday => "Quinta",
        DayOfWeek.Friday => "Sexta",
        DayOfWeek.Saturday => "Sábado",
        DayOfWeek.Sunday => "Domingo",
        _ => ""
    };

    public async Task<string> CriarAgendamentoAsync(Agendamento agendamento)
    {
        try
        {
            // 1. Buscar Configurações da Clínica
            var config = await _context.ConfiguracoesClinica.FirstOrDefaultAsync() 
                         ?? new ConfiguracaoClinica();

            // 2. Validar Dia da Semana (Agora traduzido para o português!)
            var diaEmPortugues = TraduzirDia(agendamento.DataHora.DayOfWeek);
            if (!config.DiasFuncionamento.Contains(diaEmPortugues))
            {
                return $"A clínica não abre neste dia. Dias de funcionamento: {config.DiasFuncionamento}.";
            }

            // 3. Validar Horário de Funcionamento
            var horaPura = agendamento.DataHora.TimeOfDay;
            if (horaPura < config.HoraAbertura || horaPura > config.HoraFechamento)
            {
                return $"Horário fora do expediente. Atendemos das {config.HoraAbertura:hh\\:mm} às {config.HoraFechamento:hh\\:mm}.";
            }

            // 4. Validar Colisão (Sobreposição)
            var disponivel = await VerificarColisaoAsync(agendamento.DataHora, agendamento.ProcedimentoId);
            if (!disponivel)
            {
                return "Este horário já está ocupado ou conflita com outro agendamento.";
            }

            // 5. Salvar se tudo estiver OK
            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();
            return "Sucesso";
        }
        catch (Exception ex)
        {
            return $"Erro no sistema: {ex.Message}";
        }
    }

    private async Task<bool> VerificarColisaoAsync(DateTime inicioDesejado, int procedimentoId)
    {
        var procedimento = await _context.Procedimentos.FindAsync(procedimentoId);
        if (procedimento == null) return false;

        var fimDesejado = inicioDesejado.AddMinutes(procedimento.DuracaoMinutos);

        // Busca apenas agendamentos ativos do mesmo dia para comparar
        var agendamentosDoDia = await _context.Agendamentos
            .Include(a => a.Procedimento)
            .Where(a => a.DataHora.Date == inicioDesejado.Date && a.Status != "Cancelado")
            .ToListAsync();

        foreach (var existente in agendamentosDoDia)
        {
            var inicioExistente = existente.DataHora;
            var fimExistente = existente.DataHora.AddMinutes(existente.Procedimento!.DuracaoMinutos);

            // Lógica de intersecção de intervalos
            if (inicioDesejado < fimExistente && inicioExistente < fimDesejado)
            {
                return false; // Houve colisão
            }
        }
        return true; // Horário livre
    }

    public async Task<List<Agendamento>> ListarTodosAdminAsync()
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Procedimento)
            .OrderByDescending(a => a.DataHora)
            .ToListAsync();
    }

    public async Task<List<Agendamento>> ListarPorClienteAsync(int clienteId)
    {
        return await _context.Agendamentos
            .Include(a => a.Procedimento)
            .Where(a => a.ClienteId == clienteId)
            .OrderBy(a => a.DataHora)
            .ToListAsync();
    }
    
    public async Task<List<Agendamento>> ObterAgendamentosDoDiaAsync(DateTime data)
    {
        return await _context.Agendamentos
            .Include(a => a.Procedimento)
            .Where(a => a.DataHora.Date == data.Date && a.Status != "Cancelado")
            .ToListAsync();
    }
    
    public async Task<bool> CancelarAgendamentoAsync(int agendamentoId, int clienteId)
    {
        var agendamento = await _context.Agendamentos
            .FirstOrDefaultAsync(a => a.Id == agendamentoId && a.ClienteId == clienteId);

        if (agendamento == null) return false;
        agendamento.Status = "Cancelado";
        await _context.SaveChangesAsync();
    
        return true;
    }
    
    public async Task<bool> AtualizarStatusAsync(int agendamentoId, string novoStatus)
    {
        var agendamento = await _context.Agendamentos.FindAsync(agendamentoId);
        if (agendamento == null) return false;

        agendamento.Status = novoStatus;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Agendamento>> ObterTodosAgendamentosAsync()
    {
        var agendamentos = await _context.Agendamentos.ToListAsync();
        var agora = DateTime.Now;
        bool houveMudanca = false;

        foreach (var agendamento in agendamentos.Where(a => a.Status == "Pendente"))
        {
            // Se a data/hora do agendamento já passou de agora
            if (agendamento.DataHora < agora)
            {
                agendamento.Status = "Concluído";
                houveMudanca = true;
            }
        }

        if (houveMudanca) await _context.SaveChangesAsync();

        return agendamentos;
    }
    
    // Adicione este método na AgendamentoService.cs
    public async Task<DashboardResumo> ObterResumoDashboardAsync()
    {
        var hoje = DateTime.Now.Date;
    
        // Puxa tudo que não está cancelado
        var agendamentosAtivos = await _context.Agendamentos
            .Include(a => a.Procedimento)
            .Include(a => a.Cliente)
            .Where(a => a.Status != "Cancelado")
            .ToListAsync();

        var ativosHoje = agendamentosAtivos.Where(a => a.DataHora.Date == hoje).ToList();
        var ativosMes = agendamentosAtivos.Where(a => a.DataHora.Month == hoje.Month && a.DataHora.Year == hoje.Year).ToList();

        return new DashboardResumo
        {
            AgendamentosHoje = ativosHoje.Count,
            FaturamentoHoje = ativosHoje.Sum(a => a.Procedimento?.Preco ?? 0),
            AgendamentosMes = ativosMes.Count,
            FaturamentoMes = ativosMes.Sum(a => a.Procedimento?.Preco ?? 0),
            ProximosAtendimentos = ativosHoje
                .Where(a => a.DataHora.TimeOfDay >= DateTime.Now.TimeOfDay && a.Status != "Concluído")
                .OrderBy(a => a.DataHora)
                .Take(5)
                .ToList()
        };
    }
}