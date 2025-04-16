using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Graphics;
using GestorFinanceiro.Database;

namespace GestorFinanceiro
{
    public partial class MainPage : ContentPage
    {
        HistoricoManager hm = new HistoricoManager();
        private Timer timer;
        public MainPage()
        {
            InitializeComponent();

            timer = new Timer(ExecutarVerificacao, null, 0, 10000); // Executa Verificação de email de 10 em 10 segundos

            // Somando os valores do banco
            decimal total = hm.SomarValores();

            // Atualizando o saldo na interface
            AtualizarSaldo(total);

            // Transações recentes

            var transacoes = hm.RecentesTransacoes();

            NomeTransacao3.Text = Convert.ToString(transacoes[2].Nome);
            HoraTransacao3.Text = Convert.ToString(transacoes[2].Data);
            // Valor
            if (transacoes[2].Valor > 0)
            {
                ValorTransacao3.Text = $"+ {Convert.ToString(transacoes[2].Valor)}€";
            }
            else
            {
                ValorTransacao3.Text = $"{Convert.ToString(transacoes[2].Valor)}€";
            }
            // Valor

            // Cor do Texto
            if (transacoes[2].Valor < 0)
            {
                ValorTransacao3.TextColor = Color.FromArgb("#F44336");
            }
            else
            {
                ValorTransacao3.TextColor = Color.FromArgb("#4CAF50");
            }
            // Cor do Texto
            NomeTransacao2.Text = Convert.ToString(transacoes[1].Nome);
            HoraTransacao2.Text = Convert.ToString(transacoes[1].Data);
            // Valor
            if (transacoes[1].Valor > 0)
            {
                ValorTransacao2.Text = $"+ {Convert.ToString(transacoes[1].Valor)}€";
            }
            else
            {
                ValorTransacao2.Text = $"{Convert.ToString(transacoes[1].Valor)}€";
            }
            // Valor

            // Cor do Texto
            if (transacoes[1].Valor < 0)
            {
                ValorTransacao2.TextColor = Color.FromArgb("#F44336");
            }
            else
            {
                ValorTransacao2.TextColor = Color.FromArgb("#4CAF50");
            }
            // Cor do Texto
            NomeTransacao1.Text = Convert.ToString(transacoes[0].Nome);
            HoraTransacao1.Text = Convert.ToString(transacoes[0].Data);
            // Valor
            if (transacoes[0].Valor > 0)
            {
                ValorTransacao1.Text = $"+ {Convert.ToString(transacoes[0].Valor)}€";
            }
            else
            {
                ValorTransacao1.Text = $"{Convert.ToString(transacoes[0].Valor)}€";
            }
            // Valor

            // Cor do Texto
            if (transacoes[0].Valor < 0)
            {
                ValorTransacao1.TextColor = Color.FromArgb("#F44336");
            }
            else
            {
                ValorTransacao1.TextColor = Color.FromArgb("#4CAF50");
            }
            // Cor do Texto
        }

        // Verificar email
        static void ExecutarVerificacao(object state)
        {
            try
            {
                Email email = new Email();
                email.VerificarEmail();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }
        // Verificar email

        private async void AddGanhos_Clicked(object sender, EventArgs e)
        {
            var popup = new AdicionarGanhosPopup();
            var result = await this.ShowPopupAsync(popup);
            var saldoAtual = SaldoAtual.Text;
            decimal saldoAnterior;

            if (result != null)
            {
                dynamic ganho = result;
                await DisplayAlert("Sucesso", $"Adicionado: {ganho.Nome} - {ganho.Valor:C2}", "OK");
                saldoAtual = saldoAtual.Replace("€", "");
                decimal saldo = decimal.Parse(saldoAtual);
                saldoAnterior = saldo;
                saldo += ganho.Valor;
                // Atualize sua lista/banco de dados aqui
                decimal ganhoDb = Convert.ToDecimal(ganho.Valor);
                hm.InserirHistorico(ganho.Nome, ganhoDb);
                var transacoes = hm.RecentesTransacoes();
                // Atualize sua lista/banco de dados aqui
                if (saldoAnterior == 0)
                {
                    TaxaSaldo.Text = $"N/A";
                    TaxaSaldo.TextColor = Color.FromArgb("#F44336");
                }
                else
                {
                    TaxaSaldo.Text = $"{(ganho.Valor / saldoAnterior) * 100 }%";
                }
                SaldoAtual.Text = $"{saldo}€";

                NomeTransacao3.Text =  Convert.ToString(transacoes[2].Nome);
                HoraTransacao3.Text = Convert.ToString(transacoes[2].Data);
                if (transacoes[2].Valor > 0)
                {
                    ValorTransacao3.Text = $"+ {Convert.ToString(transacoes[2].Valor)}€";
                }
                else
                {
                    ValorTransacao3.Text = $"{Convert.ToString(transacoes[2].Valor)}€";
                }

                if (transacoes[2].Valor < 0)
                {
                    ValorTransacao3.TextColor = Color.FromArgb("#F44336");
                }
                else
                {
                    ValorTransacao3.TextColor = Color.FromArgb("#4CAF50");
                }
                NomeTransacao2.Text = Convert.ToString(transacoes[1].Nome);
                HoraTransacao2.Text = Convert.ToString(transacoes[1].Data);
                ValorTransacao2.Text = $"+ {Convert.ToString(transacoes[1].Valor)}€";
                if (transacoes[1].Valor < 0)
                {
                    ValorTransacao2.TextColor = Color.FromArgb("#F44336");
                }
                else
                {
                    ValorTransacao2.TextColor = Color.FromArgb("#4CAF50");
                }
                NomeTransacao1.Text = Convert.ToString(transacoes[0].Nome);
                HoraTransacao1.Text = Convert.ToString(transacoes[0].Data);
                string valorGanho = ganho.Valor.ToString();
                ValorTransacao1.Text = $"+ {Convert.ToString(transacoes[0].Valor)}€";
                ValorTransacao1.TextColor = Color.FromArgb("#4CAF50");

       
            }
        }
        private async void AddDespesas_Clicked(object sender, EventArgs e)
        {
            var popup = new AdicionarDespesasPopup();
            var result = await this.ShowPopupAsync(popup);
            var saldoAtual = SaldoAtual.Text;
            decimal saldoAnterior;

            if (result != null)
            {
                dynamic ganho = result;
                await DisplayAlert("Sucesso", $"Adicionado: {ganho.Nome} - {ganho.Valor:C2}", "OK");
                saldoAtual = saldoAtual.Replace("€", "");
                decimal saldo = decimal.Parse(saldoAtual);
                saldoAnterior = saldo;
                saldo -= ganho.Valor;
                // Atualize sua lista/banco de dados aqui
                decimal ganhoDb = Convert.ToDecimal(ganho.Valor);
                hm.InserirHistorico(ganho.Nome, -(ganhoDb));
                var transacoes = hm.RecentesTransacoes();
                // Atualize sua lista/banco de dados aqui
                if (saldoAnterior == 0)
                {
                    TaxaSaldo.Text = $"N/A";
                    TaxaSaldo.TextColor = Color.FromArgb("#F44336");
                }
                else
                {
                    TaxaSaldo.Text = $"{(ganho.Valor / saldoAnterior) * 100}%";
                }
                SaldoAtual.Text = $"{saldo}€";

                NomeTransacao3.Text = Convert.ToString(transacoes[2].Nome);
                HoraTransacao3.Text = Convert.ToString(transacoes[2].Data);
                // Valor
                if (transacoes[2].Valor > 0)
                {
                    ValorTransacao3.Text = $"+ {Convert.ToString(transacoes[2].Valor)}€";
                }
                else
                {
                    ValorTransacao3.Text = $"{Convert.ToString(transacoes[2].Valor)}€";
                }
                // Valor

                // Cor do Texto
                if (transacoes[2].Valor < 0)
                {
                    ValorTransacao3.TextColor = Color.FromArgb("#F44336");
                }
                else
                {
                    ValorTransacao3.TextColor = Color.FromArgb("#4CAF50");
                }
                // Cor do Texto
                NomeTransacao2.Text = Convert.ToString(transacoes[1].Nome);
                HoraTransacao2.Text = Convert.ToString(transacoes[1].Data);
                // Valor
                if (transacoes[1].Valor > 0)
                {
                    ValorTransacao2.Text = $"+ {Convert.ToString(transacoes[1].Valor)}€";
                }
                else
                {
                    ValorTransacao2.Text = $"{Convert.ToString(transacoes[1].Valor)}€";
                }
                // Valor

                // Cor do Texto
                if (transacoes[1].Valor < 0)
                {
                    ValorTransacao2.TextColor = Color.FromArgb("#F44336");
                }
                else
                {
                    ValorTransacao2.TextColor = Color.FromArgb("#4CAF50");
                }
                // Cor do Texto
                NomeTransacao1.Text = Convert.ToString(transacoes[0].Nome);
                HoraTransacao1.Text = Convert.ToString(transacoes[0].Data);
                string valorGanho = ganho.Valor.ToString();
                ValorTransacao1.Text = $"- {Convert.ToString(transacoes[0].Valor)}€";
                ValorTransacao1.TextColor = Color.FromArgb("#F44336");
            }
        }


        public void AtualizarSaldo(decimal saldoAtual)
        {
            // Convertendo o valor do saldo para string e exibindo na Label
            string saldoStr = saldoAtual.ToString("C");  // "C" para formato de moeda
            SaldoAtual.Text = saldoStr;  // 'SaldoAtual' é o x:Name da Label na sua UI
        }
    }
}