#pragma once

#include "sdk_api.h"
#include "log.h"

#include "filtersLib.h"

void SignalCallback_Filters(Sensor* sensor, CallibriSignalData* data, int32_t size, void* userData)
{
	EConsole::PrintLog("=== CALLIBRI SIGNAL DATA ===");
	for (int i = 0; i < size; i++)
	{
		EConsole::PrintLog("PackNum: ", data[i].PackNum);
		for (int j = 0; j < data[i].SzSamples; j++)
			EConsole::PrintLog("Value: ", data[i].Samples[j]);
	}

	FiltersLibSample* filtersLib = (FiltersLibSample*)userData;

	for (int i = 0; i < size; i++)
	{
		int arr_size = data[i].SzSamples;
		double* arr = new double[arr_size];
		for (int j = 0; j < arr_size; j++)
			arr[j] = filtersLib->processElement(data[i].Samples[j]);
		filtersLib->printResult(arr, arr_size);
		delete[] arr;
	}
}