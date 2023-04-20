using System;

using Android;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Provider;

using AndroidX.Core.App;
using AndroidX.Core.Content;
using AndroidX.Core.Location;

using CallibriDemo.Droid.Utils;
using CallibriDemo.Utils;

using Java.Util.Concurrent.Atomic;

using Xamarin.Forms;

using Application = Android.App.Application;
using LocationRequest = Android.Gms.Location.LocationRequest;

[assembly: Dependency(typeof(SensorHelper))]

namespace CallibriDemo.Droid.Utils;

public class SensorHelper : ISensorHelper
{
    private const           int           REQUEST_PERMISSIONS = 222;
    private static readonly AtomicBoolean Ready               = new(false);

    private static Action<bool> _sensorReadyAction;

#region ISensorHelper Members
    public void EnableSensor(Action<bool> enabled)
    {
        _sensorReadyAction = enabled;
        RequestPermissions();
    }
#endregion

    private static void InvokeSensorsReady() { _sensorReadyAction?.Invoke(Ready.Get()); }

    private void RequestPermissions()
    {
        bool permissionsGranted = Build.VERSION.SdkInt >= BuildVersionCodes.S
            ? ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.BluetoothScan) == (int)Permission.Granted && ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.BluetoothConnect) == (int)Permission.Granted
            : ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.Bluetooth) == (int)Permission.Granted
           && ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted
           && ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.AccessCoarseLocation) == (int)Permission.Granted;

        if (permissionsGranted)
            turnOnBT();
        else
        {
            MessagingCenter.Subscribe<MainActivity, bool>(
                this,
                "RequestPermissions",
                (_, granted) =>
                {
                    if (granted)
                        turnOnBT();
                    else
                    {
                        Ready.Set(false);
                        InvokeSensorsReady();
                    }
                    MessagingCenter.Unsubscribe<MainActivity, bool>(this, "RequestPermissions");
                }
            );

            string[] permissions = Build.VERSION.SdkInt >= BuildVersionCodes.S
                ? new[] { Manifest.Permission.BluetoothScan, Manifest.Permission.BluetoothConnect }
                : new[] { Manifest.Permission.Bluetooth, Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation };
            ActivityCompat.RequestPermissions((Activity)MainActivity.ActivityContext, permissions, REQUEST_PERMISSIONS);
        }
    }

    private void turnOnBT()
    {
        var BTManager = MainActivity.ActivityContext.GetSystemService(Context.BluetoothService) as BluetoothManager;
        if (!BTManager.Adapter.IsEnabled)
        {
            MessagingCenter.Subscribe<MainActivity, bool>(
                this,
                "EnableBT",
                (sender, granted) =>
                {
                    MessagingCenter.Unsubscribe<MainActivity, bool>(this, "EnableBT");
                    if (granted)
                        turnOnGps();
                    else
                    {
                        Ready.Set(false);
                        InvokeSensorsReady();
                    }
                }
            );

            var enableBT = new Intent(BluetoothAdapter.ActionRequestEnable);
            ((MainActivity)MainActivity.ActivityContext).StartActivityForResult(enableBT, 555);
        }
        else
            turnOnGps();
    }

    public async void turnOnGps()
    {
        try
        {
            var locationManager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);

            if (!LocationManagerCompat.IsLocationEnabled(locationManager))
            {
                if (IsGooglePlayServicesAvailable(MainActivity.ActivityContext))
                {
                    GoogleApiClient googleApiClient = new GoogleApiClient.Builder((MainActivity)MainActivity.ActivityContext).AddApi(LocationServices.API).Build();
                    googleApiClient.Connect();
                    var locationRequest = LocationRequest.Create();
                    locationRequest.SetPriority(LocationRequest.PriorityLowPower);
                    locationRequest.SetInterval(5);
                    locationRequest.SetFastestInterval(5);

                    LocationSettingsRequest.Builder
                        locationSettingsRequestBuilder = new LocationSettingsRequest.Builder()
                           .AddLocationRequest(locationRequest);
                    locationSettingsRequestBuilder.SetAlwaysShow(false);

                    LocationSettingsResult locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
                        googleApiClient,
                        locationSettingsRequestBuilder.Build()
                    );

                    MessagingCenter.Subscribe<MainActivity, bool>(
                        this,
                        "EnableGPS",
                        (sender, granted) =>
                        {
                            MessagingCenter.Unsubscribe<MainActivity, bool>(this, "EnableGPS");
                            if (granted)
                            {
                                Ready.Set(true);
                                InvokeSensorsReady();
                            }
                            else
                            {
                                Ready.Set(false);
                                InvokeSensorsReady();
                            }
                        }
                    );

                    if (locationSettingsResult.Status.StatusCode == CommonStatusCodes.ResolutionRequired)
                    {
                        MessagingCenter.Subscribe<MainActivity, bool>(
                            this,
                            "EnableGPS",
                            (sender, granted) =>
                            {
                                MessagingCenter.Unsubscribe<MainActivity, bool>(this, "EnableGPS");
                                if (granted)
                                {
                                    Ready.Set(true);
                                    InvokeSensorsReady();
                                }
                                else
                                {
                                    Ready.Set(false);
                                    InvokeSensorsReady();
                                }
                            }
                        );
                        locationSettingsResult.Status.StartResolutionForResult((MainActivity)MainActivity.ActivityContext, 666);
                    }
                    else
                    {
                        Ready.Set(true);
                        InvokeSensorsReady();
                    }
                }
                else
                {
                    var intent = new Intent(Settings.ActionLocationSourceSettings);
                    ((MainActivity)MainActivity.ActivityContext).StartActivity(intent);
                }
            }
            else
            {
                Ready.Set(true);
                InvokeSensorsReady();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static bool IsGooglePlayServicesAvailable(Context context)
    {
        GoogleApiAvailability googleApiAvailability = GoogleApiAvailability.Instance;

        int resultCode = googleApiAvailability.IsGooglePlayServicesAvailable(context);

        return resultCode == ConnectionResult.Success;
    }
}
