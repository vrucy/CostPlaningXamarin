using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    public interface IWiFiManager
    {
        bool IsHomeWifiConnected();
        bool IsServerAvailable();
        void SyncData();
        void FristSyncData();
        Task FirstSyncOrders();
    }
}
