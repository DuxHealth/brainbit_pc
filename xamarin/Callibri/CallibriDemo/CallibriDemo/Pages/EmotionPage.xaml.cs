using System;
using System.Linq;

using CallibriDemo.NeuroImpl;

using NeuroSDK;

using Xamarin.Forms.Xaml;

namespace CallibriDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EmotionPage
{
    private readonly EmotionsController _emotionController;

    private string _emotionStatus;
    private bool   _isButtonEnabled;
    private bool   _isStarted;

    public EmotionPage()
    {
        InitializeComponent();
        BindingContext     = this;
        IsButtonEnabled    = true;
        _emotionController = new EmotionsController();
    }

    public string EmotionButtonText { get => _isStarted ? "Pause" : "Start"; }

    public bool IsStarted
    {
        get => _isStarted;

        set
        {
            _isStarted = value;
            OnPropertyChanged(nameof(EmotionButtonText));
        }
    }

    public bool IsButtonEnabled
    {
        get => _isButtonEnabled;

        set
        {
            _isButtonEnabled = value;
            OnPropertyChanged();
        }
    }

    public string EmotionStatus
    {
        get => _emotionStatus;

        set
        {
            _emotionStatus = value;
            OnPropertyChanged();
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

    protected override void OnDisappearing()
    {
        CallibriController.Instance.ConnectionStateChanged = null;
        CallibriController.Instance.BatteryChanged         = null;

        StopSignal();

        _emotionController.Dispose();

        base.OnDisappearing();
    }

    private void StartSignal()
    {
        IsStarted = true;
        _emotionController.StartCalibration();
        CallibriController.Instance.SignalReceived = OnSignalReceived;
        CallibriController.Instance.StartSignal();
    }

    private void StopSignal()
    {
        IsStarted = false;
        CallibriController.Instance.StopSignal();
    }

    private void OnSignalReceived(ISensor sensor, CallibriSignalData[] data) { EmotionStatus = _emotionController.PushData(data.SelectMany(samples => samples.Samples).ToArray()); }

    private void StartCalibration_Clicked(object sender, EventArgs e)
    {
        if (IsStarted)
            StopSignal();
        else
            StartSignal();
    }
}
