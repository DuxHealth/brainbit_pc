#pragma once

#include "sdk_api.h"
#include "log.h"

void RespirationCallback(Sensor* Sensor, CallibriRespirationData* Data, int32_t size, void* userData)
{
	for (int i = 0; i < size; i++)
	{
		EConsole::PrintLog("[RESPIRATION DATA] [PACK NUM] : ", Data[i].PackNum);
	}
}
