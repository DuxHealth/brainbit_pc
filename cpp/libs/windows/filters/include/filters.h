#ifndef FILTERS_EXPORT_H
#define FILTERS_EXPORT_H

#ifdef __cplusplus
extern "C"
{
#endif
#include "lib_export.h"

#include "f_types.h"

	typedef struct _TFilterList TFilterList;
	typedef struct _TFilter TFilter;


	SDK_SHARED TFilterList* create_TFilterList();
	SDK_SHARED void TFilterList_AddFilter(TFilterList*, TFilter*);
	SDK_SHARED double TFilterList_Filter(TFilterList*, double);
	SDK_SHARED void TFilterList_Filter_array(TFilterList*, double*, int);
	SDK_SHARED void TFilter_List_ClearFilters(TFilterList*);
	SDK_SHARED void TFilterList_Reset(TFilterList*);
	SDK_SHARED void TFilterList_Delete_TFilter(TFilterList*, TFilterID);
	SDK_SHARED void delete_TFilterList(TFilterList*);

	SDK_SHARED TFilter* create_TFilter_by_param(FilterParam);
	SDK_SHARED TFilter* create_custom_TFilter(char*); 

	SDK_SHARED void TFilter_Reset(TFilter*);
	SDK_SHARED void TFilter_ClearParams(TFilter*);
	SDK_SHARED void TFilter_SetParams(TFilter*, char*);
	SDK_SHARED double TFilter_Filter(TFilter*, double);
	SDK_SHARED void TFilter_Filter_array(TFilter*, double*, int);
	SDK_SHARED TFilterID getID_TFilter(TFilter*);
	SDK_SHARED void delete_TFilter(TFilter*);

	SDK_SHARED void get_preinstalled_filter_count(int*);
	SDK_SHARED void get_preinstalled_filter_list(FilterParam*);

#ifdef __cplusplus
}
#endif

#endif
