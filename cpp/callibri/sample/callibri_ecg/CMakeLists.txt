set(
	HEADERS
	${CMAKE_CURRENT_SOURCE_DIR}/include/callibriMathLib.h
)

set(
	SOURCES
	${CMAKE_CURRENT_SOURCE_DIR}/callibriMathLib.cpp
)

target_include_directories(${PRJ_NAME} PRIVATE include)

target_sources(${PRJ_NAME} PRIVATE ${SOURCES} ${HEADERS})