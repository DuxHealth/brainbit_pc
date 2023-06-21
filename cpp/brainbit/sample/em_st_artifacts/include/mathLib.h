#pragma once

/*

		#########################################
		#										#
		#	SAMPLE EMOTIONAL STATE ARTIFACTS	#
		#										#
		#########################################

	FiltersLibSample is a custom class for using
	library 'Emotional State Artifacts'. This 
	library is necessary for calculations of 
	spectral and mental data. The given example 
	does not provide the full functionality of the 
	library. The project uses only those commands 
	that are necessary to calculate the final data.

	Example is provided in the file 
	'main_brainbit_em_st_artifacts.cpp'.

*/

#include <iostream>
#include <vector>

#include "em_st_artifacts_common_api.h"

#include "log.h"

class MathLibSample
{
public:
	//Constructor of class
	MathLibSample();

	//Destructor of class
	~MathLibSample();

	//Push raw data
	bool pushData(RawChannels* data, int size);

	//Process convert data
	bool processData();

	//Get results & print
	void printResult();

private:
	MathLib* _mathLib = nullptr;

};