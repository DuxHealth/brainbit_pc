#include "main_callibri_spectrum.h"

void SampleCallibriSpectrum(Sensor* sensor_callibri)
{
	//Create custom object of Spectrum
	SpectrumLibSample* spectrumLib = new SpectrumLibSample();

	//If lib is null, stop code
	if (spectrumLib == nullptr)
		return;

	//Create custom object of Callibri.
	//If you want to know, how to work with device ('LECallibri')
	//you can look in folder 'callibri'.
	SampleCallibri* callibri = new SampleCallibri(sensor_callibri);

	//Check if object of Callibri is null or not
	if (callibri == nullptr)
		return;

	//Set signal setting - CallibriSignalTypeECG
	SignalTypeCallibri type = CallibriSignalTypeECG;
	callibri->setSignalSettings(type);

	//Add callback to get signal data and convert to spectrum data
	callibri->AddSignalCallbackCallibri_Spectrum(spectrumLib);

	//To execuate command we use function 'execCommand'.
	//Before use this command you need to be sure,
	//that this device supports this command

	//Create SensorCommand to execuate it
	SensorCommand command_start_signal = CommandStartSignal;

	//Check if support this device command 'CommandStartSignal'
	bool isSupport = callibri->isSupportedCommand(command_start_signal);

	//It is recommended to be sure of availabilities device. You can check and
	//execuate commands like it:
	//
	//if(callibri->isSupportedCommand(command_start_signal))
	//{
	//	callibri->execCommand(command_start_signal)
	//}

	//Check if our command is supported for device
	if (isSupport)
	{
		EConsole::PrintLog("[LOG] [This command is supported by device!");

		//If this command is supported by device, you can execuate it

		callibri->execCommand(command_start_signal);
	}
	else
	{
		EConsole::PrintLog("[WARNING] [This command is not supported by device!");
	}

	//Wait 1 second to get signal data
	Sleep(1000);

	//Create SensorCommand to execuate it
	SensorCommand command_stop_signal = CommandStopSignal;

	//Check if support this device command 'CommandStopSignal'
	isSupport = callibri->isSupportedCommand(command_stop_signal);

	//Check if our command is supported for device
	if (isSupport)
	{
		EConsole::PrintLog("[LOG] [This command is supported by device!");

		//If this command is supported by device, you can execuate it

		callibri->execCommand(command_stop_signal);
	}
	else
	{
		EConsole::PrintLog("[WARNING] [This command is not supported by device!");
	}

	//Remove signal callback
	callibri->RemoveSignalCallbackCallibri_Spectrum();

	//Wait 2 seconds
	Sleep(2000);

	//Print result
	spectrumLib->printResult();

	//Clear memory of object callibri
	delete callibri;

	//Clear memory of object class
	delete spectrumLib;
}