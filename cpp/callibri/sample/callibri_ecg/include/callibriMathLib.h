#pragma once

/*

		#############################
		#							#
		#	SAMPLE CALLIBRI ECG		#
		#							#
		#############################

	FiltersLibSample is a custom class for using
	library 'Callibri ECG'. This mathematical 
	library for working with ECG data from the 
	device 'LECallibri'. The given example does 
	not provide the full functionality of the library.

	Example is provided in the file 
	'main_callibri_ecg.cpp'.

*/

#include <iostream>
#include <vector>

#include "callibri_ecg_common_api.h"

#include "log.h"

class CallibriMathLibSample
{
public:
	//Constructor of class
	CallibriMathLibSample();

	//Destructor of class
	~CallibriMathLibSample();

	//Push raw data
	void pushDataArr(const double* data, size_t size);

	//Process convert data
	void processDataArr();

	//Get results & print
	void printResult();

private:
	CallibriMathLib* _tCallibriMathPtr = nullptr;

	std::vector<double> _arr;
	int _size = 25;
};