/*
						  WELCOME TO THE TUTORIAL
							'HOW TO USE SDK v2'

	This is the tutorial, where there are useful advices and commands,
	how to use library 'SDK v2'. It'd be wonderful, if this tutorial
	is helpful for us.

	Library 'SDK v2' is a software, which could you control devices.
	In this tutorial we show example using device - LECallibri.

	In the tutorial there are examples, how to search, connect, 
	disconnect, manipulate the above devices.

	Before reviewing the examples you're recommended to look folder
	'callibri'. It's a simple example with a detailed 
	explanation of the operation for searching settings, starting and 
	finishing scan and device management.

	In the folder 'callibri' there are command, which are for device
	'LECallibri' and need to manipulate it.

	In the file 'main_callibri.cpp' there are examples, how you can
	simplify your code.

	=== Using math libs ===

	In the folder 'em_st_artifacts' there is a custom class for using
	library 'Emotional State Artifacts'. This library is necessary 
	for calculations of spectral and mental data. The given example 
	does not provide the full functionality of the library. The 
	project uses only those commands that are necessary to calculate 
	the final data.
	Example is provided in the file 'main_callibri_em_st_artifacts.cpp'.

	In the folder 'callibri_ecg' there is a custom class for using
	library 'Callibri ECG'. This mathematical library for working 
	with ECG data from the device 'LECallibri'. The given example 
	does not provide the full functionality of the library.
	Example is provided in the file 'main_callibri_ecg.cpp'.

	In the folder 'filters' there is a custom class for using
	library 'Filters'. This library is necessary for creating 
	filters and changing data for filter characteristics.
	Example is provided in the file 'main_callibri_filters.cpp'.

	In the folder 'spectrum' there is a custom class for using
	library 'SpectrumMathLib'. Mathematical library for calculating 
	the signal spectrum. The main functionality is the calculation 
	of the raw values of the spectrum and the calculation of the 
	values of the spectrum of EEG waves.
	Example is provided in the file 'main_callibri_spectrum.cpp'.

	=== === ===

	If you want to test the operation of the example that in the 
	file 'main.cpp', change the parameter of the type 'CallibriSample'
	to run a specific code.
	
	Have a good day!

	Tutorial was created by Boldyrev Sanal.
*/

// [[ For scanning device and object of device ]]

#include "main_scanner.h"

// [[ For controlling device ]]

#include "main_callibri.h"

// [[ For using lib 'Emotional State Artifacts' ]]

#include "main_callibri_em_st_artifacts.h"

// [[ For using lib 'Callibri ECG' ]]

#include "main_callibri_ecg.h"

// [[ For using lib 'Filters' ]]

#include "main_callibri_filters.h"

// [[ For using lib 'SpectrumMathLib' ]]

#include "main_callibri_spectrum.h"

enum CallibriSample
{
	Callibri,					//For compile code to find device & connect device 'LECallibri'
	Callibri_EmStArtifacts,		//For connect device & convert raw data to spectral data with lib
	Callibri_ECG,				//For connect device & convert raw data to data ECG with lib
	Callibri_Filters,			//For connect device & convert raw data with filters
	Callibri_Spectrum			//For connect device & convert raw data to spectrum with lib
};

// =======================
// || MAIN_CALLIBRI.CPP ||
// =======================

void CallibriFunction()
{
	//Create Sensor to connect to device (LECallibri)
	Sensor* callibri = SampleScannerFunction();

	//If object is null, show information of not found device and stop code
	if (callibri == nullptr)
	{
		EConsole::PrintLog("Can't find device!");
		return;
	}

	//Compile code to connect device & control
	SampleCallibriFunction(callibri);
}

// =======================================
// || MAIN_CALLIBRI_EM_ST_ARTIFACTS.CPP ||
// =======================================

void CallibriEmStArtifactsFunction()
{
	//Create Sensor to connect to device (LECallibri)
	Sensor* callibri = SampleScannerFunction();

	//If object is null, show information of not found device and stop code
	if (callibri == nullptr)
	{
		EConsole::PrintLog("Can't find device!");
		return;
	}

	//Compile code to using lib 'Emotional State Artifacts'
	SampleCallibriEmStArtifacts(callibri);
}

// ===========================
// || MAIN_CALLIBRI_ECG.CPP ||
// ===========================

void CallibriECGFunction()
{
	//Create Sensor to connect to device (LECallibri)
	Sensor* callibri = SampleScannerFunction();

	//If object is null, show information of not found device and stop code
	if (callibri == nullptr)
	{
		EConsole::PrintLog("Can't find device!");
		return;
	}

	//Compile code to using lib 'callibri ecg'
	SampleCallibriECG(callibri);
}

// ===============================
// || MAIN_CALLIBRI_FILTERS.CPP ||
// ===============================

void CallibriFiltersFunction()
{
	//Create Sensor to connect to device (LECallibri)
	Sensor* callibri = SampleScannerFunction();

	//If object is null, show information of not found device and stop code
	if (callibri == nullptr)
	{
		EConsole::PrintLog("Can't find device!");
		return;
	}

	//Compile code to using lib 'filters'
	SampleCallibriFilters(callibri);
}

// ================================
// || MAIN_CALLIBRI_SPECTRUM.CPP ||
// ================================

void CallibriSpectrumFunction()
{
	//Create Sensor to connect to device (LECallibri)
	Sensor* callibri = SampleScannerFunction();

	//If object is null, show information of not found device and stop code
	if (callibri == nullptr)
	{
		EConsole::PrintLog("Can't find device!");
		return;
	}

	//Compile code to using lib 'spectrum'
	SampleCallibriSpectrum(callibri);
}

// ==============
// || MAIN.CPP ||
// ==============

int main()
{
	CallibriSample code = CallibriSample::Callibri_Filters;

	switch (code)
	{
	case Callibri:
		CallibriFunction();
		break;
	case Callibri_EmStArtifacts:
		CallibriEmStArtifactsFunction();
		break;
	case Callibri_ECG:
		CallibriECGFunction();
		break;
	case Callibri_Filters:
		CallibriFiltersFunction();
		break;
	case Callibri_Spectrum:
		CallibriSpectrumFunction();
		break;
	default:
		EConsole::PrintError("Invalid value of type 'CallibriSample'!");
		break;
	}

	return 0;
}