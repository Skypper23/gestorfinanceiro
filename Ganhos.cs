public class Ganho
{
    public required string Nome { get; set; }
    public required decimal Ganhos { get; set; }
    public required DateTime Data { get; set; }
    public required bool IsGanho { get; set; } // true para ganhos, false para despesas
}