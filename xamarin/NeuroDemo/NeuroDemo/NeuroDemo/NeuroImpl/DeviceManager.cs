using NeuroSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroDemo.NeuroImpl
{
    internal class DeviceManager
    {
        #region Singleton
        private static DeviceManager _instance = null;
        public static DeviceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DeviceManager();
                }
                return _instance;
            }
        }
        #endregion

        #region Search

        private DevScanner scanner;
        private Device CurrentDevice;

        public Action<SensorState> connectionStateChanged;
        public Action<int> batteryChanged;

        public SensorState ConnectionState => CurrentDevice == null ? SensorState.StateOutOfRange : CurrentDevice.connectionState;
        public int BatteryPower => CurrentDevice == null ? 0 : CurrentDevice.BattPower;
        public int SamplingFrequency => CurrentDevice == null ? 0 : CurrentDevice.SamplingFrequency;
        public List<ChannelType> Channels => CurrentDevice == null ? new List<ChannelType>() : CurrentDevice.Channels;

        public void StartSearch(Action<IReadOnlyList<SensorInfo>> deviceFounded)
        {
            if (scanner == null)
                scanner = new DevScanner();
            if (CurrentDevice != null)
            {
                Disconnect();
            }
            scanner.sensorFounded = deviceFounded;
            scanner?.StartSearch();
        }
        public void StopSearch()
        {
            scanner?.StopSearch();
            scanner.sensorFounded = null;
        }

        #endregion

        #region Connect

        public void Connect(SensorInfo info)
        {
            CurrentDevice = new Device();
            CurrentDevice.connectionStateChanged = connectionStateChanged;
            CurrentDevice.batteryChanged = batteryChanged;
            CurrentDevice.Connect(scanner, info);
        }

        public void Disconnect()
        {
            CurrentDevice.connectionStateChanged = null;
            CurrentDevice.batteryChanged = null;
            CurrentDevice.Disconnect();
            CurrentDevice.Close();
            CurrentDevice = null;
        }

        #endregion

        #region Info
        public string GetInfo()
        {
            return CurrentDevice == null ? "" : CurrentDevice.DeviceInfo();
        }
        #endregion

        #region Signal
        public void StartSignal(Action<SignalPackage> signalReceived)
        {
            CurrentDevice.onSignalRecieved = signalReceived;
            CurrentDevice?.StartSignal();
        }

        public void StopSignal()
        {
            CurrentDevice.onSignalRecieved = null;
            CurrentDevice?.StopSignal();
        }
        #endregion
    }
}
