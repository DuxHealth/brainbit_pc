#include "callibri.h"

#include "signalCallback_ecg.h"

bool SampleCallibri::AddSignalCallbackCallibri_Callibri_ECG(CallibriMathLibSample* mathlib)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = addSignalCallbackCallibri(_sensor, SignalCallback_Callibri_ECG, &_lSignalDataCallibriECGHandle, mathlib, &outStatus);

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

bool SampleCallibri::RemoveSignalCallbackCallibri_Callibri_ECG()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		if (_lSignalDataCallibriECGHandle == 0)
		{
			throw std::invalid_argument("Null Signal Data for Callibri ECG handle!");
		}

		removeSignalCallbackCallibri(_lSignalDataCallibriECGHandle);
		_lSignalDataCallibriECGHandle = 0;

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