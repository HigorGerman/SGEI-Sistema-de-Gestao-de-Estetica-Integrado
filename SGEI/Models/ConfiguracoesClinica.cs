namespace SGEI.Models;

public class ConfiguracaoClinica
{
    public int Id { get; set; }
    public TimeSpan HoraAbertura { get; set; } 
    public TimeSpan HoraFechamento { get; set; }
    public string DiasFuncionamento { get; set; } = "Segunda,Terça,Quarta,Quinta,Sexta"; 
}