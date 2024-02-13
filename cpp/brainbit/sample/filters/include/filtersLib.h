#pragma once

/*

		#######################
		#					  #
		#	SAMPLE FILTERS	  #
		#					  #
		#######################

	FiltersLibSample is a custom class for using
	library 'Filters'. This library is necessary 
	for creating filters and changing data for 
	filter characteristics. 
	
	Example is provided 
	in the file 'main_brainbit_filters.cpp'.

*/

#include <iostream>
#include <vector>

#include "filters.h"

#include "log.h"

class FiltersLibSample
{
public:
	//Constructor of class
	FiltersLibSample();

	//Destructor of class
	~FiltersLibSample();

	//Convert raw data with filters
	double processElement(double elem);


private:
	TFilter* _filter1 = nullptr;
	TFilter* _filter2 = nullptr;
	TFilter* _filter3 = nullptr;
	TFilter* _filter4 = nullptr;
	TFilterList* _flist = nullptr;
};