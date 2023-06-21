#pragma once

#include "sdk_api.h"
#include "log.h"

#include "spectrumLib.h"

void SignalCallback_Spectrum(Sensor* sensor, CallibriSignalData* data, int32_t size, void* userData)
{
	EConsole::PrintLog("=== CALLIBRI SIGNAL DATA ===");
	for (int i = 0; i < size; i++)
	{
		EConsole::PrintLog("PackNum: ", data[i].PackNum);
		for (int j = 0; j < data[i].SzSamples; j++)
			EConsole::PrintLog("Value: ", data[i].Samples[j]);
	}

	SpectrumLibSample* spectrumLib = (SpectrumLibSample*)userData;

	for (int i = 0; i < size; i++)
		spectrumLib->pushData(data[i].Samples, data[i].SzSamples);
	spectrumLib->processData();
}