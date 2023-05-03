using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuroSDK
{
    public static class SensorInfoProvider
    {
        public static Dictionary<string, string> GetCallibriSensorParameters(CallibriSensor sensor)
        {
            var dictionary = GetGeneralSensorParameter(sensor);
            var parameters = sensor.Parameters;
            foreach (var parInfo in parameters)
            {
                var paramName = parInfo.Param.ToString().Replace("Parameter", "");
                var paramValue = parInfo.Param switch
                {
                    SensorParameter.ParameterHardwareFilterState =>sensor.HardwareFilters.ToString(), // fix name
                    SensorParameter.ParameterExternalSwitchState => sensor.ExtSwInput.ToString(),
                    SensorParameter.ParameterADCInputState => sensor.ADCInput.ToString(),
                    SensorParameter.ParameterAccelerometerSens => sensor.AccSens.ToString(),
                    SensorParameter.ParameterGyroscopeSens => sensor.GyroSens.ToString(),
                    SensorParameter.ParameterStimulatorAndMAState => sensor.StimulatorMAStateCallibri.ToString(),
                    SensorParameter.ParameterStimulatorParamPack => sensor.StimulatorParamCallibri.ToString(),
                    SensorParameter.ParameterMotionAssistantParamPack => sensor.MotionAssistantParamCallibri.ToString(),
                    SensorParameter.ParameterMotionCounterParamPack => sensor.MotionCounterParamCallibri.ToString(),
                    SensorParameter.ParameterMotionCounter => sensor.MotionCounterCallibri.ToString(),
                    SensorParameter.ParameterMEMSCalibrationStatus => sensor.MEMSCalibrateStateCallibri.ToString(),
                    _ => null
                };
                if (paramValue != null)
                {
                    dictionary[paramName] = paramValue;
                }
            }

            dictionary["CallibriColor"] = sensor.ColorCallibri.ToString();
            if (sensor.IsSupportedFeature(SensorFeature.FeatureSignal))
            {
                dictionary["CallibriSignalType"] = sensor.SignalTypeCallibri.ToString();
            }
            return dictionary;
        }

        public static List<string> GetSensorCommands(ISensor sensor)
        {
            return sensor.Commands.Select(x => x.ToString().Replace("Command", "")).ToList();
        }

        public static List<string> GetSensorFeatures(ISensor sensor)
        {
            return sensor.Features.Select(x => x.ToString().Replace("Feature", "")).ToList();
        }

        private static Dictionary<string, string> GetGeneralSensorParameter(ISensor sensor)
        {
            var dictionary = new Dictionary<string, string>();
            var parameters = sensor.Parameters;
            foreach (var parInfo in parameters)
            {
                var paramName = parInfo.Param.ToString().Replace("Parameter", "");
                var paramValue = parInfo.Param switch
                {
                    SensorParameter.ParameterName => sensor.Name,
                    SensorParameter.ParameterSensorFamily => sensor.SensFamily.ToString(),
                    SensorParameter.ParameterAddress => sensor.Address,
                    SensorParameter.ParameterSerialNumber => sensor.SerialNumber,
                    SensorParameter.ParameterBattPower => sensor.BattPower.ToString(),
                    SensorParameter.ParameterState => sensor.State.ToString(),
                    SensorParameter.ParameterSamplingFrequency => sensor.SamplingFrequency.ToString(),
                    SensorParameter.ParameterGain => sensor.Gain.ToString(),
                    SensorParameter.ParameterOffset => sensor.DataOffset.ToString(),
                    SensorParameter.ParameterFirmwareMode => sensor.FirmwareMode.ToString(),
                    _ => null
                };
                if (paramValue != null)
                {
                    dictionary[paramName] = paramValue;
                }

                var ver = sensor.Version;
                dictionary["ExtMajor"] = ver.ExtMajor.ToString();
                dictionary["FwMajor"] = ver.FwMajor.ToString();
                dictionary["HwMajor"] = ver.HwMajor.ToString();
                dictionary["FwMinor"] = ver.FwMinor.ToString();
                dictionary["HwMinor"] = ver.HwMinor.ToString();
                dictionary["FwPatch"] = ver.FwPatch.ToString();
                dictionary["HwPatch"] = ver.HwPatch.ToString();
            }

            return dictionary;
        }
    }
}