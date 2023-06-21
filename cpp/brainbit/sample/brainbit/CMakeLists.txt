cmake_minimum_required (VERSION 3.14)

set(
	HEADERS
	${CMAKE_CURRENT_SOURCE_DIR}/include/brainbit.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/batteryCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/connectionCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/resistCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/signalCallback.h

	${CMAKE_CURRENT_SOURCE_DIR}/include/signalCallback_em_st_artifacts.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/signalCallback_filters.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/signalCallback_spectrum.h
)

set(
	SOURCES
	${CMAKE_CURRENT_SOURCE_DIR}/brainbit.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/brainbit_callback.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/brainbit_info.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/brainbit_settings.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/brainbit_supported_functional.cpp

	${CMAKE_CURRENT_SOURCE_DIR}/brainbit_em_st_artifacts.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/brainbit_filters.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/brainbit_spectrum.cpp
)

target_include_directories(${PRJ_NAME} PRIVATE include)

target_sources(${PRJ_NAME} PRIVATE ${SOURCES} ${HEADERS})
