import { Scanner, 
  SensorInfo, 
  SensorFamily, 
  CallibriSensor, 
  SensorState, 
  SensorCommand, 
  SensorFeature, 
  SensorParameter, 
  SensorParamAccess, 
  SensorFirmwareMode, 
  SensorDataOffset, 
  SensorGain, 
  SensorSamplingFrequency, 
  CallibriSignalData, 
  CallibriEnvelopeData, 
  CallibriElectrodeState, 
  CallibriSignalType, 
  CallibriColorType, 
  SensorFilter, 
  SensorADCInput, 
  SensorGyroscopeSensitivity, 
  SensorAccelerometerSensitivity, 
  SensorExternalSwitchInput } from "react-native-neurosdk2";
import { EventSubscription, PermissionsAndroid, Platform } from 'react-native';

let instance: CallibriController;

class CallibriController {

  private static _instance: CallibriController;

  private constructor() {
    //...
  }

  public static get Instance() {
    return this._instance || (this._instance = new this());
  }

  private _scanner: Scanner | undefined

  private async requestPermissionAndroid() {
    try {
      const result = await PermissionsAndroid.requestMultiple([
        PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION,
        PermissionsAndroid.PERMISSIONS.BLUETOOTH_CONNECT,
      ]);
      if (
        result[PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION] !=
        PermissionsAndroid.RESULTS.GRANTED
      ) {
        this.requestPermissionAndroid();
      }
      if (
        result[PermissionsAndroid.PERMISSIONS.BLUETOOTH_CONNECT] !=
        PermissionsAndroid.RESULTS.GRANTED
      ) {
        this.requestPermissionAndroid();
      }
    } catch (err) {
      console.warn(err);
    }
  }

  async startSearch(sensorFounded: (sensorFounded: SensorInfo[]) => void): Promise<void> {

    if (this._scanner != undefined) {
      this._scanner.close()
      this._scanner = undefined
    }
    this._scanner = new Scanner()
    await this._scanner.init([SensorFamily.LECallibri, SensorFamily.LEKolibri])
    this._scanner?.AddSensorListChanged(sensorFounded)
    if(Platform.OS != 'ios'){
      await this.requestPermissionAndroid()
    }
    await this._scanner?.start()
  }

  async stopSearch(): Promise<void> {
    this._scanner?.RemoveSensorListChanged()
    await this._scanner?.stop()
  }

  private _sensor: CallibriSensor | undefined
  public connectionChangedCallback: ((state: SensorState)=>void) | undefined
  public batteryCallback: ((battery: number)=>void) | undefined

  public get connectionState(): SensorState {
    return this._sensor === undefined ? SensorState.OutOfRange : this._sensor.getState();
  }

  public get batteryPower(): number {
    return this._sensor === undefined ? 0 : this._sensor.getState();
  }

  public get samplingFreq(): SensorSamplingFrequency {
    return this._sensor === undefined ? SensorSamplingFrequency.FrequencyUnsupported : this._sensor.getSamplingFrequency();
  }

  async createAndConnect(info: SensorInfo): Promise<SensorState> {
    return new Promise<SensorState>(async (resolve, reject) => {
      this._scanner?.createSensor(info)
        .then((sensor) => {
          this._sensor = sensor as CallibriSensor

          this._sensor.setSignalTypeCallibri(CallibriSignalType.EMG)
          this._sensor.setSamplingFrequency(SensorSamplingFrequency.FrequencyHz1000)

          this._sensor.setHardwareFilters([SensorFilter.FilterHPFBwhLvl1CutoffFreq1Hz])

          this._sensor.AddConnectionChanged((state) => { 
            if(this.connectionChangedCallback != undefined)
              this.connectionChangedCallback(state); 
          })

          this._sensor.AddBatteryChanged((battery) => { 
            if(this.batteryCallback != undefined)
              this.batteryCallback(battery); 
          })

          if(this.connectionChangedCallback != undefined)
              this.connectionChangedCallback(SensorState.InRange);
          resolve(SensorState.InRange)
        })
        .catch((ex) => { reject(SensorState.OutOfRange) })
    });
  }

  async connectCurrent(): Promise<SensorState> {
    return new Promise<SensorState>(async (resolve, reject) => {
      if(this._sensor?.getState() != SensorState.OutOfRange) {
        resolve(SensorState.InRange)
        return
      }
      this._sensor?.connect()
        .then(() => {
          resolve(SensorState.InRange)
        })
        .catch((ex) => { reject(SensorState.OutOfRange) })
    });
  }

  async disconnectCurrent(): Promise<void> {
    await this._sensor?.disconnect()
  }

  public signalReceivedCallback: ((data: Array<CallibriSignalData>)=>void) | undefined
  public electrodeChangedCallback: ((data: CallibriElectrodeState)=>void) | undefined
  public envelopeReceivedCallback: ((data: Array<CallibriEnvelopeData>)=>void) | undefined

  async startSignal(){
    this._sensor?.AddElectrodeStateChanged((state)=>{
      if(this.electrodeChangedCallback != undefined)
        this.electrodeChangedCallback(state)
    })
    this._sensor?.AddSignalReceived((data)=>{
      if(this.signalReceivedCallback != undefined)
        this.signalReceivedCallback(data)
    })
    await this._sensor?.execute(SensorCommand.StartSignal).catch((ex)=> console.log(ex))
  }

  async stopSignal(){
    this._sensor?.RemoveElectrodeStateChanged()
    this._sensor?.RemoveSignalReceived()
    await this._sensor?.execute(SensorCommand.StopSignal).catch((ex)=> console.log(ex))
  }

  async startEnvelope(){
    this._sensor?.AddEnvelopeDataChanged((data)=>{
      if(this.envelopeReceivedCallback != undefined)
        this.envelopeReceivedCallback(data)
    })
    await this._sensor?.execute(SensorCommand.StartEnvelope).catch((ex)=> console.log(ex))
  }

  async stopEnvelope(){
    this._sensor?.RemoveEnvelopeDataChanged();
    await this._sensor?.execute(SensorCommand.StopEnvelope).catch((ex)=> console.log(ex))
  }

  get info(): string{
    if (this._sensor === undefined || this._sensor.getState() == SensorState.OutOfRange) return `Device unreachable!`;
        var deviceInfo = ``

        var features = this._sensor.getFeatures();
        deviceInfo += `Features:\n`
        features.forEach(feature => {
               deviceInfo += ` ${SensorFeature[feature]}\n`
        });

        var commands = this._sensor.getCommands()
        deviceInfo += `\nCommands:\n`
            commands.forEach(command => {
              deviceInfo += ` ${SensorCommand[command]}\n`
            });

        var parameters = this._sensor.getParameters()
        deviceInfo += '\nParameters:\n'
        parameters.forEach(parameter => { 
          switch(parameter.Param){
            case SensorParameter.Name:
              deviceInfo += ` Name (${SensorParamAccess[parameter.ParamAccess]}): ${this._sensor?.getName()}\n`
              break;
            case SensorParameter.State:
              deviceInfo += ` State (${SensorParamAccess[parameter.ParamAccess]}): ${SensorState[this._sensor === undefined ? 1 : this._sensor.getState()]}\n`
              break;
            case SensorParameter.Address:
              deviceInfo += ` Address (${SensorParamAccess[parameter.ParamAccess]}): ${this._sensor?.getAddress()}\n`
              break;
            case SensorParameter.SerialNumber:
              deviceInfo += ` Serial number: (${SensorParamAccess[parameter.ParamAccess]}): ${this._sensor?.getSerialNumber()}\n`
              break;
            case SensorParameter.FirmwareMode:
              deviceInfo += ` Firmware mode: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorFirmwareMode[this._sensor === undefined ? 1 : this._sensor.getFirmwareMode()]}\n`
              break;
            case SensorParameter.SamplingFrequency:
              deviceInfo += ` Sampling frequency: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorSamplingFrequency[this._sensor === undefined ? 10 : this._sensor.getSamplingFrequency()]}\n`
              break;
            case SensorParameter.Gain:
              deviceInfo += `  Gain: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorGain[this._sensor === undefined ? 11 : this._sensor.getGain()]}\n`
              break;
            case SensorParameter.Offset:
              deviceInfo += ` Offset: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorDataOffset[this._sensor === undefined ? 255 : this._sensor.getDataOffset()]}\n`
              break;
            case SensorParameter.FirmwareVersion:
              var version = this._sensor?.getVersion()
              deviceInfo += ` Firmware version: (${SensorParamAccess[parameter.ParamAccess]}):\n` +
                 `  FW: ${version?.FwMajor}.${version?.FwMinor}.${version?.FwPatch}` +
                 `  HW: ${version?.HwMajor}.${version?.HwMinor}.${version?.HwPatch} ` +
                 `  Ext: ${version?.ExtMajor}\n`
              break;
            case SensorParameter.BattPower:
              deviceInfo += ` Battery power: (${SensorParamAccess[parameter.ParamAccess]}): ${this._sensor?.getBattPower()}\n`
              break;
            case SensorParameter.SensorFamily:
              deviceInfo += ` Sensor family: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorFamily[this._sensor === undefined ? 0 : this._sensor.getSensFamily()]}\n`
              break;
            case SensorParameter.ADCInputState:
              deviceInfo += ` ADC input: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorADCInput[this._sensor === undefined ? 0 : this._sensor.getADCInput()]}\n`
              break;
            case SensorParameter.GyroscopeSens:
              deviceInfo += ` Gyro sensitivity: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorGyroscopeSensitivity[this._sensor === undefined ? 0 : this._sensor.getGyroSens()]}\n`
              break;
            case SensorParameter.SamplingFrequencyMEMS:
              deviceInfo += ` Sampling frequency MEMS: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorSamplingFrequency[this._sensor === undefined ? 0 : this._sensor.getSamplingFrequencyMEMS()]}\n`
              break;
            case SensorParameter.AccelerometerSens:
              deviceInfo += ` Accelerometer sensitivity: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorAccelerometerSensitivity[this._sensor === undefined ? 0 : this._sensor.getAccSens()]}\n`
              break;
            case SensorParameter.HardwareFilterState:
              deviceInfo += ` Hardware filters: (${SensorParamAccess[parameter.ParamAccess]}): ${this._sensor === undefined ? 0 : this._sensor.getHardwareFilters()}\n`
              break;
            case SensorParameter.ExternalSwitchState:
              deviceInfo += ` External switch state: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorExternalSwitchInput[this._sensor === undefined ? 0 : this._sensor.getExtSwInput()]}\n`
              break;
          }
        });

        deviceInfo += `\nColor: ${CallibriColorType[this._sensor === undefined ? 4 : this._sensor.getColorCallibri()]}\n`
        deviceInfo += `\Signal type: ${CallibriSignalType[this._sensor === undefined ? 6 : this._sensor.getSignalTypeCallibri()]}\n`

        return deviceInfo
  }
}

const CallibriControllerInstance = CallibriController.Instance;
export default CallibriControllerInstance;
