using SQLite;

namespace CostPlaningXamarin.Models
{
    public class Device
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int UserId { get; set; }
    }
}
