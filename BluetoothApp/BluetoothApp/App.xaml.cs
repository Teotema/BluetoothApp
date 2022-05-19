using BluetoothApp.Interfaces;
using BluetoothApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BluetoothApp
{
    public partial class App : Application
    {
        public static IPessureBlutoothService  BlutoothService { get; private set; }
        public static ICommunicationService CommunicationService { get; private set; }
        public static IDynamoDbService DynamoDbService { get; private set; }
        public App()
        {
            InitializeComponent();
            CommunicationService = new CommunicationService();
            DynamoDbService = new DynamoDbService();
            BlutoothService = new PessureBlutoothService(DynamoDbService);
            
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
