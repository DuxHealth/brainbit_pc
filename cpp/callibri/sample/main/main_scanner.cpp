#include "main_scanner.h"

#include <chrono>
#include <thread>

// =============
// || SCANNER ||
// =============

Sensor* SampleScannerFunction()
{
	/*

		FOR SEARCHING DEVICES WE NEED:
		1. CREATE SCANNER
		2. START SCANNER
		3. STOP SCANNER AFTER A TIME

	*/

	//Create object of custom scanner for getting SensorScanner
	//If you want to know, how to work with commands of library
	//you can look in folder 'scanner'.
	SampleScanner* scanner = new SampleScanner();

	//Create list of devices, which we want to find. In our
	//example we choose 'SensorLECallibri'.
	SensorFamily devices[] = { SensorFamily::SensorLECallibri };

	//Use function 'CreateScanner' to create object of SensorScanner.
	scanner->CreateScanner(devices, 1);

	//Start searching devices
	scanner->StartScanner();

	//Wait 5 seconds to find devices
	std::this_thread::sleep_for(std::chrono::milliseconds(10000));

	//Stop searching devices
	scanner->StopScanner();

	//Get list of found devices
	std::vector<SensorInfo> listDevices = scanner->GetDevices();

	//Check found device scanner or not. If no devices, return null value
	if (listDevices.size() == 0)
		return nullptr;

	//Print information of all found devices
	for (int i = 0; i < listDevices.size(); i++)
	{
		EConsole::PrintLog("[SENSOR] [NAME] : ", listDevices[i].Name, " [SERIAL NUMBER] :", listDevices[i].SerialNumber, " [ADDRESS] :", listDevices[i].Address);
	}

	//Create SensorInfo of first found device to create object of Sensor
	SensorInfo device = listDevices[0];

	//Create object Sensor with function 'CreateSensor' and return pointer of this object
	Sensor* sensor = scanner->CreateSensor(device);

	//Before returning pointer we need delete all objects to clear memory.
	delete scanner;

	//Return pointer of object Sensor
	return sensor;
}