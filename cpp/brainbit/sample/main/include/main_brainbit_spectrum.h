#pragma once

/*
	#############################
	#							#
	#	BRAINBIT (SPECTRUM)		#
	#							#
	#############################

	This example is a sample, how use lib 'Spectrum'
	with device 'LEBrainBit'.

	Algorythm:
	- Find a device 'LEBrainBit'
	- Connect to device
	- Start signal ECG
	- Setting data to lib
	- Stop signal ECG
	- Getting results
*/

// [[Custom header file for logging information in console ]]
// You can use C library with std::cout for logging or use
// custom header file (log.h).

#include "log.h"

// [[Example using brainbit (folder 'brainbit') ]]

#include "brainbit.h"

// [[Custom class for using lib 'SpectrumMathLib' (folder 'spectrum') ]]

#include "spectrumLib.h"

void SampleBrainBitSpectrum(Sensor* sensor_brainbit);
