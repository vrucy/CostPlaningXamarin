using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CostPlaningXamarin.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ICollection<Order> Orders { get; set; }
    }
}
