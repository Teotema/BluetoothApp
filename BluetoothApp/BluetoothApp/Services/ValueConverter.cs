using BluetoothApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BluetoothApp.Services
{
    public class ValueConverter : IPressureValueConverter
    {
        public (uint, uint) FromBytesPreassure(byte[] value)
        {
            throw new NotImplementedException();
        }

        public uint FromBytesUint(byte[] value)
        {
            throw new NotImplementedException();
        }

        public ushort FromBytesUshort(byte[] value)
        {
            throw new NotImplementedException();
        }

        public byte ToBytesUint(uint value)
        {
            throw new NotImplementedException();
        }
    }
}
