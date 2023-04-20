using System;

namespace BrainBitDemo.Utils;

public interface ISensorHelper
{
    void EnableSensor(Action<bool> enabled);
}
