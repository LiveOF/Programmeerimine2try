using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using KooliProjekt.WpfApp;
using KooliProjekt.WpfApp.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WpfApp.UnitTests
{
    public class MainWindowViewModelTests
    {
        // Helper method to create a testable ViewModel
        private MainWindowViewModel CreateViewModel()
        {
            var mockApiClient = new Mock<IApiClient>();
            return new MainWindowViewModel(mockApiClient.Object);
        }

        [Fact]
        public void NewCommand_CanExecuteAlways()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act
            bool canExecute = viewModel.NewCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void NewCommand_ExecuteShouldClearSelectedItemAndTitle()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedItem = new Building { Id = 1, Name = "Test Building" };
            viewModel.Title = "Test Title";

            // Act
            viewModel.NewCommand.Execute(null);

            // Assert
            Assert.Null(viewModel.SelectedItem);
            Assert.Equal(string.Empty, viewModel.Title);
        }

        // Заменяем тест AddCommand на проверку SaveCommand
        [Fact]
        public void SaveCommand_CanExecuteWhenLocationIsNotEmpty()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act - Empty location scenario
            viewModel.Location = "";
            bool canExecuteEmpty = viewModel.SaveCommand.CanExecute(null);

            // Act - With location scenario
            viewModel.Location = "Test Address";
            bool canExecuteWithLocation = viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.False(canExecuteEmpty);
            Assert.True(canExecuteWithLocation);
        }

        // Исправляем тест для соответствия текущей реализации
        [Fact]
        public void SaveCommand_CanExecuteWhenLocationIsNotEmpty_RegardlessOfSelection()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act - No selection but with location
            viewModel.SelectedItem = null;
            viewModel.Location = "Test Address";
            bool canExecuteNoSelection = viewModel.SaveCommand.CanExecute(null);

            // Act - With selection but empty location
            viewModel.SelectedItem = new Building { Id = 1, Name = "Test" };
            viewModel.Location = "";
            bool canExecuteEmptyLocation = viewModel.SaveCommand.CanExecute(null);

            // Act - With selection and location
            viewModel.SelectedItem = new Building { Id = 1, Name = "Test" };
            viewModel.Location = "Test Updated";
            bool canExecuteValid = viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.True(canExecuteNoSelection);
            Assert.False(canExecuteEmptyLocation);
            Assert.True(canExecuteValid);
        }

        [Fact]
        public void DeleteCommand_CanExecuteWhenItemSelected()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act - No selection scenario
            viewModel.SelectedItem = null;
            bool canExecuteNoSelection = viewModel.DeleteCommand.CanExecute(null);

            // Act - With selection scenario
            viewModel.SelectedItem = new Building { Id = 1, Name = "Test" };
            bool canExecuteWithSelection = viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.False(canExecuteNoSelection);
            Assert.True(canExecuteWithSelection);
        }

        [Fact]
        public void SelectedItem_ChangeShouldUpdateTitle()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var testObject = new Building { Id = 1, Name = "Test Building" };

            // Act
            viewModel.SelectedItem = testObject;

            // Assert
            Assert.Equal("Test Building", viewModel.Title);
        }

        [Fact]
        public void IsItemSelected_ShouldReflectSelectedItemStatus()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act - No selection
            viewModel.SelectedItem = null;
            bool isSelectedWhenNull = viewModel.IsItemSelected;

            // Act - With selection
            viewModel.SelectedItem = new Building { Id = 1, Name = "Test" };
            bool isSelectedWithObject = viewModel.IsItemSelected;

            // Assert
            Assert.False(isSelectedWhenNull);
            Assert.True(isSelectedWithObject);
        }
    }
}
