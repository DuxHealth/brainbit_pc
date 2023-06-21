#include "brainbit.h"

std::string SampleBrainBit::readName()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		int32_t sizeName = 1;
		char* name = new char[sizeName];

		OpStatus outStatus;

		bool result = readNameSensor(_sensor, name, sizeName, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		std::string nameSensor(name);

		return nameSensor;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return empty value value means our algorythm is working incorrect.
		return "";
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return empty value means our algorythm is working incorrect.
		return "";
	}
}

bool SampleBrainBit::writeName(std::string name)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		int32_t size = name.length() + 1;
		char* nameSensor = new char[name.length() + 1];
		strcpy(nameSensor, name.c_str());

		OpStatus outStatus;

		bool result = writeNameSensor(_sensor, nameSensor, size, &outStatus);

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
		//Return false value value means our algorythm is working incorrect.
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

SensorState SampleBrainBit::readState()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		SensorState sensorState;

		OpStatus outStatus;

		bool result = readStateSensor(_sensor, &sensorState, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return sensorState;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return empty value means our algorythm is working incorrect.
		return {};
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return empty value means our algorythm is working incorrect.
		return {};
	}
}

std::string SampleBrainBit::readAddress()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		int32_t sizeAddress = 1;
		char* address = new char[sizeAddress];

		OpStatus outStatus;

		bool result = readAddressSensor(_sensor, address, sizeAddress, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		std::string addressSensor(address);

		return addressSensor;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return empty value means our algorythm is working incorrect.
		return "";
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return empty value means our algorythm is working incorrect.
		return "";
	}
}

std::string SampleBrainBit::readSerialNumber()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		int32_t sizeSerialNumber = 1;
		char* serialNumber = new char[sizeSerialNumber];

		OpStatus outStatus;

		bool result = readSerialNumberSensor(_sensor, serialNumber, sizeSerialNumber, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		std::string serialNumberSensor(serialNumber);

		return serialNumber;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return empty value means our algorythm is working incorrect.
		return "";
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return empty value means our algorythm is working incorrect.
		return "";
	}
}

bool SampleBrainBit::writeSerialNumber(std::string serialNumber)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		int32_t size = serialNumber.length() + 1;
		char* serialNumberSensor = new char[serialNumber.length() + 1];
		strcpy(serialNumberSensor, serialNumber.c_str());

		OpStatus outStatus;

		bool result = writeSerialNumberSensor(_sensor, serialNumberSensor, size, &outStatus);

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

SensorVersion SampleBrainBit::readVersion()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		SensorVersion version;

		OpStatus outStatus;

		bool result = readVersionSensor(_sensor, &version, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return version;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return empty value means our algorythm is working incorrect.
		return {};
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return empty value means our algorythm is working incorrect.
		return {};
	}
}

int32_t SampleBrainBit::readBatteryPower()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		int32_t batteryPower = 0;

		OpStatus outStatus;

		bool result = readBattPowerSensor(_sensor, &batteryPower, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return batteryPower;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return negative value means our algorythm is working incorrect.
		return -1;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return negative value means our algorythm is working incorrect.
		return -1;
	}
}

int32_t SampleBrainBit::getChannelsCount()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		int32_t count = getChannelsCountSensor(_sensor);

		return count;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return negative value means our algorythm is working incorrect.
		return -1;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return negative value means our algorythm is working incorrect.
		return -1;
	}
}

SensorFamily SampleBrainBit::getFamily()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		SensorFamily result = getFamilySensor(_sensor);

		return result;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return empty value means our algorythm is working incorrect.
		return {};
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return empty value means our algorythm is working incorrect.
		return {};
	}
}