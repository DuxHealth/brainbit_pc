import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Button, Text, View } from 'react-native';
import CallibriControllerInstance from '../NeuroImpl/CallibriController';
import { CallibriElectrodeState, CallibriSignalType, SensorSamplingFrequency } from 'react-native-neurosdk2';
import { EcgMath } from 'react-native-ecg-utils';

var ecgMathDataPackSize = -1;
var ecgMathBuffer = new Array<number>();
var ecgMath: EcgMath = null

export default function ECGScreen({ navigation }: NativeStackScreenProps<any>) {

  const [isSignal, setIsSignal] = useState(false)
  const [elState, setElState] = useState(CallibriElectrodeState.Detached)

  const [RR, setRR] = useState(0)
  const [HR, setHR] = useState(0)
  const [pressureIndex, setPressureIndex] = useState(0)
  const [moda, setModa] = useState(0)
  const [amplModa, setAmplModa] = useState(0)
  const [variationDist, setVariationDist] = useState(0)
  const [isRRdetected, setIsRRdetected] = useState(false)
  const [isInitialSignalCorrupted, setIsInitialSignalCorrupted] = useState(false)

  React.useEffect(() => {
    const unsubscribe = navigation.addListener('beforeRemove', () => {
      stopSignal()
    });

    return unsubscribe;
  }, [navigation]);

  React.useEffect(() => {
    setInterval(() => {
      updateECGMath()
    }, 100);
  }, []);

  function startSignal() {
    setIsSignal(!isSignal)
    CallibriControllerInstance.configureForSignalType(CallibriSignalType.ECG)
    CallibriControllerInstance.electrodeChangedCallback = (state) => {
      setElState(state)
    }
    CallibriControllerInstance.signalReceivedCallback = (data) => {
      data.forEach((sample) => {
        sample.Samples.forEach((value)=>{
          ecgMathBuffer.push(value * 1e6)
        })
      })
    }
    initECGMath(CallibriControllerInstance.samplingFreq)
    CallibriControllerInstance.startSignal();
  }

  function stopSignal() {
    freeECGMath()
    setIsSignal(!isSignal)
    CallibriControllerInstance.electrodeChangedCallback = undefined
    CallibriControllerInstance.signalReceivedCallback = undefined
    CallibriControllerInstance.stopSignal()
  }

  function initECGMath(sensorFreq: SensorSamplingFrequency) {
    freeECGMath()

    let samplingRate: number
    // 1000 in this sample
    switch (sensorFreq) {
        case SensorSamplingFrequency.FrequencyHz250:
            samplingRate = 250
            break
        case SensorSamplingFrequency.FrequencyHz1000:
        default:
            samplingRate = 1000
            break
    }
    // Data processing window size
    let dataWindow = samplingRate / 2
    // Number of windows to calculate SI
    let nwinsForPressureIndex = 30

    ecgMath = new EcgMath(samplingRate, dataWindow, nwinsForPressureIndex)

    // optional
    // 4. The averaging parameter of the IN calculation. Default value is 6.
    var pressure_index_average = 6
    ecgMath.setPressureAverage(pressure_index_average)

    // ecgMath.pushData() requires a length of data packets of size sensor.freq / 10
    ecgMathDataPackSize = Math.floor(samplingRate / 10);
    console.log('ecgMathDataPackSize: ' + ecgMathDataPackSize)
  }

  function updateECGMath() {
    if (ecgMath === undefined || ecgMath === null || ecgMathBuffer.length === 0) {
      return
    }

    // ecgMath.pushData() requires a length of data packets of size sensor.samplingFreq / 10
    let packNum =  Math.trunc(ecgMathBuffer.length / ecgMathDataPackSize)

    if (packNum <= 0) {
      // need accumulate data
      return
    }

    while (ecgMathBuffer.length > ecgMathDataPackSize) {
      pushAndReadECGMath(ecgMathBuffer.splice(0, ecgMathDataPackSize))
    }
  }

  function pushAndReadECGMath(data: Array<number>) {
    ecgMath.pushData(data)

    setIsRRdetected(ecgMath.isRRdetected)

    if (ecgMath.isRRdetected) {
      setIsInitialSignalCorrupted(ecgMath.isInitialSignalCorrupted)
      // RR-interval length
      setRR(ecgMath.RR)
      setHR(ecgMath.HR)
      // SI
      setPressureIndex(ecgMath.PressureIndex)
      setModa(ecgMath.Moda)
      // Amplitude of mode
      setAmplModa(ecgMath.AmplModa)
      // Variation range
      setVariationDist(ecgMath.VariationDist)

      ecgMath.setRRchecked()
    }
  }

  function freeECGMath() {
    if (ecgMath === undefined || ecgMath === null) {
      return
    }
    ecgMath.free()
    ecgMath = null
  }

  return (
    <View style={{ marginTop: 10 }}>
      <Button title={isSignal ? 'Stop' : 'Start'}
        onPress={() => { isSignal ? stopSignal() : startSignal() }} />
          <Text style={{ marginTop: 10 }}>{`data pack len for ecg lib: ${ecgMathDataPackSize}`}</Text>
          <Text style={{ marginTop: 10 }}>{`isRRdetected: ${isRRdetected}`}</Text>
        <View style={{flexDirection: 'row'}}>
          <Text>{`RR: ${RR}    `}</Text>
          <Text>{`HR: ${HR}`}</Text>
        </View>
          <Text>{`pressureIndex: ${pressureIndex}`}</Text>
        <View style={{flexDirection: 'row'}}>
          <Text>{`moda: ${moda}    `}</Text>
          <Text>{`amplModa: ${amplModa}`}</Text>
        </View>
        <View style={{flexDirection: 'row'}}>
          <Text>{`variationDist: ${variationDist}    `}</Text>
          <Text>{`isInitialSignalCorrupted: ${isInitialSignalCorrupted}`}</Text>
        </View>
      <Text>{`Electrode state: ${CallibriElectrodeState[elState]}`}</Text>
    </View>
  );
}