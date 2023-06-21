#ifndef EM_ST_ARTIFACTS_COMMON_API_H
#define EM_ST_ARTIFACTS_COMMON_API_H

#include "lib_export.h"
#include <stdint.h>
#include <stddef.h>
#ifdef __cplusplus
extern "C"
{
#endif

	typedef struct _MathLibSetting
	{
		int sampling_rate;
		int process_win_freq;
		int fft_window;
		int n_first_sec_skipped;
		bool bipolar_mode;
		int channels_number;
		int channel_for_analysis;
	} MathLibSetting;


	typedef struct _ArtifactDetectSetting
	{
		int art_bord;
		int allowed_percent_artpoints;
		int raw_betap_limit;
		int global_artwin_sec;
		bool hanning_win_spectrum;
		bool hamming_win_spectrum;
		int num_wins_for_quality_avg;
	}ArtifactDetectSetting;

	typedef struct _ShortArtifactDetectSetting
	{
		int ampl_art_detect_win_size;
		int ampl_art_zerod_area;
		int ampl_art_extremum_border;
	}ShortArtifactDetectSetting;


	typedef struct _MentalAndSpectralSetting
	{
		int n_sec_for_instant_estimation;
		int n_sec_for_averaging;
	} MentalAndSpectralSetting;

	extern const ArtifactDetectSetting ArtifactDetectSettingDefault;
	extern const MathLibSetting MathLibSettingDefault;
	extern const ShortArtifactDetectSetting ShortArtifactDetectSettingDefault;
	extern const MentalAndSpectralSetting MentalAndSpectralSettingDefault;


	typedef struct _ResStatus
	{
		uint8_t Success;
		uint32_t Error;
		char ErrorMsg[512];
	} ResStatus;

	typedef enum _SideType
	{
		LEFT,
		RIGHT,
		NONE
	} SideType;

	typedef struct _MindData {
		double Rel_Attention;
		double Rel_Relaxation;
		double Inst_Attention;
		double Inst_Relaxation;
	} MindData;

	typedef struct _SpectralDataPercents {
		double Delta;
		double Theta;
		double Alpha;
		double Beta;
		double Gamma;
	} SpectralDataPercents;

	typedef struct _RawChannels {
		double left_bipolar;
		double right_bipolar;
	} RawChannels;

	typedef struct _RawChannelsArray
	{
		double* channels;
	} RawChannelsArray;

	typedef struct _RawSpectVals {
		double alpha;
		double beta;
	} RawSpectVals;

	typedef struct _MathLib MathLib;

	SDK_SHARED MathLib* createMathLib(MathLibSetting lib_setting, ArtifactDetectSetting art_setting, ShortArtifactDetectSetting short_art_setting, MentalAndSpectralSetting mental_spectral_setting, ResStatus* opResult);
	SDK_SHARED uint8_t freeMathLib(MathLib*, ResStatus* opResult);

	SDK_SHARED uint8_t MathLibSetMentalEstimationMode(MathLib* mathLibPtr, bool independent, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibSetHanningWinSpect(MathLib* mathLibPtr, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibSetHammingWinSpect(MathLib* mathLibPtr, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibSetCallibrationLength(MathLib* mathLibPtr, int s, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibSetSkipWinsAfterArtifact(MathLib* mathLibPtr, int nwins, ResStatus* opResult);

	SDK_SHARED uint8_t MathLibPushData(MathLib* mathLibPtr, RawChannels* samples, size_t samplesCount, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibPushDataArr(MathLib* mathLibPtr, RawChannelsArray* samples, size_t samplesCount, ResStatus* opResult);

	SDK_SHARED uint8_t MathLibProcessWindow(MathLib* mathLibPtr, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibProcessData(MathLib* mathLibPtr, SideType side, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibProcessDataArr(MathLib* mathLibPtr, ResStatus* opResult);

	SDK_SHARED uint8_t MathLibSetPrioritySide(MathLib* mathLibPtr, SideType side, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibStartCalibration(MathLib* mathLibPtr, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibCalibrationFinished(MathLib* mathLibPtr, bool* result, ResStatus* opResult);

	SDK_SHARED uint8_t MathLibIsArtifactedWin(MathLib* mathLibPtr, SideType side, bool print_info, bool* result, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibIsArtifactedSequence(MathLib* mathLibPtr, bool* result, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibIsBothSidesArtifacted(MathLib* mathLibPtr, bool* result, ResStatus* opResult);

	SDK_SHARED uint8_t MathLibReadMentalDataArrSize(MathLib* mathLibPtr, int* arr_size, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibReadMentalDataArr(MathLib* mathLibPtr, MindData* mindd, int* arr_size, ResStatus* opResult);

	SDK_SHARED uint8_t MathLibReadAverageMentalData(MathLib* mathLibPtr, int n_lastwins_toaverage, MindData* minddatavals, ResStatus* opResult);

	SDK_SHARED uint8_t MathLibReadSpectralDataPercentsArrSize(MathLib* mathLibPtr, int* arr_size, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibReadSpectralDataPercentsArr(MathLib* mathLibPtr, SpectralDataPercents* spectraldata_arr, int* arr_size, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibReadRawSpectralVals(MathLib* mathLibPtr, RawSpectVals* raw_spectral_vals, ResStatus* opResult);

	SDK_SHARED uint8_t MathLibSetZeroSpectWaves(MathLib* mathLibPtr, bool active, int delta, int theta, int alpha, int beta, int gamma, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibSetWeightsForSpectra(MathLib* mathLibPtr, double delta_c, double theta_c, double alpha_c, double beta_c, double gamma_c, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibSetSpectNormalizationByBandsWidth(MathLib* mathLibPtr, bool fl, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibSetSpectNormalizationByCoeffs(MathLib* mathLibPtr, bool fl, ResStatus* opResult);
	SDK_SHARED uint8_t MathLibGetCallibrationPercents(MathLib* mathLibPtr, int* outPercents, ResStatus* opResult);


#ifdef _DEBUG

	SDK_SHARED void MathLibCheckArtifactsOnLeftChannel(MathLib* mathLibPtr, int* type, double* location, bool* start, ResStatus* opResult);
	SDK_SHARED void MathLibCheckArtifactsOnRightChannel(MathLib* mathLibPtr, int* type, double* location, bool* start, ResStatus* opResult);

	SDK_SHARED double MathLibReadLeftChannel(MathLib* mathLibPtr, ResStatus* opResult);
	SDK_SHARED double MathLibReadRightChannel(MathLib* mathLibPtr, ResStatus* opResult);

#endif

#ifdef __cplusplus
}
#endif

#endif
