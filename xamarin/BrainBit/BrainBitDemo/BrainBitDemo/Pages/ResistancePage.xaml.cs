using BrainBitDemo.NeuroImpl;

using NeuroSDK;

using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ResistancePage
{
    private string _resistanceText = "";

    public string ResistanceText
    {
        get => _resistanceText;

        set
        {
            if (_resistanceText == value) return;

            _resistanceText = value;
            OnPropertyChanged();
        }
    }

    public ResistancePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BrainBitController.Instance.ConnectionStateChanged = DevStateView.ConnectionStateChanged;
        BrainBitController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        DevStateView.ConnectionStateChanged(null, BrainBitController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, BrainBitController.Instance.BatteryPower);

        BrainBitController.Instance.ResistReceived = OnResistReceived;

        BrainBitController.Instance.StartResist();
    }

    protected override void OnDisappearing()
    {
        BrainBitController.Instance.StopResist();
        base.OnDisappearing();
    }

    private void OnResistReceived(ISensor sensor, BrainBitResistData data)
    {
        ResistanceText = $"O1: {data.O1}\n"
                       + $"O2: {data.O2}\n"
                       + $"T3: {data.T3}\n"
                       + $"T4: {data.T4}";
    }
}
