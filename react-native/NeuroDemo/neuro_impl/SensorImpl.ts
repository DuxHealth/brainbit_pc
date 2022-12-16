import {Sensor} from 'react-native-neurosdk2'
import {CallibriColorType, SensorAccelerometerSensitivity, SensorADCInput, SensorCommand, SensorDataOffset, SensorExternalSwitchInput, SensorFamily, SensorFeature, SensorFirmwareMode, SensorGain, SensorGyroscopeSensitivity, SensorInfo, SensorParamAccess, SensorParameter, SensorSamplingFrequency, SensorState} from 'react-native-neurosdk2/Types'
import ScannerImpl from './ScannerImpl';
import { EventSubscription } from 'react-native';
import EventEmitter from 'react-native/Libraries/vendor/emitter/EventEmitter';

enum ChannelType
{
    O1, O2, T3, T4, Signal, Ch1, Ch2, Ch3, Ch4, Ch5, Ch6, Ch7
}
class SignalPackage {
    type: ChannelType
    signals: number[] 
}
class SensorImpl 
{ 
    
    private TAG: String = "Sensor"
    private readonly _sensorStateChangedEvent = "sensorStateChanged"
    private readonly _sensorPowerChangedEvent = "sensorPowerChanged"
    private readonly _sensorSignalChangedEvent = "sensorSignalChanged"

    private _sensor: Sensor;
    
    private _sensorInfo: SensorInfo

    private _sensorEventEmitter: EventEmitter

    public get ConnectionState(): SensorState{ return this._sensor?.State};
    public get BattPower(): number{ return this._sensor?.BattPower}
    public get SamplingFrequency(): SensorSamplingFrequency{ return this._sensor?.SamplingFrequency}

    constructor(sensorInfo: SensorInfo)
    {
        this._sensorInfo = sensorInfo;
        this._sensorEventEmitter = new EventEmitter();
    }
    get Channels(): ChannelType[]
    {
        var Channels;
        switch(this._sensorInfo.SensFamily) {
            case SensorFamily.LECallibri: 
            case SensorFamily.LEKolibri:
                Channels = [ChannelType.Signal];
                break;
            case SensorFamily.LEBrainBit:
            case SensorFamily.LEBrainBitBlack:
            case SensorFamily.LEHeadband:
                Channels = [ChannelType.O1, ChannelType.O2, ChannelType.T3,
                    ChannelType.T4];
                break;
            case SensorFamily.LEHeadPhones: 
                Channels = [ChannelType.Ch1, ChannelType.Ch2, ChannelType.Ch3,
                    ChannelType.Ch4, ChannelType.Ch5, ChannelType.Ch6, ChannelType.Ch7];
                break;
            case SensorFamily.LESmartLeg:
            case SensorFamily.LENeurro:
            case SensorFamily.LEP300:
            case SensorFamily.LEImpulse:
            case SensorFamily.LEEarBuds: 
            case SensorFamily.Unknown: 
            case SensorFamily.SPCompactNeuro:
            default:
            break;
        }
        return Channels;
    }
    Info(): String
    {
        console.log(SensorFamily[this._sensor.SensFamily]) //
console.log(this._sensor.Features.map(feature => SensorFeature[feature])) //
console.log(this._sensor.Commands.map(command => SensorCommand[command])) //
console.log(this._sensor.Parameters.map(parameter => SensorParameter[parameter.Param] + ": " + SensorParamAccess[parameter.ParamAccess]))
console.log(this._sensor.Name) // BrainBit 
console.log(SensorState[this._sensor.State]) // SensorState.StateInRange
console.log(this._sensor.Address) // AA:BB:CC:DD:EE:FF
console.log(this._sensor.SerialNumber) // 123456
console.log(this._sensor.BattPower) // 50
console.log(SensorSamplingFrequency[this._sensor.SamplingFrequency]) // SensorSamplingFrequency.FrequencyHz250
console.log(SensorGain[this._sensor.Gain]) // SensorGain.SensorGain6
console.log(SensorDataOffset[this._sensor.DataOffset]) // SensorDataOffset.DataOffset0
console.log(this._sensor.Version) // SensorVersion(FwMajor=50, FwMinor=0, FwPatch=0, HwMajor=1, HwMinor=0, HwPatch=0, ExtMajor=65)

        if (this._sensor.State == SensorState.OutRange) return `Device unreachable!`;
        var deviceInfo = ``
        deviceInfo += `===== [Main info] =====\n`
        deviceInfo += `[Name]: ${this._sensor.Name}\n`
        deviceInfo += `[Sensor family]: ${SensorFamily[this._sensor.SensFamily]}\n`
        deviceInfo += `[Address]: ${this._sensor.Address}\n`
        deviceInfo += `[Serial number]: ${this._sensor.SerialNumber}\n`
        deviceInfo += `[Firmware mode]: ${SensorFirmwareMode[this._sensor.FirmwareMode]}\n`
        deviceInfo += `[Sensor version]: ` +
        `[FW]: ${this._sensor.Version.FwMajor}.${this._sensor.Version.FwMinor} ` +
        `[HW]: ${this._sensor.Version.HwMajor}.${this._sensor.Version.HwMinor}.${this._sensor.Version.HwPatch} ` +
        `[Ext]: ${this._sensor.Version.ExtMajor}\n`;
    
    
        var features = this._sensor.Features;
        if (features != null)
        {
            deviceInfo += `\n===== [Features] =====\n`
    
            features.forEach(feature => {
               deviceInfo += `[${SensorFeature[feature]}]: ${this._sensor.IsSupportedFeature(feature)}\n`
            });
        }
        var commands = this._sensor.Commands
        if (commands != null)
        {
            deviceInfo += `\n===== [Commands] =====\n`
            commands.forEach(command => {
              deviceInfo += `[${SensorCommand[command]}]: ${this._sensor.IsSupportedCommand(command)}\n`
            });
        }
        var parameters = this._sensor.Parameters
        if (parameters != null)
        {
            deviceInfo += `===== [Parameters] =====\n`
            parameters.forEach(parameter => {
              deviceInfo += `[${SensorParameter[parameter.Param]}]: [${this._sensor.IsSupportedParameter(parameter.Param)}][${SensorParamAccess[parameter.ParamAccess]}]\n`
            });
        }
         if (this._sensor.IsSupportedParameter(SensorParameter.SamplingFrequency) == true)
            deviceInfo += `\n[Sampling frequency]: ${SensorSamplingFrequency[this._sensor.SamplingFrequency]}\n`
    
         if (this._sensor.IsSupportedParameter(SensorParameter.Gain) == true)
             deviceInfo += `[Gain]: ${SensorGain[this._sensor.Gain]}\n`
    
         if (this._sensor.IsSupportedParameter(SensorParameter.Offset) == true)
             deviceInfo += `[Data offset]: ${SensorDataOffset[this._sensor.DataOffset]}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.ExternalSwitchState) == true)
            deviceInfo += `[External switch input]: ${SensorExternalSwitchInput[this._sensor.ExtSwInput]}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.ADCInputState) == true)
            deviceInfo += `[ADCInput]: ${SensorADCInput[this._sensor.ADCInput]}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.AccelerometerSens) == true)
            deviceInfo += `[Accelerometer sens]: ${SensorAccelerometerSensitivity[this._sensor.AccSens]}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.GyroscopeSens) == true)
            deviceInfo += `[GyroSens]: ${SensorGyroscopeSensitivity[this._sensor.GyroSens]}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.SamplingFrequencyResist) == true)
            deviceInfo += `[SamplingFrequencyReist]: ${SensorSamplingFrequency[this._sensor.SamplingFrequencyResist]}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.SamplingFrequencyMEMS) == true)
            deviceInfo += `[SamplingFrequencyMEMS]: ${SensorSamplingFrequency[this._sensor.SamplingFrequencyMEMS]}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.SamplingFrequencyFPG) == true)
            deviceInfo += `[SamplingFrequencyFPG]: ${SensorSamplingFrequency[this._sensor.SamplingFrequencyFPG]}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.StimulatorAndMAState) == true)
            deviceInfo += `[StimulatorMAStateCallibri]: ${this._sensor.StimulatorMAStateCallibri}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.StimulatorParamPack) == true)
            deviceInfo += `[StimulatorParamCallibri]: ${this._sensor.StimulatorParamCallibri}\n`
    
        if (this._sensor.IsSupportedParameter(SensorParameter.MotionAssistantParamPack) == true)
            deviceInfo += `[MotionAssistantParamCallibri]: ${this._sensor.MotionAssistantParamCallibri}\n`;
    
        if (this._sensor.IsSupportedParameter(SensorParameter.MotionCounterParamPack) == true)
            deviceInfo += `[MotionCounterParamCallibri]: ${this._sensor.MotionCounterParamCallibri}\n`;
    
        if(this._sensor.SensFamily == SensorFamily.LECallibri || this._sensor.SensFamily == SensorFamily.LEKolibri)
            deviceInfo += `[ColorCallibri]: ${CallibriColorType[this._sensor.ColorCallibri]}\n`;
    
        return deviceInfo
    }
    Connect(scanner: ScannerImpl): Promise<Boolean> 
    {
        return new Promise(async (resolve, reject) => {
            this._sensor = await scanner.scanner?.CreateSensor(this._sensorInfo);
            if (this._sensor != null) {
                this._sensor.AddBatteryChanged((power)=>{
                    this._sensorEventEmitter.emit(this._sensorPowerChangedEvent, power);
                  })
                this._sensor.AddConnectionChanged((state)=>{           
                    this._sensorEventEmitter.emit(this._sensorStateChangedEvent, state);
                })
                this._sensorEventEmitter.emit(this._sensorStateChangedEvent, SensorState.InRange);
                var sensorFamily = this._sensor?.SensFamily
                switch(sensorFamily) {
                    case SensorFamily.LECallibri: 
                    case SensorFamily.LEKolibri:
                        this._sensor?.AddCallibriSignalChanged((signalData)=>{
                            var data: SignalPackage[] = new Array<SignalPackage>();
                            signalData.forEach(element => {
                                data.push({ type: ChannelType.Signal, signals: element.Samples})
                            });
                            this._sensorEventEmitter.emit(this._sensorSignalChangedEvent, data);
                        }); 
                        break;
                    case SensorFamily.LEBrainBit:
                    case SensorFamily.LEBrainBitBlack:
                        this._sensor?.AddBrainBitSignalChanged((signalData)=>{
                            var data: SignalPackage[] = new Array<SignalPackage>();
                            var O1: number[] = new Array();
                            var O2: number[] = new Array();
                            var T3: number[] = new Array();
                            var T4: number[] = new Array();
                            signalData.forEach(element => {
                               O1.push(element.O1);
                               O2.push(element.O2);
                               T3.push(element.T3);
                               T4.push(element.T4);
                            });
                            data.push({ type: ChannelType.O1, signals: O1})
                            data.push({ type: ChannelType.O2, signals: O2})
                            data.push({ type: ChannelType.T3, signals: T3})
                            data.push({ type: ChannelType.T4, signals: T4})
                            this._sensorEventEmitter.emit(this._sensorSignalChangedEvent,data);
                        }); 
                        break;
                    case SensorFamily.LEHeadPhones: 
                        this._sensor?.AddHeadphonesSignalChanged((signalData)=>{
                            var data: SignalPackage[] = new Array<SignalPackage>();
                            var Ch1: number[] = new Array();
                            var Ch2: number[] = new Array();
                            var Ch3: number[] = new Array();
                            var Ch4: number[] = new Array();
                            var Ch5: number[] = new Array();
                            var Ch6: number[] = new Array();
                            var Ch7: number[] = new Array();
                            signalData.forEach(element => {
                               Ch1.push(element.Ch1);
                               Ch2.push(element.Ch2);
                               Ch3.push(element.Ch3);
                               Ch4.push(element.Ch4);
                               Ch5.push(element.Ch5);
                               Ch6.push(element.Ch6);
                               Ch7.push(element.Ch7);
                            });
                            data.push({ type: ChannelType.Ch1, signals: Ch1})
                            data.push({ type: ChannelType.Ch2, signals: Ch2})
                            data.push({ type: ChannelType.Ch3, signals: Ch3})
                            data.push({ type: ChannelType.Ch4, signals: Ch4})
                            data.push({ type: ChannelType.Ch5, signals: Ch5})
                            data.push({ type: ChannelType.Ch6, signals: Ch6})
                            data.push({ type: ChannelType.Ch7, signals: Ch7})
                            this._sensorEventEmitter.emit(this._sensorSignalChangedEvent, data);
                        }); 
                        break;
                    case SensorFamily.LEHeadband:
                        this._sensor?.AddHeadBandSignalChanged((signalData)=>{
                            var data: SignalPackage[] = new Array<SignalPackage>();
                            var O1: number[] = new Array();
                            var O2: number[] = new Array();
                            var T3: number[] = new Array();
                            var T4: number[] = new Array();
                            signalData.forEach(element => {
                               O1.push(element.O1);
                               O2.push(element.O2);
                               T3.push(element.T3);
                               T4.push(element.T4);
                            });
                            data.push({ type: ChannelType.O1, signals: O1})
                            data.push({ type: ChannelType.O2, signals: O2})
                            data.push({ type: ChannelType.T3, signals: T3})
                            data.push({ type: ChannelType.T4, signals: T4})
                            this._sensorEventEmitter.emit(this._sensorSignalChangedEvent,data);
                        }); 
                        break;
                    case SensorFamily.LESmartLeg:
                    case SensorFamily.LENeurro:
                    case SensorFamily.LEP300:
                    case SensorFamily.LEImpulse:
                    case SensorFamily.LEEarBuds: 
                    case SensorFamily.Unknown: 
                    case SensorFamily.SPCompactNeuro:
                    default:
                    break;
                }
            }
            await this._sensor.Connect().then(async ()=>{
                this._sensorEventEmitter.emit(this._sensorStateChangedEvent, SensorState.InRange);
                resolve(true);     
              }).catch((error)=>{
                this.Disconnect();
                reject(error);
              });
        })
    }

    connectionStateChanged(callback: (state: SensorState) => void): EventSubscription
    {
        return this._sensorEventEmitter.addListener(this._sensorStateChangedEvent, callback);
    }
    batteryChanged(callback: (power: number) => void): EventSubscription
    {
        return this._sensorEventEmitter.addListener(this._sensorPowerChangedEvent, callback);
    }
    onSignalRecieved(callback: (signalPackage: SignalPackage[]) => void): EventSubscription
    {
        return this._sensorEventEmitter.addListener(this._sensorSignalChangedEvent, callback);
    }

    private signalMode: Boolean = false
    StartSignal(): void {
        if (this.signalMode) return
        try {
            if (this._sensor?.IsSupportedCommand(SensorCommand.StartSignal) == true) {
                if(this._sensor.SensFamily == SensorFamily.LECallibri || this._sensor.SensFamily == SensorFamily.LEKolibri){
                    this._sensor.ExtSwInput = SensorExternalSwitchInput.ExtSwInMioElectrodes;
                }
                this._sensor?.ExecuteCommand(SensorCommand.StartSignal)
                this.signalMode = true
            }
        } catch (ex) {
            console.log(ex);
            this.signalMode = false
        }
    }
    StopSignal(): void {
        if (!this.signalMode) return
        try {
            if (this._sensor?.IsSupportedCommand(SensorCommand.StopSignal) == true) {
                this._sensor?.ExecuteCommand(SensorCommand.StopSignal)
                this.signalMode = false
            }
        } catch (ex) {
            console.log(ex);
            this.signalMode = false
        }
    }
    async Disconnect() {
        this._sensor?.RemoveBatteryChanged();
        this._sensor?.RemoveConnectionChanged();
        this._sensor?.RemoveBrainBitSignalChanged();
        this._sensor?.RemoveCallibriSignalChanged();
        this._sensor?.RemoveHeadBandSignalChanged();
        await this._sensor?.Disconnect();
        this._sensorEventEmitter.emit(this._sensorStateChangedEvent, SensorState.OutRange);
    }

    Close() {
        this._sensor?.Close();
    }
}
export {SensorImpl, ChannelType, SignalPackage};