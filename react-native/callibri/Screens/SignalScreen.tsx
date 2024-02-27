import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Button, Text, View } from 'react-native';
import CallibriControllerInstance from '../NeuroImpl/CallibriController';
import { LineChart, Grid } from 'react-native-svg-charts'
import { CallibriElectrodeState, CallibriSignalType } from 'react-native-neurosdk2';

var chartUpdaterId: number
const window: number = 1000 * 5;
var samples = new Array<number>()
const buffer = Array(window).fill(0)

export default function SignalScreen({ navigation }: NativeStackScreenProps<any>) {

  const [isSignal, setIsSignal] = useState(false)
  const [elState, setElState] = useState(CallibriElectrodeState.Detached)
  const [chartData, setChartData] = useState<Array<number>>(buffer)

  React.useEffect(() => {
    const unsubscribe = navigation.addListener('beforeRemove', () => {
      stopSignal()
      clearInterval(chartUpdaterId);
    });

    return unsubscribe;
  }, [navigation]);

  const [position, setPosition] = useState(0);
  React.useEffect(() => {
    chartUpdaterId = setInterval(() => {
      setPosition(position => {
        let count = samples.length

        var endIndex = count
        let newPositon = position + count
        if(newPositon-window > 0){
          endIndex = count - (newPositon-window)
          newPositon = newPositon-window
        }
        setChartData(data => {
          for(let i = 0; i < endIndex; i++){
            buffer[position++] = samples[i]
          }
          position = 0
          for(let i = endIndex; i < count; i++){
            buffer[position++] = samples[i]
          }
          samples = new Array<number>()
          return buffer;
        });        
        return newPositon
      });
    }, 200);
  }, []);

  function startSignal() {
    setIsSignal(!isSignal)
    CallibriControllerInstance.configureForSignalType(CallibriSignalType.EMG)
    CallibriControllerInstance.electrodeChangedCallback = (state) => {
      setElState(state)
    }
    CallibriControllerInstance.signalReceivedCallback = (data) => {
      data.forEach((sample) => {
        sample.Samples.forEach((value)=>{
          samples.push(value * 1e6)
        })
      })
    }
    CallibriControllerInstance.startSignal();
  }

  function stopSignal() {
    setIsSignal(!isSignal)
    CallibriControllerInstance.electrodeChangedCallback = undefined
    CallibriControllerInstance.signalReceivedCallback = undefined
    CallibriControllerInstance.stopSignal()
  }

  return (
    <View style={{ marginTop: 10 }}>
      <Button title={isSignal ? 'Stop' : 'Start'}
        onPress={() => { isSignal ? stopSignal() : startSignal() }} />
        <LineChart
          style={{ height: 300 }}
          gridMin={-1e3}
          gridMax={1e3}
          data={chartData}
          svg={{ stroke: 'rgb(134, 65, 244)' }}
          contentInset={{ top: 20, bottom: 20 }}>
          <Grid /></LineChart>
      <Text style={{color: 'black'}}>{`Electrode state: ${CallibriElectrodeState[elState]}`}</Text>
    </View>
  );
}