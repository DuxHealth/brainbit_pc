#include "brainbit.h"

SensorSamplingFrequency SampleBrainBit::readSamplingFrequency()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		SensorSamplingFrequency samplingFrequency;

		OpStatus outStatus;

		bool result = readSamplingFrequencySensor(_sensor, &samplingFrequency, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return samplingFrequency;
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

SensorGain SampleBrainBit::readGain()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		SensorGain gain;

		OpStatus outStatus;

		bool result = readGainSensor(_sensor, &gain, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return gain;
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

SensorDataOffset SampleBrainBit::readDataOffset()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		SensorDataOffset dataOffset;

		OpStatus outStatus;

		bool result = readDataOffsetSensor(_sensor, &dataOffset, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return dataOffset;
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

SensorFirmwareMode SampleBrainBit::readFirmwareMode()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		SensorFirmwareMode firmwareMode;

		OpStatus outStatus;

		bool result = readFirmwareModeSensor(_sensor, &firmwareMode, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return firmwareMode;
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
