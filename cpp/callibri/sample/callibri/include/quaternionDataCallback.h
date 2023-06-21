#pragma once

#include "sdk_api.h"
#include "log.h"

void QuaternionDataCallback(Sensor* sensor, QuaternionData* Data, int32_t size, void* userData)
{
	for (int i = 0; i < size; i++)
	{
		EConsole::PrintLog("[QUATERNION DATA] [PACK NUM] :" , Data[i].PackNum);
	}
}
