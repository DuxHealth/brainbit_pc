#include "brainbit.h"

std::vector<SensorFeature> SampleBrainBit::getSupportedFeatures()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		OpStatus outStatus;

		int32_t countFeature = 1;
		SensorFeature* listFeature = new SensorFeature[countFeature];

		bool result = getFeaturesSensor(_sensor, listFeature, &countFeature, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		std::vector<SensorFeature> sensorFeature;

		for (int i = 0; i < countFeature; i++)
		{
			sensorFeature.push_back(listFeature[i]);
		}

		return sensorFeature;
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

std::vector<SensorCommand> SampleBrainBit::getSupportedCommands()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		OpStatus outStatus;

		int32_t countCommand = 1;
		SensorCommand* listCommand = new SensorCommand[countCommand];

		bool result = getCommandsSensor(_sensor, listCommand, &countCommand, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		std::vector<SensorCommand> sensorCommand;

		for (int i = 0; i < countCommand; i++)
		{
			sensorCommand.push_back(listCommand[i]);
		}

		return sensorCommand;
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

std::vector<ParameterInfo> SampleBrainBit::getSupportedParameters()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		OpStatus outStatus;

		int32_t countParameter = 1;
		ParameterInfo* listParameter = new ParameterInfo[countParameter];

		bool result = getParametersSensor(_sensor, listParameter, &countParameter, &outStatus);

		if (!result)
		{
			throw std::invalid_argument(outStatus.ErrorMsg);
		}

		std::vector<ParameterInfo> sensorParameter;

		for (int i = 0; i < countParameter; i++)
		{
			sensorParameter.push_back(listParameter[i]);
		}

		return sensorParameter;
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

bool SampleBrainBit::isSupportedFeature(SensorFeature feature)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		return isSupportedFeatureSensor(_sensor, feature);
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

bool SampleBrainBit::isSupportedCommand(SensorCommand command)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		return isSupportedCommandSensor(_sensor, command);
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

bool SampleBrainBit::isSupportedParameter(SensorParameter parameter)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		return isSupportedParameterSensor(_sensor, parameter);
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