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


def on_resist_received(sensor, data):
    print(data)


def on_mems_received(sensor, data):
    print(data)


def on_fpg_received(sensor, data):
    print(data)


def on_amp_received(sensor, data):
    print(data)


try:
    scanner = Scanner([SensorFamily.SensorLEBrainBit, SensorFamily.SensorLEBrainBitBlack])

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

        sensor.sensorAmpModeChanged = on_amp_received

        if sensor.is_supported_feature(SensorFeature.FeatureSignal):
            sensor.signalDataReceived = on_signal_received

        if sensor.is_supported_feature(SensorFeature.FeatureResist):
            sensor.resistDataReceived = on_resist_received

        if sensor.is_supported_feature(SensorFeature.FeatureMEMS):
            sensor.memsDataReceived = on_mems_received

        if sensor.is_supported_feature(SensorFeature.FeatureFPG):
            sensor.fpgDataReceived = on_fpg_received

        if sensor.is_supported_command(SensorCommand.CommandStartSignal):
            sensor.exec_command(SensorCommand.CommandStartSignal)
            print("Start signal")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopSignal)
            print("Stop signal")

        if sensor.is_supported_command(SensorCommand.CommandStartFPG):
            sensor.exec_command(SensorCommand.CommandStartFPG)
            print("Start FPG")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopFPG)
            print("Stop FPG")

        if sensor.is_supported_command(SensorCommand.CommandStartResist):
            sensor.exec_command(SensorCommand.CommandStartResist)
            print("Start resist")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopResist)
            print("Stop resist")

        if sensor.is_supported_command(SensorCommand.CommandStartMEMS):
            sensor.exec_command(SensorCommand.CommandStartMEMS)
            print("Start MEMS")
            sleep(5)
            sensor.exec_command(SensorCommand.CommandStopMEMS)
            print("Stop MEMS")

        sensor.disconnect()
        print("Disconnect from sensor")
        del sensor

    del scanner
    print('Remove scanner')
except Exception as err:
    print(err)
