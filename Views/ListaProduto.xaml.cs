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

        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var produtoSelecionado = (Produto)e.SelectedItem;
                Navigation.PushAsync(new EditarProduto { BindingContext = produtoSelecionado });
            }
        }
    }
}