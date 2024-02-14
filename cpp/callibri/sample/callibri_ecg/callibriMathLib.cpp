#include "callibriMathLib.h"

CallibriMathLibSample::CallibriMathLibSample()
{
	try
	{
		//Create setting for lib
		int sampling_rate = 1000;
		int data_window = sampling_rate / 2;
		int nwins_for_pressure_index = 30;

		//Create object of lib
		_tCallibriMathPtr = createCallibriMathLib(sampling_rate, data_window, nwins_for_pressure_index);

		//Check if our object is null or not
		if(_tCallibriMathPtr == nullptr)
			throw std::invalid_argument("Callibri ECG lib is null!");

		CallibriMathLibInitFilter(_tCallibriMathPtr);
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

CallibriMathLibSample::~CallibriMathLibSample()
{
	try
	{
		CallibriMathLibClearData(_tCallibriMathPtr);
		freeCallibriMathLib(_tCallibriMathPtr);
		_tCallibriMathPtr = nullptr;
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

void CallibriMathLibSample::pushDataArr(const double* data, size_t size)
{
	try
	{
		if (_tCallibriMathPtr == nullptr)
			throw std::invalid_argument("Callibri ECG lib is null!");

		for (int i = 0; i < size; i++)
			_arr.push_back(data[i]);

		if (_arr.size() > _size)
		{
			CallibriMathLibPushData(_tCallibriMathPtr, _arr.data(), _arr.size());
			_arr.clear();
		}
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

void CallibriMathLibSample::processDataArr()
{
	try
	{
		if (_tCallibriMathPtr == nullptr)
			throw std::invalid_argument("Callibri ECG lib is null!");

		CallibriMathLibProcessDataArr(_tCallibriMathPtr);
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

void CallibriMathLibSample::printResult()
{
	try
	{
		if (_tCallibriMathPtr == nullptr)
			throw std::invalid_argument("Callibri ECG lib is null!");

		EConsole::PrintLog("=== CALLIBRI ECG DATA ===");

		if (CallibriMathLibRRdetected(_tCallibriMathPtr))
		{
			double RR_int = CallibriMathLibGetRR(_tCallibriMathPtr);
			double HeartRate = CallibriMathLibGetHR(_tCallibriMathPtr);
			double PressureIndex = CallibriMathLibGetPressureIndex(_tCallibriMathPtr);

			double Moda = CallibriMathLibGetModa(_tCallibriMathPtr);
			double AmplModa = CallibriMathLibGetAmplModa(_tCallibriMathPtr);
			double VariationDist = CallibriMathLibGetVariationDist(_tCallibriMathPtr);

			if (RR_int > 0)
				EConsole::PrintLog("RR (interval): ", RR_int);

			if (HeartRate > 0)
				EConsole::PrintLog("HR(bpm) : ", HeartRate);

			if (PressureIndex > 0)
			{
				EConsole::PrintLog("Pressure Index: ", PressureIndex);
				EConsole::PrintLog("Moda (s): ", Moda);
				EConsole::PrintLog("Ampl of moda (%): ", AmplModa);
				EConsole::PrintLog("Variation Dist (s): ", VariationDist);
			}

			CallibriMathLibSetRRchecked(_tCallibriMathPtr);
		}

		EConsole::PrintLog("=== === ===");
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