using System;
using System.Collections.Generic;

using CallibriDemo.NeuroImpl;

using NeuroSDK;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallibriDemo.Pages;

public class DeviceView
{
    public string Name { get; set; }

    public string Address { get; set; }

    public string SerialNumber { get; set; }
}

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SearchSensorsPage
{
    private const string StartSearchText = "Start Search";
    private const string StopSearchText  = "Stop Search";

    private bool _isSearching;

    private string _searchingText = StartSearchText;

    public IReadOnlyList<SensorInfo> FoundedDevices;

    public SearchSensorsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public string SearchingText
    {
        get => _searchingText;

        set
        {
            if (_searchingText == value) return;

            _searchingText = value;
            OnPropertyChanged();
        }
    }

    private void SensorFounded(IScanner scanner, IReadOnlyList<SensorInfo> sensors)
    {
        FoundedDevices = sensors;
        var deviceViews = new List<DeviceView>();
        foreach (SensorInfo sensor in sensors)
        {
            deviceViews.Add(new DeviceView { Name = sensor.Name, Address = sensor.Address, SerialNumber = sensor.SerialNumber });
            Console.WriteLine("[" + sensor.Name + "] " + sensor.Address);
        }
        MainThread.BeginInvokeOnMainThread(() => { DevicesList.ItemsSource = deviceViews; });
    }

    private void SearchButton_Clicked(object sender, EventArgs e)
    {
        _isSearching = !_isSearching;

        if (_isSearching)
        {
            SearchingText = StopSearchText;
            CallibriController.Instance.StartSearch(SensorFounded);
        }
        else
        {
            SearchingText = StartSearchText;
            CallibriController.Instance.StopSearch();
            DevicesList.ItemsSource = null;
        }
    }

    private async void DevicesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        await CallibriController.Instance.CreateAndConnect(FoundedDevices[e.SelectedItemIndex]);
        CallibriController.Instance.StopSearch();
        await Navigation.PopAsync();
    }

    protected override void OnDisappearing()
    {
        if (_isSearching) CallibriController.Instance.StopSearch();
        base.OnDisappearing();
    }
}
