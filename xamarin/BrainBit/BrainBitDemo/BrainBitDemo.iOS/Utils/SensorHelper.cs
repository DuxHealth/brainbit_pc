using System;

using BrainBitDemo.Utils;

using Xamarin.Forms;

[assembly: Dependency(typeof(BrainBitDemo.iOS.Utils.SensorHelper))]
namespace BrainBitDemo.iOS.Utils;

public class SensorHelper : ISensorHelper
{
    //TODO: Implement this class
    public void EnableSensor(Action<bool> enabled) { throw new NotImplementedException(); }
}
