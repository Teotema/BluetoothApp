using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BluetoothApp.Interfaces
{
    public interface IPessureBlutoothService
    {
        event EventHandler<float> PressureUpdate;
        event EventHandler<DeviceEventArgs> DeviceDiscovered;
        void StartSearch();

        void ConnectToDevice();
        void SetDevice(IDevice device);
        void StartMeasurement();
        void StopMeasurement();

        int PeriodOfTime { get; set; }
    }
}
