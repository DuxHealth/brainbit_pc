using BrainBitDemo.NeuroImpl;

using NeuroSDK;

using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DeviceStateView
{
    private int _power;

    public int Power
    {
        get => _power;

        set
        {
            _power = value;
            OnPropertyChanged();
        }
    }

    private bool _state;

    public bool State
    {
        get => _state;

        set
        {
            _state = value;
            OnPropertyChanged();
        }
    }

    public DeviceStateView()
    {
        InitializeComponent();
        ConnectionStateChanged(null, BrainBitController.Instance.ConnectionState);
        BatteryChanged(null, BrainBitController.Instance.BatteryPower);
        BindingContext = this;
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
