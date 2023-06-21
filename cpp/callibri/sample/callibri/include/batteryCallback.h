#pragma once

#include "sdk_api.h"
#include "log.h"

void BatteryCallback(Sensor* sensor, int32_t battery, void* userData)
{
	EConsole::PrintLog("[BATTERY] : ", battery);
}