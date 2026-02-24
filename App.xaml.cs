using agenda02.Helpers;


namespace agenda02
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;

        // Cria uma conexão global para o banco de dados que todas as telas podem usar
        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    // Define onde o arquivo do banco vai ficar salvo no celular
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabaseHelper(path);
                }
                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            // Define a tela inicial do aplicativo (com uma barra de navegação no topo)
            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}