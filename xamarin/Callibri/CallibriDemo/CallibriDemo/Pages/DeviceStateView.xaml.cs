using CallibriDemo.NeuroImpl;

using NeuroSDK;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallibriDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DeviceStateView : ContentView
{
    private int  _power;
    private bool _state;

    public DeviceStateView()
    {
        InitializeComponent();
        ConnectionStateChanged(null, CallibriController.Instance.ConnectionState);
        BatteryChanged(null, CallibriController.Instance.BatteryPower);
        BindingContext = this;
    }

    public bool State
    {
        get => _state;

        set
        {
            _state = value;
            OnPropertyChanged();
        }
    }

    public int Power
    {
        get => _power;

        set
        {
            _power = value;
            OnPropertyChanged();
        }
    }

    public void ConnectionStateChanged(ISensor sensor, SensorState state)
    {
        if (state == SensorState.StateInRange)
            State = true;
        else
        {
            State = false;
            Power = 0;
        }
    }

    public void BatteryChanged(ISensor sensor, int power) { Power = power; }
}
