using KooliProjekt.WpfApp.Api;
using System.Windows;

namespace KooliProjekt.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Provide the required "baseUrl" argument to the ApiClient constructor
            var apiClient = new ApiClient("https://localhost:7136");
            var viewModel = new MainWindowViewModel(apiClient);

            viewModel.ConfirmDelete = list =>
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete selected list?",
                    "Delete list",
                    MessageBoxButton.YesNo
                );

                return result == MessageBoxResult.Yes;
            };

            DataContext = viewModel;

            await viewModel.Load();
        }
    }
}
