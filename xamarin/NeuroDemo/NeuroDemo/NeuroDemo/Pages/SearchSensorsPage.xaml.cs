using NeuroDemo.NeuroImpl;
using NeuroSDK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeuroDemo.Pages
{
    public class DeviceView
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string SerialNumber { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchSensorsPage : ContentPage
    {
        public string SearchingText
        {
            get
            {
                return _searchingText;
            }
            set
            {
                if (_searchingText != value)
                {
                    _searchingText = value;
                    OnPropertyChanged("SearchingText");
                }
            }
        }
        public IReadOnlyList<SensorInfo> FoundedDevices;


        private const string _startSearchText = "Start Search";
        private const string _stopSearchText = "Stop Search";

        private bool _isSearching = false;

        private string _searchingText = _startSearchText;
        
        public SearchSensorsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void sensorFounded(IReadOnlyList<NeuroSDK.SensorInfo> sensors)
        {
            FoundedDevices = sensors;
            List<DeviceView> deviceViews = new List<DeviceView>();
            foreach (NeuroSDK.SensorInfo sensor in sensors)
            {
                deviceViews.Add(new DeviceView() {
                   Name = sensor.Name,
                   Address = sensor.Address,
                   SerialNumber = sensor.SerialNumber
                });
                Console.WriteLine("[" + sensor.Name + "] " + sensor.Address);
            }
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DevicesList.ItemsSource = deviceViews;
            });
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            _isSearching = !_isSearching;

            if (_isSearching)
            {
                SearchingText = _stopSearchText;
                DeviceManager.Instance.StartSearch(sensorFounded);
            }
            else
            {
                SearchingText = _startSearchText;
                DeviceManager.Instance.StopSearch();
                DevicesList.ItemsSource = null;
            }
        }

        private async void DevicesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DeviceManager.Instance.Connect(FoundedDevices[e.SelectedItemIndex]);
            DeviceManager.Instance.StopSearch();
            await Navigation.PopAsync();
        }
        protected override void OnDisappearing()
        {
            if(_isSearching) DeviceManager.Instance.StopSearch();
            base.OnDisappearing();
        }
    }
}