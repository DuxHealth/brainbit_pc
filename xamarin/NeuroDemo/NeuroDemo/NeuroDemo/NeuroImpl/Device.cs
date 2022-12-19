using NeuroSDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeuroDemo.NeuroImpl
{

    internal class Device
    {
        ISensor sensor;

        public Action<int> batteryChanged;
        public Action<SensorState> connectionStateChanged;
        public Action<SignalPackage> onSignalRecieved { get; set; }

        public SensorState connectionState
        {
            get
            {
                SensorState st = SensorState.StateOutOfRange;
                try
                {
                    st = sensor.State;
                }
                finally
                { }
                return st;
            }
        }
        public int BattPower
        {
            get
            {
                int batt = 0;
                try
                {
                    batt = sensor.BattPower;
                }
                finally
                { }
                return batt;
            }
        }
        public int SamplingFrequency { get {
                int sf = 0;
                try
                {
                    SensorSamplingFrequency samplingFrequency = sensor.SamplingFrequency;
                    switch (samplingFrequency)
                    {
                        case SensorSamplingFrequency.FrequencyHz10:
                            sf = 10;
                            break;
                        case SensorSamplingFrequency.FrequencyHz100:
                            sf = 100;
                            break;
                        case SensorSamplingFrequency.FrequencyHz125:
                            sf = 125;
                            break;
                        case SensorSamplingFrequency.FrequencyHz250:
                            sf = 250;
                            break;
                        case SensorSamplingFrequency.FrequencyHz500:
                            sf = 500;
                            break;
                        case SensorSamplingFrequency.FrequencyHz1000:
                            sf = 1000;
                            break;
                        case SensorSamplingFrequency.FrequencyHz2000:
                            sf = 2000;
                            break;
                        case SensorSamplingFrequency.FrequencyHz4000:
                            sf = 4000;
                            break;
                        case SensorSamplingFrequency.FrequencyHz8000:
                            sf = 8000;
                            break;
                        case SensorSamplingFrequency.FrequencyUnsupported:
                            break;
                    }
                }
                finally
                { }
                return sf;
            }
        }

        public List<ChannelType> Channels
        {
            get
            {
                return sensor.SensFamily switch
                {
                    SensorFamily.SensorUnknown => throw new NotImplementedException(),
                    SensorFamily.SensorLECallibri => new List<ChannelType>() { ChannelType.Signal },
                    SensorFamily.SensorLEKolibri => new List<ChannelType>() { ChannelType.Signal },
                    SensorFamily.SensorLEBrainBit => new List<ChannelType>() { ChannelType.O1, ChannelType.O2, ChannelType.T3, ChannelType.T4 },
                    SensorFamily.SensorLEBrainBitBlack => new List<ChannelType>() { ChannelType.O1, ChannelType.O2, ChannelType.T3, ChannelType.T4 },
                    SensorFamily.SensorLEHeadPhones => throw new NotImplementedException(),
                    SensorFamily.SensorLESmartLeg => throw new NotImplementedException(),
                    SensorFamily.SensorLENeurro => throw new NotImplementedException(),
                    SensorFamily.SensorLEP300 => throw new NotImplementedException(),
                    SensorFamily.SensorLEImpulse => throw new NotImplementedException(),
                    SensorFamily.SensorLEHeadband => throw new NotImplementedException(),
                    SensorFamily.SensorLEEarBuds => throw new NotImplementedException(),
                    SensorFamily.SensorSPCompactNeuro => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
        }


        public bool Connect(DevScanner scanner, SensorInfo info)
        {
            try
            {
                sensor = scanner.scanner.CreateSensor(info);
            }
            catch(Exception ex) { 
            Console.WriteLine(ex.ToString());
                return false;
            }
            
            if(sensor != null )
            {
                sensor.EventSensorStateChanged += EventSensorStateChanged;
                sensor.EventBatteryChanged += EventBatteryChanged;

                switch(sensor.SensFamily)
                {
                    case SensorFamily.SensorLECallibri:
                    case SensorFamily.SensorLEKolibri:
                        sensor.EventCallibriSignalDataRecived += onCallibriSignalDataRecived;
                        break;
                    case SensorFamily.SensorLEBrainBit:
                    case SensorFamily.SensorLEBrainBitBlack:
                        sensor.EventBrainBitSignalDataRecived += onBrainBitSignalDataRecived;
                        break;
                    case SensorFamily.SensorLEHeadPhones:
                    case SensorFamily.SensorLESmartLeg:
                    case SensorFamily.SensorLENeurro:
                    case SensorFamily.SensorLEP300:
                    case SensorFamily.SensorLEImpulse:
                    case SensorFamily.SensorLEHeadband:
                    case SensorFamily.SensorLEEarBuds:
                    case SensorFamily.SensorSPCompactNeuro:
                    case SensorFamily.SensorUnknown:
                        break;
                }
            }
            return true;
        }

        public string DeviceInfo()
        {
            if (sensor?.State == SensorState.StateOutOfRange) return "Device unreachable!";
            string deviceInfo = "";
            deviceInfo += $"===== [Main info] =====\n";
            deviceInfo += $"[Name]: {sensor.Name}\n";
            deviceInfo += $"[Sensor family]: {sensor.SensFamily}\n";
            deviceInfo += $"[Address]: {sensor.Address}\n";
            deviceInfo += $"[Serial number]: {sensor.SerialNumber}\n";
            deviceInfo += $"[Firmware mode]: {sensor.FirmwareMode}\n";
            deviceInfo += $"[Sensor version]: " +
                $"[FW]: {sensor.Version.FwMajor}.{sensor.Version.FwMinor} " +
                $"[HW]: {sensor.Version.HwMajor}.{sensor.Version.HwMinor}.{sensor.Version.HwPatch} " +
                $"[Ext]: {sensor.Version.ExtMajor}\n";


            var features = sensor.Features;
            if (features != null)
            {
                deviceInfo += $"\n===== [Modules] =====\n";
                foreach (var feature in features)
                {
                    deviceInfo += $"[{feature}]: {sensor.IsSupportedFeature(feature)}\n";
                }
            }
            var commands = sensor.Commands;
            if (commands != null)
            {
                deviceInfo += $"\n===== [Commands] =====\n";
                foreach (var command in commands)
                {
                    deviceInfo += $"[{command}]: {sensor.IsSupportedCommand(command)}\n";
                }
            }
            var parameters = sensor.Parameters;
            if (parameters != null)
            {
                deviceInfo += $"===== [Parameters] =====";
                foreach (var parameter in parameters)
                {
                    deviceInfo += $"[{parameter.Param}]: [{sensor.IsSupportedParameter(parameter.Param)}][{parameter.ParamAccess}]\n";
                }
            }
            if (sensor.IsSupportedParameter(SensorParameter.ParameterSamplingFrequency))
                deviceInfo += $"\n[Sampling frequency]: {sensor.SamplingFrequency}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterGain))
                deviceInfo += $"[Gain]: {sensor.Gain}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterOffset))
                deviceInfo += $"[Data offset]: {sensor.DataOffset}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterExternalSwitchState))
                deviceInfo += $"[External switch input]: {sensor.ExtSwInput}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterADCInputState))
                deviceInfo += $"[ADCInput]: {sensor.ADCInput}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterAccelerometerSens))
                deviceInfo += $"[Accelerometer sens]: {sensor.AccSens}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterGyroscopeSens))
                deviceInfo += $"[GyroSens]: {sensor.GyroSens}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterSamplingFrequencyResist))
                deviceInfo += $"[SamplingFrequencyReist]: {sensor.SamplingFrequencyResist}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterSamplingFrequencyMEMS))
                deviceInfo += $"[SamplingFrequencyMEMS]: {sensor.SamplingFrequencyMEMS}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterSamplingFrequencyFPG))
                deviceInfo += $"[SamplingFrequencyFPG]: {sensor.SamplingFrequencyFPG}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterStimulatorAndMAState))
                deviceInfo += $"[StimulatorMAStateCallibri]: {sensor.StimulatorMAStateCallibri}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterStimulatorParamPack))
                deviceInfo += $"[StimulatorParamCallibri]: {sensor.MotionAssistantParamCallibri}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterMotionAssistantParamPack))
                deviceInfo += $"[MotionAssistantParamCallibri]: {sensor.MotionCounterParamCallibri}\n";

            if (sensor.IsSupportedParameter(SensorParameter.ParameterMotionCounterParamPack))
                deviceInfo += $"[MotionCounterParamCallibri]: {sensor.MotionCounterCallibri}\n";

            if (sensor.SensFamily == SensorFamily.SensorLECallibri || sensor.SensFamily == SensorFamily.SensorLEKolibri)
                deviceInfo += $"[ColorCallibri]: {sensor.ColorCallibri}\n";

            return deviceInfo;
        }

        private bool signalMode = false;
        public void StartSignal()
        {
            if (signalMode) return;
            try
            {
                sensor.ExecCommand(SensorCommand.CommandStartSignal);
                signalMode = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while starting signal: " + ex.Message);
                signalMode = false;
            }
        }

        public void StopSignal()
        {
            if (!signalMode) return;
            try
            {
                sensor.ExecCommand(SensorCommand.CommandStopSignal);
                signalMode = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while stopping signal: " + ex.Message);
                signalMode = false;
            }
        }
        private void onBrainBitSignalDataRecived(ISensor sensor, BrainBitSignalData[] data)
        {
            double[] samplesO1 = new double[data.Length];
            double[] samplesO2 = new double[data.Length];
            double[] samplesT3 = new double[data.Length];
            double[] samplesT4 = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                samplesO1[i] = data[i].O1;
                samplesO2[i] = data[i].O2;
                samplesT3[i] = data[i].T3;
                samplesT4[i] = data[i].T4;
            }
            Dictionary<ChannelType, double[]> samplesData = new Dictionary<ChannelType, double[]>
            {
                { ChannelType.O1, samplesO1 },
                { ChannelType.O2, samplesO2 },
                { ChannelType.T3, samplesT3 },
                { ChannelType.T4, samplesT4 }
            };

            SignalPackage samplesPackage = new SignalPackage()
            {
                sPackage = samplesData,
            };
            onSignalRecieved?.Invoke(samplesPackage);

        }

        private void onCallibriSignalDataRecived(ISensor sensor, CallibriSignalData[] data)
        {
            List<double> samples = new List<double>();
            Array.ForEach(data, (sample) => { samples.AddRange(sample.Samples); });
            Dictionary<ChannelType, double[]> samplesData = new Dictionary<ChannelType, double[]>
            {
                { ChannelType.Signal, samples.ToArray() }
            };
            SignalPackage samplesPackage = new SignalPackage()
            {
                sPackage = samplesData
            };
            onSignalRecieved?.Invoke(samplesPackage);

        }
        private void EventSensorStateChanged(ISensor sensor, SensorState sensorState)
        {
            connectionStateChanged?.Invoke(sensorState);
        }

        private void EventBatteryChanged(ISensor sensor, int battPower)
        {
            batteryChanged?.Invoke(battPower);
        }

        public void Disconnect()
        {
            if (sensor != null) {
                sensor.EventSensorStateChanged -= EventSensorStateChanged;
                sensor.EventBatteryChanged -= EventBatteryChanged;
                sensor.EventCallibriSignalDataRecived -= onCallibriSignalDataRecived;
                sensor.EventBrainBitSignalDataRecived -= onBrainBitSignalDataRecived;
            }
            sensor?.Disconnect();
        }

        public void Close()
        {
            sensor?.Dispose();
            sensor = null;
        }
    }
}
