cmake_minimum_required (VERSION 3.14)

set(PRJ_NAME callibri_sample)

project (${PRJ_NAME} LANGUAGES CXX)

if(WIN32)
	set(SYSTEM_NAME_PREF "windows")
elseif (UNIX)
	set(SYSTEM_NAME_PREF "linux")
endif ()

set(EMOTIONS_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/em_st_artifacts")
set(FLIB_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/filters")
set(NEUROLIB_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/neurosdk2")
set(CALLIBRI_ECG_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/callibri_ecg")
set(SPECTRUM_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/spectrum")

if(WIN32)
	if (${CMAKE_SIZEOF_VOID_P} MATCHES 8)
		set(FLIB_DLL_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/filters/x64")
		set(LIB_EMOTIONS "em_st_artifacts-x64")
		set(LIB_NEURO "neurosdk2-x64")
		set(LIB_CALLIBRI_ECG "callibri_utils-x64")
		set(LIB_SPECTRUM "spectrumlib-x64")
	else()
		set(FLIB_DLL_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/filters/x86")
		set(LIB_EMOTIONS "em_st_artifacts-x86")
		set(LIB_NEURO "neurosdk2-x32")
		set(LIB_CALLIBRI_ECG "callibri_utils-x86")
		set(LIB_SPECTRUM "spectrumlib-x86")
	endif()
set(LIB_FILTERS "filters")
elseif (UNIX)
	set(LIB_NEURO "libneurosdk2.so")
	set(LIB_FILTERS "libfilters.so")
	set(LIB_SPECTRUM "libspectrumlib.so")
	set(LIB_CALLIBRI_ECG "libcallibri_utils.so")
	set(LIB_EMOTIONS "libem_st_artifacts.so")
endif()

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

set(
	MAIN_SOURCE
	${CMAKE_CURRENT_SOURCE_DIR}/sample/main/main.cpp
)

add_executable (${PRJ_NAME} ${MAIN_SOURCE})

add_subdirectory(sample)

target_include_directories(${PRJ_NAME}
PRIVATE 
	"${EMOTIONS_PATH}/include"
	"${FLIB_PATH}/include"
	"${NEUROLIB_PATH}/include"
	"${CALLIBRI_ECG_PATH}/include"
	"${SPECTRUM_PATH}/include"
)

if (UNIX)
	# Optionally add dependencies
	file(GLOB DEPENDENCES
			"${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/dependences/*.*"
	)
	target_link_libraries(${PRJ_NAME} PRIVATE ${DEPENDENCES})
endif()

if(WIN32)
	target_link_directories(${PRJ_NAME} PRIVATE "${FLIB_DLL_PATH}")
	find_file(FLIB_DLL NAMES "${LIB_FILTERS}.dll" HINTS "${FLIB_DLL_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
	target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_FILTERS})
	add_custom_command(TARGET ${PRJ_NAME} POST_BUILD COMMAND ${CMAKE_COMMAND} -E copy ${FLIB_DLL} $<TARGET_FILE_DIR:${PRJ_NAME}>)
	
	target_link_directories(${PRJ_NAME} PRIVATE "${EMOTIONS_PATH}")
	find_file(EMOTIONS_DLL NAMES "${LIB_EMOTIONS}.dll" HINTS "${EMOTIONS_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
	target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_EMOTIONS})
	add_custom_command(TARGET ${PRJ_NAME} POST_BUILD COMMAND ${CMAKE_COMMAND} -E copy ${EMOTIONS_DLL} $<TARGET_FILE_DIR:${PRJ_NAME}>)
	
	target_link_directories(${PRJ_NAME} PRIVATE "${NEUROLIB_PATH}")
	find_file(NEURO_DLL NAMES "${LIB_NEURO}.dll" HINTS "${NEUROLIB_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
	target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_NEURO})
	add_custom_command(TARGET ${PRJ_NAME} POST_BUILD COMMAND ${CMAKE_COMMAND} -E copy ${NEURO_DLL} $<TARGET_FILE_DIR:${PRJ_NAME}>)
	
	target_link_directories(${PRJ_NAME} PRIVATE "${CALLIBRI_ECG_PATH}")
	find_file(CALLIBRI_ECG_DLL NAMES "${LIB_CALLIBRI_ECG}.dll" HINTS "${CALLIBRI_ECG_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
	target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_CALLIBRI_ECG})
	add_custom_command(TARGET ${PRJ_NAME} POST_BUILD COMMAND ${CMAKE_COMMAND} -E copy ${CALLIBRI_ECG_DLL} $<TARGET_FILE_DIR:${PRJ_NAME}>)
	
	target_link_directories(${PRJ_NAME} PRIVATE "${SPECTRUM_PATH}")
	find_file(SPECTRUM_DLL NAMES "${LIB_SPECTRUM}.dll" HINTS "${SPECTRUM_PATH}" REQUIRED NO_CMAKE_FIND_ROOT_PATH)
	target_link_libraries(${PRJ_NAME} PRIVATE ${LIB_SPECTRUM})
	add_custom_command(TARGET ${PRJ_NAME} POST_BUILD COMMAND ${CMAKE_COMMAND} -E copy ${SPECTRUM_DLL} $<TARGET_FILE_DIR:${PRJ_NAME}>)
elseif(UNIX)
	target_link_libraries(${PRJ_NAME} PRIVATE "${EMOTIONS_PATH}/${LIB_EMOTIONS}"
		"${CALLIBRI_ECG_PATH}/${LIB_CALLIBRI_ECG}"
		"${NEUROLIB_PATH}/${LIB_NEURO}"
		"${SPECTRUM_PATH}/${LIB_SPECTRUM}"
		"${FLIB_PATH}/${LIB_FILTERS}")
endif()