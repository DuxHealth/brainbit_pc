#include "main_callibri.h"

// ==============
// || CALLIBRI ||
// ==============

void SampleCallibriFunction(Sensor* sensor_callibri)
{
	//Create custom object of Callibri.
	//If you want to know, how to work with device ('LECallibri')
	//you can look in folder 'callibri'.
	SampleCallibri* callibri = new SampleCallibri(sensor_callibri);

	//In this examples you can see some functions to
	//control device 'LECallibri'.

	//Read name of device
	std::string name = callibri->readName();

	//Get color of device
	CallibriColorType color = callibri->readColor();

	//Disconnect from device
	callibri->disconnect();

	//Connect to device
	callibri->connect();

	//Add callback to get information about batter power of device
	callibri->AddBatteryCallback();

	//Remove callback to stop getting information about batter power of device
	//If you use callbacks, don't forget to remove them.
	//In custom class 'SampleCallibri' if user forget to remove, in
	//destructor there is a algorythm to check, remove or not callbacks
	callibri->RemoveBatteryCallback();

	//To get information signal data from device we need to use
	//callback 'SignalDataCallbackCallibri'.
	callibri->AddSignalCallbackCallibri();

	//Don't forget to remove callback signal data
	callibri->RemoveSignalCallbackCallibri();

	//To get data offset of callibri use function 'readDataOffset'
	SensorDataOffset dataOffset = callibri->readDataOffset();

	//To get firmware mode of callibri use function 'readFirmwareMode'
	SensorFirmwareMode firmwareMode = callibri->readFirmwareMode();

	//To get ADCInput of callibri use function 'readADCInput'
	SensorADCInput adcInput = callibri->readADCInput();

	//To get stimulation parameters of callibri use function 'readStimulatorParam'
	CallibriStimulationParams stimulationParams = callibri->readStimulatorParam();

	//To execuate command we use function 'execCommand'.
	//Before use this command you need to be sure,
	//that this device supports this command

	SensorCommand command = SensorCommand::CommandStartCurrentStimulation;

	bool isSupport = callibri->isSupportedCommand(command);

	if (isSupport)
	{
		EConsole::PrintLog("[LOG] [This command is supported by device!");

		//If this command is supported by device, you can execuate it

		callibri->execCommand(command);
	}
	else
	{
		EConsole::PrintLog("[WARNING] [This command is not supported by device!");
	}

	//If you don't use object of custom class, you need to
	//delete to clear memory.
	delete callibri;
}