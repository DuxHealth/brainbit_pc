using NeuroDemo.NeuroImpl;
using NeuroSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeuroDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceStateView : ContentView
    {
        private bool _state = false;
        public bool State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }
        private int _power = 0;
        public int Power
        {
            get
            {
                return _power;
            }
            set
            {
                _power = value;
                OnPropertyChanged(nameof(Power));
            }
        }

        public DeviceStateView()
        {
            InitializeComponent();
            ConnectionStateChanged(DeviceManager.Instance.ConnectionState);
            BatteryChanged(DeviceManager.Instance.BatteryPower);
            BindingContext = this;
        }
        public void ConnectionStateChanged(SensorState state)
        {
            if (state == SensorState.StateInRange)
            {
                State = true;
            }
            else
            {
                State = false;
                Power = 0;
            }
        }

        public void BatteryChanged(int power)
        {
            Power = power;
        }
    }
}