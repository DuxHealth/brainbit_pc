import { Scanner, 
  SensorInfo, 
  SensorFamily, 
  NeuroEEGSensor, 
  SensorState, 
  SensorCommand, 
  SensorFeature,
  SensorParameter, 
  SensorParamAccess, 
  SensorFirmwareMode, 
  SensorDataOffset, 
  SensorGain, 
  SensorSamplingFrequency, 
  SignalChannelsData, 
  ResistChannelsData, 
  EEGChannelInfo,
  NeuroEEGAmplifierParam, 
  EEGRefMode, 
  EEGChannelMode,
  SensorAmpMode,
  EEGChannelType} from "react-native-neurosdk2";
import { PermissionsAndroid, Platform } from 'react-native';

//let instance: NeuroEEGController;

class NeuroEEGController {

  private static _instance: NeuroEEGController;

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
      if (
        result[PermissionsAndroid.PERMISSIONS.BLUETOOTH_SCAN] !=
        PermissionsAndroid.RESULTS.GRANTED
      ) {
        this.requestPermissionAndroid();
      }
    } catch (err) {
      console.warn(err);
    }

  }

  async startSearch(sensorFounded: (sensorFounded: SensorInfo[]) => void): Promise<void> {

    if (this._scanner == undefined) {
      this._scanner = new Scanner()
      await this._scanner.init([SensorFamily.LENeuroEEG])
    }
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

  private _sensor: NeuroEEGSensor | undefined
  public connectionChangedCallback: ((state: SensorState)=>void) | undefined
  public batteryCallback: ((battery: number)=>void) | undefined

  public get connectionState(): SensorState {
    return this._sensor === undefined ? SensorState.OutOfRange : this._sensor.getState();
  }

  public get batteryPower(): number {
    return this._sensor === undefined ? 0 : this._sensor.getBattPower();
  }

  public get maxChannelsCount(): number {
    return NeuroEEGSensor.getMaxChCount();
  }

  async createAndConnect(info: SensorInfo): Promise<SensorState> {
    return new Promise<SensorState>(async (resolve, reject) => {
      this._scanner?.createSensor(info)
        .then((sensor) => {
          this._sensor = sensor as NeuroEEGSensor

          let ampMode = this._sensor.getAmpMode()
          console.log
          if(ampMode !== SensorAmpMode.AmpModeIdle && ampMode !== SensorAmpMode.AmpModePowerDown){
            this._sensor.execute(SensorCommand.StopSignal)
            this._sensor.execute(SensorCommand.StopSignalAndResist)
            this._sensor.execute(SensorCommand.StopResist)
          }

          let maxChCount = NeuroEEGSensor.getMaxChCount();
          let ampParam: NeuroEEGAmplifierParam = {
            ReferentResistMesureAllow: false,
            Frequency: SensorSamplingFrequency.FrequencyHz250,
            ReferentMode: EEGRefMode.A1A2,
            ChannelMode: Array(maxChCount).fill(EEGChannelMode.EEGChannelModeSignalResist),
            ChannelGain: Array(maxChCount).fill(SensorGain.Gain6)
          };
          this._sensor.setAmplifierParam(ampParam)

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

  channelsList(): Array<String> {
    var result = Array(this._sensor?.getChannelsCount()).fill("")
    this._sensor?.getSupportedChannels()?.forEach((ch)=>{
      result[ch.Num] = ch.Name
    })

    return result
  }

  public signalReceivedCallback: ((data: Array<SignalChannelsData>)=>void) | undefined
  public resistReceivedCallback: ((data: Array<ResistChannelsData>)=>void) | undefined
  public signalResistReceivedCallback: ((signalData: Array<SignalChannelsData>, resistData: Array<ResistChannelsData>)=>void) | undefined

  async startSignal(){
    this._sensor?.AddSignalDataReceived((data)=>{
      if(this.signalReceivedCallback != undefined)
        this.signalReceivedCallback(data)
    })
    await this._sensor?.execute(SensorCommand.StartSignal).catch((ex)=> console.log(ex))
  }

  async stopSignal(){
    this._sensor?.RemoveSignalDataReceived()
    await this._sensor?.execute(SensorCommand.StopSignal).catch((ex)=> console.log(ex))
  }

  async startResist(){
    this._sensor?.AddResistDataReceived((data)=>{
      if(this.resistReceivedCallback != undefined)
        this.resistReceivedCallback(data)
    })
    await this._sensor?.execute(SensorCommand.StartResist).catch((ex)=> console.log(ex))
  }

  async stopResist(){
    this._sensor?.RemoveResistDataReceived();
    await this._sensor?.execute(SensorCommand.StopResist).catch((ex)=> console.log(ex))
  }

  async startSignalResist(){
    this._sensor?.AddSignalResistReceived((signalData, resistData)=>{
      if(this.signalResistReceivedCallback != undefined){
        this.signalResistReceivedCallback(signalData, resistData)
      }
    })
    await this._sensor?.execute(SensorCommand.StartSignalAndResist).catch((ex)=> console.log(ex))
  }

  async stopSignalResist(){
    this._sensor?.RemoveSignalResistReceived()
    await this._sensor?.execute(SensorCommand.StopSignalAndResist).catch((ex)=> console.log(ex))
  }

  get info(): string{
    if (this._sensor === undefined || this._sensor.getState() == SensorState.OutOfRange) return `Device unreachable!`;
        var deviceInfo = ``

        var features = this._sensor.getFeatures();
        deviceInfo += `Features:\n`
        features.forEach(feature => {
               deviceInfo += ` ${SensorFeature[feature]}\n`
        });

        deviceInfo += '\n';

        var commands = this._sensor.getCommands()
        deviceInfo += `Commands:\n`
            commands.forEach(command => {
              deviceInfo += ` ${SensorCommand[command]}\n`
            });

        deviceInfo += '\n';

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
              deviceInfo += ` Signal frequency: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorSamplingFrequency[this._sensor === undefined ? 10 : this._sensor.getSamplingFrequency()]}\n`
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
            case SensorParameter.SamplingFrequencyResist:
              deviceInfo += ` Resist frequency: (${SensorParamAccess[parameter.ParamAccess]}): ${SensorSamplingFrequency[this._sensor === undefined ? 10 : this._sensor.getSamplingFrequencyResist()]}\n`
              break;
          }
        });

        deviceInfo += '\n';
        
        deviceInfo += `  survey id = ${this._sensor.getSurveyId()}\n`;

        deviceInfo += `  Channel count: ${this._sensor.getChannelsCount()}\n`;
        deviceInfo += `  Max channel count: ${NeuroEEGSensor.getMaxChCount()}\n`;

        deviceInfo += `  supported channels: [${this._sensor.getSupportedChannels().map((ch, i) => {
          return `\n   {Id: ${ch.Id}, Num: ${ch.Num}, Name: ${ch.Name}, Type: ${EEGChannelType[ch.ChType]}}`
        })}\n   ]\n`;

        var ampParams = this._sensor?.getAmplifierParam()

        deviceInfo += `  Amplifier params: \n` + 
        `   ReferentResistMesureAllow = ${ampParams.ReferentResistMesureAllow}\n` +
        `   Frequency = ${SensorSamplingFrequency[ampParams.Frequency]}\n` + 
        `   ReferentMode = ${EEGRefMode[ampParams.ReferentMode]}\n` + 
        `   ChannelMode = [ ${ampParams.ChannelMode.map((mode, i) => {
          return `\n      ${i}: ${EEGChannelMode[mode]} `
        })}\n   ]\n` +
        `   ChannelGain = [ ${ ampParams.ChannelGain.map((gain, i) => {
          return `\n      ${i}: ${SensorGain[gain]} `
        })}\n   ]\n`;

        deviceInfo += '\n';

        return deviceInfo
  }
}

const NEEGControllerInstance = NeuroEEGController.Instance;
export default NEEGControllerInstance;