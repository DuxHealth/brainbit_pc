#include "spectrumLib.h"

SpectrumLibSample::SpectrumLibSample()
{
	try
	{
		int sampling_rate = 250;
		int process_win_rate = 5;
		int fft_window = 1000;

		_spectrumMath = createSpectrumMath(sampling_rate, fft_window, process_win_rate);

		if (_spectrumMath == nullptr)
			throw std::invalid_argument("SpectrumMathLib is null!");

		int bord_frequency = 50;
		SpectrumMathInitParams(_spectrumMath, bord_frequency, true);

		double delta_coef = 0.0;
		double theta_coef = 1.0;
		double alpha_coef = 1.0;
		double beta_coef = 1.0;
		double gamma_coef = 0.0;
		SpectrumMathSetWavesCoeffs(_spectrumMath, delta_coef, theta_coef, alpha_coef, beta_coef, gamma_coef);


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

SpectrumLibSample::~SpectrumLibSample()
{
	try
	{
		freeSpectrumMath(_spectrumMath);
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

void SpectrumLibSample::processData(double* arr, int size)
{
	try
	{
		SpectrumMathPushData(_spectrumMath, arr, size);
        SpectrumMathProcessData(_spectrumMath);
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

void SpectrumLibSample::setNewSamplesSize(){
    try
    {
        SpectrumMathSetNewSampleSize(_spectrumMath);
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

void SpectrumLibSample::printResult()
{
	try
	{
		if (_spectrumMath == nullptr)
			throw std::invalid_argument("SpectrumMathLib is null!");

		EConsole::PrintLog("=== RAW SPECTRUM DATA ===");

		double n_bins_for_1Hz = SpectrumMathGetFFTBinsFor1Hz(_spectrumMath);

		uint32_t arr_sz = SpectrumMathReadSpectrumArrSize(_spectrumMath);
		RawSpectrumData* raw_spect_data = new RawSpectrumData[arr_sz];
		SpectrumMathReadRawSpectrumInfoArr(_spectrumMath, raw_spect_data, &arr_sz);

		for (size_t i = 0; i < arr_sz; ++i)
			std::cout << n_bins_for_1Hz << " " << raw_spect_data[i].all_bins_nums << " " << raw_spect_data[i].total_raw_pow << std::endl;

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