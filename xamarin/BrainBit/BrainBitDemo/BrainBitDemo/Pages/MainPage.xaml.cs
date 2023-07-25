using System;

using BrainBitDemo.NeuroImpl;

using NeuroSDK;

namespace BrainBitDemo.Pages;

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

    public MainPage()
    {
        InitializeComponent();

        BindingContext = this;
    }

    private void ConnectionStateChanged(ISensor sensor, SensorState state)
    {
        IsDeviceConnected = state == SensorState.StateInRange;
        DevStateView.ConnectionStateChanged(null, state);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BrainBitController.Instance.ConnectionStateChanged = ConnectionStateChanged;
        BrainBitController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        ConnectionStateChanged(null, BrainBitController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, BrainBitController.Instance.BatteryPower);
    }

    private async void SearchButton_Clicked(object     sender, EventArgs e) { await Navigation.PushAsync(new SearchSensorsPage()); }
    private async void DeviceInfoButton_Clicked(object sender, EventArgs e) { await Navigation.PushAsync(new DeviceInfoPage()); }

    private async void ResistanceButton_Clicked(object sender, EventArgs e) { await Navigation.PushAsync(new ResistancePage()); }
    private async void SignalButton_Clicked(object     sender, EventArgs e) { await Navigation.PushAsync(new SignalsPage()); }
    private async void EmotionButton_Clicked(object sender, EventArgs e) { await Navigation.PushAsync(new EmotionPage()); }
    private async void SpectrumButton_Clicked(object sender, EventArgs e) { await Navigation.PushAsync(new SpectrumPage()); }
}
