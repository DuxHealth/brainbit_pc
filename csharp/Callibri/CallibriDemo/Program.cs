using NeuroSDK;
using System;

Console.WriteLine("Search devices (15s):");
Scanner scanner = new Scanner(SensorFamily.SensorLECallibri, SensorFamily.SensorLEKolibri);
scanner.EventSensorsChanged += onDeviceFound;
scanner.Start();
Thread.Sleep(15000);
Console.WriteLine("Cancel search");
scanner.Stop();
scanner.EventSensorsChanged -= onDeviceFound;


foreach (SensorInfo sens in scanner.Sensors) {
    Console.WriteLine($"Connect to {sens.Name} ({sens.Address})");
    CallibriSensor? sensor = scanner.CreateSensor(sens) as CallibriSensor;
    if (sensor != null) {
        Console.WriteLine($"Successfully connected to {sens.Name}!");

        sensor.EventBatteryChanged += Sensor_EventBatteryChanged;
        sensor.EventSensorStateChanged += Sensor_EventSensorStateChanged;

        sensor.SignalTypeCallibri = CallibriSignalType.ECG;

        var features = sensor?.Features;
        Console.WriteLine($"{sensor?.Name} features:");
        foreach (var feature in features) 
        {
            Console.WriteLine(feature.ToString());
        }

        var commands = sensor?.Commands;
        Console.WriteLine($"{sensor?.Name} commands: ");
        foreach (var command in commands)
        {
            Console.WriteLine(command.ToString());
        }

        var parameters = sensor?.Parameters;
        Console.WriteLine($"{sensor.Name} parameters:");
        foreach (ParameterInfo parameter in parameters)
        {
            switch (parameter.Param)
            {
                case SensorParameter.ParameterName:
                    Console.WriteLine($"Name: {sensor?.Name} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterState:
                    Console.WriteLine($"Connection state: {sensor?.State} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterAddress:
                    Console.WriteLine($"Address: {sensor?.Address} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterSerialNumber:
                    Console.WriteLine($"Serial number: {sensor?.SerialNumber} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterFirmwareMode:
                    Console.WriteLine($"Firmware mode: {sensor?.FirmwareMode} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterSamplingFrequency:
                    Console.WriteLine($"Sampling frequency: {sensor?.SamplingFrequency} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterGain:
                    Console.WriteLine($"Gain: {sensor?.Gain} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterOffset:
                    Console.WriteLine($"Offset: {sensor?.DataOffset} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterFirmwareVersion:
                    Console.WriteLine($"Sensor version (Access: {parameter.ParamAccess}): "
                                + $"[FW]: {sensor?.Version.FwMajor}.{sensor?.Version.FwMinor} "
                                + $"[HW]: {sensor?.Version.HwMajor}.{sensor?.Version.HwMinor}.{sensor?.Version.HwPatch} "
                                + $"[Ext]: {sensor?.Version.ExtMajor}");
                    break;
                case SensorParameter.ParameterBattPower:
                    Console.WriteLine($"Battery: {sensor?.BattPower} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterSensorFamily:
                    Console.WriteLine($"Sensor family: {sensor?.SensFamily} (Access: {parameter.ParamAccess})");
                    break;
                case SensorParameter.ParameterSensorMode:
                    Console.WriteLine($"Sensor mode (Access: {parameter.ParamAccess}): {sensor?.FirmwareMode}");
                    break;
                case SensorParameter.ParameterExternalSwitchState:
                    Console.WriteLine($"External switch input (Access: {parameter.ParamAccess}): {sensor?.ExtSwInput}");
                    break;
                case SensorParameter.ParameterADCInputState:
                    Console.WriteLine($"ADC input (Access: {parameter.ParamAccess}): {sensor?.ADCInput}");
                    break;
                case SensorParameter.ParameterHardwareFilterState:
                    Console.WriteLine($"ADC input (Access: {parameter.ParamAccess}): {sensor?.HardwareFilters}");
                    break;
                case SensorParameter.ParameterAccelerometerSens:
                    Console.WriteLine($"Accelerometer sensitivity (Access: {parameter.ParamAccess}): {sensor?.AccSens}");
                    break;
                case SensorParameter.ParameterGyroscopeSens:
                    Console.WriteLine($"Gyroscope sensitivity (Access: {parameter.ParamAccess}): {sensor?.GyroSens}");
                    break;
                case SensorParameter.ParameterStimulatorAndMAState:
                    Console.WriteLine($"StimulatorAndMAState (Access: {parameter.ParamAccess}): StimulatorState - {sensor?.StimulatorMAStateCallibri.StimulatorState}, {sensor?.StimulatorMAStateCallibri.MAState}");
                    break;
                case SensorParameter.ParameterStimulatorParamPack:
                    Console.WriteLine($"Stimulator param (Access: {parameter.ParamAccess}): PulseWidth - {sensor?.StimulatorParamCallibri.PulseWidth}," +
                        $" StimulusDuration - {sensor?.StimulatorParamCallibri.StimulusDuration}, " +
                        $"Current - {sensor?.StimulatorParamCallibri.Current}, " +
                        $"Frequency - {sensor?.StimulatorParamCallibri.Frequency}");
                    break;
                case SensorParameter.ParameterMotionAssistantParamPack:
                    Console.WriteLine($"MotionAssistant param (Access: {parameter.ParamAccess}): " +
                        $"GyroStop - {sensor?.MotionAssistantParamCallibri.GyroStop}," +
                        $" GyroStart - {sensor?.MotionAssistantParamCallibri.GyroStart}, " +
                        $"Limb - {sensor?.MotionAssistantParamCallibri.Limb}, " +
                        $"MinPauseMs - {sensor?.MotionAssistantParamCallibri.MinPauseMs}");
                    break;
                case SensorParameter.ParameterMEMSCalibrationStatus:
                    Console.WriteLine($"MotionAssistant param (Access: {parameter.ParamAccess}): {sensor?.MEMSCalibrateStateCallibri}");
                    break;
                case SensorParameter.ParameterMotionCounterParamPack:
                    Console.WriteLine($"MotionAssistant param (Access: {parameter.ParamAccess}): " +
                        $"InsenseThresholdMG - {sensor?.MotionCounterParamCallibri.InsenseThresholdMG}, " +
                        $"InsenseThresholdSample - {sensor?.MotionCounterParamCallibri.InsenseThresholdSample}");
                    break;
                case SensorParameter.ParameterMotionCounter:
                    Console.WriteLine($"MotionAssistant param (Access: {parameter.ParamAccess}): {sensor?.MotionCounterCallibri}");
                    break;
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

        Console.WriteLine($"Envelope (10s) in mV:");
        sensor.EventCallibriEnvelopeDataRecived += Sensor_EventCallibriEnvelopeDataRecived;
        sensor.ExecCommand(SensorCommand.CommandStartEnvelope);
        Thread.Sleep(10000);
        sensor.ExecCommand(SensorCommand.CommandStopEnvelope);
        sensor.EventCallibriEnvelopeDataRecived -= Sensor_EventCallibriEnvelopeDataRecived;

        Console.WriteLine($"Signal (10s) in mV:");
        sensor.EventCallibriElectrodeStateChanged += Sensor_EventCallibriElectrodeStateChanged;
        sensor.EventCallibriSignalDataRecived += Sensor_EventCallibriSignalDataRecived; ;
        sensor.ExecCommand(SensorCommand.CommandStartSignal);
        Thread.Sleep( 10000 );
        sensor.ExecCommand(SensorCommand.CommandStopSignal);
        sensor.EventCallibriElectrodeStateChanged -= Sensor_EventCallibriElectrodeStateChanged;
        sensor.EventCallibriSignalDataRecived -= Sensor_EventCallibriSignalDataRecived;

        Console.WriteLine($"Disconnecting from {sensor.Name}...");
        sensor.Disconnect();
        sensor.EventBatteryChanged -= Sensor_EventBatteryChanged;
        sensor.EventSensorStateChanged -= Sensor_EventSensorStateChanged;
        Console.WriteLine($"And remove device...");
        sensor.Dispose();
        sensor = null;
    }
}

void Sensor_EventCallibriElectrodeStateChanged(ISensor sensor, CallibriElectrodeState elState)
{
    Console.WriteLine($"Electrodes: {elState}");
}

void Sensor_EventCallibriSignalDataRecived(ISensor sensor, CallibriSignalData[] data)
{
    if (data == null)
        return;
    foreach (var it in data)
    {
        if (it.Samples == null)
            continue;
        string sLine = $"[{it.PackNum}]";
        foreach (var itSmaple in it.Samples)
            sLine = sLine + $"[{itSmaple}]";
        Console.WriteLine(sLine);
    }
}

void Sensor_EventCallibriEnvelopeDataRecived(ISensor sensor, CallibriEnvelopeData[] data)
{
    foreach (var it in data)
    {
        Console.WriteLine($"[{it.PackNum}][{it.Sample}]");
    }
}

void onDeviceFound(IScanner scanner, IReadOnlyList<SensorInfo> sensors)
{
    Console.WriteLine($"Found {sensors.Count} devices");
    foreach (var sensor in sensors)
    {
        Console.WriteLine($"With name {sensor.Name} and adress {sensor.Address}");
    }
}

void Sensor_EventSensorStateChanged(ISensor sensor, SensorState sensorState)
{
    Console.WriteLine($"{sensor.Name} connection state is {sensorState}");
}

void Sensor_EventBatteryChanged(ISensor sensor, int battPower)
{
    Console.WriteLine($"{sensor.Name} battery is {battPower}");
}

scanner.Dispose();