using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Moq;
using WpfApp1;
using Xunit;

namespace KooliProjekt.WpfApp.UnitTests
{
    public class MainWindowViewModelTests
    {
        // Helper method to create a testable ViewModel
        private MainWindowViewModel CreateViewModel()
        {
            return new MainWindowViewModel();
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
        public void NewCommand_ExecuteShouldClearSelectedItemAndObjectName()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.SelectedItem = new MyObject { Id = 1, Name = "Test Object" };
            viewModel.ObjectName = "Test Name";

            // Act
            viewModel.NewCommand.Execute(null);

            // Assert
            Assert.Null(viewModel.SelectedItem);
            Assert.Equal(string.Empty, viewModel.ObjectName);
        }

        [Fact]
        public void AddCommand_CanExecuteWhenObjectNameIsNotEmpty()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act - Empty name scenario
            viewModel.ObjectName = "";
            bool canExecuteEmpty = viewModel.AddCommand.CanExecute(null);

            // Act - With name scenario
            viewModel.ObjectName = "Test Object";
            bool canExecuteWithName = viewModel.AddCommand.CanExecute(null);

            // Assert
            Assert.False(canExecuteEmpty);
            Assert.True(canExecuteWithName);
        }

        [Fact]
        public void SaveCommand_CanExecuteWhenItemSelectedAndNameNotEmpty()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act - No selection scenario
            viewModel.SelectedItem = null;
            viewModel.ObjectName = "Test";
            bool canExecuteNoSelection = viewModel.SaveCommand.CanExecute(null);

            // Act - With selection but empty name
            viewModel.SelectedItem = new MyObject { Id = 1, Name = "" };
            viewModel.ObjectName = "";
            bool canExecuteEmptyName = viewModel.SaveCommand.CanExecute(null);

            // Act - With selection and name
            viewModel.SelectedItem = new MyObject { Id = 1, Name = "Test" };
            viewModel.ObjectName = "Test Updated";
            bool canExecuteValid = viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.False(canExecuteNoSelection);
            Assert.False(canExecuteEmptyName);
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
            viewModel.SelectedItem = new MyObject { Id = 1, Name = "Test" };
            bool canExecuteWithSelection = viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.False(canExecuteNoSelection);
            Assert.True(canExecuteWithSelection);
        }

        [Fact]
        public void SelectedItem_ChangeShouldUpdateObjectName()
        {
            // Arrange
            var viewModel = CreateViewModel();
            var testObject = new MyObject { Id = 1, Name = "Test Object" };

            // Act
            viewModel.SelectedItem = testObject;

            // Assert
            Assert.Equal("Test Object", viewModel.ObjectName);
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
            viewModel.SelectedItem = new MyObject { Id = 1, Name = "Test" };
            bool isSelectedWithObject = viewModel.IsItemSelected;

            // Assert
            Assert.False(isSelectedWhenNull);
            Assert.True(isSelectedWithObject);
        }
    }
}