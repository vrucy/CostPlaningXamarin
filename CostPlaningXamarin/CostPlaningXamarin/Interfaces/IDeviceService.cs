using CostPlaningXamarin.Models;

namespace CostPlaningXamarin.Interfaces
{
    interface IDeviceService
    {
        Device PostCurrentDevice(int userId);
    }
}
