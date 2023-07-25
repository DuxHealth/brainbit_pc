using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using BrainBitDemo.NeuroImpl;

using NeuroSDK;

using NeuroTech.Spectrum;

using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SpectrumPage
{
    private readonly SpectrumController _controllerO1;
    private readonly SpectrumController _controllerO2;
    private readonly SpectrumController _controllerT3;
    private readonly SpectrumController _controllerT4;

    public string SpectrumButtonText { get => IsStarted ? "Pause" : "Start"; }

    private bool _isStarted;

    private bool IsStarted
    {
        get => _isStarted;

        set
        {
            if (_isStarted == value) return;

            _isStarted = value;
            OnPropertyChanged(nameof(SpectrumButtonText));
        }
    }

    public SpectrumPage()
    {
        InitializeComponent();

        BindingContext = this;

        int samplingFrequencyHz = BrainBitController.Instance.SamplingFrequency;
        _controllerO1 = new SpectrumController(samplingFrequencyHz);
        _controllerO2 = new SpectrumController(samplingFrequencyHz);
        _controllerT3 = new SpectrumController(samplingFrequencyHz);
        _controllerT4 = new SpectrumController(samplingFrequencyHz);

        BrainBitController.Instance.SignalReceived = OnSignalReceived;

        InitCharts();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _controllerO1.ProcessedData = OnProcessedDataO1;
        _controllerO2.ProcessedData = OnProcessedDataO2;
        _controllerT3.ProcessedData = OnProcessedDataT3;
        _controllerT4.ProcessedData = OnProcessedDataT4;

        BrainBitController.Instance.ConnectionStateChanged = DevStateView.ConnectionStateChanged;
        BrainBitController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        DevStateView.ConnectionStateChanged(null, BrainBitController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, BrainBitController.Instance.BatteryPower);
    }

    protected override void OnDisappearing()
    {
        BrainBitController.Instance.StopSignal();

        BrainBitController.Instance.SignalReceived = null;

        _controllerO1.ProcessedData = null;
        _controllerO2.ProcessedData = null;
        _controllerT3.ProcessedData = null;
        _controllerT4.ProcessedData = null;

        _controllerO1.Dispose();
        _controllerO2.Dispose();
        _controllerT3.Dispose();
        _controllerT4.Dispose();

        base.OnDisappearing();
    }

    private void OnSignalReceived(ISensor sensor, BrainBitSignalData[] signalData)
    {
        _controllerO1.ProcessSamples(signalData.Select(sample => sample.O1).ToArray());
        _controllerO2.ProcessSamples(signalData.Select(sample => sample.O2).ToArray());
        _controllerT3.ProcessSamples(signalData.Select(sample => sample.T3).ToArray());
        _controllerT4.ProcessSamples(signalData.Select(sample => sample.T4).ToArray());
    }

    private void OnProcessedDataO1(RawSpectrumData[] rawSpectrumData, WavesSpectrumData[] wavesSpectrumData) { OnProcessedData(rawSpectrumData, wavesSpectrumData, ChannelType.O1); }

    private void OnProcessedDataO2(RawSpectrumData[] rawSpectrumData, WavesSpectrumData[] wavesSpectrumData) { OnProcessedData(rawSpectrumData, wavesSpectrumData, ChannelType.O2); }

    private void OnProcessedDataT3(RawSpectrumData[] rawSpectrumData, WavesSpectrumData[] wavesSpectrumData) { OnProcessedData(rawSpectrumData, wavesSpectrumData, ChannelType.T3); }

    private void OnProcessedDataT4(RawSpectrumData[] rawSpectrumData, WavesSpectrumData[] wavesSpectrumData) { OnProcessedData(rawSpectrumData, wavesSpectrumData, ChannelType.T4); }

    private void OnProcessedData(IReadOnlyCollection<RawSpectrumData> rawSpectrumData, IReadOnlyCollection<WavesSpectrumData> wavesSpectrumData, ChannelType type)
    {
        if (wavesSpectrumData.Count > 0)
        {
            double avgAlphaRaw = wavesSpectrumData.Select(it => it.Alpha_Raw).Average();
            double avgBetaRaw  = wavesSpectrumData.Select(it => it.BetaRaw).Average();
            double avgGammaRaw = wavesSpectrumData.Select(it => it.GammaRaw).Average();
            double avgDeltaRaw = wavesSpectrumData.Select(it => it.DeltaRaw).Average();
            double avgThetaRaw = wavesSpectrumData.Select(it => it.ThetaRaw).Average();

            double avgAlphaRel = wavesSpectrumData.Select(it => it.Alpha_Rel).Average();
            double avgBetaRel  = wavesSpectrumData.Select(it => it.BetaRel).Average();
            double avgGammaRel = wavesSpectrumData.Select(it => it.GammaRel).Average();
            double avgDeltaRel = wavesSpectrumData.Select(it => it.DeltaRel).Average();
            double avgThetaRel = wavesSpectrumData.Select(it => it.ThetaRel).Average();

            string wavesDataText = $"\n{type}:\n"
                                 + $"Alpha Raw: {avgAlphaRaw:F}\n"
                                 + $"Beta Raw: {avgBetaRaw:F}\n"
                                 + $"Gamma Raw: {avgGammaRaw:F}\n"
                                 + $"Delta Raw: {avgDeltaRaw:F}\n"
                                 + $"Theta Raw: {avgThetaRaw:F}\n\n"
                                 + $"Alpha Rel: {avgAlphaRel:F}\n"
                                 + $"Beta Rel: {avgBetaRel:F}\n"
                                 + $"Gamma Rel: {avgGammaRel:F}\n"
                                 + $"Delta Rel: {avgDeltaRel:F}\n"
                                 + $"Theta Rel: {avgThetaRel:F}\n";

            Debug.WriteLine(wavesDataText, nameof(SpectrumPage));
        }

        if (rawSpectrumData.Count <= 0) return;

        switch (type)
        {
        case ChannelType.O1:
            foreach (RawSpectrumData spectrumData in rawSpectrumData.Where(it => it.AllBinsValues.Length != 0)) ChartO1.Entries = spectrumData.AllBinsValues;
            break;
        case ChannelType.O2:
            foreach (RawSpectrumData spectrumData in rawSpectrumData.Where(it => it.AllBinsValues.Length != 0)) ChartO2.Entries = spectrumData.AllBinsValues;
            break;
        case ChannelType.T3:
            foreach (RawSpectrumData spectrumData in rawSpectrumData.Where(it => it.AllBinsValues.Length != 0)) ChartT3.Entries = spectrumData.AllBinsValues;
            break;
        case ChannelType.T4:
            foreach (RawSpectrumData spectrumData in rawSpectrumData.Where(it => it.AllBinsValues.Length != 0)) ChartT4.Entries = spectrumData.AllBinsValues;
            break;
        }
    }

    private void InitCharts()
    {
        ChartO1.Init((int)(BrainBitController.Instance.SamplingFrequency / _controllerO1.FftBinsFor1Hz), 20);
        ChartO2.Init((int)(BrainBitController.Instance.SamplingFrequency / _controllerO1.FftBinsFor1Hz), 20);
        ChartT3.Init((int)(BrainBitController.Instance.SamplingFrequency / _controllerO1.FftBinsFor1Hz), 20);
        ChartT4.Init((int)(BrainBitController.Instance.SamplingFrequency / _controllerO1.FftBinsFor1Hz), 20);
    }

    private void SpectrumButton_OnClicked(object sender, EventArgs e)
    {
        IsStarted = !IsStarted;

        if (IsStarted)
        {
            _controllerO1.ProcessedData = OnProcessedDataO1;
            _controllerO2.ProcessedData = OnProcessedDataO2;
            _controllerT3.ProcessedData = OnProcessedDataT3;
            _controllerT4.ProcessedData = OnProcessedDataT4;

            BrainBitController.Instance.StartSignal();
        }
        else
        {
            _controllerO1.ProcessedData = null;
            _controllerO2.ProcessedData = null;
            _controllerT3.ProcessedData = null;
            _controllerT4.ProcessedData = null;

            BrainBitController.Instance.StopSignal();
        }
    }
}
