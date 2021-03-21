using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    public interface IWiFiManager
    {
        bool IsHomeWifiConnected();
        bool IsServerAvailable();
        void SyncData();
        Task FristSyncData();
        Task FirstSyncOrders();
        Task FirstSyncCategories();
    }
}
