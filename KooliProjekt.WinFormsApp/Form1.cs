using KooliProjekt.WinFormsApp.Api;

namespace KooliProjekt.WinFormsApp
{
    public partial class Form1 : Form, IBuildingView
    {
        public IList<Building> Buildings
        {
            get
            {
                return (IList<Building>)BuildingsGrid.DataSource;
            }
            set
            {
                BuildingsGrid.DataSource = value;
            }
        }

        public Building SelectedItem { get; set; }

        public BuildingPresenter Presenter { get; set; }

        public string Location
        {
            get
            {
                return LocationField.Text;
            }
            set
            {
                LocationField.Text = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return DateField.Value;
            }
            set
            {
                DateField.Value = value;
            }
        }

        public int Id
        {
            get
            {
                return int.Parse(IdField.Text);
            }
            set
            {
                IdField.Text = value.ToString();
            }
        }

        public Form1()
        {
            InitializeComponent();

            BuildingsGrid.AutoGenerateColumns = true;
            BuildingsGrid.SelectionChanged += BuildingsGrid_SelectionChanged;

            NewButton.Click += AddButton_Click;
            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;

            Load += Form1_Load;
        }

        private async void DeleteButton_Click(object? sender, EventArgs e)
        {
            await Presenter.Delete();
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            await Presenter.Save();
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            Presenter.UpdateView(new Building());
        }

        private void BuildingsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (BuildingsGrid.SelectedRows.Count == 0)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = (Building)BuildingsGrid.SelectedRows[0].DataBoundItem;
            }

            Presenter.UpdateView(SelectedItem);
        }

        private async void Form1_Load(object? sender, EventArgs e)
        {
            await Presenter.Load();
        }
    }
}
