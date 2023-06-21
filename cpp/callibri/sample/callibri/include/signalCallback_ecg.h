#pragma once

#include "sdk_api.h"
#include "log.h"

#include "callibriMathLib.h"

void SignalCallback_Callibri_ECG(Sensor* sensor, CallibriSignalData* data, int32_t size, void* userData)
{
	EConsole::PrintLog("=== CALLIBRI SIGNAL DATA ===");
	for (int i = 0; i < size; i++)
	{
		EConsole::PrintLog("PackNum: ", data[i].PackNum);
		for (int j = 0; j < data[i].SzSamples; j++)
			EConsole::PrintLog("Value: ", data[i].Samples[j]);
	}

	CallibriMathLibSample* callibriMathLib = (CallibriMathLibSample*)userData;
	for (int i = 0; i < size; i++)
	{
		double* raw_data = new double[data[i].SzSamples];
		for (int j = 0; j < data[i].SzSamples; j++)
			raw_data[j] = data[i].Samples[j];
		callibriMathLib->pushDataArr(raw_data, data[i].SzSamples);
	}
	callibriMathLib->processDataArr();
}