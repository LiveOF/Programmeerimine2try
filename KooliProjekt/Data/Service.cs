namespace KooliProjekt.Data
{
    public class Service
    {
        public int Id { get; set; }

        public Building Building { get; set; }
        public int BuildingId { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
        public string Title { get; set; }
    }
}
