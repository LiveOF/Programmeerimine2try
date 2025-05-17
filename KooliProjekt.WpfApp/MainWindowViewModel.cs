using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;
        private ObservableCollection<Building> _buildings;
        private Building _selectedItem;
        private string _location;
        private DateTime _date;
        private string _title;

        public ObservableCollection<Building> Buildings
        {
            get => _buildings;
            set => SetProperty(ref _buildings, value);
        }

        public Building SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    // Обновить свойства при изменении выбранного элемента
                    if (value != null)
                    {
                        Location = value.Address ?? string.Empty;
                        Date = value.CreatedAt;
                        Title = value.Name ?? string.Empty;
                    }
                    else
                    {
                        Location = string.Empty;
                        Date = DateTime.Now;
                        Title = string.Empty;
                    }

                    OnPropertyChanged(nameof(IsItemSelected));
                }
            }
        }

        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsItemSelected => SelectedItem != null;

        public ICommand LoadCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand NewCommand { get; private set; }

        // Делегат для подтверждения удаления
        public Func<Building, bool> ConfirmDelete { get; set; }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
            Buildings = new ObservableCollection<Building>();
            Date = DateTime.Now;

            // Явно делаем асинхронный вызов
            LoadCommand = new RelayCommand<object>(async _ => await Load());
            NewCommand = new RelayCommand<object>(_ => CreateNewBuilding());
            SaveCommand = new RelayCommand<object>(async _ => await SaveAsync(), _ => !string.IsNullOrWhiteSpace(Location));
            DeleteCommand = new RelayCommand<object>(async _ => await DeleteAsync(), _ => IsItemSelected);
        }

        private void CreateNewBuilding()
        {
            // Создать новое здание
            SelectedItem = null;
            Location = string.Empty;
            Title = string.Empty;
            Date = DateTime.Now;
        }

        public async Task Load()
        {
            try
            {
                var buildings = await _apiClient.List();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Buildings.Clear();
                    foreach (var building in buildings)
                    {
                        Buildings.Add(building);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки зданий: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                var building = SelectedItem ?? new Building();
                building.Address = Location;
                building.CreatedAt = Date;
                building.Name = Title;
                building.UserId = "1";

                var result = await _apiClient.Save(building);

                if (result != null && !result.HasError)
                {
                    await Load();
                }
                else
                {
                    string errorMessage = result?.Error ?? "Неизвестная ошибка";
                    MessageBox.Show($"Ошибка сохранения: {errorMessage}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения здания: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedItem == null)
                return;

            if (ConfirmDelete == null || ConfirmDelete(SelectedItem))
            {
                try
                {
                    var result = await _apiClient.Delete(SelectedItem.Id);

                    if (result != null && !result.HasError)
                    {
                        await Load();
                        SelectedItem = null;
                    }
                    else
                    {
                        string errorMessage = result?.Error ?? "Неизвестная ошибка";
                        MessageBox.Show($"Ошибка удаления: {errorMessage}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления здания: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
