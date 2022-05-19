using BluetoothApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothApp.Interfaces
{
    public interface IDynamoDbService
    {
        Task AddPreassureMeasurement(PreassureMeasurement measurement);
    }
}
