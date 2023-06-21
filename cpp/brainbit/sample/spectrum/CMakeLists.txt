set(
	HEADERS
	${CMAKE_CURRENT_SOURCE_DIR}/include/spectrumLib.h
)

set(
	SOURCES
	${CMAKE_CURRENT_SOURCE_DIR}/spectrumLib.cpp
)

target_include_directories(${PRJ_NAME} PRIVATE include)

target_sources(${PRJ_NAME} PRIVATE ${SOURCES} ${HEADERS})