using System;
using System.Collections.Generic;

using BrainBitDemo.NeuroImpl;

using NeuroSDK;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainBitDemo.Pages;

public class DeviceView
{
    public string Name { get; set; }

    public string Address { get; set; }

    public string SerialNumber { get; set; }
}

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SearchSensorsPage
{
    public string SearchingText { get => IsSearching ? "Stop Search" : "Start Search"; }

    private bool _isSearching;

    private bool IsSearching
    {
        get => _isSearching;

        set
        {
            if (_isSearching == value) return;

            _isSearching = value;

            OnPropertyChanged(SearchingText);
        }
    }

    public IReadOnlyList<SensorInfo> FoundedDevices;

    public SearchSensorsPage()
    {
        InitializeComponent();
        BindingContext = this;
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
        IsSearching = !IsSearching;

        SearchButton.Text = SearchingText;

        if (IsSearching)
            BrainBitController.Instance.StartSearch(SensorFounded);
        else
        {
            BrainBitController.Instance.StopSearch();
            DevicesList.ItemsSource = null;
        }
    }

    private async void DevicesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        await BrainBitController.Instance.CreateAndConnect(FoundedDevices[e.SelectedItemIndex]);
        BrainBitController.Instance.StopSearch();
        await Navigation.PopAsync();
    }

    protected override void OnDisappearing()
    {
        if (_isSearching) BrainBitController.Instance.StopSearch();
        base.OnDisappearing();
    }
}
