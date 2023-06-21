#pragma once

/*

		#######################
		#					  #
		#	SAMPLE SPECTRUM	  #
		#					  #
		#######################

	SpectrumLibSample is a custom class
	for using library 'SpectrumMathLib'. 
	Mathematical library for calculating 
	the signal spectrum. The main 
	functionality is the calculation 
	of the raw values of the spectrum 
	and the calculation of the values of 
	the spectrum of EEG waves.

	Example is provided in the file 'main_callibri_spectrum.cpp'.

*/

#include <iostream>

#include "common_api_spect.h"

#include "log.h"

class SpectrumLibSample
{
public:

	SpectrumLibSample();

	~SpectrumLibSample();

	void pushData(double* arr, int size);
	void processData();

	void printResult();

private:
	SpectrumMath* _spectrumMath;
};