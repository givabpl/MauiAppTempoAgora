// App.xaml.cs: Inicializa o banco de dados SQLite e configura a cultura da aplicação.

namespace MauiAppTempoAgora
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
