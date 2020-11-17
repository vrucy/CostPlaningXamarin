using CostPlanningXamarin.Http.Net.Interfaces;

namespace CostPlanningXamarin.Http.Net
{
    public class FixedAddressServerLocator : IServerLocator
    {
        private readonly ServerAddress _address;

        public FixedAddressServerLocator(ServerAddress address)
        {
            _address = address;
        }

        public ServerAddress GetServerAddress()
        {
            return _address;
        }
    }
}
