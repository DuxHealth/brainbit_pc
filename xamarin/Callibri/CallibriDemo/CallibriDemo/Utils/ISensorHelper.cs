using System;

namespace CallibriDemo.Utils;

public interface ISensorHelper
{
    void EnableSensor(Action<bool> enabled);
}
