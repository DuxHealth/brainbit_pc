#include "callibri.h"

SampleCallibri::SampleCallibri(Sensor* sensor)
{
	//Constructor of class
	//This functions will be called when you create class.
	// 
	//ALWAY INIZIALISE OBJECTS!

	//Copy object of Sensor
	_sensor = sensor;
}

SampleCallibri::~SampleCallibri()
{
	//Destructor of class
	//This functions will be called when you delete class.
	try
	{
		//Safety delete object of sensor. Use it for deleting objects
		freeSensor(_sensor);

		//If user user callbacks and forget to remove them, 
		//we wull check them and delete using callbacks.
		//This is a good variant for safety deleting 

		if(_lBattPowerHandle != 0)
			removeBatteryCallback(_lBattPowerHandle);
		if (_lStateHandle != 0)
			removeConnectionStateCallback(_lStateHandle);
		if (_lSignalDataHandle != 0)
			removeSignalCallbackCallibri(_lSignalDataHandle);
		if (_lRespirationDataHandle != 0)
			removeRespirationCallbackCallibri(_lRespirationDataHandle);
		if (_lElectrodeStateHandle != 0)
			removeElectrodeStateCallbackCallibri(_lElectrodeStateHandle);
		if (_lEnvelopeDataHandle != 0)
			removeEnvelopeDataCallbackCallibri(_lEnvelopeDataHandle);
		if (_lMEMSDataHandle != 0)
			removeMEMSDataCallback(_lMEMSDataHandle);
		if (_lQuaternionDataHandle != 0)
			removeQuaternionDataCallback(_lQuaternionDataHandle);

		if (_lSignalDataEmStArtifactsHandle != 0)
			removeSignalCallbackCallibri(_lSignalDataEmStArtifactsHandle);
		if (_lSignalDataCallibriECGHandle != 0)
			removeSignalCallbackCallibri(_lSignalDataCallibriECGHandle);
		if (_lSignalDataFiltersHandle != 0)
			removeSignalCallbackCallibri(_lSignalDataFiltersHandle);
		if (_lSignalDataSpectrumHandle != 0)
			removeSignalCallbackCallibri(_lSignalDataSpectrumHandle);

		_sensor = nullptr;
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

bool SampleCallibri::connect()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::disconnect()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::execCommand(SensorCommand command)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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