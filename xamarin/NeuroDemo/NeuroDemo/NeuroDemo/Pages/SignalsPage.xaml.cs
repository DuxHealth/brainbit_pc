using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ChartsXamarinGLv2.Charts.Interfaces;
using ChartsXamarinGLv2.Charts.Linear;
using System.Numerics;
using NeuroDemo.NeuroImpl;

namespace NeuroDemo.Pages
{
    public class TimeValueFormatter : IValueFormatter
    {
        public string Get(float value)
        {
            return TimeSpan.FromSeconds(value).ToString(@"mm\:ss");
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignalsPage : ContentPage
    {
        public string SignalButtonText => _isStarted ? "Pause" : "Start";
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                _isStarted = value;
                OnPropertyChanged(nameof(SignalButtonText));
            }
        }
        public bool HasLegend { get; set; } = true;

        private Random _random = new Random();

        private bool _isStarted = false;
        private bool _isFit = false;

        private ChannelType _selectedChannel;

        private LinearChartSeries _seriesX;

        private DateTime _startTime;
        private TimeSpan _offsetTime;

        public SignalsPage()
        {
            InitializeComponent();

            BindingContext = this;

            _isFit = true;
            InitChart(Color.DarkRed, "Accel. X-axis");
            InitChannelPicker();
            chart.Config.TrackQuadtree = false;
        }
        private void InitChart(Color chartColor, string name)
        {
            int n = DeviceManager.Instance.SamplingFrequency;
            chart.Clear();

            _seriesX = new LinearChartSeries(n, chartColor) { Name = name };

            chart.InitSeries(new List<LinearChartSeries> { _seriesX });
            chart.XValueFormatter = new TimeValueFormatter();
        }

        private void StartChart()
        {
            _startTime = DateTime.Now;
            IsStarted = true;
            DeviceManager.Instance.StartSignal(onSignalRecieved);
        }
        private void StopChart()
        {
            DeviceManager.Instance.StopSignal();
            _offsetTime += DateTime.Now - _startTime;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StartChart();

            DeviceManager.Instance.connectionStateChanged = DevStateView.ConnectionStateChanged;
            DeviceManager.Instance.batteryChanged = DevStateView.BatteryChanged;
            DevStateView.ConnectionStateChanged(DeviceManager.Instance.ConnectionState);
            DevStateView.BatteryChanged(DeviceManager.Instance.BatteryPower);
        }

        private void onSignalRecieved(SignalPackage signals)
        {
            List<(List<Vector2>, int)> seriesData = new List<(List<Vector2>, int)>(1);
            float x = (float)(DateTime.Now - _startTime + _offsetTime).TotalSeconds;

            foreach (double signal in signals.sPackage[_selectedChannel]) 
            { 
                seriesData.Add((new List<Vector2> { new Vector2(x, (float)signal) }, 0));
            }

            chart.AddPoints(seriesData);
            if (_isFit)
            {
                chart.Viewport = chart.GetSeriesBound(0);
            }
            chart.Update();
        }

        protected override void OnDisappearing()
        {  
            DeviceManager.Instance.StopSignal();
            base.OnDisappearing();
        }

        private void InitChannelPicker()
        {
            channelPicker.Items.Clear();
            foreach(ChannelType type in DeviceManager.Instance.Channels){
                channelPicker.Items.Add(type.ToString());
            }
            channelPicker.SelectedIndex = 0;
        }
        private void channelPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            _selectedChannel =  DeviceManager.Instance.Channels[channelPicker.SelectedIndex];
            _startTime = DateTime.Now;
            _offsetTime = TimeSpan.Zero;

            string name = "Channel " + channelPicker.Items[channelPicker.SelectedIndex];
            Color color = Color.FromRgb(_random.Next(30, 150), _random.Next(30, 150), _random.Next(30, 150));

            chart.Clear();

            _seriesX = new LinearChartSeries(2000, color) { Name = name };
            chart.InitSeries(new List<LinearChartSeries> { _seriesX });
        }

        private void SignalButton_Clicked(object sender, EventArgs e)
        {
            IsStarted = !IsStarted;

            if (IsStarted)
            {
                StartChart();
            }
            else
            {
                StopChart();
            }
        }

    }
}