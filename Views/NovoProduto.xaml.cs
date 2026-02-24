using agenda02.Models;

namespace agenda02.Views
{
    public partial class NovoProduto : ContentPage
    {
        public NovoProduto()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                Produto p = new Produto
                {
                    Descricao = txt_descricao.Text,
                    Quantidade = Convert.ToDouble(txt_quantidade.Text),
                    Preco = Convert.ToDouble(txt_preco.Text)
                };

                await App.Db.Insert(p);
                await DisplayAlert("Sucesso", "Produto inserido com sucesso!", "OK");
                await Navigation.PopAsync(); // Volta para a lista
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}