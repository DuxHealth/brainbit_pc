from neurosdk.scanner import Scanner
from neurosdk.cmn_types import *

import concurrent.futures
from time import sleep


def sensor_found(scanner, sensors):
    for index in range(len(sensors)):
        print('Sensor found: %s' % sensors[index])


def on_sensor_state_changed(sensor, state):
    print('Sensor {0} is {1}'.format(sensor.name, state))


def on_battery_changed(sensor, battery):
    print('Battery: {0}'.format(battery))


def on_signal_received(sensor, data):
    print(data)


def on_electrodes_state_changed(sensor, data):
    print(data)


def on_envelope_received(sensor, data):
    print(data)


def on_respiration_received(sensor, data):
    print(data)


def on_mems_received(sensor, data):
    print(data)


def on_quaternion_received(sensor, data):
    print(data)


try:
    scanner = Scanner([SensorFamily.SensorLECallibri, SensorFamily.SensorLEKolibri])

    scanner.sensorsChanged = sensor_found
    scanner.start()
    print("Starting search for 5 sec...")
    sleep(5)
    scanner.stop()

    sensorsInfo = scanner.sensors()
    for i in range(len(sensorsInfo)):
        current_sensor_info = sensorsInfo[i]
        print(sensorsInfo[i])


        def device_connection(sensor_info):
            return scanner.create_sensor(sensor_info)


        with concurrent.futures.ThreadPoolExecutor() as executor:
            future = executor.submit(device_connection, current_sensor_info)
            sensor = future.result()
            print("Device connected")

        sensor.sensorStateChanged = on_sensor_state_changed
        sensor.batteryChanged = on_battery_changed

        sensFamily = sensor.sens_family

        print(sensFamily)
        print(sensor.features)
        print(sensor.commands)
        print(sensor.parameters)
        print(sensor.name)
        print(sensor.state)
        print(sensor.address)
        print(sensor.serial_number)
        print(sensor.batt_power)
        if sensor.is_supported_parameter(SensorParameter.ParameterSamplingFrequency):
            print(sensor.sampling_frequency)
        if sensor.is_supported_parameter(SensorParameter.ParameterGain):
            print(sensor.gain)
        if sensor.is_supported_parameter(SensorParameter.ParameterOffset):
            print(sensor.data_offset)
        print(sensor.version)

        sensor.electrodeStateChanged = on_electrodes_state_changed

        if sensor.is_supported_feature(SensorFeature.FeatureSignal):
            sensor.signalDataReceived = on_signal_received

        if sensor.is_supported_feature(SensorFeature.FeatureRespiration):
            sensor.respirationDataReceived = on_respiration_received

        if sensor.is_supported_feature(SensorFeature.FeatureEnvelope):
            sensor.envelopeDataReceived = on_envelope_received

        if sensor.is_supported_feature(SensorFeature.FeatureMEMS):
            sensor.memsDataReceived = on_mems_received
            sensor.quaternionDataReceived = on_quaternion_received

        if sensor.is_supported_command(SensorCommand.CommandStartSignal):
            sensor.exec_command(SensorCommand.CommandStartSignal)
            print("Start signal")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopSignal)
            print("Stop signal")

        if sensor.is_supported_command(SensorCommand.CommandStartEnvelope):
            sensor.exec_command(SensorCommand.CommandStartEnvelope)
            print("Start envelope")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopEnvelope)
            print("Stop envelope")

        if sensor.is_supported_command(SensorCommand.CommandStartRespiration):
            sensor.exec_command(SensorCommand.CommandStartRespiration)
            print("Start respiration")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopRespiration)
            print("Stop respiration")

        if sensor.is_supported_command(SensorCommand.CommandStartCurrentStimulation):
            sensor.exec_command(SensorCommand.CommandStartCurrentStimulation)
            print("Start CurrentStimulation")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopCurrentStimulation)
            print("Stop CurrentStimulation")

        if sensor.is_supported_command(SensorCommand.CommandStartMEMS):
            sensor.exec_command(SensorCommand.CommandStartMEMS)
            print("Start MEMS")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopMEMS)
            print("Stop MEMS")

        if sensor.is_supported_command(SensorCommand.CommandStartAngle):
            sensor.exec_command(SensorCommand.CommandStartAngle)
            print("Start Quaternion")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopAngle)
            print("Stop Quaternion")

        sensor.disconnect()
        print("Disconnect from sensor")
        del sensor

    del scanner
    print('Remove scanner')
except Exception as err:
    print(err)
