namespace GestorFinanceiro
{
    public class Transacao
    {
        public required string Nome { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
