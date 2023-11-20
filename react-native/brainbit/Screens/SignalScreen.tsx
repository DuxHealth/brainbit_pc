import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Button, ScrollView, Text, View } from 'react-native';
import BBControllerInstance from '../NeuroImpl/BrainBitController';
import { LineChart, Grid } from 'react-native-svg-charts'
import { BrainBitSignalData } from 'react-native-neurosdk2';

var chartUpdaterId: number
const window: number = 250 * 5;
var O1Samples = new Array<number>()
var O2Samples = new Array<number>()
var T3Samples = new Array<number>()
var T4Samples = new Array<number>()
const bufferO1 = Array(window).fill(0)
const bufferO2 = Array(window).fill(0)
const bufferT3 = Array(window).fill(0)
const bufferT4 = Array(window).fill(0)



export default function SignalScreen({ navigation }: NativeStackScreenProps<any>) {

  const [isSignal, setIsSignal] = useState(false)
  const [O1ChartData, setO1ChartData] = useState<Array<number>>(bufferO1)
  const [O2ChartData, setO2ChartData] = useState<Array<number>>(bufferO2)
  const [T3ChartData, setT3ChartData] = useState<Array<number>>(bufferT3)
  const [T4ChartData, setT4ChartData] = useState<Array<number>>(bufferT4)

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
        let count = O1Samples.length

        let newPositon = position + count
        if(newPositon-window > 0){
          newPositon = newPositon-window
        }
        setO1ChartData(data => {
          O1Samples.forEach(s => {
            bufferO1.shift()
            bufferO1.push(s)
          });
          O1Samples = []
          return bufferO1;
        });
        setO2ChartData(data => {
          O2Samples.forEach(s => {
            bufferO2.shift()
            bufferO2.push(s)
          });
          O2Samples = []
          return bufferO2;
        });
        setT3ChartData(data => {
          T3Samples.forEach(s => {
            bufferT3.shift()
            bufferT3.push(s)
          });
          T3Samples = []
          return bufferT3;
        });
        setT4ChartData(data => {
          T4Samples.forEach(s => {
            bufferT4.shift()
            bufferT4.push(s)
          });
          T4Samples = []
          return bufferT4;
        });
        
        return newPositon
      });
    }, 200);
  }, []);

  function startSignal() {
    setIsSignal(!isSignal)
    BBControllerInstance.signalReceivedCallback = (data) => {
      data.forEach((sample) => {
        O1Samples.push(sample.O1 * 1e6)
        O2Samples.push(sample.O2 * 1e6)
        T3Samples.push(sample.T3 * 1e6)
        T4Samples.push(sample.T4 * 1e6)
      })
    }
    BBControllerInstance.startSignal();
  }

  function stopSignal() {
    setIsSignal(!isSignal)
    BBControllerInstance.signalReceivedCallback = undefined
    BBControllerInstance.stopSignal()
  }

  return (
    <View style={{ marginTop: 10, flex: 1 }}>
      <Button title={isSignal ? 'Stop' : 'Start'}
        onPress={() => { isSignal ? stopSignal() : startSignal() }} />
   
        <Text>O1</Text>
        <LineChart
          style={{ flex: 1 }}
          gridMin={-1e3}
          gridMax={1e3}
          data={O1ChartData}
          svg={{ stroke: 'rgb(134, 65, 244)' }}
          contentInset={{ top: 20, bottom: 20 }}>
          <Grid /></LineChart>
        <Text>O2</Text>
        <LineChart
          style={{ flex: 1 }}
          gridMin={-1e3}
          gridMax={1e3}
          data={O2ChartData}
          svg={{ stroke: 'rgb(20, 187, 200)' }}
          contentInset={{ top: 20, bottom: 20 }}>
          <Grid /></LineChart>
        <Text>T3</Text>
        <LineChart
          style={{ flex: 1 }}
          gridMin={-1e3}
          gridMax={1e3}
          data={T3ChartData}
          svg={{ stroke: 'rgb(128, 127, 137)' }}
          contentInset={{ top: 20, bottom: 20 }}>
          <Grid /></LineChart>
        <Text>T4</Text>
        <LineChart
          style={{ flex: 1 }}
          gridMin={-1e3}
          gridMax={1e3}
          data={T4ChartData}
          svg={{ stroke: 'rgb(255, 0, 127)' }}
          contentInset={{ top: 20, bottom: 20 }}>
          <Grid /></LineChart>
   

    </View>
  );
}