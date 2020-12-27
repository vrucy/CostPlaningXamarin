using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CostPlaningXamarin.Models
{
    public class User
    {
        [Preserve(AllMembers = true)]
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public bool DeviceUser { get; set; }
        //[NotMapped]
        //public bool WriteInDb { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Order> Orders { get; set; }
    }
}
