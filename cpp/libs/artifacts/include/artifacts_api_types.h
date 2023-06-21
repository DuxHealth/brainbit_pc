#ifndef ARTIFACTS_COMMON_API_TYPES_H
#define ARTIFACTS_COMMON_API_TYPES_H

#include <iostream>

#define SIZE_ERROR_MESSAGE (512)

#ifdef __cplusplus
extern "C"
{
#endif

	typedef struct _ArtifactData {
		int artifact_location = -1;
		bool if_location_of_start = true;
		int type_of_artifact = 0;
	} ArtifactData;

	typedef struct _ArtifactParams {
		int amplitude_border = 150;
		int beta_pow_border = 110000;
		int art_allowed_percent = 10;
        int num_wins_for_quality_avg = 100;
	} ArtifactParams;

	typedef struct _ArtifactSetting
	{
		int sampling_rate;
		int process_win_freq = 25;
		int fft_window;
	} ArtifactSetting;

	typedef struct _OpStatus
	{
		uint8_t Success;
		uint32_t Error;
		char ErrorMsg[SIZE_ERROR_MESSAGE];
	} OutStatus;

#ifdef __cplusplus
}
#endif

#endif

