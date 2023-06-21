#pragma once

/*
		#######################
		#					  #
		#	SAMPLE BRAINBIT	  #
		#					  #
		#######################


	Task: A simple work with
	device 'LEBrainBit'

	BrainBit supports:

	functions:
	- connect
	- disconnect
	
	- getSupportedFeatures
	- getSupportedCommands
	- getSupportedParameters

	- execCommand

	- getChannelsCount

	- getFamily

	- readName
	- writeName
	- readState
	- readAddress
	- readSerialNumber
	- writeSerialNumber
	- readBatteryPower
	- readSamplingFrequency
	- readGain
	- readDataOffset
	- readFirmwareMode
	- readVersion

	Callbacks:

	- BatteryCallback
	- ConnectionCallback
	- ResistCallback
	- SignalCallback

*/

#include "sdk_api.h"
#include "log.h"

#include <iostream>

#include "mathLib.h"
#include "filtersLib.h"
#include "spectrumLib.h"

class SampleBrainBit
{
public:

	// ==============
	// || BRAINBIT ||
	// ==============

	//Constructor % Destructor
	SampleBrainBit(Sensor* sensor);
	~SampleBrainBit();

	//Connect to device
	bool connect();

	//Disconnect from device
	bool disconnect();

	//Execute command
	bool execCommand(SensorCommand command);

	// =======================
	// || BRAINBIT SETTINGS ||
	// =======================

	//Read & write sampling frequency of device
	SensorSamplingFrequency readSamplingFrequency();

	//Read & write gain of device
	SensorGain readGain();

	//Read & write data offset of device
	SensorDataOffset readDataOffset();

	//Read & write firmware mode of device
	SensorFirmwareMode readFirmwareMode();

	// ===================================
	// || BRAINBIT SUPPORTED FUNCTIONAL ||
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
	// || BRAINBIT INFO ||
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

	//Read version of device
	SensorVersion readVersion();

	//Read battery power of device
	int32_t readBatteryPower();

	//Get count of channels
	int32_t getChannelsCount();

	//Get Family of device
	SensorFamily getFamily();

	// =======================
	// || BRAINBIT CALLBACK ||
	// =======================

//Add & remove Battery Power callback
	bool AddBatteryCallback();
	bool RemoveBatteryCallback();

	//Add & remove connection state callback
	bool AddConnectionStateCallback();
	bool RemoveConnectionStateCallback();

	//Add & remove signal data callback
	bool AddSignalCallbackBrainBit();
	bool RemoveSignalCallbackBrainBit();

	//Add & remove respiration data callback
	bool AddResistDataCallbackBrainBit();
	bool RemoveResistDataCallbackBrainBit();

	// =======================
	// || BRAINBIT CALLBACK ||
	// =======================

	bool AddSignalCallbackBrainBit_EmStArtifacts(MathLibSample* mathlib);
	bool RemoveSignalCallbackBrainBit_EmStArtifacts();

	bool AddSignalCallbackBrainBit_Filters(FiltersLibSample* mathlib);
	bool RemoveSignalCallbackBrainBit_Filters();

	bool AddSignalCallbackBrainBit_Spectrum(SpectrumLibSample* mathlib);
	bool RemoveSignalCallbackBrainBit_Spectrum();

private:
	//Object Sensor (our device)
	Sensor* _sensor;

	//Handle of Battery Power Callback
	BattPowerListenerHandle _lBattPowerHandle = 0;

	//Handle of Connection State Device Callback
	SensorStateListenerHandle _lStateHandle = 0;

	//Handle of Signal Data Device Callback
	BrainBitSignalDataListenerHandle _lSignalDataHandle = 0;

	//Handle of Resist Data Device Callback
	BrainBitResistDataListenerHandle _lResistDataHandle = 0;

	//Handle of Signal Data Device Callback (Em_St_Artifacts)
	BrainBitSignalDataListenerHandle _lSignalDataEmStArtifactsHandle = 0;

	//Handle of Signal Data Device Callback (Filters)
	BrainBitSignalDataListenerHandle _lSignalDataFiltersHandle = 0;

	//Handle of Signal Data Device Callback (Spectrum)
	BrainBitSignalDataListenerHandle _lSignalDataSpectrumHandle = 0;
};