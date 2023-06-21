set(
	HEADERS
	${CMAKE_CURRENT_SOURCE_DIR}/include/mathLib.h
)

set(
	SOURCES
	${CMAKE_CURRENT_SOURCE_DIR}/mathLib.cpp
)

target_include_directories(${PRJ_NAME} PRIVATE include)

target_sources(${PRJ_NAME} PRIVATE ${SOURCES} ${HEADERS})