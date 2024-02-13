#include "filtersLib.h"

FilterParam findFilterParamWithList(FilterParam* list, int count, FilterType type, int samplingRate, double cutoffFrequency)
{
	for (int i = 0; i < count; i++)
		if (list[i].type == type && list[i].samplingFreq == samplingRate && list[i].cutoffFreq == cutoffFrequency)
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
		auto* params = new FilterParam[fcount];
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
		FilterParam param1 = findFilterParamWithList(params, fcount, FtHP, 250, 4);
		FilterParam param2 = findFilterParamWithList(params, fcount, FtLP, 250, 30);
		FilterParam param3 = findFilterParamWithList(params, fcount, FtBandStop, 250, 50);
		FilterParam param4 = findFilterParamWithList(params, fcount, FtBandStop, 250, 60);

		//Create object of filters
		_filter1 = create_TFilter_by_param(param1, &error);
		_filter2 = create_TFilter_by_param(param2, &error);
		_filter3 = create_TFilter_by_param(param3, &error);
		_filter4 = create_TFilter_by_param(param4, &error);

		if (_filter1 == nullptr)
			throw std::invalid_argument("TFilter1 is null!");

		if (_filter2 == nullptr)
			throw std::invalid_argument("TFilter2 is null!");

		if (_filter3 == nullptr)
			throw std::invalid_argument("TFilter3 is null!");

		if (_filter4 == nullptr)
			throw std::invalid_argument("TFilter4 is null!");

		//Add filters to list
		TFilterList_AddFilter(_flist, _filter1, &error);
		TFilterList_AddFilter(_flist, _filter2, &error);
		TFilterList_AddFilter(_flist, _filter3, &error);
		TFilterList_AddFilter(_flist, _filter4, &error);
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
		delete_TFilter(_filter4, &error);
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
		return 0;
	}
	catch (...)
	{
		//Print Error Message on Console (unknown error).
		EConsole::PrintScreen("[[ERROR : Invalid error! ]]");
		return 0;
	}
}
