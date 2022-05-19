using System;
using System.Collections.Generic;
using System.Text;

namespace BluetoothApp.Interfaces
{
    public interface IPressureValueConverter
    {
        uint FromBytesUint(byte[] value);
        ushort FromBytesUshort(byte[] value);

        byte ToBytesUint(uint value);


        (uint, uint) FromBytesPreassure(byte[] value);

    }
}
