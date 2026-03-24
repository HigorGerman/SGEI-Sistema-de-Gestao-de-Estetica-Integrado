namespace SGEI.Models;

public class Anamnese
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    
    public bool PossuiAlergia { get; set; }
    public string? AlergiasDescricao { get; set; }
    public bool EstaGestante { get; set; }
    public string? HistoricoMedico { get; set; }
    public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;
}