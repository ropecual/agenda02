using agenda02.Models;

namespace agenda02.Views
{
    public partial class EditarProduto : ContentPage
    {
        public EditarProduto()
        {
            InitializeComponent();
        }

        private async void btnAtualizar_Clicked(object sender, EventArgs e)
        {
            try
            {
                Produto p = (Produto)BindingContext;

                p.Descricao = txt_descricao.Text;
                p.Quantidade = Convert.ToDouble(txt_quantidade.Text);
                p.Preco = Convert.ToDouble(txt_preco.Text);
                p.Categoria = pck_categoria.SelectedItem?.ToString() ?? "Outros";

                await App.Db.Update(p);

                await DisplayAlert("Sucesso", "Produto atualizado com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void btnExcluir_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool confirma = await DisplayAlert("Confirmar", "Tem certeza que deseja excluir este produto?", "Sim", "Não");

                if (confirma)
                {
                    Produto p = (Produto)BindingContext;
                    await App.Db.Delete(p.Id);

                    await DisplayAlert("Sucesso", "Produto excluído!", "OK");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}