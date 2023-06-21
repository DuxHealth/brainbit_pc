cmake_minimum_required (VERSION 3.14)


set(
	HEADERS
	${CMAKE_CURRENT_SOURCE_DIR}/include/scanner.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/sensorsCallback.h
)

set(
	SOURCES
	${CMAKE_CURRENT_SOURCE_DIR}/scanner.cpp
)

target_include_directories(${PRJ_NAME} PRIVATE include)

target_sources(${PRJ_NAME} PRIVATE ${SOURCES} ${HEADERS})
