cmake_minimum_required (VERSION 3.14)

set(
	HEADERS
	${CMAKE_CURRENT_SOURCE_DIR}/include/log.h
)

target_include_directories(${PRJ_NAME} PRIVATE include)

target_sources(${PRJ_NAME} PRIVATE ${HEADERS})


