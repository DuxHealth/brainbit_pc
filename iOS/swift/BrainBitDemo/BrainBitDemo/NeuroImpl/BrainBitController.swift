//
//  BrainBitController.swift
//  BrainBitDemo
//
//  Created by Aseatari on 20.02.2023.
//

import Foundation
import neurosdk2

typealias SensorsChangedCallback = (_ sensors: [NTSensorInfo]) -> Void

typealias SensorStateCallback = (_ state : NTSensorState)  -> Void
typealias DevicePowerCallback = (_ power : NSNumber)  -> Void

class BrainbitController{
    
    static let shared = BrainbitController()
    private init() { }
    
// MARK: - Scanner
    var scanner: NTScanner?

    func startSearch(sensorsChanged:  @escaping SensorsChangedCallback){
        if(scanner != nil){
            scanner = nil
        }
        if(scanner == nil){
            scanner = NTScanner(sensorFamily: [NTSensorFamily.leBrainBit.rawValue, NTSensorFamily.leBrainBitBlack.rawValue])
        }
        scanner?.setSensorsCallback(sensorsChanged)
        scanner?.startScan()
    }
    
    func stpoSearch(){
        scanner?.stopScan()
    }
    
    var availableSensors: [NTSensorInfo]? {
        get{
            scanner?.sensors
        }
    }
    
// MARK: - Device
    var sensor: NTBrainBit?
    
    var connectionStateChanged: SensorStateCallback?
    var batteryChanged: DevicePowerCallback?
	
    func createAndConnect(sensorInfo: NTSensorInfo, onConnectionResult: @escaping SensorStateCallback){
        DispatchQueue.global(qos: .userInitiated).async { [self, onConnectionResult] in
            sensor = scanner?.createSensor(sensorInfo) as? NTBrainBit
            if(sensor != nil){
                sensor?.setConnectionStateCallback(connectionStateChanged)
                sensor?.setBatteryCallback(batteryChanged)
                
                connectionStateChanged?(NTSensorState.inRange)
                onConnectionResult(NTSensorState.inRange)
            }
            else
            {
                onConnectionResult(NTSensorState.outOfRange)
            }
        }
        
    }
    
    func connectCurrent(onConnectionResult: @escaping SensorStateCallback){
        if(connectionState == NTSensorState.outOfRange){
            DispatchQueue.global(qos: .userInitiated).async { [self, onConnectionResult] in
                sensor?.connect()
                onConnectionResult(NTSensorState(rawValue: (sensor?.state)!.rawValue) ?? NTSensorState.outOfRange)
            }
        }
    }
    
    func disconnectCurrent(){
        sensor?.disconnect()
    }
    
    func closeSensor(){
        sensor = nil
    }
    
    var connectionState: NTSensorState? {
        get{
            sensor?.state
        }
    }
    
// MARK: - Parameters
    func fullInfo() -> String{
        var result = ""
        
        result += "Parameters: "
        for param in sensor?.parameters ?? [] {
            result += "\n\tName: \(paramEnumToString(param: param.param))"
            result += "\n\t\tAccess: \(accessEnumToString(access: param.paramAccess))"
            switch(param.param){
            case .name:
                result += "\n\t\tValue: \(sensor?.name ?? "")"
            case .state:
                result += "\n\t\tValue: \(stateEnumToString(state: sensor?.state ?? .outOfRange))"
            case .address:
                result += "\n\t\tValue: \(sensor?.address ?? "")"
            case .serialNumber:
                result += "\n\t\tValue: \(sensor?.serialNumber ?? "")"
            case .firmwareMode:
                result += "\n\t\tValue: \(modeEnumToString(mode: sensor?.firmwareMode ?? .application))"
            case .samplingFrequency:
                result += "\n\t\tValue: \(sfEnumToString(sf: sensor?.samplingFrequency ?? .hz250))"
            case .gain:
                result += "\n\t\tValue: \(gainToString(gain: sensor?.gain ?? .gain6))"
            case .offset:
                result += "\n\t\tValue: \(String(describing: sensor?.dataOffset))"
            case .firmwareVersion:
                let version = sensor?.version
                result += "\n\t\tFW version: \(version?.fwMajor ?? 0).\(version?.fwMinor ?? 0).\(version?.fwPatch ?? 0)"
                result += "\n\t\tHW version: \(version?.hwMajor ?? 0).\(version?.hwMinor ?? 0).\(version?.hwPatch ?? 0)"
                result += "\n\t\tExtension: \(version?.extMajor ?? 0)"
            case .battPower:
                result += "\n\t\tValue: \(sensor?.battPower ?? 0)"
            case .sensorFamily:
                result += "\n\t\tValue: \(sensor?.sensFamily ?? .leBrainBit)"
            case .sensorMode:
                result += "\n\t\tValue: \(sensor?.firmwareMode ?? .none)"
            case .hardwareFilterState, .externalSwitchState, .adcInputState, .accelerometerSens, .gyroscopeSens, .stimulatorAndMAState, .stimulatorParamPack, .stimulatorAndMAState, .motionAssistantParamPack, .memsCalibrationStatus, .motionCounterParamPack, .motionCounter, .irAmplitude, .redAmplitude, .envelopeAvgWndSz, .envelopeDecimation, .samplingFrequencyResist, .samplingFrequencyMEMS, .samplingFrequencyFPG, .amplifier, .sensorChannels: break
            @unknown default:
                break
            }
        }
        
        result += "\nFeatures:"
        for feature in sensor?.features ?? [] {
            result += "\n\t\(featureNumberToString(number: feature))"
        }
        
        result += "\nCommands:"
        for commandElement in sensor?.commands ?? [] {
            result += "\n\t\(commandNumberToString(number: commandElement))"
        }
        return result
    }
    
    private func commandNumberToString(number: NSNumber) -> String {
        let bindedEnum = NTSensorCommand(rawValue: UInt8(number))
        switch(bindedEnum) {
        case .startSignal:
            return "start signal"
        case .stopSignal:
            return "stop signal"
        case .startResist:
            return "start resist"
        case .stopResist:
            return "stop resist"
        case .startMEMS:
            return "start MEMS"
        case .stopMEMS:
            return "stop MEMS"
        case .startRespiration:
            return "start respiration"
        case .stopRespiration:
            return "stop respiration"
        case .startCurrentStimulation:
            return "start current stimulation"
        case .stopCurrentStimulation:
            return "stop current stimulation"
        case .enableMotionAssistant:
            return "enable motion assistant"
        case .disableMotionAssistant:
            return "disable motion assistant"
        case .findMe:
            return "find me"
        case .startAngle:
            return "start angle"
        case .stopAngle:
            return "stop angle"
        case .calibrateMEMS:
            return "calibrate MEMS"
        case .resetQuaternion:
            return "reset quaternion"
        case .startEnvelope:
            return "start envelope"
        case .stopEnvelope:
            return "stop envelope"
        case .resetMotionCounter:
            return "reset motion counter"
        case .calibrateStimulation:
            return "calibrate stimulation"
        case .idle:
            return "idle"
        case .powerDown:
            return "power down"
        case .startFPG:
            return "start FPG"
        case .stopFPG:
            return "stop FPG"
        case .startSignalAndResist:
            return "start signal and resist"
        case .stopSignalAndResist:
            return "stop signal and resist"
        case .startPhotoStimulation:
            return "start photo stimulation"
        case .stopPhotoStimulation:
            return "stop photo stimulation"
        case .startAcousticStimulation:
            return "start acoustic stimulation"
        case .stopAcousticStimulation:
            return "stop acoustic stimulation"
        @unknown default:
            return "unsupported"
        
        }
    }
    
    private func featureNumberToString(number: NSNumber) -> String {
        let bindedEnum = NTSensorFeature(rawValue: UInt8(number))
        switch(bindedEnum) {
        case .signal:
            return "signal"
        case .MEMS:
            return "MEMS"
        case .currentStimulator:
            return "current stimulator"
        case .respiration:
            return "respiration"
        case .resist:
            return "resist"
        case .FPG:
            return "FPG"
        case .envelope:
            return "envelope"
        case .photoStimulator:
            return "photo stimulator"
        case .acousticStimulator:
            return "acoustic stimulator"
        @unknown default:
            return "unsupported"
        }
    }
    
    private func paramEnumToString(param: NTSensorParameter) -> String{
        switch(param){
        case .name:
            return "name";
        case .state:
            return "state";
        case .address:
            return "address";
        case .serialNumber:
            return "serialNumber";
        case .firmwareMode:
            return "firmwareMode";
        case .samplingFrequency:
            return "samplingFrequency";
        case .gain:
            return "gain";
        case .offset:
            return "offset";
        case .firmwareVersion:
            return "firmwareVersion";
        case .battPower:
            return "battPower";
        case .sensorFamily:
            return "sensorFamily";
        case .sensorMode:
            return "sensorMode";
        case .hardwareFilterState, .externalSwitchState, .adcInputState, .accelerometerSens, .gyroscopeSens, .stimulatorAndMAState, .stimulatorParamPack, .motionAssistantParamPack, .memsCalibrationStatus, .motionCounterParamPack, .motionCounter, .irAmplitude, .redAmplitude, .envelopeAvgWndSz, .envelopeDecimation, .samplingFrequencyResist, .samplingFrequencyMEMS, .samplingFrequencyFPG, .amplifier, .sensorChannels:
            return "unsupported";
        @unknown default:
            return "unsupported";
        }
    }
    
    private func accessEnumToString(access: NTSensorParamAccess) -> String{
        switch(access){
        case .read:
            return "read"
        case .readWrite:
            return "readWrite"
        case .readNotify:
            return "readNotify"
        @unknown default:
            return "unsupported"
        }
    }
    
    private func stateEnumToString(state: NTSensorState) -> String{
        switch(state){
        case .inRange:
            return "inRange"
        case .outOfRange:
            return "outOfRange"
        @unknown default:
            return "unsupported"
        }
    }
    
    private func modeEnumToString(mode: NTSensorFirmwareMode) -> String{
        switch(mode){
        case .bootloader:
            return "bootloader"
        case .application:
            return "application"
        @unknown default:
            return "unsupported"
        }
    }
    
    private func sfEnumToString(sf: NTSensorSamplingFrequency) -> String{
        switch(sf){
        case .hz10:
            return "hz10"
        case .hz100:
            return "hz100"
        case .hz125:
            return "hz10"
        case .hz250:
            return "hz250"
        case .hz500:
            return "hz500"
        case .hz1000:
            return "hz1000"
        case .hz2000:
            return "hz2000"
        case .hz4000:
            return "hz4000"
        case .hz8000:
            return "hz8000"
        case .unsupported:
            return "unsupported"
        @unknown default:
            return "unsupported"
        }
    }
    
    private func gainToString(gain: NTSensorGain) -> String{
        switch(gain){
        case .gain1:
            return "gain1"
        case .gain2:
            return "gain2"
        case .gain3:
            return "gain3"
        case .gain4:
            return "gain4"
        case .gain6:
            return "gain6"
        case .gain8:
            return "gain8"
        case .gain12:
            return "gain12"
        case .gain24:
            return "gain24"
        case .gainUnsupported:
            return "unsupported"
        @unknown default:
            return "unsupported"
        }
    }
    
// MARK: - Signal
    func startSignal(signalRecieved: @escaping (_ data: [NTBrainBitSignalData]) -> Void){
        sensor?.setSignalDataCallback(signalRecieved)
        
        executeCommand(command: NTSensorCommand.startSignal)
    }
    
    func stopSignal(){
        sensor?.setSignalDataCallback(nil)
        
        executeCommand(command: NTSensorCommand.stopSignal)
    }
// MARK: - Resist
    func startResist(resistRecieved: @escaping (_ data: NTBrainBitResistData) -> Void){
        sensor?.setResistCallback(resistRecieved)
        
        executeCommand(command: NTSensorCommand.startResist)
    }
    
    func stopResist(){
        sensor?.setResistCallback(nil)
        
        executeCommand(command: NTSensorCommand.stopResist)
    }
    
// MARK: - Utils
    func executeCommand(command: NTSensorCommand){
        DispatchQueue.global(qos: .userInitiated).async { [self] in
            do {
                try ObjcEx.catchException {
                    self.sensor?.execCommand(command)
                }
            } catch let error {
                print(error.localizedDescription)
            }
            
        }
    }
    
}
