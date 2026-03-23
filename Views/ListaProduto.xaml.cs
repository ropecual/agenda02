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

        private void AtualizarTotal()
        {
            double total = lista.Sum(p => p.Preco * p.Quantidade);
            lbl_total.Text = $"Total Gasto: R$ {total:F2}";
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
                AtualizarTotal();
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

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string busca = e.NewTextValue;
            var produtos = await App.Db.Search(busca);
            lista.Clear();
            foreach (var p in produtos)
            {
                lista.Add(p);
            }
            AtualizarTotal();
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
                MenuItem selecionado = (MenuItem)sender;
                Produto p = selecionado.BindingContext as Produto;

                bool confirma = await DisplayAlert("Tem Certeza?", $"Deseja remover {p.Descricao}?", "Sim", "Não");

                if (confirma)
                {
                    await App.Db.Delete(p.Id);
                    await DisplayAlert("Sucesso!", "Produto excluído", "OK");

                    var produtos = await App.Db.GetAll();
                    lista.Clear();
                    foreach (var item in produtos)
                    {
                        lista.Add(item);
                    }
                    AtualizarTotal();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void pck_filtro_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string categoriaSelecionada = pck_filtro_categoria.SelectedItem?.ToString();
            var todosProdutos = await App.Db.GetAll();

            lista.Clear();

            var produtosFiltrados = todosProdutos;
            if (categoriaSelecionada != "Todas" && !string.IsNullOrEmpty(categoriaSelecionada))
            {
                produtosFiltrados = todosProdutos.Where(p => p.Categoria == categoriaSelecionada).ToList();
            }

            foreach (var p in produtosFiltrados)
            {
                lista.Add(p);
            }

            AtualizarTotal();
        }
    }
}