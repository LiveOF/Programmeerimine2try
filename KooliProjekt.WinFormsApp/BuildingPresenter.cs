using KooliProjekt.WinFormsApp.Api;

namespace KooliProjekt.WinFormsApp
{
    public class BuildingPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IBuildingView _buildingView;

        public BuildingPresenter(IBuildingView buildingView, IApiClient apiClient)
        {
            _apiClient = apiClient;
            _buildingView = buildingView;

            buildingView.Presenter = this;
        }

        public void UpdateView(Building building)
        {
            if (building == null)
            {
                _buildingView.Location = string.Empty;
                _buildingView.Id = 0;
                _buildingView.Date = DateTime.Now;
            }
            else
            {
                _buildingView.Id = building.Id;
                _buildingView.Location = building.Location;
                _buildingView.Date = building.Date;
            }
        }

        public async Task Delete()
        {
            if (_buildingView.SelectedItem == null)
            {
                MessageBox.Show("Palun vali hoone, mida kustutada!", "Hoiatus", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var building = _buildingView.SelectedItem;

            var result = MessageBox.Show($"Kas oled kindel, et soovid hoone '{building.Location}' kustutada?",
                                        "Kustutamise kinnitus", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var deleteResult = await _apiClient.Delete(building.Id);
                if (deleteResult.IsSuccess)
                {
                    MessageBox.Show("Hoone kustutatud!", "Teade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Lae andmed uuesti
                    await Load();
                }
                else
                {
                    MessageBox.Show($"Kustutamine ebaõnnestus: {deleteResult.Error}",
                                    "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public async Task Save()
        {
            var building = new Building
            {
                Id = _buildingView.Id,
                Location = _buildingView.Location,
                Date = _buildingView.Date,
                UserId = "1" // Можно использовать текущего пользователя, если необходимо
            };

            var result = await _apiClient.Save(building);
            if (result.IsSuccess)
            {
                MessageBox.Show("Hoone salvestatud!", "Teade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await Load();
            }
            else
            {
                MessageBox.Show($"Salvestamine ebaõnnestus: {result.Error}", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task Load()
        {
            var buildings = await _apiClient.List();

            if (buildings.IsSuccess)
            {
                _buildingView.Buildings = buildings.Value;
            }
            else
            {
                MessageBox.Show($"Andmete laadimine ebaõnnestus: {buildings.Error}", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
