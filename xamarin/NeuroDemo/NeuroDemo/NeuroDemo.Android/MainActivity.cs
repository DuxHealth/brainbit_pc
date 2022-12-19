using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Com.Neurosdk2.Helpers;
using Com.Neurosdk2.Helpers.Interfaces;
using System.Collections.Generic;
using Android.Content;
using Xamarin.Forms;

namespace NeuroDemo.Droid
{
    [Activity(Label = "NeuroDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static Context ActivityContext { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActivityContext = this;

            NeuroSDK.NeuroSDKSetup.Initialize();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == 333)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    MessagingCenter.Send<MainActivity, bool>(this, "RequestBT", true);
                }
                else
                {
                    MessagingCenter.Send<MainActivity, bool>(this, "RequestBT", false);
                }
            }

            if (requestCode == 444)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    MessagingCenter.Send<MainActivity, bool>(this, "RequestLocation", true);
                }
                else
                {
                    MessagingCenter.Send<MainActivity, bool>(this, "RequestLocation", false);
                }
            }

            if (requestCode == 222)
            {
                bool allGranted = true;
                foreach (int result in grantResults)
                {
                    if (result != (int)Permission.Granted)
                    {
                        allGranted = false;
                        break;
                    }
                }
                MessagingCenter.Send<MainActivity, bool>(this, "RequestPermissions", allGranted);
            }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 555)
            {
                MessagingCenter.Send<MainActivity, bool>(this, "EnableBT", resultCode == Result.Ok);
            }
            if (requestCode == 666)
            {
                MessagingCenter.Send<MainActivity, bool>(this, "EnableGPS", resultCode == Result.Ok);
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }


    }
}