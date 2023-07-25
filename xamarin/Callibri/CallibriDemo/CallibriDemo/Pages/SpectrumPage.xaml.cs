using System;
using System.Diagnostics;
using System.Linq;

using CallibriDemo.NeuroImpl;

using NeuroSDK;

using NeuroTech.Spectrum;

using Xamarin.Forms.Xaml;

namespace CallibriDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SpectrumPage
{
    private readonly SpectrumController _controller;

    private bool _isStarted;

    public SpectrumPage()
    {
        InitializeComponent();

        BindingContext = this;

        int samplingFrequencyHz = CallibriController.Instance.SamplingFrequency;
        _controller = new SpectrumController(samplingFrequencyHz);

        CallibriController.Instance.SignalReceived = OnSignalReceived;

        InitChart();
    }

    public string SpectrumButtonText { get => IsStarted ? "Pause" : "Start"; }

    public bool IsStarted
    {
        get => _isStarted;

        set
        {
            if (_isStarted == value) return;

            _isStarted = value;
            OnPropertyChanged(nameof(SpectrumButtonText));
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _controller.ProcessedData = OnProcessedData;

        CallibriController.Instance.ConnectionStateChanged = DevStateView.ConnectionStateChanged;
        CallibriController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        DevStateView.ConnectionStateChanged(null, CallibriController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, CallibriController.Instance.BatteryPower);
    }

    protected override void OnDisappearing()
    {
        CallibriController.Instance.StopSignal();

        CallibriController.Instance.SignalReceived = null;
        _controller.ProcessedData                  = null;

        _controller.Dispose();

        base.OnDisappearing();
    }

    private void OnSignalReceived(ISensor sensor, CallibriSignalData[] signalData) { _controller.ProcessSamples(signalData.SelectMany(it => it.Samples).ToArray()); }

    
    private void OnProcessedData(RawSpectrumData[] rawSpectrumData, WavesSpectrumData[] wavesSpectrumData)
    {
        if (rawSpectrumData.Length > 0)
            foreach (RawSpectrumData data in rawSpectrumData.Where(it => it.AllBinsValues.Length != 0))
            {
                int bufSize = (int)(CallibriController.Instance.SamplingFrequency / _controller.FftBinsFor1Hz);
                double[] buffer = new double[bufSize];
                Array.Copy(data.AllBinsValues.Select(it => it * 1e3).ToArray(), buffer, data.AllBinsValues.Length);
            }
             
        if (wavesSpectrumData.Length <= 0) return;

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

        string wavesDataText = $"\nAlpha Raw: {avgAlphaRaw:F}\n"
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

    private void InitChart() { Chart.Init((int)(CallibriController.Instance.SamplingFrequency / _controller.FftBinsFor1Hz), 200); }

    private void SpectrumButton_OnClicked(object sender, EventArgs e)
    {
        IsStarted = !IsStarted;

        if (IsStarted)
        {
            CallibriController.Instance.StartSignal();
            _controller.ProcessedData = OnProcessedData;
        }
        else
        {
            CallibriController.Instance.StopSignal();
            _controller.ProcessedData = null;
        }
    }
}
