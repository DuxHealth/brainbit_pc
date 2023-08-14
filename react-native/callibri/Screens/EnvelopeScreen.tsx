import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Button, FlatList, Text, View } from 'react-native';
import CallibriControllerInstance from '../NeuroImpl/CallibriController';
import { CallibriSignalType } from 'react-native-neurosdk2';
import { Grid, LineChart } from 'react-native-svg-charts';

var chartUpdaterId: number
const window: number = 20 * 5;
var samples = new Array<number>()
const buffer = Array(window).fill(0)

export default function EnvelopeScreen({navigation} : NativeStackScreenProps<any>) {
  const [isEnvelope, setIsEnvelope] = useState(false)
  const [chartData, setChartData] = useState<Array<number>>(buffer)

  React.useEffect(() => {
    const unsubscribe = navigation.addListener('beforeRemove', () => {
      stopEnvelope()
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

  function startEnvelope() {
    setIsEnvelope(!isEnvelope)
    CallibriControllerInstance.configureForSignalType(CallibriSignalType.EMG)
    CallibriControllerInstance.envelopeReceivedCallback = (data) => {
      data.forEach((sample) => {
        samples.push(sample.Sample * 1e6)
      })
    }
    CallibriControllerInstance.startEnvelope();
  }

  function stopEnvelope() {
    setIsEnvelope(!isEnvelope)
    CallibriControllerInstance.signalReceivedCallback = undefined
    CallibriControllerInstance.stopEnvelope()
  }

  return (
    <View style={{ marginTop: 10 }}>
      <Button title={isEnvelope ? 'Stop' : 'Start'}
        onPress={() => { isEnvelope ? stopEnvelope() : startEnvelope() }} />
        <LineChart
          style={{ height: 300 }}
          gridMin={-1e3}
          gridMax={1e3}
          data={chartData}
          svg={{ stroke: 'rgb(134, 65, 244)' }}
          contentInset={{ top: 20, bottom: 20 }}>
          <Grid /></LineChart>
    </View>
  );
}