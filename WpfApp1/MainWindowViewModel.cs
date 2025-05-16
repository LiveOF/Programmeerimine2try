using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Api;

namespace WpfApp1
{
    // Class for representing an object
    public class MyObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;
        private ObservableCollection<MyObject> _objects;
        private MyObject _selectedItem;
        private string _objectName;

        public ObservableCollection<MyObject> Objects
        {
            get => _objects;
            set => SetProperty(ref _objects, value);
        }

        public MyObject SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    // Update object name when selected item changes
                    ObjectName = value?.Name ?? string.Empty;
                    OnPropertyChanged(nameof(IsItemSelected));
                }
            }
        }

        public string ObjectName
        {
            get => _objectName;
            set => SetProperty(ref _objectName, value);
        }

        public bool IsItemSelected => SelectedItem != null;

        public ICommand LoadCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand NewCommand { get; private set; }

        // Action to handle errors
        public Action<string> OnError { get; set; }

        public MainWindowViewModel() : this(new ApiClient(new System.Net.Http.HttpClient(), "http://localhost:5000/api/objects"))
        {
        }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
            Objects = new ObservableCollection<MyObject>();

            LoadCommand = new RelayCommand(_ => Load());
            AddCommand = new RelayCommand(_ => Add(), _ => !string.IsNullOrWhiteSpace(ObjectName));
            SaveCommand = new RelayCommand(_ => Save(), _ => IsItemSelected && !string.IsNullOrWhiteSpace(ObjectName));
            DeleteCommand = new RelayCommand(_ => Delete(), _ => IsItemSelected);
            NewCommand = new RelayCommand(_ => CreateNewObject());

            // Load objects when the view model is created
            Load();
        }

        private void CreateNewObject()
        {
            // Clear the selected item and set up for a new object
            SelectedItem = null;
            ObjectName = string.Empty;
        }

        public async void Load()
        {
            var result = await _apiClient.GetObjectsAsync();

            if (result.IsSuccess)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Objects.Clear();
                    foreach (var obj in result.Value)
                    {
                        Objects.Add(obj);
                    }
                });
            }
            else
            {
                OnError?.Invoke(result.Error);
            }
        }

        private async void Add()
        {
            if (string.IsNullOrWhiteSpace(ObjectName))
                return;

            var newObject = new MyObject { Name = ObjectName };
            var result = await _apiClient.AddObjectAsync(newObject);

            if (result.IsSuccess)
            {
                Load();
                ObjectName = string.Empty;
            }
            else
            {
                OnError?.Invoke(result.Error);
            }
        }

        private async void Save()
        {
            if (SelectedItem == null || string.IsNullOrWhiteSpace(ObjectName))
                return;

            SelectedItem.Name = ObjectName;
            var result = await _apiClient.SaveObjectAsync(SelectedItem);

            if (result.IsSuccess)
            {
                Load();
            }
            else
            {
                OnError?.Invoke(result.Error);
            }
        }

        private async void Delete()
        {
            if (SelectedItem == null)
                return;

            if (SelectedItem.Id == 0)
            {
                OnError?.Invoke("Cannot delete an object with unset Id.");
                return;
            }

            var messageResult = MessageBox.Show($"Are you sure you want to delete '{SelectedItem.Name}'?",
                                        "Confirm Delete",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (messageResult == MessageBoxResult.Yes)
            {
                var deleteResult = await _apiClient.DeleteObjectAsync(SelectedItem.Id);

                if (deleteResult.IsSuccess)
                {
                    Load();
                    SelectedItem = null;
                    ObjectName = string.Empty;
                }
                else
                {
                    OnError?.Invoke(deleteResult.Error);
                }
            }
        }
    }
}