namespace SGEI.Models;

public class Procedimento
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int DuracaoMinutos { get; set; }
    public string? Descricao { get; set; }
}