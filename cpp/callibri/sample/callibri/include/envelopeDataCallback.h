#pragma once

#include "sdk_api.h"
#include "log.h"

void EnvelopeDataCallback(Sensor* Sensor, CallibriEnvelopeData* Data, int32_t size, void* userData)
{
	for (int i = 0; i < size; i++)
	{
		EConsole::PrintLog("[ENVELOPE DATA] [PACK NUM] : ", Data[i].PackNum);
	}
}
