using System;
using System.Diagnostics;
using System.Linq;

using CallibriDemo.NeuroImpl;

using NeuroSDK;

using Xamarin.Forms.Xaml;

namespace CallibriDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ECGPage
{
    private readonly ECGController _ecgController;

    private string _ecgStatus;
    private bool   _isStarted;

    public ECGPage()
    {
        InitializeComponent();

        BindingContext = this;

        _ecgController = new ECGController(CallibriController.Instance.SamplingFrequency);
    }

    public bool IsStarted
    {
        get => _isStarted;

        set
        {
            if (_isStarted == value) return;

            _isStarted = value;
            OnPropertyChanged(nameof(ECGButtonText));
        }
    }

    public string ECGButtonText { get => IsStarted ? "Pause" : "Start"; }

    public string EcgStatus
    {
        get => _ecgStatus;

        set
        {
            if (_ecgStatus == value) return;

            _ecgStatus = value;
            OnPropertyChanged();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _ecgController.ProcessedResult = OnProcessedResult;

        CallibriController.Instance.SignalReceived         = OnSignalReceived;
        CallibriController.Instance.ConnectionStateChanged = DevStateView.ConnectionStateChanged;
        CallibriController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        CallibriController.Instance.StartSignal();

        DevStateView.ConnectionStateChanged(null, CallibriController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, CallibriController.Instance.BatteryPower);
    }

    protected override void OnDisappearing()
    {
        CallibriController.Instance.ConnectionStateChanged = null;
        CallibriController.Instance.BatteryChanged         = null;
        CallibriController.Instance.SignalReceived         = null;

        _ecgController.ProcessedResult = null;

        _ecgController.Dispose();

        base.OnDisappearing();
    }

    private void OnProcessedResult(ECGData data)
    {
        EcgStatus = $"{nameof(data.RR)}: {data.RR:N}\n"
                  + $"{nameof(data.HR)}: {data.HR:N}\n"
                  + $"{nameof(data.PressureIndex)}: {data.PressureIndex:N}\n"
                  + $"{nameof(data.Moda)}: {data.Moda:N}\n"
                  + $"{nameof(data.AmplModa)}: {data.AmplModa:N}\n"
                  + $"{nameof(data.VariationDist)}: {data.VariationDist:N}";

        Debug.WriteLine(EcgStatus);
    }

    private void OnSignalReceived(ISensor sensor, CallibriSignalData[] signalData) { _ecgController.ProcessSamples(signalData.SelectMany(samples => samples.Samples).ToArray()); }

    private void EcgButton_OnClicked(object sender, EventArgs e)
    {
        IsStarted = !IsStarted;

        if (IsStarted)
            CallibriController.Instance.StopSignal();
        else
            CallibriController.Instance.StartSignal();
    }
}
