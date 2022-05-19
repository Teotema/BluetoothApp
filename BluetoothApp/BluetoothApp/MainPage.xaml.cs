using BluetoothApp.Models;
using Plugin.BLE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using BluetoothApp.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.DynamoDBv2.DataModel;
using Amazon;
using Amazon.DynamoDBv2.Model;

namespace BluetoothApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

       
    }
    public class BluetoothDevicesViewModel : NotifyingViewModel
    {
        public ObservableCollection<IDevice> Devices { get; private set; }
        public ICommand DeviceClick { get; private set; }
        public ICommand RefreshDevices { get; private set; }
        
        public BluetoothDevicesViewModel()
        {

            Devices = new ObservableCollection<IDevice>();
          
            RefreshDevices = new Command(() =>
            {
                Devices.Clear();
                App.BlutoothService.DeviceDiscovered += (s, a) =>
                {
                    Devices.Add(a.Device);
                };
                App.BlutoothService.StartSearch();
            });
            
            DeviceClick = new Command<IDevice>(x =>
            {
                App.BlutoothService.SetDevice(x);
                App.Current.MainPage.Navigation.PushAsync(new DisplayPage());            
            });
        }
      
      
    }
}
