using agenda02.Models;
using System.Collections.ObjectModel;

namespace agenda02.Views
{
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

        public ListaProduto()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var produtos = await App.Db.GetAll();
                lista.Clear();
                foreach (var p in produtos)
                {
                    lista.Add(p);
                }
                lst_produtos.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NovoProduto());
        }
        // Método anterior, aqui apertavamos o botão de busca e ele fazia a busca, agora vamos fazer a busca enquanto o usuário digita, usando o evento TextChanged
        private async void txt_search_SearchButtonPressed(object sender, EventArgs e)
        {
            var busca = txt_search.Text;
            var produtos = await App.Db.Search(busca);
            lista.Clear();
            foreach (var p in produtos)
            {
                lista.Add(p);
            }
        }

        // Método novo, toda vez que o texto mudar, ele vai fazer a busca, sem precisar apertar o botão de busca
        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Pega o texto novo a cada letra digitada
            string busca = e.NewTextValue;

            // Chama a busca no banco (invoca a busca já feita no SQLiteDatabaseHelper)
            var produtos = await App.Db.Search(busca);

            // Atualiza a lista na tela
            lista.Clear();
            foreach (var p in produtos)
            {
                lista.Add(p);
            }
        }

        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var produtoSelecionado = (Produto)e.SelectedItem;
                Navigation.PushAsync(new EditarProduto { BindingContext = produtoSelecionado });
            }
        }


        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Botão clicado
                MenuItem selecionado = (MenuItem)sender;

                // Descobre qual produto está ligado a este botão
                Produto p = selecionado.BindingContext as Produto;

                // Pergunta de confirmação (DisplayAlert)
                bool confirma = await DisplayAlert("Tem Certeza?", $"Deseja remover {p.Descricao}?", "Sim", "Não");

                if (confirma)
                {
                    // Exclui do banco
                    await App.Db.Delete(p.Id);
                    await DisplayAlert("Sucesso!", "Produto excluído", "OK");

                    // Recarrega a lista para a interface atualizar
                    var produtos = await App.Db.GetAll();
                    lista.Clear();
                    foreach (var item in produtos)
                    {
                        lista.Add(item);
                    }
                }
            }
            catch (Exception ex) // Tratamento de Erro
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }


}