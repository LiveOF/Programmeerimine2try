using System;

namespace KooliProjekt.WpfApp.Api
{
    public class Building
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
    }
}
