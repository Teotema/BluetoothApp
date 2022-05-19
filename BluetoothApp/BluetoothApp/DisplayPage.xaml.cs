using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BluetoothApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayPage : ContentPage
    {
        
        public DisplayPage()
        {
            InitializeComponent();
            App.BlutoothService.ConnectToDevice();
            App.BlutoothService.PressureUpdate += BlutoothService_PressureUpdate;

        }

        private void BlutoothService_PressureUpdate(object sender, float e)
        {
            textJ.Text = (e/100).ToString();
        }

        

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.BlutoothService.StartMeasurement();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            App.BlutoothService.StopMeasurement();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }
    }

       
}