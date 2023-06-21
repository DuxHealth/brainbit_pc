set(
	HEADERS
	${CMAKE_CURRENT_SOURCE_DIR}/include/callibri.h

	${CMAKE_CURRENT_SOURCE_DIR}/include/batteryCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/connectionCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/electrodeStateCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/envelopeDataCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/memsDataCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/quaternionDataCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/respirationCallback.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/signalCallback.h

	${CMAKE_CURRENT_SOURCE_DIR}/include/signalCallback_em_st_artifacts.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/signalCallback_ecg.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/signalCallback_filters.h
	${CMAKE_CURRENT_SOURCE_DIR}/include/signalCallback_spectrum.h
)

set(
	SOURCES
	${CMAKE_CURRENT_SOURCE_DIR}/callibri.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/callibri_info.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/callibri_settings.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/callibri_supported_functional.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/callibri_callback.cpp

	${CMAKE_CURRENT_SOURCE_DIR}/callibri_em_st_artifacts.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/callibri_ecg.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/callibri_filters.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/callibri_spectrum.cpp
)

target_include_directories(${PRJ_NAME} PRIVATE include)

target_sources(${PRJ_NAME} PRIVATE ${SOURCES} ${HEADERS})