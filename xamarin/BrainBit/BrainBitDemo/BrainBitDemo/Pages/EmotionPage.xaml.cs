using System;

using BrainBitDemo.NeuroImpl;

using NeuroSDK;

using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EmotionPage
{
    private readonly EmotionsController _emotionController;

    public string EmotionButtonText { get => _isStarted ? "Pause" : "Start"; }

    private string _emotionStatus;

    public string EmotionStatus
    {
        get => _emotionStatus;

        set
        {
            _emotionStatus = value;
            OnPropertyChanged();
        }
    }

    private bool _isButtonEnabled;

    public bool IsButtonEnabled
    {
        get => _isButtonEnabled;

        set
        {
            _isButtonEnabled = value;
            OnPropertyChanged();
        }
    }

    private bool _isStarted;

    public bool IsStarted
    {
        get => _isStarted;

        set
        {
            _isStarted = value;
            OnPropertyChanged(nameof(EmotionButtonText));
        }
    }

    public EmotionPage()
    {
        InitializeComponent();

        BindingContext     = this;
        IsButtonEnabled    = true;
        _emotionController = new EmotionsController();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BrainBitController.Instance.ConnectionStateChanged = DevStateView.ConnectionStateChanged;
        BrainBitController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        DevStateView.ConnectionStateChanged(null, BrainBitController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, BrainBitController.Instance.BatteryPower);
    }

    protected override void OnDisappearing()
    {
        BrainBitController.Instance.ConnectionStateChanged = null;
        BrainBitController.Instance.BatteryChanged         = null;

        BrainBitController.Instance.StopSignal();

        _emotionController.Dispose();

        base.OnDisappearing();
    }

    private void OnSignalReceived(ISensor sensor, BrainBitSignalData[] data) { EmotionStatus = _emotionController.PushData(data); }

    private void StartCalibration_Clicked(object sender, EventArgs e)
    {
        if (IsStarted)
            BrainBitController.Instance.StopSignal();
        else
        {
            _emotionController.StartCalibration();
            BrainBitController.Instance.SignalReceived = OnSignalReceived;
            BrainBitController.Instance.StartSignal();
        }


        IsStarted = !IsStarted;
    }
}
