using System;
using System.Linq;
using System.Threading.Tasks;

using BrainBitDemo.Utils;

using NeuroSDK;

using Xamarin.Forms;

namespace BrainBitDemo.NeuroImpl;

internal class BrainBitController
{
#region Singleton
    private static BrainBitController _instance = null;
    public static  BrainBitController Instance { get => _instance ??= new BrainBitController(); }
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

                _scanner ??= new Scanner(SensorFamily.SensorLEBrainBit, SensorFamily.SensorLEBrainBitBlack);

                _sensorFounded               =  sensorChanged;
                _scanner.EventSensorsChanged += _sensorFounded;
                _scanner.Start();
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
    private BrainBitSensor _sensor;

    public SensorStateChanged ConnectionStateChanged;
    public BatteryChanged     BatteryChanged;

    public SensorState ConnectionState { get => _sensor?.State ?? SensorState.StateOutOfRange; }

    public int BatteryPower { get => _sensor?.BattPower ?? 0; }

    public int SamplingFrequency { get => _sensor == null ? 0 : 250; }

    public Task<SensorState> CreateAndConnect(SensorInfo info)
    {
        SensorState outState = SensorState.StateOutOfRange;

        return Task<SensorState>.Factory.StartNew(
            () =>
            {
                try
                {
                    _sensor = _scanner.CreateSensor(info) as BrainBitSensor;

                    if (_sensor != null)
                    {
                        _sensor.EventSensorStateChanged += ConnectionStateChanged;
                        _sensor.EventBatteryChanged     += BatteryChanged;

                        outState = _sensor.State;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
#endregion

#region Signal
    public BrainBitSignalDataRecived SignalReceived;
    public BrainBitResistDataRecived ResistReceived;

    public void StartSignal()
    {
        _sensor.EventBrainBitSignalDataRecived += SignalReceived;
        _sensor?.ExecCommand(SensorCommand.CommandStartSignal);
    }

    public void StopSignal()
    {
        _sensor.EventBrainBitSignalDataRecived -= SignalReceived;
        _sensor?.ExecCommand(SensorCommand.CommandStopSignal);
    }

    public void StartResist()
    {
        _sensor.EventBrainBitResistDataRecived += ResistReceived;
        _sensor?.ExecCommand(SensorCommand.CommandStartResist);
    }

    public void StopResist()
    {
        _sensor.EventBrainBitResistDataRecived -= ResistReceived;
        _sensor?.ExecCommand(SensorCommand.CommandStopResist);
    }

#endregion

#region Info
    public string GetInfo()
    {
        if (_sensor?.State == SensorState.StateOutOfRange) return "Device unreachable!";

        var deviceInfo = "";

        var features = _sensor?.Features;
        deviceInfo += "\n===== [Features] =====\n";
        deviceInfo =  features?.Aggregate(deviceInfo, (current, feature) => current + $"[{feature}]: {_sensor.IsSupportedFeature(feature)}\n");

        var commands = _sensor?.Commands;
        deviceInfo += "\n===== [Commands] =====\n";
        deviceInfo =  commands?.Aggregate(deviceInfo, (current, command) => current + $" {command}\n");

        var parameters = _sensor?.Parameters;
        if (parameters == null) return deviceInfo;

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
            case SensorParameter.ParameterFirmwareVersion:
                deviceInfo += $" Sensor version (Access: {parameter.ParamAccess}): "
                            + $"[FW]: {_sensor.Version.FwMajor}.{_sensor.Version.FwMinor} "
                            + $"[HW]: {_sensor.Version.HwMajor}.{_sensor.Version.HwMinor}.{_sensor.Version.HwPatch} "
                            + $"[Ext]: {_sensor.Version.ExtMajor}\n";
                break;
            case SensorParameter.ParameterBattPower:
                deviceInfo += $" (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterSensorFamily:
                deviceInfo += $"Sensor family: {_sensor.SensFamily} (Access: {parameter.ParamAccess})\n";
                break;
            case SensorParameter.ParameterSensorMode:
                deviceInfo += $" Sensor mode (Access: {parameter.ParamAccess}): {_sensor.FirmwareMode}\n";
                break;
            case SensorParameter.ParameterExternalSwitchState:
            case SensorParameter.ParameterADCInputState:
            case SensorParameter.ParameterAccelerometerSens:
            case SensorParameter.ParameterGyroscopeSens:
            case SensorParameter.ParameterStimulatorAndMAState:
            case SensorParameter.ParameterStimulatorParamPack:
            case SensorParameter.ParameterMotionAssistantParamPack:
            case SensorParameter.ParameterMEMSCalibrationStatus:
            case SensorParameter.ParameterMotionCounterParamPack:
            case SensorParameter.ParameterMotionCounter:
            case SensorParameter.ParameterIrAmplitude:
            case SensorParameter.ParameterRedAmplitude:
            case SensorParameter.ParameterEnvelopeAvgWndSz:
            case SensorParameter.ParameterEnvelopeDecimation:
            case SensorParameter.ParameterSamplingFrequencyResist:
            case SensorParameter.ParameterSamplingFrequencyMEMS:
            case SensorParameter.ParameterSamplingFrequencyFPG:
            case SensorParameter.ParameterAmplifier:
            case SensorParameter.ParameterSensorChannels:
            case SensorParameter.ParameterSamplingFrequencyResp:
            case SensorParameter.ParameterSurveyId:
            case SensorParameter.ParameterFileSystemStatus:
            default:
                break;
            }
        }

        return deviceInfo;
    }
#endregion
}
