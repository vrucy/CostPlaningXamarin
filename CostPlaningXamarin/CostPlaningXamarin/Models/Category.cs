using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
//using SQLite;

namespace CostPlaningXamarin.Models
{
    public class Category
    {
        //[Preserve(AllMembers = true)]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public string Name { get; set; }
        public bool IsDisable { get; set; }
        public bool IsWriteToDB { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Order> Orders { get; set; }
    }
}
