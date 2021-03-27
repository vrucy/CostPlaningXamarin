using System.Threading.Tasks;

namespace CostPlaningXamarin.ViewModels
{
    public interface IAsyncInitialization
    {
        Task Initialization { get; }
    }
}