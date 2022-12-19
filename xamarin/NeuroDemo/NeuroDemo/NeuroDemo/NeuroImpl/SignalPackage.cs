using NeuroSDK;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeuroDemo.NeuroImpl
{
    public class SignalPackage
    {
        public Dictionary<ChannelType, double[]> sPackage = new Dictionary<ChannelType, double[]>();
        public SignalPackage() { }
    }
}
