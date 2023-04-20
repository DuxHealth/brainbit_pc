using System;
using System.Collections.Generic;
using System.Linq;

using BrainBitDemo.NeuroImpl;

using NeuroSDK;


using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SignalsPage
{
    private readonly object _lockObj = new();

    public string SignalButtonText { get => _isStarted ? "Pause" : "Start"; }

    private bool _isStarted;

    public bool IsStarted
    {
        get => _isStarted;

        set
        {
            _isStarted = value;
            OnPropertyChanged(nameof(SignalButtonText));
        }
    }

    public SignalsPage()
    {
        InitializeComponent();

        BindingContext = this;

        InitChart();
    }

    private void InitChart()
    {
        int n = BrainBitController.Instance.SamplingFrequency;
        ChartO1.Init(5, n, "O1");
        ChartO1.Amplitude = 10000 / 1e6f;

        ChartO2.Init(5, n, "O2");
        ChartO2.Amplitude = 10000 / 1e6f;

        ChartT3.Init(5, n, "T3");
        ChartT3.Amplitude = 10000 / 1e6f;

        ChartT4.Init(5, n, "T4");
        ChartT4.Amplitude = 10000 / 1e6f;
    }

    private void StartChart()
    {
        IsStarted                                  = true;
        BrainBitController.Instance.SignalReceived = OnSignalReceived;

        BrainBitController.Instance.StartSignal();
        ChartO1.StartAnimation();
        ChartO2.StartAnimation();
        ChartT3.StartAnimation();
        ChartT4.StartAnimation();
    }

    private void StopChart()
    {
        BrainBitController.Instance.StopSignal();
        ChartO1.StopAnimation();
        ChartO2.StopAnimation();
        ChartT3.StopAnimation();
        ChartT4.StopAnimation();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        StartChart();

        BrainBitController.Instance.ConnectionStateChanged = DevStateView.ConnectionStateChanged;
        BrainBitController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        DevStateView.ConnectionStateChanged(null, BrainBitController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, BrainBitController.Instance.BatteryPower);
    }

    protected override void OnDisappearing()
    {
        BrainBitController.Instance.StopSignal();
        base.OnDisappearing();
    }

    private void OnSignalReceived(ISensor sensor, BrainBitSignalData[] data)
    {
        double[] valuesO1 = data.Select(sample => sample.O1).ToArray();
        double[] valuesO2 = data.Select(sample => sample.O2).ToArray();
        double[] valuesT3 = data.Select(sample => sample.T3).ToArray();
        double[] valuesT4 = data.Select(sample => sample.T4).ToArray();

        ChartO1.AddSamples(valuesO1);
        ChartO2.AddSamples(valuesO2);
        ChartT3.AddSamples(valuesT3);
        ChartT4.AddSamples(valuesT4);
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
