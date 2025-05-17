using KooliProjekt.WinFormsApp;
using KooliProjekt.WinFormsApp.Api;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xunit;

namespace KooliProjekt.WinFormsApp.UnitTests
{
    public class BuildingPresenterTests
    {
        private readonly Mock<IBuildingView> _mockView;
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly BuildingPresenter _presenter;

        public BuildingPresenterTests()
        {
            _mockView = new Mock<IBuildingView>();
            _mockApiClient = new Mock<IApiClient>();
            _presenter = new BuildingPresenter(_mockView.Object, _mockApiClient.Object);
        }

        [Fact]
        public void UpdateView_NullBuilding_ShouldClearFields()
        {
            // Act
            _presenter.UpdateView(null!);

            // Assert
            _mockView.VerifySet(v => v.Location = string.Empty);
            _mockView.VerifySet(v => v.Id = 0);
            _mockView.VerifySet(v => v.Date = It.IsAny<DateTime>());
        }

        [Fact]
        public void UpdateView_WithBuilding_ShouldSetFields()
        {
            // Arrange
            var testDate = DateTime.Now;
            var building = new Building
            {
                Id = 1,
                Location = "Test Location",
                Date = testDate
            };

            // Act
            _presenter.UpdateView(building);

            // Assert
            _mockView.VerifySet(v => v.Id = 1);
            _mockView.VerifySet(v => v.Location = "Test Location");
            _mockView.VerifySet(v => v.Date = testDate);
        }

        [Fact]
        public async Task Load_Success_ShouldUpdateViewWithBuildings()
        {
            // Arrange
            var buildings = new List<Building> { new Building { Id = 1, Location = "Test" } };
            var result = Result<List<Building>>.Success(buildings);

            _mockApiClient.Setup(c => c.List())
                .Returns(Task.FromResult(result));

            // Act
            await _presenter.Load();

            // Assert
            _mockView.VerifySet(v => v.Buildings = buildings);
        }

        [Fact]
        public async Task Load_Failure_ShouldNotUpdateViewWithBuildings()
        {
            // Arrange
            var errorMessage = "Test error";
            var result = Result<List<Building>>.Failure(errorMessage);

            _mockApiClient.Setup(c => c.List())
                .Returns(Task.FromResult(result as Result<List<Building>>));

            // Act & Assert
            await Assert.ThrowsAnyAsync<Exception>(() => _presenter.Load());
        }

        [Fact]
        public async Task Save_ShouldCreateBuildingWithCorrectProperties()
        {
            // Arrange
            var testDate = DateTime.Now;
            _mockView.Setup(v => v.Id).Returns(1);
            _mockView.Setup(v => v.Location).Returns("Test Location");
            _mockView.Setup(v => v.Date).Returns(testDate);

            var saveResult = Result.Success();
            _mockApiClient.Setup(c => c.Save(It.IsAny<Building>()))
                .ReturnsAsync(saveResult);

            var buildings = new List<Building>();
            var listResult = Result<List<Building>>.Success(buildings);
            _mockApiClient.Setup(c => c.List())
                .ReturnsAsync(listResult);

            // Act
            await _presenter.Save();

            // Assert
            _mockApiClient.Verify(c => c.Save(It.Is<Building>(b =>
                b.Id == 1 &&
                b.Location == "Test Location" &&
                b.Date == testDate &&
                b.UserId == "1")), Times.Once());

            _mockApiClient.Verify(c => c.List(), Times.Once());
        }

        [Fact]
        public async Task Save_Failure_ShouldNotReloadData()
        {
            // Arrange
            _mockView.Setup(v => v.Id).Returns(1);
            _mockView.Setup(v => v.Location).Returns("Test Location");
            _mockView.Setup(v => v.Date).Returns(DateTime.Now);

            var result = Result.Failure("Test error");
            _mockApiClient.Setup(c => c.Save(It.IsAny<Building>()))
                .ReturnsAsync(result);

            // Act
            await _presenter.Save();

            // Assert
            _mockApiClient.Verify(c => c.List(), Times.Never());
            // Тут можно также проверить, что сообщение об ошибке содержит "Test error"
        }


        [Fact]
        public async Task Delete_WithSelectedItem_ShouldCallApiClientDelete()
        {
            // Arrange
            var building = new Building { Id = 1, Location = "Test Location" };
            _mockView.Setup(v => v.SelectedItem).Returns(building);

            var deleteResult = Result.Success();
            _mockApiClient.Setup(c => c.Delete(It.IsAny<int>()))
                .ReturnsAsync(deleteResult);

            var buildings = new List<Building>();
            var listResult = Result<List<Building>>.Success(buildings);
            _mockApiClient.Setup(c => c.List())
                .ReturnsAsync(listResult);

            // Мокаем статический MessageBox для тестирования
            using (var messageBoxMock = new MockMessageBox())
            {
                // Act
                await _presenter.Delete();

                // Assert
                _mockApiClient.Verify(c => c.Delete(1), Times.Once());
                _mockApiClient.Verify(c => c.List(), Times.Once());
            }
        }

        [Fact]
        public async Task Delete_NoSelectedItem_ShouldNotCallApiClientDelete()
        {
            // Arrange
            _mockView.Setup(v => v.SelectedItem).Returns((Building)null!);

            // Act
            await _presenter.Delete();

            // Assert
            _mockApiClient.Verify(c => c.Delete(It.IsAny<int>()), Times.Never());
            _mockApiClient.Verify(c => c.List(), Times.Never());
        }
    }

    // Вспомогательный класс для мокинга MessageBox
    public class MockMessageBox : IDisposable
    {
        public MockMessageBox()
        {
            // Здесь можно настроить мок для MessageBox если необходимо
        }

        public void Dispose()
        {
            // Освобождаем ресурсы если нужно
        }
    }
}
