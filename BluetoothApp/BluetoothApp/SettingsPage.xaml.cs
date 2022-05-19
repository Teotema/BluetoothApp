using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BluetoothApp;
namespace BluetoothApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        
        public SettingsPage()
        {
            InitializeComponent();

            stepper.Maximum = 15;
            stepper.Minimum = 1;
            
            stepper.Value = App.BlutoothService.PeriodOfTime;
            App.BlutoothService.PeriodOfTime = 1;
            stepper.ValueChanged += Stepper_ValueChanged;
            value.Text = stepper.Value.ToString();
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            value.Text = stepper.Value.ToString();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.BlutoothService.PeriodOfTime = (int) stepper.Value;
            Navigation.PopAsync();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            stepper.Value = App.BlutoothService.PeriodOfTime;
        }
    }

    
}