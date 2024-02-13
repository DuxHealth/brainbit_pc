#pragma once

#include "sdk_api.h"
#include "log.h"

#include "filtersLib.h"

void SignalCallback_Filters(Sensor* sensor, BrainBitSignalData* data, int32_t size, void* userData)
{
	EConsole::PrintLog("=== BRAIN BIT SIGNAL DATA ===");
    FiltersLibSample* filtersLib = (FiltersLibSample*)userData;
	for (int i = 0; i < size; i++)
	{
        EConsole::PrintLog("O1 before filtering: ", data[i].O1);
        double filtered = filtersLib->processElement(data[i].O1);
		EConsole::PrintLog("O1 after filtering: ", filtered);
	}

}