using NeuroDemo.NeuroImpl;
using NeuroSDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeuroDemo.Pages
{
    public partial class MainPage : ContentPage
    {  
        public bool IsDeviceConnected
        {
            get
            {
                return _isDeviceConnected;
            }
            set
            {
                if(_isDeviceConnected != value)
                {
                    _isDeviceConnected = value;
                    OnPropertyChanged("IsDeviceConnected");
                }
            }
        }

        private bool _isDeviceConnected = false;
      
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchSensorsPage());
        }

        private async void DeviceInfoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeviceInfoPage());
        }

        private async void SignalButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignalsPage());
        }

        private void ConnectionStateChanged(SensorState state) 
        {
            IsDeviceConnected = state == SensorState.StateInRange;
            DevStateView.ConnectionStateChanged(state);
        }

        protected override void OnAppearing()
        { 
            base.OnAppearing();
            DeviceManager.Instance.connectionStateChanged = ConnectionStateChanged;
            DeviceManager.Instance.batteryChanged = DevStateView.BatteryChanged;
            ConnectionStateChanged(DeviceManager.Instance.ConnectionState);
            DevStateView.BatteryChanged(DeviceManager.Instance.BatteryPower);
        }
    }
}
