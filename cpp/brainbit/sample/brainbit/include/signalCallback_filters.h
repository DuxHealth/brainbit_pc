#pragma once

#include "sdk_api.h"
#include "log.h"

#include "filtersLib.h"

void SignalCallback_Filters(Sensor* sensor, BrainBitSignalData* data, int32_t size, void* userData)
{
	EConsole::PrintLog("=== BRAIN BIT SIGNAL DATA ===");
	RawChannels* arr = new RawChannels[size];
	for (int i = 0; i < size; i++)
	{
		arr[i].left_bipolar = data[i].T3 - data[i].O1;
		arr[i].right_bipolar = data[i].T4 - data[i].O2;

		EConsole::PrintLog("Left bipolar: ", arr[i].left_bipolar);
		EConsole::PrintLog("Right bipolar: ", arr[i].right_bipolar);
	}

	FiltersLibSample* filtersLib = (FiltersLibSample*)userData;

	double* arr_filter = new double[size];
	for (int i = 0; i < size; i++)
		arr_filter[i] = filtersLib->processElement(arr[i].left_bipolar);

	filtersLib->printResult(arr_filter, size);
	delete[] arr;
	delete[] arr_filter;
}