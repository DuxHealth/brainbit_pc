#pragma once

#include "sdk_api.h"
#include "log.h"

void ResistCallback(Sensor* sensor, BrainBitResistData data, void* userData)
{
	EConsole::PrintLog("[RESIST DATA] [O1] :", data.O1 );
}