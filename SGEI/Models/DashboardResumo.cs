namespace SGEI.Models;

public class DashboardResumo
{
    public int AgendamentosHoje { get; set; }
    public decimal FaturamentoHoje { get; set; }
    public int AgendamentosMes { get; set; }
    public decimal FaturamentoMes { get; set; }
    public List<Agendamento> ProximosAtendimentos { get; set; }
}