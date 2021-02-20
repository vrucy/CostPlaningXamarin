using SQLite;
using SQLiteNetExtensions.Attributes;
using System;


namespace CostPlaningXamarin.Models
{
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public bool IsDisable { get; set; }
        public DateTime Date { get; set; }
        public double Cost { get; set; }
        public bool IsWriteToDB { get; set; }
        public string Description { get; set; }
        [ForeignKey(typeof(User))]
        public int UserId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public User User { get; set; }
        [ForeignKey(typeof(Category))]
        public int CategoryId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public Category Category{ get; set; }
    }
}
