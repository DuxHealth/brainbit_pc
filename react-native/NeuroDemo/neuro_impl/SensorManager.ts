
import { DeviceEventEmitter, EventEmitter, EventSubscription } from "react-native";
import { SensorInfo, SensorSamplingFrequency, SensorState } from "react-native-neurosdk2/Types";
import ScannerImpl from "./ScannerImpl";
import {SignalPackage, SensorImpl, ChannelType} from "./SensorImpl";

class SensorManager
{
    private TAG: String = "SensorManager"

    private _scanner: ScannerImpl
    private _currentDevice: SensorImpl
    private _sensorFounded: EventSubscription

    private readonly _sensorStateChangedEvent = "sensorStateChanged"
    private readonly _sensorPowerChangedEvent = "sensorPowerChanged"

    connectionStateChanged(callback: (state: SensorState) => void): EventSubscription
    {
        return DeviceEventEmitter.addListener(this._sensorStateChangedEvent, callback);
    }
    batteryChanged(callback: (power: number) => void): EventSubscription
    {
        return DeviceEventEmitter.addListener(this._sensorPowerChangedEvent, callback);
    }

    StartSearch(callback: (sensorFounded: SensorInfo[]) => void): void {
        if (this._scanner == null) {
            this._scanner = new ScannerImpl();
        }
        if (this._currentDevice != null){
            this.Disconnect();
        }
        this._sensorFounded = this._scanner?.sensorFounded(callback);
        this._scanner?.StartSearch()
    }

    StopSearch()
    {
        this._scanner?.StopSearch()
        this._sensorFounded?.remove();
        this._sensorFounded = null;
    }
    private _batterySub : EventSubscription
    private _stateSub : EventSubscription
    private _connectIsBusy: Boolean
    Connect(info: SensorInfo): Promise<any>{
        if(this._connectIsBusy) return new Promise<any>((reject)=>{reject("We are already connecting, wait.")});
        this._connectIsBusy = true;
        return new Promise(async (resolve, reject) => {
            this._currentDevice = new SensorImpl(info);
            this._batterySub = this._currentDevice.batteryChanged((power) => {
                DeviceEventEmitter.emit(this._sensorPowerChangedEvent, power);
            });
            this._stateSub = this._currentDevice.connectionStateChanged((state)=>{
                DeviceEventEmitter.emit(this._sensorStateChangedEvent, state);
            });
            await this._currentDevice.Connect(this._scanner)
            .then((b)=> resolve(b))
            .catch((r)=> reject(r));
            this._connectIsBusy = false;
        })
    }

    Disconnect() : void{
        this._currentDevice.Disconnect().then(()=>{
            this._currentDevice.Close()
            this._batterySub.remove();
            this._stateSub.remove();
            this._currentDevice = null;
        })
    }


    Info(): String {
        return this._currentDevice?.Info();
    }

    ConnectionState() : SensorState {
        return this._currentDevice?.ConnectionState;
    }

    BatteryPower(): number {
        return this._currentDevice?.BattPower;
    }

    Channels(): ChannelType[]{
        return this._currentDevice?.Channels;
    }

    SamplingFrequency(): SensorSamplingFrequency {
        if(this._currentDevice?.SamplingFrequency != null) {
            console.log(this.TAG + `Sampling frequency: ${this._currentDevice?.SamplingFrequency!!}`)
            return this._currentDevice?.SamplingFrequency
        } else{
            console.log(this.TAG + `Sampling frequency: ${SensorSamplingFrequency.FrequencyUnsupported}`)
            return SensorSamplingFrequency.FrequencyUnsupported
        }
    }

    private signalSub : EventSubscription
    StartSignal(signalReceived: (signalPackage: SignalPackage[])=> EventSubscription): void{
        this.signalSub = this._currentDevice?.onSignalRecieved(signalReceived)
        this._currentDevice?.StartSignal()
    }

    StopSignal(){
        this.signalSub.remove();
        this._currentDevice?.StopSignal()
    }
}
export default SensorManager;