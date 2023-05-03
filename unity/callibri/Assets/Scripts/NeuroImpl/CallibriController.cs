using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroSDK;

public sealed class CallibriController
{
    #region Singleton
    private static readonly Lazy<CallibriController> lazy =
            new Lazy<CallibriController>(() => new CallibriController());

    public static CallibriController Instance { get { return lazy.Value; } }

    private CallibriController() { }
    #endregion
    private Scanner _scanner;
    private CallibriSensor _sensor;

    public SensorState ConnectionState => _sensor?.State ?? SensorState.StateOutOfRange;

    public event Action<IReadOnlyList<SensorInfo>> sensorsFound;

    public event Action<int> batteryChanged;
    public event Action<SensorState> connectionStateChanged;

    public event Action<CallibriSignalData[]> signalReceived;
    public event Action<CallibriEnvelopeData[]> envelopeReceived;
    public event Action<CallibriElectrodeState> electrodeStateChanged;

    #region Scanner

    public void StartSearch()
    {
        if (_scanner == null)
        {
            _scanner = new Scanner(SensorFamily.SensorLECallibri, SensorFamily.SensorLEKolibri);
            _scanner.EventSensorsChanged += OnSensorsFound;
        }

        _scanner.Start();
    }

    public void OnSensorsFound(IScanner scanner, IReadOnlyList<SensorInfo> sensors)
    {
        sensorsFound?.Invoke(sensors);
    }

    public void StopSearch()
    {
        _scanner?.Stop();
    }

    #endregion

    #region Sensor state

    public async Task<SensorState> CreateAndConnect(SensorInfo sensorInfo)
    {
        _sensor = await Task.Run(() => _scanner.CreateSensor(sensorInfo)) as CallibriSensor;

        if (_sensor is null)
            return SensorState.StateOutOfRange;

        
        _sensor.EventBatteryChanged += OnBatteryChanged;
        _sensor.EventSensorStateChanged += OnSensorStateChanged;
        
        _sensor.Gain = SensorGain.SensorGain6;
        _sensor.DataOffset = SensorDataOffset.DataOffset3;
        _sensor.ADCInput = SensorADCInput.ADCInputResistance;
        _sensor.SamplingFrequency = SensorSamplingFrequency.FrequencyHz250;
        _sensor.ExtSwInput = SensorExternalSwitchInput.ExtSwInMioElectrodes;

        connectionStateChanged?.Invoke(SensorState.StateInRange);
        return SensorState.StateInRange;
    }

    private void OnBatteryChanged(ISensor sensor, int battPower) => batteryChanged?.Invoke(battPower);
    private void OnSensorStateChanged(ISensor sensor, SensorState sensorState) => connectionStateChanged?.Invoke(sensorState);

    public async Task<SensorState> ConnectCurrent()
    {
        if (_sensor is null) return SensorState.StateOutOfRange;
        if (_sensor.State == SensorState.StateInRange) return SensorState.StateInRange;

        await Task.Run(() => _sensor.Connect());

        return _sensor.State;
    }

    public void DisconnectCurrent()
    {
        if (_sensor?.State == SensorState.StateInRange)
            _sensor.Disconnect();
    }

    public void CloseSensor() => _sensor?.Dispose();

    #endregion

    #region Parameters
    public string FullInfo()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Features\n");
        var featuresLines = SensorInfoProvider.GetSensorFeatures(_sensor).Select(x => x.ToString());
        stringBuilder.Append(string.Join(Environment.NewLine, featuresLines));
        stringBuilder.Append("\nCommands\n");
        var commandLines = SensorInfoProvider.GetSensorCommands(_sensor).Select(x => x.ToString());
        stringBuilder.Append(string.Join(Environment.NewLine, commandLines));
        stringBuilder.Append("\nParameters\n");
        var parameterLines = SensorInfoProvider.GetCallibriSensorParameters(_sensor).Select
            (kvp => kvp.Key.ToString() + " - " + kvp.Value);
        stringBuilder.Append(string.Join(Environment.NewLine, parameterLines));
        return stringBuilder.ToString();
    }

    #endregion

    #region Signal

    public void StartSignal()
    {
        _sensor.EventCallibriSignalDataRecived += OnSignalDataRecived;

        executeCommand(SensorCommand.CommandStartSignal);
    }

    public void StopSignal()
    {
        _sensor.EventCallibriSignalDataRecived -= OnSignalDataRecived;

        executeCommand(SensorCommand.CommandStopSignal);
    }

    private void OnSignalDataRecived(ISensor sensor, CallibriSignalData[] data) => signalReceived?.Invoke(data);
    #endregion

    #region Envelope

    public void StartEnvelope()
    {
        _sensor.EventCallibriEnvelopeDataRecived += OnEnvelopeDataRecived;
        executeCommand(SensorCommand.CommandStartSignal);
    }

    public void StopEnvelope()
    {
        _sensor.EventCallibriEnvelopeDataRecived -= OnEnvelopeDataRecived;
        executeCommand(SensorCommand.CommandStopSignal);
    }

    private void OnEnvelopeDataRecived(ISensor sensor, CallibriEnvelopeData[] data) => envelopeReceived?.Invoke(data);

    #endregion

    #region ElectrodeState

    public void StartElectrode()
    {
        _sensor.EventCallibriElectrodeStateChanged += OnElectrodeStateChanged;
        executeCommand(SensorCommand.CommandStartSignal);
    }

    public void StopElectrode()
    {
        _sensor.EventCallibriEnvelopeDataRecived -= OnEnvelopeDataRecived;
        executeCommand(SensorCommand.CommandStopSignal);
    }

    private void OnElectrodeStateChanged(ISensor sensor, CallibriElectrodeState state) => electrodeStateChanged?.Invoke(state);

    #endregion

    private void executeCommand(SensorCommand command)
    {
        try
        {
            if (_sensor?.IsSupportedCommand(command) == true)
            {
                _sensor?.ExecCommand(command);

                //onCommandExecuted(true)
            }
            else
            {
                //onCommandExecuted(false)
            }
        }
        catch (Exception ex)
        {

        }
    }

}
