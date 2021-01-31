using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CostPlaningXamarin.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public string Name { get; set; }
        public bool IsDisable { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Order> Orders { get; set; }
    }
}
