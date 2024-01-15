using BrainBitDemo.NeuroImpl;
using NeuroSDK;
using SignalMath;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmotionMonopolarPage : ContentPage
    {
        private readonly EmotionsMonopolarController _emotionController;
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

        public EmotionMonopolarPage()
        {
            InitializeComponent();

            BindingContext = this;
            _emotionController = new EmotionsMonopolarController();
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
            BrainBitController.Instance.BatteryChanged = DevStateView.BatteryChanged;

            DevStateView.ConnectionStateChanged(null, BrainBitController.Instance.ConnectionState);
            DevStateView.BatteryChanged(null, BrainBitController.Instance.BatteryPower);
        }

        protected override void OnDisappearing()
        {
            BrainBitController.Instance.ConnectionStateChanged -= DevStateView.ConnectionStateChanged;
            BrainBitController.Instance.ConnectionStateChanged -= OnConnectionStateChanged;
            BrainBitController.Instance.BatteryChanged = null;

            BrainBitController.Instance.StopSignal();

            _emotionController.Dispose();

            base.OnDisappearing();
        }

        private void OnConnectionStateChanged(ISensor sensor, SensorState sensorState)
        {
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

        private void calibrationCallback(int progress, string channel)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                switch (channel)
                {
                    case "O1":
                        O1CalibrationPB.Progress = progress * 0.01;
                        break;
                    case "O2":
                        O2CalibrationPB.Progress = progress * 0.01;
                        break;
                    case "T3":
                        T3CalibrationPB.Progress = progress * 0.01;
                        break;
                    case "T4":
                        T4CalibrationPB.Progress = progress * 0.01;
                        break;
                }
            });

        }

        private void mindDataCallback(MindData data, string channel)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                switch (channel)
                {
                    case "O1":
                        O1AttentionPercentLabel.Text = $"{data.RelAttention}%";
                        O1RelaxPercentLabel.Text = $"{data.RelRelaxation}%";
                        O1AttentionRawLabel.Text = $"{data.InstAttention}";
                        O1RelaxRawLabel.Text = $"{data.InstRelaxation}";
                        break;
                    case "O2":
                        O2AttentionPercentLabel.Text = $"{data.RelAttention}%";
                        O2RelaxPercentLabel.Text = $"{data.RelRelaxation}%";
                        O2AttentionRawLabel.Text = $"{data.InstAttention}";
                        O2RelaxRawLabel.Text = $"{data.InstRelaxation}";
                        break;
                    case "T3":
                        T3AttentionPercentLabel.Text = $"{data.RelAttention}%";
                        T3RelaxPercentLabel.Text = $"{data.RelRelaxation}%";
                        T3AttentionRawLabel.Text = $"{data.InstAttention}";
                        T3RelaxRawLabel.Text = $"{data.InstRelaxation}";
                        break;
                    case "T4":
                        T4AttentionPercentLabel.Text = $"{data.RelAttention}%";
                        T4RelaxPercentLabel.Text = $"{data.RelRelaxation}%";
                        T4AttentionRawLabel.Text = $"{data.InstAttention}";
                        T4RelaxRawLabel.Text = $"{data.InstRelaxation}";
                        break;
                }
            });
        }

        private void isArtifactedSequenceCallback(bool artifacted, string channel)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                switch (channel)
                {
                    case "O1":
                        O1ArtSequenceLabel.Text = $"Artefacted sequence: {artifacted}";
                        break;
                    case "O2":
                        O2ArtSequenceLabel.Text = $"Artefacted sequence: {artifacted}";
                        break;
                    case "T3":
                        T3ArtSequenceLabel.Text = $"Artefacted sequence: {artifacted}";
                        break;
                    case "T4":
                        T4ArtSequenceLabel.Text = $"Artefacted sequence: {artifacted}";
                        break;
                }
                
            });
        }

        private void isBothSidesArtifactedCallback(bool artifacted, string channel)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                switch (channel)
                {
                    case "O1":
                        O1ArtBothSidesLabel.Text = $"Artefacted both side: {artifacted}";
                        break;
                    case "O2":
                        O2ArtBothSidesLabel.Text = $"Artefacted both side: {artifacted}";
                        break;
                    case "T3":
                        T3ArtBothSidesLabel.Text = $"Artefacted both side: {artifacted}";
                        break;
                    case "T4":
                        T4ArtBothSidesLabel.Text = $"Artefacted both side: {artifacted}";
                        break;
                }
            });
        }

        private void lastSpectralDataCallback(SpectralDataPercents spectralData, string channel)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                switch (channel)
                {
                    case "O1":
                        O1DeltaPercentLabel.Text = $"{(int)(spectralData.Delta * 100)}%";
                        O1ThetaPercentLabel.Text = $"{(int)(spectralData.Theta * 100)}%";
                        O1AlphaPercentLabel.Text = $"{(int)(spectralData.Alpha * 100)}%";
                        O1BetaPercentLabel.Text = $"{(int)(spectralData.Beta * 100)}%";
                        O1GammaPercentLabel.Text = $"{(int)(spectralData.Gamma * 100)}%";
                        break;
                    case "O2":
                        O2DeltaPercentLabel.Text = $"{(int)(spectralData.Delta * 100)}%";
                        O2ThetaPercentLabel.Text = $"{(int)(spectralData.Theta * 100)}%";
                        O2AlphaPercentLabel.Text = $"{(int)(spectralData.Alpha * 100)}%";
                        O2BetaPercentLabel.Text = $"{(int)(spectralData.Beta * 100)}%";
                        O2GammaPercentLabel.Text = $"{(int)(spectralData.Gamma * 100)}%";
                        break;
                    case "T3":
                        T3DeltaPercentLabel.Text = $"{(int)(spectralData.Delta * 100)}%";
                        T3ThetaPercentLabel.Text = $"{(int)(spectralData.Theta * 100)}%";
                        T3AlphaPercentLabel.Text = $"{(int)(spectralData.Alpha * 100)}%";
                        T3BetaPercentLabel.Text = $"{(int)(spectralData.Beta * 100)}%";
                        T3GammaPercentLabel.Text = $"{(int)(spectralData.Gamma * 100)}%";
                        break;
                    case "T4":
                        T4DeltaPercentLabel.Text = $"{(int)(spectralData.Delta * 100)}%";
                        T4ThetaPercentLabel.Text = $"{(int)(spectralData.Theta * 100)}%";
                        T4AlphaPercentLabel.Text = $"{(int)(spectralData.Alpha * 100)}%";
                        T4BetaPercentLabel.Text = $"{(int)(spectralData.Beta * 100)}%";
                        T4GammaPercentLabel.Text = $"{(int)(spectralData.Gamma * 100)}%";
                        break;
                }
            });
        }

        private void rawSpectralDataCallback(RawSpectVals spectVals, string channel)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                switch (channel)
                {
                    case "O1":
                        O1AlphaRawLabel.Text = $"{spectVals.Alpha}";
                        O1BetaRawLabel.Text = $"{spectVals.Beta}";
                        break;
                    case "O2":
                        O2AlphaRawLabel.Text = $"{spectVals.Alpha}";
                        O2BetaRawLabel.Text = $"{spectVals.Beta}";
                        break;
                    case "T3":
                        T3AlphaRawLabel.Text = $"{spectVals.Alpha}";
                        T3BetaRawLabel.Text = $"{spectVals.Beta}";
                        break;
                    case "T4":
                        T4AlphaRawLabel.Text = $"{spectVals.Alpha}";
                        T4BetaRawLabel.Text = $"{spectVals.Beta}";
                        break;
                }
            });
        }

    }
}