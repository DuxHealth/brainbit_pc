using Android;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Java.Util.Concurrent.Atomic;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android.Widget;
using Android.Gms.Common;
using AndroidX.Core.Location;
using NeuroDemo.NeuroImpl;
using NeuroDemo.Utils;

[assembly: Dependency(typeof(NeuroDemo.Droid.Utils.SensorHelper))]
namespace NeuroDemo.Droid.Utils
{
    public class SensorHelper : ISensorHelper
    {
        private readonly int REQUEST_PERMISSIONS = 222;

        private static AtomicBoolean ready = new AtomicBoolean(false);
        private static Action<bool> sensorReadyAction;

        public void EnableSensor(Action<bool> enabled)
        {
            sensorReadyAction = enabled;
            requestPermissions();
        }

        private static void invokeSensorsReady()
        {
            sensorReadyAction?.Invoke(ready.Get());
        }

        private void requestPermissions()
        {
            bool permissionsGranted = Build.VERSION.SdkInt >= BuildVersionCodes.S ?
                (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.BluetoothScan) == (int)Permission.Granted &&
                 ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.BluetoothConnect) == (int)Permission.Granted) :
                (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.Bluetooth) == (int)Permission.Granted &&
                 ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted &&
                 ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.AccessCoarseLocation) == (int)Permission.Granted);

            if (permissionsGranted)
            {
                turnOnBT();
            }
            else
            {
                MessagingCenter.Subscribe<MainActivity, bool>(this, "RequestPermissions", (sender, granted) =>
                {
                    if (granted)
                    {
                        turnOnBT();
                    }
                    else
                    {
                        ready.Set(false);
                        invokeSensorsReady();
                    }
                    MessagingCenter.Unsubscribe<MainActivity, bool>(this, "RequestPermissions");
                });
                String[] permissions = Build.VERSION.SdkInt >= BuildVersionCodes.S ?
                    new String[] { Manifest.Permission.BluetoothScan, Manifest.Permission.BluetoothConnect } :
                    new String[] { Manifest.Permission.Bluetooth, Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation };
                ActivityCompat.RequestPermissions((Activity)MainActivity.ActivityContext, permissions, REQUEST_PERMISSIONS);
            }
        }

        private void turnOnBT()
        {
            BluetoothManager BTManager = MainActivity.ActivityContext.GetSystemService(Context.BluetoothService) as BluetoothManager;
            if (!BTManager.Adapter.IsEnabled)
            {
                MessagingCenter.Subscribe<MainActivity, bool>(this, "EnableBT", (sender, granted) =>
                {
                    MessagingCenter.Unsubscribe<MainActivity, bool>(this, "EnableBT");
                    if (granted)
                    {
                        turnOnGps();
                    }
                    else
                    {
                        ready.Set(false);
                        invokeSensorsReady();
                    }
                });
                Intent enableBT = new Intent(BluetoothAdapter.ActionRequestEnable);
                ((MainActivity)MainActivity.ActivityContext).StartActivityForResult(enableBT, 555);


            }
            else
            {
                turnOnGps();
            }

        }

        public async void turnOnGps()
        {
            try
            {
                LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
                if (!LocationManagerCompat.IsLocationEnabled(locationManager))
                {
                    if (isGooglePlayServicesAvailable(MainActivity.ActivityContext))
                    {
                        GoogleApiClient googleApiClient = new GoogleApiClient.Builder((MainActivity)MainActivity.ActivityContext).AddApi(LocationServices.API).Build();
                        googleApiClient.Connect();
                        Android.Gms.Location.LocationRequest locationRequest = Android.Gms.Location.LocationRequest.Create();
                        locationRequest.SetPriority(Android.Gms.Location.LocationRequest.PriorityLowPower);
                        locationRequest.SetInterval(5);
                        locationRequest.SetFastestInterval(5);

                        LocationSettingsRequest.Builder
                            locationSettingsRequestBuilder = new LocationSettingsRequest.Builder()
                            .AddLocationRequest(locationRequest);
                        locationSettingsRequestBuilder.SetAlwaysShow(false);

                        LocationSettingsResult locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
                            googleApiClient, locationSettingsRequestBuilder.Build());

                        MessagingCenter.Subscribe<MainActivity, bool>(this, "EnableGPS", (sender, granted) =>
                        {
                            MessagingCenter.Unsubscribe<MainActivity, bool>(this, "EnableGPS");
                            if (granted)
                            {
                                ready.Set(true);
                                invokeSensorsReady();
                            }
                            else
                            {
                                ready.Set(false);
                                invokeSensorsReady();
                            }
                        });

                        if (locationSettingsResult.Status.StatusCode == LocationSettingsStatusCodes.ResolutionRequired)
                        {
                            MessagingCenter.Subscribe<MainActivity, bool>(this, "EnableGPS", (sender, granted) =>
                            {
                                MessagingCenter.Unsubscribe<MainActivity, bool>(this, "EnableGPS");
                                if (granted)
                                {
                                    ready.Set(true);
                                    invokeSensorsReady();
                                }
                                else
                                {
                                    ready.Set(false);
                                    invokeSensorsReady();
                                }
                            });
                            locationSettingsResult.Status.StartResolutionForResult((MainActivity)MainActivity.ActivityContext, 666);
                        }
                        else
                        {
                            ready.Set(true);
                            invokeSensorsReady();
                        }
                    }
                    else
                    {
                        Intent intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                        ((MainActivity)MainActivity.ActivityContext).StartActivity(intent);
                    }
                }
                else
                {
                    ready.Set(true);
                    invokeSensorsReady();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public bool isGooglePlayServicesAvailable(Context context)
        {
            GoogleApiAvailability googleApiAvailability = GoogleApiAvailability.Instance;
            int resultCode = googleApiAvailability.IsGooglePlayServicesAvailable(context);
            return resultCode == ConnectionResult.Success;
        }
    }
}