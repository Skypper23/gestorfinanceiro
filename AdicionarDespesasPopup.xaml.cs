using CommunityToolkit.Maui.Views;

namespace GestorFinanceiro;

public partial class AdicionarDespesasPopup : Popup
{
    public AdicionarDespesasPopup()
    {
        InitializeComponent();
        DataPickerDespesas.Date = DateTime.Now;
    }

    private void OnAdicionarClicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(DespesasEntry.Text, out decimal valor) && !string.IsNullOrWhiteSpace(NomeDespesasEntry.Text))
        {
            var ganho = new
            {
                Valor = valor,
                Nome = NomeDespesasEntry.Text,
                Data = DataPickerDespesas.Date.ToString("dd/MM/yyyy")
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