#include "filtersLib.h"

FilterParam findFilterParamWithList(FilterParam* list, int count, FilterType type, int samplingRate)
{
	for (int i = 0; i < count; i++)
		if (list[i].type == type && list[i].samplingFreq == samplingRate)
			return list[i];
	return {};
}

FiltersLibSample::FiltersLibSample()
{
	try
	{
		TOpStatus error;
		//Create list of filters
		_flist = create_TFilterList(&error);

		//Get count of list filters
		int fcount = -1;
		get_preinstalled_filter_count(&fcount, &error);

		//Get list available filters
		FilterParam* params = new FilterParam[fcount];
		get_preinstalled_filter_list(params, &error);

		//Print available filters
		EConsole::PrintLog("=== FILTER PARAMS LIST ===");
		for (int i = 0; i < fcount; i++)
		{
			int num = params[i].type;
			EConsole::PrintLog("FilterType: ", num);
			EConsole::PrintLog("SamplingFreq: ", params[i].samplingFreq);
			EConsole::PrintLog("CutOffFreq: ", params[i].cutoffFreq);
		}
		EConsole::PrintLog("=== === ===");

		if (_flist == nullptr)
			throw std::invalid_argument("TFilterList is null!");

		//Create parameters for filters
		FilterParam param1 = findFilterParamWithList(params, fcount, FtHP, 1000);
		FilterParam param2 = findFilterParamWithList(params, fcount, FtLP, 100);
		FilterParam param3 = findFilterParamWithList(params, fcount, FtBandStop, 1000);

		//Create object of filters
		_filter1 = create_TFilter_by_param(param1, &error);
		_filter2 = create_TFilter_by_param(param2, &error);
		_filter3 = create_TFilter_by_param(param3, &error);

		if (_filter1 == nullptr)
			throw std::invalid_argument("TFilter1 is null!");

		if (_filter2 == nullptr)
			throw std::invalid_argument("TFilter2 is null!");

		if (_filter3 == nullptr)
			throw std::invalid_argument("TFilter3 is null!");

		//Add filters to list
		TFilterList_AddFilter(_flist, _filter1, &error);
		TFilterList_AddFilter(_flist, _filter2, &error);
		TFilterList_AddFilter(_flist, _filter3, &error);
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

FiltersLibSample::~FiltersLibSample()
{
	try
	{
		TOpStatus error;
		delete_TFilterList(_flist, &error);
		delete_TFilter(_filter1, &error);
		delete_TFilter(_filter2, &error);
		delete_TFilter(_filter3, &error);
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

double FiltersLibSample::processElement(double elem)
{
	try
	{
		TOpStatus error;

		if (_flist == nullptr)
			throw std::invalid_argument("TFilterList is null!");

		return TFilterList_Filter(_flist, elem, &error);
	}
	catch (std::exception error)
	{
		//Print Error Message on Console.
		EConsole::PrintScreen("[[ERROR : ", error.what(), " ]]");
		return NULL;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		return NULL;
	}
}

void FiltersLibSample::printResult(double* arr, int size)
{
	try
	{
		EConsole::PrintLog("=== FILTERS DATA ===");

		for (int j = 0; j < size; j++)
			EConsole::PrintLog(arr[j]);

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