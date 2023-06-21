cmake_minimum_required (VERSION 3.14)

set(
	MAIN_SOURCE
	${CMAKE_CURRENT_SOURCE_DIR}/main.cpp
)

add_executable (${PRJ_NAME} ${MAIN_SOURCE})

set(
	HEADERS
	${CMAKE_CURRENT_SOURCE_DIR}/include/main_scanner.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/main_brainbit.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/main_brainbit_em_st_artifacts.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/main_brainbit_filters.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/main_brainbit_spectrum.h
)

set(
	SOURCES
	${CMAKE_CURRENT_SOURCE_DIR}/main_scanner.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/main_brainbit.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/main_brainbit_em_st_artifacts.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/main_brainbit_filters.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/main_brainbit_spectrum.cpp
)

target_include_directories(${PRJ_NAME} PRIVATE include)

target_sources(${PRJ_NAME} PRIVATE ${SOURCES} ${HEADERS})