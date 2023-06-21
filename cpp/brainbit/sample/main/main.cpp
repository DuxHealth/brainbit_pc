/*
						  WELCOME TO THE TUTORIAL
							'HOW TO USE SDK v2'

	This is the tutorial, where there are useful advices and commands,
	how to use library 'SDK v2'. It'd be wonderful, if this tutorial
	is helpful for us.

	Library 'SDK v2' is a software, which could you control devices.
	In this tutorial we show example using device - LEBrainBit & LEBrainBitBlack.

	In the tutorial there are examples, how to search, connect, 
	disconnect, manipulate the above devices.

	Before reviewing the examples you're recommended to look folder
	'brainbit'. It's a simple example with a detailed 
	explanation of the operation for searching settings, starting and 
	finishing scan and device management.

	In the folder 'brainbit' there are command, which are for device
	'LEBrainBit' & 'LEBrainBitBlack' and need to manipulate it.

	In the file 'main_brainbit.cpp' there are examples, how you can
	simplify your code.

	=== Using math libs ===

	In the folder 'em_st_artifacts' there is a custom class for using
	library 'Emotional State Artifacts'. This library is necessary 
	for calculations of spectral and mental data. The given example 
	does not provide the full functionality of the library. The 
	project uses only those commands that are necessary to calculate 
	the final data.
	Example is provided in the file 'main_brainbit_em_st_artifacts.cpp'.

	In the folder 'filters' there is a custom class for using
	library 'Filters'. This library is necessary for creating 
	filters and changing data for filter characteristics.
	Example is provided in the file 'main_brainbit_filters.cpp'.

	In the folder 'spectrum' there is a custom class for using
	library 'SpectrumMathLib'. Mathematical library for calculating 
	the signal spectrum. The main functionality is the calculation 
	of the raw values of the spectrum and the calculation of the 
	values of the spectrum of EEG waves.
	Example is provided in the file 'main_brainbit_spectrum.cpp'.

	=== === ===

	If you want to test the operation of the example that in the 
	file 'main.cpp', change the parameter of the type 'BrainBitSample'
	to run a specific code.
	
	Have a good day!

	Tutorial was created by Boldyrev Sanal.
*/

// [[Custom header file for logging information in console ]]
// You can use C library with std::cout for logging or use
// custom header file (log.h).

#include "log.h"

// [[ For scanning device and object of device ]]

#include "main_scanner.h"

// [[ For controlling device ]]

#include "main_brainbit.h"

// [[ For using lib 'Emotional State Artifacts' ]]

#include "main_brainbit_em_st_artifacts.h"

// [[ For using lib 'Filters' ]]

#include "main_brainbit_filters.h"

// [[ For using lib 'SpectrumMathLib' ]]

#include "main_brainbit_spectrum.h"

enum BrainBitSample
{
	BrainBit,					//For compile code to find device & connect device 'LEBrainBit' & 'LEBrainBitBlack'
	BrainBit_EmStArtifacts,		//For connect device & convert raw data to spectral data with lib
	BrainBit_Filters,			//For connect device & convert raw data with filters
	BrainBit_Spectrum			//For connect device & convert raw data to spectrum with lib
};

// =======================
// || MAIN_BRAINBIT.CPP ||
// =======================

void BrainBitFunction()
{
	//Create Sensor to connect to device (LEBrainBit)
	Sensor* brainbit = SampleScannerFunction();

	//If object is null, show information of not found device and stop code
	if (brainbit == nullptr)
	{
		EConsole::PrintLog("Can't find device!");
		return;
	}

	//Compile code to connect device & control
	SampleBrainBitFunction(brainbit);
}

// =======================================
// || MAIN_BRAINBIT_EM_ST_ARTIFACTS.CPP ||
// =======================================

void BrainBitEmStArtifactsFunction()
{
	//Create Sensor to connect to device (LEBrainBit)
	Sensor* brainbit = SampleScannerFunction();

	//If object is null, show information of not found device and stop code
	if (brainbit == nullptr)
	{
		EConsole::PrintLog("Can't find device!");
		return;
	}

	//Compile code to using lib 'Emotional State Artifacts'
	SampleBrainBitEmStArtifacts(brainbit);
}

// ===============================
// || MAIN_BRAINBIT_FILTERS.CPP ||
// ===============================

void BrainBitFiltersFunction()
{
	//Create Sensor to connect to device (LEBrainBit)
	Sensor* brainbit = SampleScannerFunction();

	//If object is null, show information of not found device and stop code
	if (brainbit == nullptr)
	{
		EConsole::PrintLog("Can't find device!");
		return;
	}

	//Compile code to using lib 'filters'
	SampleBrainBitFilters(brainbit);
}

// ================================
// || MAIN_BRAINBIT_SPECTRUM.CPP ||
// ================================

void BrainBitSpectrumFunction()
{
	//Create Sensor to connect to device (LEBrainBit)
	Sensor* brainbit = SampleScannerFunction();

	//If object is null, show information of not found device and stop code
	if (brainbit == nullptr)
	{
		EConsole::PrintLog("Can't find device!");
		return;
	}

	//Compile code to using lib 'spectrum'
	SampleBrainBitSpectrum(brainbit);
}

// ==============
// || MAIN.CPP ||
// ==============

int main()
{
	BrainBitSample code = BrainBitSample::BrainBit_EmStArtifacts;

	switch (code)
	{
	case BrainBit:
		BrainBitFunction();
		break;
	case BrainBit_EmStArtifacts:
		BrainBitEmStArtifactsFunction();
		break;
	case BrainBit_Filters:
		BrainBitFiltersFunction();
		break;
	case BrainBit_Spectrum:
		BrainBitSpectrumFunction();
		break;
	default:
		EConsole::PrintError("Invalid value of type 'BrainBitSample'!");
		break;
	}

	return 0;
}