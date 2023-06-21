#pragma once

#include "sdk_api.h"
#include "log.h"

void ElectrodeStateCallback(Sensor* Sensor, CallibriElectrodeState state, void* userData)
{
	EConsole::PrintLog("[ELECTRODE STATE] : ", state);
}