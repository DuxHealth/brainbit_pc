cmake_minimum_required (VERSION 3.14)

set(PRJ_NAME window_sample_callibri)

project (${PRJ_NAME} LANGUAGES CXX)

set(EMOTIONS_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/em_st_artifacts")
set(FLIB_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/filters")
set(NEUROLIB_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/neurosdk2")
set(CALLIBRI_ECG_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/callibri_ecg")
set(SPECTRUM_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/spectrum")

if (${CMAKE_SIZEOF_VOID_P} MATCHES 8)
	set(FLIB_DLL_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/filters/x64")
	set(LIB_EMOTIONS "em_st_artifacts-x64")
	set(LIB_NEURO "neurosdk2-x64")
	set(LIB_CALLIBRI_ECG "callibri_utils-x64")
	set(LIB_SPECTRUM "spectrumlib-x64")
else()
	set(FLIB_DLL_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/filters/x86")
	set(LIB_EMOTIONS "em_st_artifacts-x86")
	set(LIB_NEURO "neurosdk2-x32")
	set(LIB_CALLIBRI_ECG "callibri_utils-x86")
	set(LIB_SPECTRUM "spectrumlib-x86")
endif()
set(LIB_FILTERS "filters")

set(PRJ_VER_MAJOR 1)
set(PRJ_VER_MINOR 0)
set(PRJ_VER_PATCH 0)
set(PRJ_VER_REVISION 1)
set(PRJ_VER_CMN ${PRJ_VER_MAJOR}.${PRJ_VER_MINOR}.${PRJ_VER_PATCH})
set(PRJ_VER_FULL ${PRJ_VER_CMN}.${PRJ_VER_REVISION})

message(STATUS [Sample Version]:[${PRJ_VER_FULL}])
project(${PRJ_NAME} VERSION ${PRJ_VER_FULL} LANGUAGES CXX )
set (CMAKE_CONFIGURATION_TYPES Debug;Release)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED YES)
set(CMAKE_CXX_EXTENSIONS NO)
set(CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS ON)
set(CMAKE_CONFIGURATION_TYPES Debug;Release)

add_subdirectory(sample)

target_include_directories(${PRJ_NAME}
PRIVATE 
	"${EMOTIONS_PATH}/include"
	"${FLIB_PATH}/include"
	"${NEUROLIB_PATH}/include"
	"${CALLIBRI_ECG_PATH}/include"
	"${SPECTRUM_PATH}/include"
)

set(LIB_FILE ${CMAKE_CURRENT_SOURCE_DIR}/out/build/x64-Debug/sample/main)

target_link_directories(${PRJ_NAME} PRIVATE "${FLIB_DLL_PATH}")
find_file(FLIB_DLL NAMES "${LIB_FILTERS}.dll" HINTS "${FLIB_DLL_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_FILTERS})
install(FILES ${FLIB_DLL} DESTINATION ${LIB_FILE})

target_link_directories(${PRJ_NAME} PRIVATE "${EMOTIONS_PATH}")
find_file(EMOTIONS_DLL NAMES "${LIB_EMOTIONS}.dll" HINTS "${EMOTIONS_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_EMOTIONS})
install(FILES ${EMOTIONS_DLL} DESTINATION ${LIB_FILE})

target_link_directories(${PRJ_NAME} PRIVATE "${NEUROLIB_PATH}")
find_file(NEURO_DLL NAMES "${LIB_NEURO}.dll" HINTS "${NEUROLIB_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_NEURO})
install(FILES ${NEURO_DLL} DESTINATION ${LIB_FILE})

target_link_directories(${PRJ_NAME} PRIVATE "${CALLIBRI_ECG_PATH}")
find_file(CALLIBRI_ECG_DLL NAMES "${LIB_CALLIBRI_ECG}.dll" HINTS "${CALLIBRI_ECG_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_CALLIBRI_ECG})
install(FILES ${CALLIBRI_ECG_DLL} DESTINATION ${LIB_FILE})

target_link_directories(${PRJ_NAME} PRIVATE "${SPECTRUM_PATH}")
find_file(SPECTRUM_DLL NAMES "${LIB_SPECTRUM}.dll" HINTS "${SPECTRUM_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_SPECTRUM})
install(FILES ${SPECTRUM_DLL} DESTINATION ${LIB_FILE})