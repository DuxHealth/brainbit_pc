import { Scanner, SensorInfo, SensorFamily, BrainBit2Sensor, SensorState, SignalChannelsData, ResistRefChannelsData, BrainBit2ChannelMode, EEGChannelType, SensorCommand, SensorFeature, SensorParameter, SensorParamAccess, SensorFirmwareMode, SensorDataOffset, SensorGain, SensorSamplingFrequency } from "react-native-neurosdk2";
import { PermissionsAndroid, Platform } from 'react-native';

let instance: BrainBitController;

class BrainBitController {

  private static _instance: BrainBitController;

  private constructor() {
    //...
  }

  public static get Instance() {
    // Do you need arguments? Make it a regular static method instead.
    return this._instance || (this._instance = new this());
  }

  private _scanner: Scanner | undefined

  private async requestPermissionAndroid() {
    try {
      const result = await PermissionsAndroid.requestMultiple([
        PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION,
        PermissionsAndroid.PERMISSIONS.BLUETOOTH_CONNECT,,
        PermissionsAndroid.PERMISSIONS.BLUETOOTH_SCAN,  // for android 12 (api 31+)
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
    await this._scanner.init([SensorFamily.LEBrainBit2, SensorFamily.LEBrainBitFlex, SensorFamily.LEBrainBitPro])
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

  private _sensor: BrainBit2Sensor | undefined
  public connectionChangedCallback: ((state: SensorState)=>void) | undefined
  public batteryCallback: ((battery: number)=>void) | undefined

  public get connectionState(): SensorState {
    return this._sensor === undefined ? SensorState.OutOfRange : this._sensor.getState();
  }

  public get batteryPower(): number {
    return this._sensor === undefined ? 0 : this._sensor.getBattPower();
  }

  public get channelsCount(): number {
    return this._sensor?.getChannelsCount() ?? 0;
  }

  channelsList(): Array<String> {
    var result = Array(this._sensor?.getChannelsCount()).fill("")
    this._sensor?.getSupportedChannels()?.forEach((ch)=>{
      result[ch.Num] = ch.Name + ch.Num
    })

    return result
  }

  async createAndConnect(info: SensorInfo): Promise<SensorState> {
    return new Promise<SensorState>(async (resolve, reject) => {
      this._scanner?.createSensor(info)
        .then((sensor) => {
          this._sensor = sensor as BrainBit2Sensor
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

  public signalReceivedCallback: ((data: Array<SignalChannelsData>)=>void) | undefined
  public resistReceivedCallback: ((data: Array<ResistRefChannelsData>)=>void) | undefined

  async startSignal(){
    this._sensor?.AddSignalReceived((data)=>{
      if(this.signalReceivedCallback != undefined)
        this.signalReceivedCallback(data)
    })
    await this._sensor?.execute(SensorCommand.StartSignal).catch((ex)=> console.log(ex))
  }

  async stopSignal(){
    this._sensor?.RemoveSignalReceived()
    await this._sensor?.execute(SensorCommand.StopSignal).catch((ex)=> console.log(ex))
  }

  async startResist(){
    this._sensor?.AddResistanceReceived((data)=>{
      if(this.resistReceivedCallback != undefined)
        this.resistReceivedCallback(data)
    })
    await this._sensor?.execute(SensorCommand.StartResist).catch((ex)=> console.log(ex))
  }

  async stopResist(){
    this._sensor?.RemoveResistanceReceived();
    await this._sensor?.execute(SensorCommand.StopResist).catch((ex)=> console.log(ex))
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
        deviceInfo += `Commands:\n`
            commands.forEach(command => {
              deviceInfo += ` ${SensorCommand[command]}\n`
            });

        var parameters = this._sensor.getParameters()
        deviceInfo += 'Parameters:\n'
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
          }
        });

        deviceInfo += '\n';
        deviceInfo += `  supported channels: [${this._sensor.getSupportedChannels().map((ch, i) => {
          return `\n   {Id: ${ch.Id}, Num: ${ch.Num}, Name: ${ch.Name}, Type: ${EEGChannelType[ch.ChType]}}`
        })}\n   ]\n`;

        var ampParams = this._sensor?.getAmplifierParam()

        deviceInfo += `  Amplifier params: \n` + 
        `   Current = ${ampParams.Current}\n` +
        `   ChResistUse = [ ${ampParams.ChResistUse.map((resist, i) => {
          return `\n      ${i}: ${resist} `
        })}\n   ]\n` +
        `   ChSignalMode = [ ${ampParams.ChSignalMode.map((mode, i) => {
          return `\n      ${i}: ${BrainBit2ChannelMode[mode]} `
        })}\n   ]\n` +
        `   ChannelGain = [ ${ ampParams.ChGain.map((gain, i) => {
          return `\n      ${i}: ${SensorGain[gain]} `
        })}\n   ]\n`;

        deviceInfo += '\n';

        return deviceInfo
  }
}

const BBControllerInstance = BrainBitController.Instance;
export default BBControllerInstance;