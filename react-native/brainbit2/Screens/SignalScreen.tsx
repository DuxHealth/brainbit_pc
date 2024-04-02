import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { StyleSheet, Button, Text, View } from 'react-native';
import BBControllerInstance from '../NeuroImpl/BrainBitController';
import { LineChart, Grid } from 'react-native-svg-charts'
import SelectDropdown from 'react-native-select-dropdown'

var chartUpdaterId: NodeJS.Timeout
const window: number = 250 * 4;
var samples = new Map<String, Array<Number>>()
const buffer = Array(window).fill(0)


export default function SignalScreen({ navigation }: NativeStackScreenProps<any>) {

  const [isSignal, setIsSignal] = useState(false)
  const [chartData, setChartData] = useState<Array<Number>>(buffer)
  const [channelInfos, setChannelInfos] = useState<Array<String>>([])
  const [position, setPosition] = useState(0);
  const [currentShowChannel, setCurrentShowChannel] = useState("O1") // <- ?????

  React.useEffect(() => {
    const unsubscribe = navigation.addListener('beforeRemove', () => {
      stopSignal()
      clearInterval(chartUpdaterId);
    });

    return unsubscribe;
  }, [navigation]);

  React.useEffect(() => {
    setChannelInfos(BBControllerInstance.channelsList())
    setCurrentShowChannel(BBControllerInstance.channelsList()[0] as string)

  }, []);

  function startSignal() {
    setIsSignal(!isSignal)

    chartUpdaterId = setInterval(() => {
      setPosition(position => {
        if(!samples.has(currentShowChannel) || (samples.get(currentShowChannel)?.length ?? 0) < 1) return 0

        let count = samples.get(currentShowChannel)?.length ?? 0

        var endIndex = count
        let newPositon = position + count
        if(newPositon-window > 0){
          endIndex = count - (newPositon-window)
          newPositon = newPositon-window
        }

        setChartData(data => {
          samples.get(currentShowChannel)?.forEach(s => {
            buffer.shift()
            buffer.push(s)
          });
          samples.set(currentShowChannel, [])
          return buffer;
        });
        return newPositon
      });
    }, 200);

    let channelsCount = BBControllerInstance.channelsCount;
    BBControllerInstance.signalReceivedCallback = (data) => {
      for (let i = 0; i < channelsCount; i++) {
        data.forEach((sample) => {
          if(samples.has(channelInfos[i])){
            if((samples.get(channelInfos[i])?.length ?? 0) >= window) {
              samples.get(channelInfos[i])?.pop();
            }
            samples.get(channelInfos[i])?.push(sample.Samples[i] * 1e6)
          } else {
            samples.set(channelInfos[i], [sample.Samples[i] * 1e6])
          }
        })
      }
    }
    BBControllerInstance.startSignal();
  }

  function stopSignal() {
    setIsSignal(!isSignal)
    clearInterval(chartUpdaterId);
    BBControllerInstance.signalReceivedCallback = undefined
    BBControllerInstance.stopSignal()
  }

  return (
    <View style={{ marginTop: 10, flex: 1 }}>
      <Button title={isSignal ? 'Stop' : 'Start'}
        onPress={() => { isSignal ? stopSignal() : startSignal() }} />
        <SelectDropdown
          defaultButtonText={channelInfos[0] as string}
        	data={channelInfos}
        	onSelect={(selectedItem, index) => {
            setCurrentShowChannel(selectedItem)
        		console.log(selectedItem, index)
        	}}
        	buttonTextAfterSelection={(selectedItem, index) => {
        		return selectedItem
        	}}
        	rowTextForSelection={(item, index) => {
        		return item
        	}}
        />
        <LineChart
          style={{ flex: 1 }}
          gridMin={-1e6}
          gridMax={1e6}
          data={chartData}
          svg={{ stroke: 'rgb(134, 65, 244)' }}
          animate={false}
          contentInset={{ top: 20, bottom: 20 }}>
          <Grid />
          </LineChart>
   

    </View>
  );
}

const styles = StyleSheet.create({
  blackText: {
    color: 'black',
  }
});