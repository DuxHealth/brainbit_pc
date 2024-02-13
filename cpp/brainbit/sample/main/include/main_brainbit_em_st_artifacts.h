#pragma once

/*

	#############################################
	#											#
	#	BRAINBIT (EMOTIONAL STATE ARTIFACTS)	#
	#											#
	#############################################

	This example is a sample, how use lib 'Emotional State Artifacts'
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

// [[Custom class for using lib 'Emotional State Artifacts' (folder 'em_st_artifacts') ]]

#include "mathLib.h"

#include <chrono>
#include <thread>

void SampleBrainBitEmStArtifacts(Sensor* sensor_brainbit);