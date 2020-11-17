using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace CostPlaningXamarin.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ICollection<Order> Orders { get; set; }
    }
}
