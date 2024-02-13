#pragma once

#include "sdk_api.h"
#include "log.h"

#include "spectrumLib.h"

void SignalCallback_Spectrum(Sensor* sensor, BrainBitSignalData* data, int32_t size, void* userData)
{
	EConsole::PrintLog("=== BRAIN BIT SIGNAL DATA ===");

    auto* arr_spectrum = new double[size];

	for (int i = 0; i < size; i++)
	{
        arr_spectrum[i] = data[i].O1;

		EConsole::PrintLog("O1: ", arr_spectrum[i]);
	}

	auto* spectrumLib = (SpectrumLibSample*)userData;
    spectrumLib->processData(arr_spectrum, size);
    spectrumLib->printResult();
    spectrumLib->setNewSamplesSize();

	delete[] arr_spectrum;
}