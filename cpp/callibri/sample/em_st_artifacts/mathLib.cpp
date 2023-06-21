#include "mathLib.h"

MathLibSample::MathLibSample()
{
	try
	{
		//Create setting for lib
		MathLibSetting mathLibSetting;
		mathLibSetting.sampling_rate = 250;
		mathLibSetting.process_win_freq = 25;
		mathLibSetting.n_first_sec_skipped = 4;
		mathLibSetting.fft_window = 1000;
		mathLibSetting.bipolar_mode = false;
		mathLibSetting.channels_number = 2;
		mathLibSetting.channel_for_analysis = 1;

		ArtifactDetectSetting artifactDetectSetting;
		artifactDetectSetting.art_bord = 70;
		artifactDetectSetting.allowed_percent_artpoints = 50;
		artifactDetectSetting.raw_betap_limit = 800000;
		artifactDetectSetting.global_artwin_sec = 4;
		artifactDetectSetting.num_wins_for_quality_avg = 125;
		artifactDetectSetting.hanning_win_spectrum = true;

		ShortArtifactDetectSetting shortArtifactDetectSetting;

		shortArtifactDetectSetting.ampl_art_detect_win_size = 200;
		shortArtifactDetectSetting.ampl_art_zerod_area = 200;
		shortArtifactDetectSetting.ampl_art_extremum_border = 25;

		MentalAndSpectralSetting mentalAndSpectralSetting;
		mentalAndSpectralSetting.n_sec_for_averaging = 2;
		mentalAndSpectralSetting.n_sec_for_instant_estimation = 2;

		//Create object of lib
		ResStatus opSt;
		_mathLib = createMathLib(mathLibSetting, artifactDetectSetting, shortArtifactDetectSetting, mentalAndSpectralSetting, &opSt);

		//Check if operation is success or not
		if (!opSt.Success)
			throw std::invalid_argument(opSt.ErrorMsg);
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
	}
}

MathLibSample::~MathLibSample()
{
	try
	{
		ResStatus opSt;
		freeMathLib(_mathLib, &opSt);
		if (!opSt.Success)
			throw std::invalid_argument(opSt.ErrorMsg);

		_mathLib = nullptr;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
	}
}

bool MathLibSample::pushDataArr(std::vector<RawChannelsArray> data)
{
	try
	{
		if (_mathLib == nullptr)
			throw std::invalid_argument("MathLib is null!");

		ResStatus opSt;
		MathLibPushDataArr(_mathLib, data.data(), data.size(), &opSt);
		if (!opSt.Success)
			throw std::invalid_argument(opSt.ErrorMsg);
		return true;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		return false;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		return false;
	}
}

bool MathLibSample::processDataArr()
{
	try
	{
		if (_mathLib == nullptr)
			throw std::invalid_argument("MathLib is null!");

		ResStatus opSt;
		MathLibProcessDataArr(_mathLib, &opSt);
		if (!opSt.Success)
			throw std::invalid_argument(opSt.ErrorMsg);
		return true;
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		return false;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		return false;
	}
}

void MathLibSample::printResult()
{
	try
	{
		if (_mathLib == nullptr)
			throw std::invalid_argument("MathLib is null!");

		EConsole::PrintLog("=== SPECTRAL PERCENTS DATA ===");

		int size = 1;
		SpectralDataPercents* spectralData = new SpectralDataPercents[size];
		ResStatus opSt;
		MathLibReadSpectralDataPercentsArr(_mathLib, spectralData, &size, &opSt);
		if (!opSt.Success)
			throw std::invalid_argument(opSt.ErrorMsg);

		for (int i = 0; i < size; i++)
		{
			EConsole::PrintLog("Delta: ", spectralData[i].Delta * 100.0, "%");
			EConsole::PrintLog("Theta: ", spectralData[i].Theta * 100.0, "%");
			EConsole::PrintLog("Alpha: ", spectralData[i].Alpha * 100.0, "%");
			EConsole::PrintLog("Beta: ", spectralData[i].Beta * 100.0, "%");
			EConsole::PrintLog("Gamma: ", spectralData[i].Gamma * 100.0, "%");
		}

		delete[] spectralData;

		EConsole::PrintLog("=== === ===");
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
	}
}