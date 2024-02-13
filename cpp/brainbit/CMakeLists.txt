cmake_minimum_required (VERSION 3.14)

set(PRJ_NAME brainbit_sample)

project (${PRJ_NAME} LANGUAGES CXX)

if(WIN32)
	set(SYSTEM_NAME_PREF "windows")
elseif (UNIX)
	set(SYSTEM_NAME_PREF "linux")
endif ()

set(EMOTIONS_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/em_st_artifacts")
set(FLIB_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/filters")
set(NEUROLIB_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/neurosdk2")
set(SPECTRUM_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/spectrum")

if(WIN32)
	if (${CMAKE_SIZEOF_VOID_P} MATCHES 8)
		set(FLIB_DLL_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/filters/x64.dll")
		set(LIB_EMOTIONS "em_st_artifacts-x64.dll")
		set(LIB_NEURO "neurosdk2-x64.dll")
		set(LIB_SPECTRUM "spectrumlib-x64.dll")
	else()
		set(FLIB_DLL_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/filters/x86.dll")
		set(LIB_EMOTIONS "em_st_artifacts-x86.dll")
		set(LIB_NEURO "neurosdk2-x32.dll")
		set(LIB_SPECTRUM "spectrumlib-x86.dll")
	endif()
	set(LIB_FILTERS "filters.dll")
elseif (UNIX)
	set(LIB_EMOTIONS "libem_st_artifacts.so")
	set(LIB_NEURO "libneurosdk2.so")
	set(LIB_FILTERS "libfilters.so")
	set(LIB_SPECTRUM "libspectrumlib.so")
endif ()


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
	"${SPECTRUM_PATH}/include"
)

if (UNIX)
	# Optionally add dependencies
	file(GLOB DEPENDENCES
			"${CMAKE_CURRENT_SOURCE_DIR}/../libs/${SYSTEM_NAME_PREF}/dependences/*.*"
	)
	target_link_libraries(${PRJ_NAME} PRIVATE ${DEPENDENCES})
endif()

target_link_libraries(${PRJ_NAME} PRIVATE "${EMOTIONS_PATH}/${LIB_EMOTIONS}"
		"${NEUROLIB_PATH}/${LIB_NEURO}"
		"${FLIB_PATH}/${LIB_FILTERS}"
		"${SPECTRUM_PATH}/${LIB_SPECTRUM}")
