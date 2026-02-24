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
                // Puxa o produto que foi passado para a tela
                Produto p = (Produto)BindingContext;

                // Atualiza os valores com o que está digitado nos campos
                p.Descricao = txt_descricao.Text;
                p.Quantidade = Convert.ToDouble(txt_quantidade.Text);
                p.Preco = Convert.ToDouble(txt_preco.Text);

                // Manda para o banco de dados atualizar
                await App.Db.Update(p);

                await DisplayAlert("Sucesso", "Produto atualizado com sucesso!", "OK");
                await Navigation.PopAsync(); // Volta para a tela anterior
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
                // Pede uma confirmação antes de apagar
                bool confirma = await DisplayAlert("Confirmar", "Tem certeza que deseja excluir este produto?", "Sim", "Não");

                if (confirma)
                {
                    Produto p = (Produto)BindingContext;

                    // Manda para o banco de dados excluir usando o ID
                    await App.Db.Delete(p.Id);

                    await DisplayAlert("Sucesso", "Produto excluído!", "OK");
                    await Navigation.PopAsync(); // Volta para a lista
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}