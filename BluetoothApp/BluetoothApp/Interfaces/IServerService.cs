using BluetoothApp.Services;
using System.Threading.Tasks;

namespace BluetoothApp.Interfaces
{
    public interface IServerService
    {
        Task SendPreassures(PreassureMeasurement preassure);
    }
}
