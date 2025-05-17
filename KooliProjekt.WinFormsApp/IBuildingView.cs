using KooliProjekt.WinFormsApp.Api;

namespace KooliProjekt.WinFormsApp
{
    public interface IBuildingView
    {
        IList<Building> Buildings { get; set; }
        Building SelectedItem { get; set; }
        string Location { get; set; }
        DateTime Date { get; set; }
        int Id { get; set; }
        BuildingPresenter Presenter { get; set; }
    }
}
