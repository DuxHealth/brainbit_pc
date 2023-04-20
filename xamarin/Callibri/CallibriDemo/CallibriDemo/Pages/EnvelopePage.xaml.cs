using System;
using System.Linq;

using CallibriDemo.NeuroImpl;

using NeuroSDK;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallibriDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EnvelopePage : ContentPage
{
    private bool _isStarted;

    public EnvelopePage()
    {
        InitializeComponent();
        BindingContext = this;
        InitChart();
    }

    public string EnvelopeButtonText { get => _isStarted ? "Pause" : "Start"; }

    public bool IsStarted
    {
        get => _isStarted;

        set
        {
            _isStarted = value;
            OnPropertyChanged(nameof(EnvelopeButtonText));
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        StartChart();

        CallibriController.Instance.ConnectionStateChanged = DevStateView.ConnectionStateChanged;
        CallibriController.Instance.BatteryChanged         = DevStateView.BatteryChanged;
        DevStateView.ConnectionStateChanged(null, CallibriController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, CallibriController.Instance.BatteryPower);
    }

    protected override void OnDisappearing()
    {
        CallibriController.Instance.StopSignal();
        CallibriController.Instance.ConnectionStateChanged = null;
        CallibriController.Instance.BatteryChanged         = null;
        base.OnDisappearing();
    }

    private void InitChart()
    {
        chart.Init(5, 20);
        chart.Amplitude = 10000 / 1e6f;
    }

    private void StartChart()
    {
        IsStarted                                    = true;
        CallibriController.Instance.EnvelopeReceived = onSignalRecieved;
        CallibriController.Instance.StartEnvelope();
        chart.StartAnimation();
    }

    private void StopChart()
    {
        CallibriController.Instance.StopEnvelope();
        chart.StopAnimation();
    }

    private void onSignalRecieved(ISensor sensor, CallibriEnvelopeData[] data)
    {
        double[] values = data.Select(sample => sample.Sample).ToArray();
        chart.AddSamples(values);
    }

    private void EnvelopeButton_Clicked(object sender, EventArgs e)
    {
        IsStarted = !IsStarted;

        if (IsStarted)
            StartChart();
        else
            StopChart();
    }
}
