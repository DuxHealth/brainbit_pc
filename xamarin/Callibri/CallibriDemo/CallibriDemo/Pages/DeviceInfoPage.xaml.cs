using CallibriDemo.NeuroImpl;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallibriDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DeviceInfoPage : ContentPage
{
    private string _deviceInfoText = "";

    public DeviceInfoPage()
    {
        InitializeComponent();

        BindingContext = this;

        DeviceInfoText = CallibriController.Instance.GetInfo();
    }

    public string DeviceInfoText
    {
        get => _deviceInfoText;

        set
        {
            if (_deviceInfoText != value)
            {
                _deviceInfoText = value;
                OnPropertyChanged();
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CallibriController.Instance.ConnectionStateChanged = DevStateView.ConnectionStateChanged;
        CallibriController.Instance.BatteryChanged         = DevStateView.BatteryChanged;
        DevStateView.ConnectionStateChanged(null, CallibriController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, CallibriController.Instance.BatteryPower);
    }
}
