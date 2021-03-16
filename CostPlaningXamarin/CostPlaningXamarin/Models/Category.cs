using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CostPlaningXamarin.Models
{
    public class Category
    {
        [PrimaryKey, Unique]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Order> Orders { get; set; }
    }
}
