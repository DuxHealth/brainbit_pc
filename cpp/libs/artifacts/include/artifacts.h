#ifndef ARTIFACTS_COMMON_API_H
#define ARTIFACTS_COMMON_API_H

#include "artifacts_api_types.h"
#include "lib_export.h"
#include <vector>

#ifdef __cplusplus
extern "C"
{
#endif

	typedef struct _ArtifactsMath ArtifactsMath;

	SDK_SHARED ArtifactsMath* createArtifactsMath(int channel_number, ArtifactSetting artifact_setting, OutStatus*);

	SDK_SHARED OutStatus ArtifactsMathInit(ArtifactsMath* artMathPtr, int ampl_bord, int beta_pow_bord, int allowed_percent_artpoints, int num_wins_for_quality_avg);
	SDK_SHARED OutStatus ArtifactsMathIsArtifactedWin(bool*, int*, ArtifactsMath* artMathPtr, double* vals_arr, int arr_size, bool print_info);

	SDK_SHARED OutStatus ArtifactsMathSetHanningWinSpect(ArtifactsMath* artMathPtr);
	SDK_SHARED OutStatus ArtifactsMathSetHammingWinSpect(ArtifactsMath* artMathPtr);

	SDK_SHARED OutStatus ArtifactsMathReadArtifactInfo(ArtifactsMath* artMathPtr, ArtifactData* artifactsinfo);
	SDK_SHARED OutStatus ArtifactsMathReadArtifactInfoArr(ArtifactsMath* artMathPtr, ArtifactData* artifactsinfo_arr, uint32_t* arr_size);
	SDK_SHARED OutStatus ArtifactsMathReadArtifactInfoArrSize(uint32_t* result, ArtifactsMath* artMathPtr);

	SDK_SHARED OutStatus freeArtifactsMath(ArtifactsMath*);
	SDK_SHARED OutStatus ArtifactsClearData(ArtifactsMath* ArtifactsMathPtr);

#ifdef __cplusplus
}
#endif

#endif