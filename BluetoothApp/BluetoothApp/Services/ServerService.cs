using BluetoothApp.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace BluetoothApp.Services
{
    public class ServerService : IServerService
    {
        private readonly ICommunicationService _communicationService;
        public ServerService(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }
        public async Task SendPreassures(PreassureMeasurement preassure)
        {
          
            string json = JsonConvert.SerializeObject(preassure);
            const string PATH = "http://192.168.1.4:7111/api/Pressure/sendPresuares/";
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            var res = await _communicationService.PostAsObjectAsync<string>(PATH, content);
        }
    }
}
