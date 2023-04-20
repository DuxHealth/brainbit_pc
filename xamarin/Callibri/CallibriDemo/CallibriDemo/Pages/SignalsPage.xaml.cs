using System;
using System.Collections.Generic;
using System.Linq;

using CallibriDemo.NeuroImpl;

using NeuroSDK;


using Xamarin.Forms.Xaml;

namespace CallibriDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SignalsPage
{
    private readonly object     _lockObj = new();
    private          bool       _isStarted;

    public SignalsPage()
    {
        InitializeComponent();

        BindingContext = this;
        InitChart();
    }

    public string SignalButtonText { get => _isStarted ? "Pause" : "Start"; }

    public bool IsStarted
    {
        get => _isStarted;

        set
        {
            _isStarted = value;
            OnPropertyChanged(nameof(SignalButtonText));
        }
    }

    private void InitChart()
    {
        int n = CallibriController.Instance.SamplingFrequency;
        Chart.Init(5, n);
        Chart.Amplitude = 10000 / 1e6f;
    }

    private void StartChart()
    {
        IsStarted                                  = true;
        CallibriController.Instance.SignalReceived = OnSignalReceived;

        CallibriController.Instance.StartSignal();
        Chart.StartAnimation();
    }

    private void StopChart()
    {
        CallibriController.Instance.StopSignal();
        Chart.StopAnimation();
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

    private void OnSignalReceived(ISensor sensor, CallibriSignalData[] data)
    {
        double[] values = data.SelectMany(samples => samples.Samples).ToArray();
        Chart.AddSamples(values);
    }

    protected override void OnDisappearing()
    {
        CallibriController.Instance.StopSignal();
        base.OnDisappearing();
    }

    private void SignalButton_Clicked(object sender, EventArgs e)
    {
        IsStarted = !IsStarted;

        if (IsStarted)
            StartChart();
        else
            StopChart();
    }

}
