#pragma once

#include "sdk_api.h"
#include "log.h"

void ConnectionCallback(Sensor* sensor, SensorState State, void* userData)
{
	EConsole::PrintLog("[CONNECTION] : ", State);
}
