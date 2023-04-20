using System;

using CallibriDemo.Utils;
using CallibriDemo.UWP.Utils;

using Xamarin.Forms;

[assembly: Dependency(typeof(SensorHelper))]

namespace CallibriDemo.UWP.Utils;

public class SensorHelper : ISensorHelper
{
#region ISensorHelper Members
    public void EnableSensor(Action<bool> enabled) { enabled?.Invoke(true); }
#endregion
}
