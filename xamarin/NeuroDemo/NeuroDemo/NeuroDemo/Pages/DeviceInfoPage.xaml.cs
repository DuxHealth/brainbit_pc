using NeuroDemo.NeuroImpl;
using NeuroSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeuroDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceInfoPage : ContentPage
    {
        private string _deviceInfoText = "";
        public string DeviceInfoText
        {
            get
            {
                return _deviceInfoText;
            }
            set
            {
                if(_deviceInfoText != value)
                {
                    _deviceInfoText = value;
                    OnPropertyChanged("DeviceInfoText");
                }
            }
        }
        public DeviceInfoPage()
        {
            InitializeComponent();

            BindingContext = this;

            DeviceInfoText = DeviceManager.Instance.GetInfo();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DeviceManager.Instance.connectionStateChanged = DevStateView.ConnectionStateChanged;
            DeviceManager.Instance.batteryChanged = DevStateView.BatteryChanged;
            DevStateView.ConnectionStateChanged(DeviceManager.Instance.ConnectionState);
            DevStateView.BatteryChanged(DeviceManager.Instance.BatteryPower);
        }
    }
}