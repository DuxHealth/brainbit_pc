using System;

using CallibriDemo.NeuroImpl;

using NeuroSDK;

namespace CallibriDemo.Pages;

public partial class MainPage
{
    private bool _isDeviceConnected;

    public bool IsDeviceConnected
    {
        get => _isDeviceConnected;

        set
        {
            if (_isDeviceConnected == value) return;

            _isDeviceConnected = value;
            OnPropertyChanged();
        }
    }

    private bool _isEnvelopeSupported;

    public bool IsEnvelopeSupported
    {
        get => _isEnvelopeSupported;

        set
        {
            if (_isEnvelopeSupported == value) return;

            _isEnvelopeSupported = value;

            OnPropertyChanged();
        }
    }

    public MainPage()
    {
        InitializeComponent();

        BindingContext = this;
    }

    private void ConnectionStateChanged(ISensor sensor, SensorState state)
    {
        IsDeviceConnected = state == SensorState.StateInRange;

        IsEnvelopeSupported = CallibriController.Instance.IsEnvelopeSupported && IsDeviceConnected;

        DevStateView.ConnectionStateChanged(null, state);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        CallibriController.Instance.ConnectionStateChanged = ConnectionStateChanged;
        CallibriController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        ConnectionStateChanged(null, CallibriController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, CallibriController.Instance.BatteryPower);
    }

    private async void SearchButton_Clicked(object sender, EventArgs e) { await Navigation.PushAsync(new SearchSensorsPage()); }

    private async void DeviceInfoButton_Clicked(object sender, EventArgs e) { await Navigation.PushAsync(new DeviceInfoPage()); }

    private async void SignalButton_Clicked(object sender, EventArgs e) { await Navigation.PushAsync(new SignalsPage()); }

    private async void EnvelopeButton_Clicked(object sender, EventArgs e) { await Navigation.PushAsync(new EnvelopePage()); }

}
