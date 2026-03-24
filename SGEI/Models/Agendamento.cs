namespace SGEI.Models;


public class Agendamento
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; } 
    
    public int ProcedimentoId { get; set; }
    public Procedimento? Procedimento { get; set; } 
    
    public DateTime DataHora { get; set; }
    public string? Observacoes { get; set; }
    public string Status { get; set; } = "Pendente"; 
}
