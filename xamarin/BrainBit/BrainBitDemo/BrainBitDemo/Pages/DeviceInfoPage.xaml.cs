using BrainBitDemo.NeuroImpl;

using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DeviceInfoPage
{
    private string _deviceInfoText = "";

    public string DeviceInfoText
    {
        get => _deviceInfoText;

        set
        {
            if (_deviceInfoText == value) return;

            _deviceInfoText = value;
            OnPropertyChanged();
        }
    }

    public DeviceInfoPage()
    {
        InitializeComponent();

        BindingContext = this;

        DeviceInfoText = BrainBitController.Instance.GetInfo();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BrainBitController.Instance.ConnectionStateChanged = DevStateView.ConnectionStateChanged;
        BrainBitController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        DevStateView.ConnectionStateChanged(null, BrainBitController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, BrainBitController.Instance.BatteryPower);
    }
}
