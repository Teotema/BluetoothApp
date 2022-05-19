using BluetoothApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothApp
{
    public interface IBluetoothService
    {
        IEnumerable<BluetoothDeviceDto> GetDevices();
        Task<bool> ConnectToDevice(string deviceName);


    }
}
