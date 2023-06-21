#pragma once

#include "sdk_api.h"
#include "log.h"

void SignalCallback(Sensor* sensor, BrainBitSignalData* data, int32_t size, void* userData)
{
	for (int i = 0; i < size; i++)
	{
		EConsole::PrintLog("[SIGNAL DATA] [PACK NUM] :", data[i].PackNum);
	}
}