using BluetoothApp.Interfaces;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BluetoothApp.Services
{
    public class PreassureMeasurement
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<uint> Preassures { get; set; }
       
    }
    public class TestDevice : IDevice
    {
        private Guid _id;
        private string _name;
        public TestDevice(Guid id, string name)
        {
            _id = id;
            _name = name;
        }
        public Guid Id => _id;

        public string Name => _name;

        public int Rssi => throw new NotImplementedException();

        public object NativeDevice => throw new NotImplementedException();

        public DeviceState State => throw new NotImplementedException();

        public IList<AdvertisementRecord> AdvertisementRecords => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IService> GetServiceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<IService>> GetServicesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> RequestMtuAsync(int requestValue)
        {
            throw new NotImplementedException();
        }

        public bool UpdateConnectionInterval(ConnectionInterval interval)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRssiAsync()
        {
            throw new NotImplementedException();
        }
    }
    internal class TestBlutoothService : IPessureBlutoothService
    {
        private readonly IServerService _serverService;
        private PreassureMeasurement _currentMeasurement;
        private bool _isMeasurement;
        private IDevice _device;
        private int Time
        {
            get => Preferences.Get("TimePeriod", 5);

            set => Preferences.Set("TimePeriod", value);
        }
        public int PeriodOfTime
        {
            get => Time;
            set { Time = value; }
        }
        public TestBlutoothService(IServerService serverService)
        {
            _serverService = serverService;
        }
        public event EventHandler<float> PressureUpdate;
        public event EventHandler<DeviceEventArgs> DeviceDiscovered;

        public void ConnectToDevice()
        {
          
        }

        public void SetDevice(IDevice device)
        {
            _device = device;
        }

        public void StartMeasurement()
        {
           _currentMeasurement = new PreassureMeasurement();
           _currentMeasurement.Start = DateTime.Now;
            _currentMeasurement.Preassures = new List<uint>();
            Random rnd = new Random();
            _isMeasurement = true;
            Device.StartTimer(new TimeSpan(0, 0, Time), () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var val = (uint)rnd.Next(200, 800);
                    _currentMeasurement.Preassures.Add(val);
                    PressureUpdate?.Invoke(this,val);
                });
                return _isMeasurement;
            });
        }

        public void StartSearch()
        {
            DeviceDiscovered?.Invoke(this, new DeviceEventArgs()
            {
                Device = new TestDevice(Guid.NewGuid(), "Device 1")
            });
            DeviceDiscovered?.Invoke(this, new DeviceEventArgs()
            {
                Device = new TestDevice(Guid.NewGuid(), "Device 3")
            });
        }

        public void StopMeasurement()
        {
            _isMeasurement = false;
            _currentMeasurement.End = DateTime.Now;
            _serverService.SendPreassures(_currentMeasurement);
        }
    }
}
