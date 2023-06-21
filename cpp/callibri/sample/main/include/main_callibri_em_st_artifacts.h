#pragma once

/*

	#############################################
	#											#
	#	CALLIBRI (EMOTIONAL STATE ARTIFACTS)	#
	#											#
	#############################################

	This example is a sample, how use lib 'Emotional State Artifacts'
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

// [[Custom class for using lib 'Emotional State Artifacts' (folder 'em_st_artifacts') ]]

#include "mathLib.h"

void SampleCallibriEmStArtifacts(Sensor* sensor_callibri);