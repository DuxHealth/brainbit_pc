cmake_minimum_required (VERSION 3.14)

add_subdirectory(main)
add_subdirectory(scanner)
add_subdirectory(callibri)
add_subdirectory(em_st_artifacts)
add_subdirectory(callibri_ecg)
add_subdirectory(filters)
add_subdirectory(spectrum)
add_subdirectory(log)

set(
	SDK_FOLDER
	${CMAKE_CURRENT_SOURCE_DIR}/log/include

	${CMAKE_CURRENT_SOURCE_DIR}/callibri/include
	${CMAKE_CURRENT_SOURCE_DIR}/callibri_ecg/include
	${CMAKE_CURRENT_SOURCE_DIR}/em_st_artifacts/include
	${CMAKE_CURRENT_SOURCE_DIR}/filters/include
	${CMAKE_CURRENT_SOURCE_DIR}/spectrum/include

	${CMAKE_CURRENT_SOURCE_DIR}/scanner/include
)

target_include_directories(${PRJ_NAME} PUBLIC ${SDK_FOLDER})