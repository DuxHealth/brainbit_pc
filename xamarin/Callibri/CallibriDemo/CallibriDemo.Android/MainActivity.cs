using System.Linq;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

using NeuroSDK;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Platform = Xamarin.Essentials.Platform;

namespace CallibriDemo.Droid;

[Activity(Label = "CallibriDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : FormsAppCompatActivity
{
    internal static Context ActivityContext { get; private set; }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ActivityContext = this;

        NeuroSDKSetup.Initialize();

        Platform.Init(this, savedInstanceState);
        Forms.Init(this, savedInstanceState);
        LoadApplication(new App());
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        switch (requestCode)
        {
        case 333 when grantResults.Length == 1 && grantResults[0] == Permission.Granted:
            MessagingCenter.Send(this, "RequestBT", true);
            break;
        case 333:
            MessagingCenter.Send(this, "RequestBT", false);
            break;
        case 444 when grantResults.Length == 1 && grantResults[0] == Permission.Granted:
            MessagingCenter.Send(this, "RequestLocation", true);
            break;
        case 444:
            MessagingCenter.Send(this, "RequestLocation", false);
            break;

        case 222: {
            bool allGranted = grantResults.Cast<int>().All(result => result == (int)Permission.Granted);

            MessagingCenter.Send(this, "RequestPermissions", allGranted);
            break;
        }
        }

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        switch (requestCode)
        {
        case 555:
            MessagingCenter.Send(this, "EnableBT", resultCode == Result.Ok);
            break;
        case 666:
            MessagingCenter.Send(this, "EnableGPS", resultCode == Result.Ok);
            break;
        }

        base.OnActivityResult(requestCode, resultCode, data);
    }
}
