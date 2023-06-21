#include "brainbit.h"

#include "batteryCallback.h"
#include "connectionCallback.h"
#include "resistCallback.h"
#include "signalCallback.h"

bool SampleBrainBit::AddBatteryCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		OpStatus outStatus;

		bool result = addBatteryCallback(_sensor, BatteryCallback, &_lBattPowerHandle, nullptr, &outStatus);

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

bool SampleBrainBit::AddConnectionStateCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		OpStatus outStatus;
		
		bool result = addConnectionStateCallback(_sensor, ConnectionCallback, &_lStateHandle, nullptr, &outStatus);

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

bool SampleBrainBit::AddSignalCallbackBrainBit()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		OpStatus outStatus;

		bool result = addSignalDataCallbackBrainBit(_sensor, SignalCallback, &_lSignalDataHandle, nullptr, &outStatus);

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

bool SampleBrainBit::AddResistDataCallbackBrainBit()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		OpStatus outStatus;

		bool result = addResistCallbackBrainBit(_sensor, ResistCallback, &_lResistDataHandle, nullptr, &outStatus);

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

bool SampleBrainBit::RemoveBatteryCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		//Check if handle is empty or not.
		if (_lBattPowerHandle == 0)
		{
			throw std::invalid_argument("Null Battery power handle!");
		}

		removeBatteryCallback(_lBattPowerHandle);
		_lBattPowerHandle = 0;

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

bool SampleBrainBit::RemoveConnectionStateCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		//Check if handle is empty or not.
		if (_lStateHandle == 0)
		{
			throw std::invalid_argument("Null Sensor state handle!");
		}

		removeConnectionStateCallback(_lStateHandle);
		_lStateHandle = 0;

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

bool SampleBrainBit::RemoveSignalCallbackBrainBit()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		//Check if handle is empty or not.
		if (_lSignalDataHandle == 0)
		{
			throw std::invalid_argument("Null Signal data handle!");
		}

		removeSignalDataCallbackBrainBit(_lSignalDataHandle);
		_lSignalDataHandle = 0;

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

bool SampleBrainBit::RemoveResistDataCallbackBrainBit()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
		{
			throw std::invalid_argument("Null sensor!");
		}

		//Check if handle is empty or not.
		if (_lResistDataHandle == 0)
		{
			throw std::invalid_argument("Null Respiration data handle!");
		}

		removeResistCallbackBrainBit(_lResistDataHandle);
		_lResistDataHandle = 0;

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