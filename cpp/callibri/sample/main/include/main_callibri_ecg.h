#pragma once

/*
	#################################
	#								#
	#	CALLIBRI (CALLIBRI ECG)		#
	#								#
	#################################

	This example is a sample, how use lib 'Callibri ECG'
	with device 'LECallibri'.

	Algorythm:
	- Find a device 'LECallibri'
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

// [[Example using callibri (folder 'callibri') ]]

#include "callibri.h"

// [[Custom class for using lib 'Callibri ECG' (folder 'callibri_ecg') ]]

#include "callibriMathLib.h"

void SampleCallibriECG(Sensor* sensor_callibri);