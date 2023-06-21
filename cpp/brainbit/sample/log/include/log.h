#pragma once

/*
	This library was created by Boldyrev Sanal. You can
	use this part of code in non-commercial purposes.
	Please, communicate to author for consulting in the
	code and its further use for commercial purposes.
*/

#include <iostream>
#include <string>
#include <ctime>
#include <Windows.h>

namespace EConsole
{
	//Print Console Text
	template<typename ... _TypeList>
	bool PrintScreen(_TypeList ... List)
	{
		try
		{
			const int _TestLixt[] = { (std::cout << List << ' ', 0)... };
			static_cast<void>(_TestLixt);
			std::cout << std::endl;
			return true;
		}
		catch (...)
		{
			return false;
		}
	}
	//Print Console Text to Line
	template<typename ... _TypeList>
	bool PrintScreenLine(_TypeList ... List)
	{
		try
		{
			const int _TestLixt[] = { (std::cout << List << std::endl, 0)... };
			static_cast<void>(_TestLixt);
			return true;
		}
		catch (...)
		{
			return false;
		}

	}

	inline std::string _convertIntTimeToString(int num)
	{
		if (num < 100)
		{
			if (num < 10)
			{
				return "0" + std::to_string(num);
			}
			return std::to_string(num);
		}
		return "";
	}

	//Print Now Time
	inline bool PrintNowTime()
	{
		try
		{
			__time64_t long_time;
			struct tm t_now;
			errno_t err = localtime_s(&t_now, &long_time);

			if (err)
				throw std::invalid_argument("Invalid argument to _localtime64_s!");

			std::cout << "[" << t_now.tm_year + 1900 << "-"
				<< _convertIntTimeToString(t_now.tm_mon) << "-"
				<< _convertIntTimeToString(t_now.tm_mday) << " "
				<< _convertIntTimeToString(t_now.tm_hour) << ":"
				<< _convertIntTimeToString(t_now.tm_min) << ":"
				<< _convertIntTimeToString(t_now.tm_sec) << "] ";

			return true;
		}
		catch (...)
		{
			return false;
		}

	}

	//Print Error Text to Line
	template<typename ... _TypeList>
	bool PrintError(_TypeList ... List)
	{
		try
		{
			PrintNowTime();
			std::cout << "[DEBUG LOG] [\x1B[31mERROR\033[0m] ";
			const int _TestLixt[] = { (std::cout << List << std::endl, 0)... };
			static_cast<void>(_TestLixt);
			return true;
		}
		catch (...)
		{
			return false;
		}

	}

	//Print Log
	template<typename ... _TypeList>
	bool PrintLog(_TypeList ... List)
	{
		try
		{
			PrintNowTime();
			std::cout << "[DEBUG LOG] [\x1B[94mDEBUG\033[0m] ";
			const int _TestLixt[] = { (std::cout << List, 0)... };
			static_cast<void>(_TestLixt);
			std::cout << std::endl;
			return true;
		}
		catch (...)
		{
			return false;
		}

	}
}