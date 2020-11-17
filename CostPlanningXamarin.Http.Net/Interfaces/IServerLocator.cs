using System;
using System.Collections.Generic;
using System.Text;

namespace CostPlanningXamarin.Http.Net.Interfaces
{
    public interface IServerLocator
    {
        ServerAddress GetServerAddress();
    }
}
