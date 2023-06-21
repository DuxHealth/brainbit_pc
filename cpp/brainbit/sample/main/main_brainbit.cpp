#include "main_brainbit.h"

// ==============
// || BRAINBIT ||
// ==============

void SampleBrainBitFunction(Sensor* sensor_brainbit)
{
	//Create custom object of BrainBit.
	//If you want to know, how to work with device ('LEBrainBit & LEBrainBitBlack')
	//you can look in folder 'brainbit'.
	SampleBrainBit* brainbit = new SampleBrainBit(sensor_brainbit);

	//In this examples you can see some functions to
	//control device 'LEBrainBit & LEBrainBitBlack'.

	//Read name of device
	std::string name = brainbit->readName();

	//Disconnect from device
	brainbit->disconnect();

	//Connect to device
	brainbit->connect();

	//Add callback to get information about batter power of device
	brainbit->AddBatteryCallback();

	//Remove callback to stop getting information about batter power of device
	//If you use callbacks, don't forget to remove them.
	//In custom class 'SampleBrainBit' if user forget to remove, in
	//destructor there is a algorythm to check, remove or not callbacks
	brainbit->RemoveBatteryCallback();

	//To get information signal data from device we need to use
	//callback 'SignalCallbackBrainBit'.
	brainbit->AddSignalCallbackBrainBit();

	//Don't forget to remove callback signal data
	brainbit->RemoveSignalCallbackBrainBit();

	//To get information resist data from device we need to use
	//callback 'ResistDataCallbackBrainBit'.
	brainbit->AddResistDataCallbackBrainBit();

	//Don't forget to remove callback resist data
	brainbit->RemoveResistDataCallbackBrainBit();

	//To get data offset of brainbit use function 'readDataOffset'
	SensorDataOffset dataOffset = brainbit->readDataOffset();

	//To get firmware mode of brainbit use function 'readFirmwareMode'
	SensorFirmwareMode firmwareMode = brainbit->readFirmwareMode();

	//To execuate command we use function 'execCommand'.
	//Before use this command you need to be sure,
	//that this device supports this command

	SensorCommand command = SensorCommand::CommandStartSignal;

	bool isSupport = brainbit->isSupportedCommand(command);

	if (isSupport)
	{
		EConsole::PrintLog("[LOG] [This command is supported by device!");

		//If this command is supported by device, you can execuate it

		brainbit->execCommand(command);
	}
	else
	{
		EConsole::PrintLog("[WARNING] [This command is not supported by device!");
	}

	//If you don't use object of custom class, you need to
	//delete to clear memory.
	delete brainbit;
}