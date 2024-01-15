using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BrainBitDemo.NeuroImpl;

using NeuroSDK;
using SignalMath;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EmotionPage
{
    private readonly EmotionsController _emotionController;

    public string EmotionButtonText { get => _isStarted ? "Pause" : "Start"; }


    private bool _isButtonEnabled = true;

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
        _emotionController = new EmotionsController();
        _emotionController.progressCalibrationCallback = calibrationCallback;
        _emotionController.isArtefactedSequenceCallback = isArtifactedSequenceCallback;
        _emotionController.isBothSidesArtifactedCallback = isBothSidesArtifactedCallback;
        _emotionController.lastMindDataCallback = mindDataCallback;
        _emotionController.lastSpectralDataCallback = lastSpectralDataCallback;
        _emotionController.rawSpectralDataCallback = rawSpectralDataCallback;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BrainBitController.Instance.ConnectionStateChanged += DevStateView.ConnectionStateChanged;
        BrainBitController.Instance.ConnectionStateChanged += OnConnectionStateChanged;
        BrainBitController.Instance.BatteryChanged         = DevStateView.BatteryChanged;

        DevStateView.ConnectionStateChanged(null, BrainBitController.Instance.ConnectionState);
        DevStateView.BatteryChanged(null, BrainBitController.Instance.BatteryPower);
    }

    protected override void OnDisappearing()
    {
        BrainBitController.Instance.ConnectionStateChanged -= DevStateView.ConnectionStateChanged;
        BrainBitController.Instance.ConnectionStateChanged -= OnConnectionStateChanged;
        BrainBitController.Instance.BatteryChanged         = null;

        BrainBitController.Instance.StopSignal();

        _emotionController.Dispose();

        base.OnDisappearing();
    }

    private void OnConnectionStateChanged(ISensor sensor, SensorState sensorState) {
        IsButtonEnabled = sensorState == SensorState.StateInRange;
    }

    private void OnSignalReceived(ISensor sensor, BrainBitSignalData[] data) { _emotionController.ProcessData(data); }

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

    private void calibrationCallback(int progress)
    {
        MainThread.BeginInvokeOnMainThread(() => {
            CalibrationPB.Progress = progress * 0.01;
        });
        
    }

    private void mindDataCallback(MindData data)
    {
        MainThread.BeginInvokeOnMainThread(() => {
            AttentionPercentLabel.Text = $"{data.RelAttention}%";
            RelaxPercentLabel.Text = $"{data.RelRelaxation}%";
            AttentionRawLabel.Text = $"{data.InstAttention}";
            RelaxRawLabel.Text = $"{data.InstRelaxation}";
        });
    }

    private void isArtifactedSequenceCallback(bool artifacted)
    {
        MainThread.BeginInvokeOnMainThread(() => {
            ArtSequenceLabel.Text = $"Artefacted sequence: {artifacted}";
        });
    }

    private void isBothSidesArtifactedCallback(bool artifacted)
    {
        MainThread.BeginInvokeOnMainThread(() => {
            ArtBothSidesLabel.Text = $"Artefacted both side: {artifacted}";
        });
    }

    private void lastSpectralDataCallback(SpectralDataPercents spectralData)
    {
        MainThread.BeginInvokeOnMainThread(() => {
            DeltaPercentLabel.Text = $"{(int)(spectralData.Delta * 100)}%";
            ThetaPercentLabel.Text = $"{(int)(spectralData.Theta * 100)}%";
            AlphaPercentLabel.Text = $"{(int)(spectralData.Alpha * 100)}%";
            BetaPercentLabel.Text = $"{(int)(spectralData.Beta * 100)}%";
            GammaPercentLabel.Text = $"{(int)(spectralData.Gamma * 100)}%";
        });
    }

    private void rawSpectralDataCallback(RawSpectVals spectVals)
    {
        MainThread.BeginInvokeOnMainThread(() => {
            AlphaRawLabel.Text = $"{spectVals.Alpha}";
            BetaRawLabel.Text = $"{spectVals.Beta}";
        });
    }
}
