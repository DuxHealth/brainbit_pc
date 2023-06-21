#include "callibri.h"

#include "batteryCallback.h"
#include "connectionCallback.h"
#include "electrodeStateCallback.h"
#include "envelopeDataCallback.h"
#include "memsDataCallback.h"
#include "quaternionDataCallback.h"
#include "respirationCallback.h"
#include "signalCallback.h"

bool SampleCallibri::AddBatteryCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::AddConnectionStateCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::AddSignalCallbackCallibri()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = addSignalCallbackCallibri(_sensor, SignalCallback, &_lSignalDataEmStArtifactsHandle, nullptr, &outStatus);

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

bool SampleCallibri::AddRespirationCallbackCallibri()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = addRespirationCallbackCallibri(_sensor, RespirationCallback, &_lRespirationDataHandle, nullptr, &outStatus);

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

bool SampleCallibri::AddElectrodeStateCallbackCallibri()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = addElectrodeStateCallbackCallibri(_sensor, ElectrodeStateCallback, &_lElectrodeStateHandle, nullptr, &outStatus);

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

bool SampleCallibri::AddEnvelopeDataCallbackCallibri()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = addEnvelopeDataCallbackCallibri(_sensor, EnvelopeDataCallback, &_lEnvelopeDataHandle, nullptr, &outStatus);

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

bool SampleCallibri::AddQuaternionDataCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = addQuaternionDataCallback(_sensor, QuaternionDataCallback, &_lQuaternionDataHandle, nullptr, &outStatus);

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

bool SampleCallibri::AddMEMSDataCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = addMEMSDataCallback(_sensor, MEMSDataCallback, &_lMEMSDataHandle, nullptr, &outStatus);

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

bool SampleCallibri::RemoveBatteryCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::RemoveConnectionStateCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

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

bool SampleCallibri::RemoveSignalCallbackCallibri()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		//Check if handle is empty or not.
		if (_lSignalDataHandle == 0)
		{
			throw std::invalid_argument("Null Signal data handle!");
		}

		removeSignalCallbackCallibri(_lSignalDataHandle);
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

bool SampleCallibri::RemoveRespirationCallbackCallibri()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		//Check if handle is empty or not.
		if (_lRespirationDataHandle == 0)
		{
			throw std::invalid_argument("Null Respiration data handle!");
		}

		removeRespirationCallbackCallibri(_lRespirationDataHandle);
		_lRespirationDataHandle = 0;

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

bool SampleCallibri::RemoveElectrodeStateCallbackCallibri()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		//Check if handle is empty or not.
		if (_lElectrodeStateHandle == 0)
		{
			throw std::invalid_argument("Null Electrode state handle!");
		}

		removeElectrodeStateCallbackCallibri(_lElectrodeStateHandle);
		_lElectrodeStateHandle = 0;

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

bool SampleCallibri::RemoveEnvelopeDataCallbackCallibri()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		//Check if handle is empty or not.
		if (_lEnvelopeDataHandle == 0)
		{
			throw std::invalid_argument("Null Envelope data handle!");
		}

		removeEnvelopeDataCallbackCallibri(_lEnvelopeDataHandle);
		_lEnvelopeDataHandle = 0;

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

bool SampleCallibri::RemoveQuaternionDataCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		//Check if handle is empty or not.
		if (_lQuaternionDataHandle == 0)
		{
			throw std::invalid_argument("Null Quaternion data handle!");
		}

		removeQuaternionDataCallback(_lQuaternionDataHandle);
		_lQuaternionDataHandle = 0;

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

bool SampleCallibri::RemoveMEMSDataCallback()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		//Check if handle is empty or not.
		if (_lMEMSDataHandle == 0)
		{
			throw std::invalid_argument("Null MEMS data handle!");
		}

		removeMEMSDataCallback(_lMEMSDataHandle);
		_lMEMSDataHandle = 0;

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