#pragma once

#include "sdk_api.h"
#include "log.h"

#include "spectrumLib.h"

void SignalCallback_Spectrum(Sensor* sensor, BrainBitSignalData* data, int32_t size, void* userData)
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

	SpectrumLibSample* spectrumLib = (SpectrumLibSample*)userData;

	double* arr_spectrum = new double[size];
	for (int i = 0; i < size; i++)
		arr_spectrum[i] = arr[i].left_bipolar;
	spectrumLib->pushData(arr_spectrum, size);
	spectrumLib->processData();

	delete[] arr_spectrum;
	delete[] arr;
}