using System;
using System.Collections.Generic;
using System.Text;

namespace NeuroDemo.Utils
{
    public interface ISensorHelper
    {
        void EnableSensor(Action<bool> enabled);
    }

}
