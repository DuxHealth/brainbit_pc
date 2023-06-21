#pragma once

#include <vector>

#include "sdk_api.h"
#include "log.h"

#include "mathLib.h"

void SignalCallback_EmStArtifacts(Sensor* sensor, BrainBitSignalData* data, int32_t size, void* userData)
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

	MathLibSample* mathLib = (MathLibSample*)userData;

	mathLib->pushData(arr, size);
	mathLib->processData();

	delete[] arr;
}