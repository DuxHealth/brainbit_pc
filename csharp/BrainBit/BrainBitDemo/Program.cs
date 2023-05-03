using NeuroSDK;

Console.WriteLine("Search devices (15s):");
Scanner scanner = new Scanner(SensorFamily.SensorLEBrainBit);
scanner.EventSensorsChanged += onDeviceFound;
scanner.Start();
Thread.Sleep(15000);
Console.WriteLine("Cancel search");
scanner.Stop();
scanner.EventSensorsChanged -= onDeviceFound;


foreach (SensorInfo sens in scanner.Sensors) {
    Console.WriteLine($"Connect to {sens.Name} ({sens.Address})");
    BrainBitSensor? sensor = scanner.CreateSensor(sens) as BrainBitSensor;
    if (sensor != null) {
        Console.WriteLine($"Successfully connected to {sens.Name}!");

        sensor.EventBatteryChanged += Sensor_EventBatteryChanged;
        sensor.EventSensorStateChanged += Sensor_EventSensorStateChanged;

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
                case SensorParameter.ParameterHardwareFilterState:
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

        Console.WriteLine($"Resistance (10s) in mV:");
        sensor.EventBrainBitResistDataRecived += Sensor_EventBrainBitResistDataRecived;
        sensor.ExecCommand(SensorCommand.CommandStartResist);
        Thread.Sleep(10000);
        sensor.ExecCommand(SensorCommand.CommandStopResist);
        sensor.EventBrainBitResistDataRecived -= Sensor_EventBrainBitResistDataRecived;

        Console.WriteLine($"Signal (10s) in mV:");
        sensor.EventBrainBitSignalDataRecived += Sensor_EventBrainBitSignalDataRecived;
        sensor.ExecCommand(SensorCommand.CommandStartSignal);
        Thread.Sleep( 10000 );
        sensor.ExecCommand(SensorCommand.CommandStopSignal);
        sensor.EventBrainBitSignalDataRecived -= Sensor_EventBrainBitSignalDataRecived;

        
        Console.WriteLine($"Disconnecting from {sensor.Name}...");
        sensor.Disconnect();
        Console.WriteLine($"And remove device...");
        sensor.Dispose();
        sensor = null;
    }
}

void Sensor_EventBrainBitResistDataRecived(ISensor sensor, BrainBitResistData data)
{
    Console.WriteLine($"O1: {data.O1 * 1e3} O2: {data.O2 * 1e3} T3: {data.T3 * 1e3} T4: {data.T4 * 1e3}");
}

void Sensor_EventBrainBitSignalDataRecived(ISensor sensor, BrainBitSignalData[] data)
{
    foreach (BrainBitSignalData signal in data) {
        Console.WriteLine($"O1: {signal.O1 * 1e3} O2: {signal.O2 * 1e3} T3: {signal.T3 * 1e3} T4: {signal.T4 * 1e3}");
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