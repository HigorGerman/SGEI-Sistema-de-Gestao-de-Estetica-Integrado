namespace SGEI.Models;

public class Pagamento
{
    public int Id { get; set; }
    public int AgendamentoId { get; set; }
    public Agendamento? Agendamento { get; set; }
    
    public decimal ValorPago { get; set; }
    public string MetodoPagamento { get; set; } = "Pix"; 
    public DateTime DataPagamento { get; set; } = DateTime.Now;
}