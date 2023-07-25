using System;
using System.Collections.Generic;
using System.Linq;

using BrainBitDemo.NeuroImpl;
using BrainBitDemo.Pages.ChooseFilters;
using NeuroSDK;
using Neurotech.Filters;
using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SignalsPage
{
    #region Filters init

    private FilterList _currentFilterListO1;
    public FilterList CurrentFilterListO1
    {
        get
        {
            lock (_lockObj) return _currentFilterListO1;
        }

        set
        {
            lock (_lockObj) _currentFilterListO1 = value;
        }
    }
    private FilterList _currentFilterListO2;
    public FilterList CurrentFilterListO2
    {
        get
        {
            lock (_lockObj) return _currentFilterListO2;
        }

        set
        {
            lock (_lockObj) _currentFilterListO2 = value;
        }
    }
    private FilterList _currentFilterListT3;
    public FilterList CurrentFilterListT3
    {
        get
        {
            lock (_lockObj) return _currentFilterListT3;
        }

        set
        {
            lock (_lockObj) _currentFilterListT3 = value;
        }
    }
    private FilterList _currentFilterListT4;
    public FilterList CurrentFilterListT4
    {
        get
        {
            lock (_lockObj) return _currentFilterListT4;
        }

        set
        {
            lock (_lockObj) _currentFilterListT4 = value;
        }
    }
    private readonly object _lockObj = new();

    #endregion

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

    private List<FilterParam> _selectedFilters;

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

        CurrentFilterListO1?.FilterArray(valuesO1);
        CurrentFilterListO2?.FilterArray(valuesO2);
        CurrentFilterListT3?.FilterArray(valuesT3);
        CurrentFilterListT4?.FilterArray(valuesT4);

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

    private async void Filters_Clicked(object sender, EventArgs e)
    {
        var filtersPopUp = new ChooseFiltersPage(_selectedFilters);
        _selectedFilters = await filtersPopUp.Show(Navigation);

        if (_selectedFilters is { Count: > 0 })
        {
            var newFilterListO1 = new FilterList();
            foreach (FilterParam item in _selectedFilters) newFilterListO1.AddFilter(new Filter(item));
            CurrentFilterListO1 = newFilterListO1;

            var newFilterListO2 = new FilterList();
            foreach (FilterParam item in _selectedFilters) newFilterListO2.AddFilter(new Filter(item));
            CurrentFilterListO2 = newFilterListO2;

            var newFilterListT3 = new FilterList();
            foreach (FilterParam item in _selectedFilters) newFilterListT3.AddFilter(new Filter(item));
            CurrentFilterListT3 = newFilterListT3;

            var newFilterListT4 = new FilterList();
            foreach (FilterParam item in _selectedFilters) newFilterListT4.AddFilter(new Filter(item));
            CurrentFilterListT4 = newFilterListT4;
        }
        else
        {
            CurrentFilterListO1 = null;
            CurrentFilterListO2 = null;
            CurrentFilterListT3 = null;
            CurrentFilterListT4 = null;
        }
    }

}
