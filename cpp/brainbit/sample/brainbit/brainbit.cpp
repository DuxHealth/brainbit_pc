#include "brainbit.h"

SampleBrainBit::SampleBrainBit(Sensor* sensor)
{
	//Constructor of class
	//This functions will be called when you create class.
	// 
	//ALWAY INIZIALISE OBJECTS!

	//Copy object of Sensor
	_sensor = sensor;
}

SampleBrainBit::~SampleBrainBit()
{
	//Destructor of class
	//This functions will be called when you delete class.
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		//Safety delete object of sensor. Use it for deleting objects
		freeSensor(_sensor);

		//If user user callbacks and forget to remove them, 
		//we wull check them and delete using callbacks.
		//This is a good variant for safety deleting 

		if (_lBattPowerHandle != 0)
			removeBatteryCallback(_lBattPowerHandle);
		if (_lStateHandle != 0)
			removeConnectionStateCallback(_lStateHandle);
		if (_lSignalDataHandle != 0)
			removeSignalDataCallbackBrainBit(_lSignalDataHandle);
		if (_lResistDataHandle != 0)
			removeResistCallbackBrainBit(_lResistDataHandle);

		if (_lSignalDataEmStArtifactsHandle != 0)
			removeSignalDataCallbackBrainBit(_lSignalDataEmStArtifactsHandle);
		if (_lSignalDataFiltersHandle != 0)
			removeSignalDataCallbackBrainBit(_lSignalDataFiltersHandle);
		if (_lSignalDataSpectrumHandle != 0)
			removeSignalDataCallbackBrainBit(_lSignalDataSpectrumHandle);
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
	}
}

bool SampleBrainBit::connect()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		//Create boolean for get information of successful or failed operation
		OpStatus outStatus;

		//Function 'connectSensor' is need to connect to device
		bool result = connectSensor(_sensor, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return true;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return false value means our algorythm is working incorrect.
		return false;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return false value means our algorythm is working incorrect.
		return false;
	}
}

bool SampleBrainBit::disconnect()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		//Create boolean for get information of successful or failed operation (disconnection from device)
		OpStatus outStatus;

		//Function 'disconnectSensor' is need to disconnect to device
		bool result = disconnectSensor(_sensor, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return true;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return false value means our algorythm is working incorrect.
		return false;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return false value means our algorythm is working incorrect.
		return false;
	}
}

bool SampleBrainBit::execCommand(SensorCommand command)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		//Create boolean for get information of successful or failed operation
		OpStatus outStatus;

		//Function 'execCommandSensor' need to execuate commands
		bool result = execCommandSensor(_sensor, command, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return true;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return false value means our algorythm is working incorrect.
		return false;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return false value means our algorythm is working incorrect.
		return false;
	}
}