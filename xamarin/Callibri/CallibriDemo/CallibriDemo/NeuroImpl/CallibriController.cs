using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using CallibriDemo.Utils;

using NeuroSDK;

using Xamarin.Forms;

namespace CallibriDemo.NeuroImpl;

internal class CallibriController
{
#region Info
    public string GetInfo()
    {
        if (_sensor?.State == SensorState.StateOutOfRange) return "Device unreachable!";

        var deviceInfo = "";

        var features = _sensor.Features;
        deviceInfo += "\n===== [Features] =====\n";
        deviceInfo =  features.Aggregate(deviceInfo, (current, feature) => current + $"[{feature}]: {_sensor.IsSupportedFeature(feature)}\n");

        var commands = _sensor.Commands;
        deviceInfo += "\n===== [Commands] =====\n";
        deviceInfo =  commands.Aggregate(deviceInfo, (current, command) => current + $" {command}\n");

        var parameters = _sensor.Parameters;
        deviceInfo += "\n===== [Parameters] =====\n";
        foreach (ParameterInfo parameter in parameters)
        {
            switch (parameter.Param)
            {
            case SensorParameter.ParameterName:
                deviceInfo += $" Name: {_sensor.Name} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterState:
                deviceInfo += $" Connection state: {_sensor.State} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterAddress:
                deviceInfo += $" Address: {_sensor.Address} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterSerialNumber:
                deviceInfo += $" Serial number: {_sensor.SerialNumber} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterHardwareFilterState:
                deviceInfo += $" Hardware filters (Access: {parameter.ParamAccess}): \n";
                deviceInfo =  _sensor.HardwareFilters.Aggregate(deviceInfo, (current, filter) => current + $" {filter}\n");
                break;
            case SensorParameter.ParameterFirmwareMode:
                deviceInfo += $" Firmware mode: {_sensor.FirmwareMode} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterSamplingFrequency:
                deviceInfo += $" Sampling frequency: {_sensor.SamplingFrequency} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterGain:
                deviceInfo += $" Gain: {_sensor.Gain} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterOffset:
                deviceInfo += $" Offset: {_sensor.DataOffset} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterExternalSwitchState:
                deviceInfo += $" External switch state: {_sensor.ExtSwInput} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterADCInputState:
                deviceInfo += $" ADCInput state: {_sensor.ADCInput} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterAccelerometerSens:
                deviceInfo += $" Accelerometer sensitivity: {_sensor.AccSens} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterGyroscopeSens:
                deviceInfo += $" Gyroscope sensitivity: {_sensor.GyroSens} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterStimulatorAndMAState:
                deviceInfo += $" Stimulator state: {_sensor.StimulatorMAStateCallibri.StimulatorState} (Access: {parameter.ParamAccess})\n";
                deviceInfo += $" Stimulator state: {_sensor.StimulatorMAStateCallibri.MAState} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterStimulatorParamPack:
                deviceInfo += $" Stimulator param pack (Access: {parameter.ParamAccess}):\n";
                deviceInfo += $"  PulseWidth: {_sensor.StimulatorParamCallibri.PulseWidth}\n";
                deviceInfo += $"  StimulusDuration: {_sensor.StimulatorParamCallibri.StimulusDuration}\n";
                deviceInfo += $"  Frequency: {_sensor.StimulatorParamCallibri.Frequency}\n";
                deviceInfo += $"  Current: {_sensor.StimulatorParamCallibri.Current}\n";
                break;
            case SensorParameter.ParameterMotionAssistantParamPack:
                deviceInfo += $" MotionAssistantParamPack (Access: {parameter.ParamAccess}):\n";
                deviceInfo += $"  GyroStop: {_sensor.MotionAssistantParamCallibri.GyroStop}\n";
                deviceInfo += $"  GyroStart: {_sensor.MotionAssistantParamCallibri.GyroStart}\n";
                deviceInfo += $"  Limb: {_sensor.MotionAssistantParamCallibri.Limb}\n";
                deviceInfo += $"  MinPauseMs: {_sensor.MotionAssistantParamCallibri.MinPauseMs}\n";
                break;
            case SensorParameter.ParameterFirmwareVersion:
                deviceInfo += $" Sensor version (Access: {parameter.ParamAccess}): "
                            + $"[FW]: {_sensor.Version.FwMajor}.{_sensor.Version.FwMinor} "
                            + $"[HW]: {_sensor.Version.HwMajor}.{_sensor.Version.HwMinor}.{_sensor.Version.HwPatch} "
                            + $"[Ext]: {_sensor.Version.ExtMajor}\n";
                break;
            case SensorParameter.ParameterMEMSCalibrationStatus:
                deviceInfo += $" {_sensor.MEMSCalibrateStateCallibri}(Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterMotionCounterParamPack:
                deviceInfo += $" MotionCounterParamPack (Access: {parameter.ParamAccess}):\n";
                deviceInfo += $"  InsenseThresholdMG: {_sensor.MotionCounterParamCallibri.InsenseThresholdMG}\n";
                deviceInfo += $"  InsenseThresholdSample: {_sensor.MotionCounterParamCallibri.InsenseThresholdSample}\n";
                break;
            case SensorParameter.ParameterMotionCounter:
                deviceInfo += $" Motion counter (Access: {parameter.ParamAccess}): {_sensor.MotionCounterCallibri}\n";
                break;
            case SensorParameter.ParameterBattPower:
                deviceInfo += $" Battery: {_sensor.BattPower} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterSensorFamily:
                deviceInfo += $"Sensor family: {_sensor.SensFamily} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterSensorMode:
                deviceInfo += $" Sensor mode (Access: {parameter.ParamAccess}): {_sensor.FirmwareMode}\n";
                break;
            case SensorParameter.ParameterSamplingFrequencyResp:
                deviceInfo += $" (Access: {parameter.ParamAccess})\n";
                break;
            }
        }

        deviceInfo += $" Signal type: {_sensor.SignalTypeCallibri}\n";

        deviceInfo += $" ColorCallibri: {_sensor.ColorCallibri}\n";

        return deviceInfo;
    }
#endregion

#region Singleton
    private static CallibriController _instance;

    public static CallibriController Instance { get => _instance ??= new CallibriController(); }
#endregion

#region Search
    private Scanner        _scanner;
    private SensorsChanged _sensorFounded;

    public void StartSearch(SensorsChanged sensorChanged)
    {
        ISensorHelper sensorHelper = DependencyService.Get<ISensorHelper>();

        sensorHelper.EnableSensor(
            enabled =>
            {
                if (!enabled) return;

                _scanner ??= new Scanner(SensorFamily.SensorLECallibri, SensorFamily.SensorLEKolibri);

                _sensorFounded               =  sensorChanged;
                _scanner.EventSensorsChanged += _sensorFounded;
                _scanner?.Start();
            }
        );
    }

    public void StopSearch()
    {
        _scanner?.Stop();
        if (_scanner != null) _scanner.EventSensorsChanged -= _sensorFounded;
        _sensorFounded = null;
    }
#endregion

#region Sensor
    private CallibriSensor _sensor;

    public SensorStateChanged ConnectionStateChanged;
    public BatteryChanged     BatteryChanged;

    public bool IsEnvelopeSupported
    {
        get
        {
            try
            {
                if (_sensor?.State == SensorState.StateInRange)
                {
                    return _sensor.IsSupportedCommand(SensorCommand.CommandStartEnvelope);
                }
                else return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return false;
        }
    }

    public SensorState ConnectionState { get => _sensor?.State ?? SensorState.StateOutOfRange; }

    public int BatteryPower { get => _sensor?.BattPower ?? 0; }

    public int SamplingFrequency
    {
        get
        {
            try
            {
                SensorSamplingFrequency samplingFrequency = _sensor?.SamplingFrequency ?? 0;
                switch (samplingFrequency)
                {
                case SensorSamplingFrequency.FrequencyHz10:
                    return 10;
                case SensorSamplingFrequency.FrequencyHz20:
                    return 20;
                case SensorSamplingFrequency.FrequencyHz100:
                    return 100;
                case SensorSamplingFrequency.FrequencyHz125:
                    return 125;
                case SensorSamplingFrequency.FrequencyHz250:
                    return 250;
                case SensorSamplingFrequency.FrequencyHz500:
                    return 500;
                case SensorSamplingFrequency.FrequencyHz1000:
                    return 1000;
                case SensorSamplingFrequency.FrequencyHz2000:
                    return 2000;
                case SensorSamplingFrequency.FrequencyHz4000:
                    return 4000;
                case SensorSamplingFrequency.FrequencyHz8000:
                    return 8000;
                case SensorSamplingFrequency.FrequencyUnsupported:
                default:
                    return 0;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return 0;
        }
    }

    public Task<SensorState> CreateAndConnect(SensorInfo info)
    {
        SensorState outState = SensorState.StateOutOfRange;
        return Task<SensorState>.Factory.StartNew(
            () =>
            {
                try
                {
                    _sensor = _scanner.CreateSensor(info) as CallibriSensor;
                    if (_sensor != null)
                    {
                        _sensor.EventSensorStateChanged += ConnectionStateChanged;
                        _sensor.EventBatteryChanged     += BatteryChanged;

                        _sensor.SamplingFrequency = SensorSamplingFrequency.FrequencyHz1000;

                        _sensor.SignalTypeCallibri = CallibriSignalType.ECG;
                        outState = _sensor.State;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return outState;
            }
        );
    }

    public Task<SensorState> ConnectCurrent()
    {
        return Task<SensorState>.Factory.StartNew(
            () =>
            {
                if (_sensor.State != SensorState.StateOutOfRange) return SensorState.StateInRange;

                _sensor?.Connect();
                return _sensor.State;
            }
        );
    }

    public void DisconnectCurrent()
    {
        try
        {
            _sensor?.Disconnect();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
#endregion

#region Signal
    public CallibriSignalDataRecived   SignalReceived;
    public CallibriEnvelopeDataRecived EnvelopeReceived;

    public void StartSignal()
    {
        _sensor.EventCallibriSignalDataRecived += SignalReceived;
        _sensor?.ExecCommand(SensorCommand.CommandStartSignal);
    }

    public void StopSignal()
    {
        _sensor.EventCallibriSignalDataRecived -= SignalReceived;
        _sensor?.ExecCommand(SensorCommand.CommandStopSignal);
    }

    public void StartEnvelope()
    {
        _sensor.EventCallibriEnvelopeDataRecived += EnvelopeReceived;
        _sensor?.ExecCommand(SensorCommand.CommandStartEnvelope);
    }

    public void StopEnvelope()
    {
        _sensor.EventCallibriEnvelopeDataRecived -= EnvelopeReceived;
        _sensor?.ExecCommand(SensorCommand.CommandStopEnvelope);
    }
#endregion
}
