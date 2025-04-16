using CommunityToolkit.Maui.Views;

namespace GestorFinanceiro;

public partial class AdicionarGanhosPopup : Popup
{
    public AdicionarGanhosPopup()
    {
        InitializeComponent();
        DataPickerGanhos.Date = DateTime.Now;
    }

    private void OnAdicionarClicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(GanhosEntry.Text, out decimal valor) && !string.IsNullOrWhiteSpace(NomeGanhosEntry.Text))
        {
            var ganho = new
            {
                Valor = valor,
                Nome = NomeGanhosEntry.Text,
                Data = DataPickerGanhos.Date.ToString("dd/MM/yyyy")
            };
            Close(ganho);
        }
        else
        {
            Close(null);
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        Close(null);
    }
}