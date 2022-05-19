using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using BluetoothApp.Interfaces;
using Newtonsoft.Json;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BluetoothApp.Services
{
    public class PessureBlutoothService : IPessureBlutoothService
    {
        private readonly IDynamoDbService _dynamoDbService;
        private PreassureMeasurement _currentMeasurement;
        private bool _isMeasurement;
        private List<uint> _pressures;
        private int Time
        {
            get => Preferences.Get("TimePeriod", 5);

            set => Preferences.Set("TimePeriod", value);
        }
        public int PeriodOfTime 
        {
            get => Time; 
            set => Time = value;
        }
        private async void setT(int t)
        {
            ushort val = (ushort)t;
            var bytes = BitConverter.GetBytes(val);
            var res = await _settings.WriteAsync(bytes);
            
        }
        private int GetT()
        {
            GetSettings();
            return Time;
        }
        private async Task<int> GetSettings()
        {
            var k = await _settings.ReadAsync();
            var l = BitConverter.ToUInt16(k, 0);
            return Time;
        }
        private const string PATH = "http://192.168.1.4:7111/api/Pressure/sendPresuares";
        private const string ID_OF_PREASURE_SERVICE = "42821a40-e477-11e2-82d0-0002a5d5c51b";
        private const string ID_OF_READ_PREASURE = "00100000-0001-11e1-ac36-0002a5d5c51b";

        private const string ID_OF_SETTINGS_SERVICE = "51132170-9861-11e1-9ab4-0002a5d5c51b";
        private const string ID_OF_SETTINGS = "00000002-9861-11e1-ac36-0002a5d5c51b";
        private const string ID_OF_SET_DATETIME = "00000001-9861-11e1-ac36-0002a5d5c51b";


        private IPressureValueConverter _valueConverter;
        private IBluetoothLE _ble;
        private IAdapter _adapter;
        private IDevice _device;
        private ICharacteristic _preassure;
        private ICharacteristic _settings;
        public event EventHandler<DeviceEventArgs> DeviceDiscovered;
        public event EventHandler<float> PressureUpdate;

        public PessureBlutoothService(IDynamoDbService  dynamoDbService)
        {
            _valueConverter = new ValueConverter();
            _ble = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
            _dynamoDbService = dynamoDbService;
            _adapter.DeviceDiscovered += (s, a) => DeviceDiscovered?.Invoke(s, a);
            _adapter.DeviceConnected += DeviceConnected;
            CheckPermitions();
        }
        
        private async void DeviceConnected(object sender, DeviceEventArgs e)
        {
            var preassureServise = await e.Device.GetServiceAsync(Guid.Parse(ID_OF_PREASURE_SERVICE));
            _preassure = await preassureServise.GetCharacteristicAsync(Guid.Parse(ID_OF_READ_PREASURE));
            
            var settingsService = await e.Device.GetServiceAsync(Guid.Parse(ID_OF_SETTINGS_SERVICE));
            _settings = await settingsService.GetCharacteristicAsync(Guid.Parse(ID_OF_SETTINGS));
            var datetimeChar = await settingsService.GetCharacteristicAsync(Guid.Parse(ID_OF_SET_DATETIME));
            var now = DateTime.UtcNow;
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var l = BitConverter.GetBytes(unixTimestamp);
            var res = await datetimeChar.WriteAsync(l);
        }
        public async void StartSearch()
        {
            try
            {
               
                if (!_ble.Adapter.IsScanning)                
                    await _adapter.StartScanningForDevicesAsync();
                
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Notice", ex.Message.ToString(), "Error !");
            }
        }
        private async void CheckPermitions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                _ = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            var status1 = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            if (status1 != PermissionStatus.Granted)
            {
                _ = await Permissions.RequestAsync<Permissions.LocationAlways>();
            }

        }

        public void SetDevice(IDevice device)
        {
            _device = device;
        }

        public async void ConnectToDevice()
        {
            try
            {
                var parameters = new ConnectParameters(forceBleTransport: true);
                await _adapter.ConnectToDeviceAsync(_device, parameters);
            }
            catch (DeviceConnectionException e)
            {
                await App.Current.MainPage.DisplayAlert(e.Source, "Exp", "Ok");
                return;
            }
            
        }

        public void StartMeasurement()
        {
            _currentMeasurement = new PreassureMeasurement();
            _currentMeasurement.Start = DateTime.Now;
            _currentMeasurement.Preassures = new List<uint>();
            _isMeasurement = true;
            Device.StartTimer(new TimeSpan(0, 0, Time), () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (_isMeasurement == false)
                        return;
                    var o = await _preassure.ReadAsync();
                    var bytes1 = o.Take(4).ToArray();
                    var bytes2 = o.Skip(4).Take(4).ToArray();
                    uint res1 = BitConverter.ToUInt32(bytes1, 0);
                    uint res2 = BitConverter.ToUInt32(bytes2, 0);
                    Console.WriteLine();
                    _currentMeasurement.Preassures.Add(res1);
                    PressureUpdate?.Invoke(this, res1);
                });
                return _isMeasurement;
            });
          
            
          
        }

        private void _preassure_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            var vaL = e.Characteristic.Value;
            PressureUpdate?.Invoke(this,1);
        }

        public void StopMeasurement()
        {
            _isMeasurement = false;
            _adapter.DisconnectDeviceAsync(_device);
           
            _currentMeasurement.End = DateTime.Now;
            _dynamoDbService.AddPreassureMeasurement(_currentMeasurement);
        }
    }
}
