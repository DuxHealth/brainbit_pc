#pragma once

/*
		#######################
		#					  #
		#	SAMPLE CALLIBRI	  #
		#					  #
		#######################


	Task: A simple work with
	device 'LECallibri'


	Callibri Supports:

	functions:
	- connect
	- disconnect
	
	- getSupportedFeatures
	- getSupportedCommands
	- getSupportedParameters

	- execCommand

	- getChannelsCount

	- readName
	- writeName
	- readState
	- readAddress
	- readSerialNumber
	- writeSerialNumber
	- readBatteryPower
	- readSamplingFrequency
	- writeSamplingFrequency
	- readGain
	- writeGain
	- readDataOffset
	- writeDataOffset
	- readFirmwareMode
	- writeFirmwareMode
	- readVersion
	- readHardwareFilters
	- writeHardwareFilters
	- readExternalSwitch
	- writeExternalSwitch
	- readADCInput
	- writeADCInput

	- readStimulatorAndMAState
	- readStimulatorParam
	- writeStimulatorParam
	- readMotionAssistantParam
	- writeMotionAssisstantParam
	- readMotionCounterParam
	- writeMotionCounterParam
	- readMotionCounter
	- readColor
	- readMEMSCalibrateState
	- readSamplingFrequencyResp

	- setSignalSettings
	- getSignalSettings

	- readAccelerometerSens
	- writeAccelerometerSens
	- readGyroscopeSens
	- writeGyroscopeSens

	execCommands:
	- StartSignal
	- StopSignal

	- StartEnvelope
	- StopEnvelope

	Callbacks:

	- BatteryCallback
	- ConnectionCallback
	- RespirationCallback
	- ElectrodeCallback
	- EnvelopeCallback
	- SignalCallback
	- QuaternionCallback
	- MEMSDataCallback
*/
#include <iostream>
#include <vector>

#include "sdk_api.h"
#include "log.h"

#include "mathLib.h"
#include "callibriMathLib.h"
#include "filtersLib.h"
#include "spectrumLib.h"

class SampleCallibri
{
public:

	// ==============
	// || CALLIBRI ||
	// ==============

	//Constructor of class
	SampleCallibri(Sensor* sensor);

	//Destructor of class
	~SampleCallibri();

	//Connect to device
	bool connect();

	//Disconnect from device
	bool disconnect();

	//Execute command
	bool execCommand(SensorCommand command);

	// =======================
	// || CALLIBRI SETTINGS ||
	// =======================

	//Read & write sampling frequency of device
	SensorSamplingFrequency readSamplingFrequency();
	bool writeSamplingFrequency(SensorSamplingFrequency samplingFrequency);

	//Read & write gain of device
	SensorGain readGain();
	bool writeGain(SensorGain gain);

	//Read & write data offset of device
	SensorDataOffset readDataOffset();
	bool writeDataOffset(SensorDataOffset dataOffset);

	//Read & write firmware mode of device
	SensorFirmwareMode readFirmwareMode();
	bool writeFirmwareMode(SensorFirmwareMode firmwareMode);

	//Read & write list of hardware filters
	std::vector<SensorFilter> readHardwareFilters();
	bool writeHardwareFilters(std::vector<SensorFilter> hardwareFilters);

	//Read & write external switch of device
	SensorExternalSwitchInput readExternalSwitch();
	bool writeExternalSwitch(SensorExternalSwitchInput externalSwitchInput);

	//Read & write ADCInput of device
	SensorADCInput readADCInput();
	bool writeADCInput(SensorADCInput adcInput);

	//Read stimulator MA state
	CallibriStimulatorMAState readStimulatorAndMAState();

	//Read & write stimulator parameters
	CallibriStimulationParams readStimulatorParam();
	bool writeStimulatorParam(CallibriStimulationParams stimulatorParam);

	//Read & write motion assistant parameters
	CallibriMotionAssistantParams readMotionAssistantParam();
	bool writeMotionAssisstantParam(CallibriMotionAssistantParams motionAssistantParam);

	//Read & write motion counter parameters
	CallibriMotionCounterParam readMotionCounterParam();
	bool writeMotionCounterParam(CallibriMotionCounterParam motionAssistantCounter);

	//Read motion counter
	uint32_t readMotionCounter();

	//Read MEMS calibrate state
	bool readMEMSCalibrateState();

	//Read sampling frequency response
	SensorSamplingFrequency readSamplingFrequencyResp();

	//Get & set signal settings
	bool setSignalSettings(SignalTypeCallibri signalSetting);
	SignalTypeCallibri getSignalSettings();

	//Read & write accelerometer sensitivity
	SensorAccelerometerSensitivity readAccelerometerSens();
	bool writeAccelerometerSens(SensorAccelerometerSensitivity accerometerSens);

	//Read & write gyroscope sensitivity
	SensorGyroscopeSensitivity readGyroscopeSens();
	bool writeGyroscopeSens(SensorGyroscopeSensitivity gyroscopeSens);

	// ===================================
	// || CALLIBRI SUPPORTED FUNCTIONAL ||
	// ===================================

	//Get list of supported features by device
	std::vector<SensorFeature> getSupportedFeatures();

	//Get list of supported commands by device
	std::vector<SensorCommand> getSupportedCommands();

	//Get list of supported parameters by device
	std::vector<ParameterInfo> getSupportedParameters();

	//Get true result, if device supports current feature
	bool isSupportedFeature(SensorFeature feature);

	//Get true result, if device supports current command
	bool isSupportedCommand(SensorCommand command);

	//Get true result, if device supports current parameter
	bool isSupportedParameter(SensorParameter parameter);

	// ===================
	// || CALLIBRI INFO ||
	// ===================

	//Read & write name of device
	std::string readName();
	bool writeName(std::string name);

	//Read state of device
	SensorState readState();

	//Read address of device
	std::string readAddress();

	//Read & write serial number of device
	std::string readSerialNumber();
	bool writeSerialNumber(std::string serialNumber);

	//Read color of device
	CallibriColorType readColor();

	//Read version of device
	SensorVersion readVersion();

	//Read battery power of device
	int32_t readBatteryPower();

	//Get count of channels
	int32_t getChannelsCount();

	//Get Family of device
	SensorFamily getFamily();

	// =======================
	// || CALLIBRI CALLBACK ||
	// =======================

	//Add & remove Battery Power callback
	bool AddBatteryCallback();
	bool RemoveBatteryCallback();

	//Add & remove connection state callback
	bool AddConnectionStateCallback();
	bool RemoveConnectionStateCallback();

	//Add & remove signal data callback
	bool AddSignalCallbackCallibri();
	bool RemoveSignalCallbackCallibri();

	//Add & remove respiration data callback
	bool AddRespirationCallbackCallibri();
	bool RemoveRespirationCallbackCallibri();

	//Add & remove electrode state callback
	bool AddElectrodeStateCallbackCallibri();
	bool RemoveElectrodeStateCallbackCallibri();

	//Add & remove envelope data callback
	bool AddEnvelopeDataCallbackCallibri();
	bool RemoveEnvelopeDataCallbackCallibri();

	//Add & remove quaternion data callback
	bool AddQuaternionDataCallback();
	bool RemoveQuaternionDataCallback();

	//Add & remove mems data callback
	bool AddMEMSDataCallback();
	bool RemoveMEMSDataCallback();

	// =======================
	// || CALLIBRI CALLBACK ||
	// =======================

	bool AddSignalCallbackCallibri_EmStArtifacts(MathLibSample* mathlib);
	bool RemoveSignalCallbackCallibri_EmStArtifacts();

	bool AddSignalCallbackCallibri_Callibri_ECG(CallibriMathLibSample* mathlib);
	bool RemoveSignalCallbackCallibri_Callibri_ECG();

	bool AddSignalCallbackCallibri_Filters(FiltersLibSample* mathlib);
	bool RemoveSignalCallbackCallibri_Filters();

	bool AddSignalCallbackCallibri_Spectrum(SpectrumLibSample* mathlib);
	bool RemoveSignalCallbackCallibri_Spectrum();

private:
	//Object Sensor (our device)
	Sensor* _sensor;

	//Handle of Battery Power Callback
	BattPowerListenerHandle _lBattPowerHandle = nullptr;

	//Handle of Connection State Device Callback
	SensorStateListenerHandle _lStateHandle = nullptr;

	//Handle of Signal Data Device Callback
	CallibriSignalDataListenerHandle _lSignalDataHandle = nullptr;

	//Handle of Respiration Data Device Callback
	CallibriRespirationDataListenerHandle _lRespirationDataHandle = nullptr;

	//Handle of Electrode State Device Callback
	CallibriElectrodeStateListenerHandle _lElectrodeStateHandle = nullptr;

	//Handle of Envelope Data Device Callback
	CallibriEnvelopeDataListenerHandle _lEnvelopeDataHandle = nullptr;

	//Handle of MEMS Data Device Callback
	MEMSDataListenerHandle _lMEMSDataHandle = nullptr;

	//Handle of Quaternion Data Device Callback
	QuaternionDataListenerHandle _lQuaternionDataHandle = nullptr;

	//Handle of Signal Data Device Callback (Em_St_Artifacts)
	CallibriSignalDataListenerHandle _lSignalDataEmStArtifactsHandle = nullptr;

	//Handle of Signal Data Device Callback (Callibri ECG)
	CallibriSignalDataListenerHandle _lSignalDataCallibriECGHandle = nullptr;

	//Handle of Signal Data Device Callback (Filters)
	CallibriSignalDataListenerHandle _lSignalDataFiltersHandle = nullptr;

	//Handle of Signal Data Device Callback (Spectrum)
	CallibriSignalDataListenerHandle _lSignalDataSpectrumHandle = nullptr;
};
