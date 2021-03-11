using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Services;
using Plugin.DeviceInfo;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceService))]
namespace CostPlaningXamarin.Services
{
    class DeviceService : IDeviceService
    {
        public Models.Device PostCurrentDevice(int userId)
        {
             return new Models.Device()
            {
                UserId = userId,
                DeviceId = CrossDeviceInfo.Current.Id,
                DeviceName = CrossDeviceInfo.Current.DeviceName
             };
        }
    }
}
