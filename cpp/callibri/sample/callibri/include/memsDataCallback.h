#pragma once

#include "sdk_api.h"
#include "log.h"

void MEMSDataCallback(Sensor* sensor, MEMSData* Data, int32_t size, void* userData)
{
	for (int i = 0; i < size; i++)
	{
		EConsole::PrintLog("[MEMS DATA] [PACK NUM] :", Data[i].PackNum);
	}
}
