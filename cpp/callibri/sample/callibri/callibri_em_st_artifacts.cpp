#include "callibri.h"

#include "signalCallback_em_st_artifacts.h"

bool SampleCallibri::AddSignalCallbackCallibri_EmStArtifacts(MathLibSample* mathlib)
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		OpStatus outStatus;

		bool result = addSignalCallbackCallibri(_sensor, SignalCallback_EmStArtifacts, &_lSignalDataEmStArtifactsHandle, mathlib, &outStatus);

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

bool SampleCallibri::RemoveSignalCallbackCallibri_EmStArtifacts()
{
	try
	{
		//Check if our sensor is empty or not.
		if (_sensor == nullptr)
			throw std::invalid_argument("Null sensor!");

		if (_lSignalDataEmStArtifactsHandle == 0)
		{
			throw std::invalid_argument("Null Signal Data for EmStArtifacts handle!");
		}

		removeSignalCallbackCallibri(_lSignalDataEmStArtifactsHandle);
		_lSignalDataEmStArtifactsHandle = 0;

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