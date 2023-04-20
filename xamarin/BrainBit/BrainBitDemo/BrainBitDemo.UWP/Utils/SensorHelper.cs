using System;

using BrainBitDemo.Utils;

using Xamarin.Forms;

[assembly: Dependency(typeof(BrainBitDemo.UWP.Utils.SensorHelper))]

namespace BrainBitDemo.UWP.Utils;

public class SensorHelper : ISensorHelper
{
    public void EnableSensor(Action<bool> enabled) { enabled.Invoke(true); }
}
