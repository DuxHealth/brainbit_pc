#include "callibri.h"

SensorSamplingFrequency SampleCallibri::readSamplingFrequency()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::writeSamplingFrequency(SensorSamplingFrequency samplingFrequency)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeSamplingFrequencySensor(_sensor, samplingFrequency, &outStatus);

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

SensorGain SampleCallibri::readGain()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::writeGain(SensorGain gain)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeGainSensor(_sensor, gain, &outStatus);

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

SensorDataOffset SampleCallibri::readDataOffset()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::writeDataOffset(SensorDataOffset dataOffset)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeDataOffsetSensor(_sensor, dataOffset, &outStatus);

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

SensorFirmwareMode SampleCallibri::readFirmwareMode()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::writeFirmwareMode(SensorFirmwareMode firmwareMode)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeFirmwareModeSensor(_sensor, firmwareMode, &outStatus);

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

std::vector<SensorFilter> SampleCallibri::readHardwareFilters()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		int32_t countFilter = 1;

		SensorFilter* filters = new SensorFilter[countFilter];

		OpStatus outStatus;

		bool result = readHardwareFiltersSensor(_sensor, filters, &countFilter, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		std::vector<SensorFilter> listFilter;

		for (int i = 0; i < countFilter; i++)
		{
			listFilter.push_back(filters[i]);
		}

		return listFilter;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return empty array value means our algorythm is working incorrect.
		return {};
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return empty array value means our algorythm is working incorrect.
		return {};
	}
}

bool SampleCallibri::writeHardwareFilters(std::vector<SensorFilter> hardwareFilters)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		int32_t size = hardwareFilters.size();
		SensorFilter* filters = hardwareFilters.data();

		OpStatus outStatus;

		bool result = writeHardwareFiltersSensor(_sensor, filters, size, &outStatus);

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

SensorExternalSwitchInput SampleCallibri::readExternalSwitch()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		SensorExternalSwitchInput externalSwitchInput;

		OpStatus outStatus;

		bool result = readExternalSwitchSensor(_sensor, &externalSwitchInput, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return externalSwitchInput;
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

bool SampleCallibri::writeExternalSwitch(SensorExternalSwitchInput externalSwitchInput)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeExternalSwitchSensor(_sensor, externalSwitchInput, &outStatus);

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

SensorADCInput SampleCallibri::readADCInput()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		SensorADCInput adcInput;

		OpStatus outStatus;

		bool result = readADCInputSensor(_sensor, &adcInput, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return adcInput;
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

bool SampleCallibri::writeADCInput(SensorADCInput adcInput)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeADCInputSensor(_sensor, adcInput, &outStatus);

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


CallibriStimulatorMAState SampleCallibri::readStimulatorAndMAState()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		CallibriStimulatorMAState stimulatorMAState;

		OpStatus outStatus;

		bool result = readStimulatorAndMAStateCallibri(_sensor, &stimulatorMAState, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return stimulatorMAState;
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

CallibriStimulationParams SampleCallibri::readStimulatorParam()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		CallibriStimulationParams stimulationParams;

		OpStatus outStatus;

		bool result = readStimulatorParamCallibri(_sensor, &stimulationParams, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return stimulationParams;
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

bool SampleCallibri::writeStimulatorParam(CallibriStimulationParams stimulatorParam)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeStimulatorParamCallibri(_sensor, stimulatorParam, &outStatus);

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

CallibriMotionAssistantParams SampleCallibri::readMotionAssistantParam()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		CallibriMotionAssistantParams motionAssistantParams;

		OpStatus outStatus;

		bool result = readMotionAssistantParamCallibri(_sensor, &motionAssistantParams, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return motionAssistantParams;
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

bool SampleCallibri::writeMotionAssisstantParam(CallibriMotionAssistantParams motionAssistantParam)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeMotionAssistantParamCallibri(_sensor, motionAssistantParam, &outStatus);

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

CallibriMotionCounterParam SampleCallibri::readMotionCounterParam()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		CallibriMotionCounterParam motionCounterParam;

		OpStatus outStatus;

		bool result = readMotionCounterParamCallibri(_sensor, &motionCounterParam, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return motionCounterParam;
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

bool SampleCallibri::writeMotionCounterParam(CallibriMotionCounterParam motionAssistantCounter)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeMotionCounterParamCallibri(_sensor, motionAssistantCounter, &outStatus);

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

uint32_t SampleCallibri::readMotionCounter()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		uint32_t motionCounter = 1;

		OpStatus outStatus;

		bool result = readMotionCounterCallibri(_sensor, &motionCounter, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return motionCounter;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		//Return null value means our algorythm is working incorrect.
		return 0;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		//Return null value means our algorythm is working incorrect.
		return 0;
	}
}

bool SampleCallibri::readMEMSCalibrateState()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		uint8_t calibrateState = false;

		OpStatus outStatus;

		bool result = readMEMSCalibrateStateCallibri(_sensor, &calibrateState, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return calibrateState;
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

SensorSamplingFrequency SampleCallibri::readSamplingFrequencyResp()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		SensorSamplingFrequency samplingFrequency;

		OpStatus outStatus;

		bool result = readSamplingFrequencyRespSensor(_sensor, &samplingFrequency, &outStatus);

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

bool SampleCallibri::setSignalSettings(SignalTypeCallibri signalSetting)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = setSignalSettingsCallibri(_sensor, signalSetting, &outStatus);

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

SignalTypeCallibri SampleCallibri::getSignalSettings()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		SignalTypeCallibri signalType;

		OpStatus outStatus;

		bool result = getSignalSettingsCallibri(_sensor, &signalType, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return signalType;
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

SensorAccelerometerSensitivity SampleCallibri::readAccelerometerSens()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		SensorAccelerometerSensitivity accelerometerSens;

		OpStatus outStatus;

		bool result = readAccelerometerSensSensor(_sensor, &accelerometerSens, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return accelerometerSens;
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

bool SampleCallibri::writeAccelerometerSens(SensorAccelerometerSensitivity accerometerSens)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeAccelerometerSensSensor(_sensor, accerometerSens, &outStatus);

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

SensorGyroscopeSensitivity SampleCallibri::readGyroscopeSens()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		SensorGyroscopeSensitivity gyroscopeSens;

		OpStatus outStatus;

		bool result = readGyroscopeSensSensor(_sensor, &gyroscopeSens, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		return gyroscopeSens;
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

bool SampleCallibri::writeGyroscopeSens(SensorGyroscopeSensitivity gyroscopeSens)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = writeGyroscopeSensSensor(_sensor, gyroscopeSens, &outStatus);

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