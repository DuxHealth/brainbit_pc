#pragma once

#include <vector>

#include "sdk_api.h"
#include "log.h"

#include "mathLib.h"

void SignalCallback_EmStArtifacts(Sensor* sensor, CallibriSignalData* data, int32_t size, void* userData)
{
	
	EConsole::PrintLog("=== CALLIBRI SIGNAL DATA ===");
	
	for (int i = 0; i < size; i++)
	{
		EConsole::PrintLog("PackNum: ", data[i].PackNum);
		for (int j = 0; j < data[i].SzSamples; j++)
			EConsole::PrintLog("Value: ", data[i].Samples[j]);
	}

	MathLibSample* mathLib = (MathLibSample*)userData;
	std::vector<RawChannelsArray> arr;
	for (int i = 0; i < size; i++)
	{
		RawChannelsArray elem;
		elem.channels = data[i].Samples;
		arr.push_back(elem);
	}
	mathLib->pushDataArr(arr);
	mathLib->processDataArr();

}