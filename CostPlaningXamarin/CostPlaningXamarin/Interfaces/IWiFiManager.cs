using System;
using System.Collections.Generic;
using System.Text;

namespace CostPlaningXamarin.Interfaces
{
    public interface IWiFiManager
    {
        bool IsHomeWifiConnected();
        void SyncData();
    }
}
