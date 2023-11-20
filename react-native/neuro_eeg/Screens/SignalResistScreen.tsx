import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { StyleSheet, Button, FlatList, ScrollView, Text, View } from 'react-native';
import NEEGControllerInstance from '../NeuroImpl/NeuroEEGController';
import { LineChart, Grid } from 'react-native-svg-charts'
import { ResistChannelsData, SignalChannelsData } from 'react-native-neurosdk2';
import SelectDropdown from 'react-native-select-dropdown'

var screenUpdater: number
var resistTmp: ResistChannelsData

const window: number = 500 * 2;
var samples = new Map<String, Array<Number>>()
const buffer = Array(window).fill(0)


export default function SignalResistScreen({ navigation }: NativeStackScreenProps<any>) {

    const [isSignal, setIsSignal] = useState(false)
    const [chartData, setChartData] = useState<Array<Number>>(buffer)
    const [channelInfos, setChannelInfos] = useState<Array<String>>([])
    const [resist, setResist] = useState<ResistChannelsData>()
    const [position, setPosition] = useState(0);
    const [currentShowChannel, setCurrentShowChannel] = useState("O1")
    const [currentChannelId, setCurrentChannelId] = useState(0)
    const [lastSignalData, setLastSignalData] = useState('')


    React.useEffect(() => {
        const unsubscribe = navigation.addListener('beforeRemove', () => {
          stopSignal()
          clearInterval(screenUpdater);
        });
    
        return unsubscribe;
      }, [navigation]);

      React.useEffect(() => {
        setChannelInfos(NEEGControllerInstance.channelsList())
        screenUpdater = setInterval(() => {
            setPosition(position => {
                if(!samples.has(currentShowChannel) || samples.get(currentShowChannel)?.length < 1) return 0
        
                let count = samples.get(currentShowChannel)?.length
        
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

          setResist(resistTmp)
        }, 200);
      }, []);

      function startSignal() {
        setIsSignal(!isSignal)
        let channelsCount = NEEGControllerInstance.maxChannelsCount;
        NEEGControllerInstance.signalResistReceivedCallback = (signalData, resistData) => {
          for (let i = 0; i < channelsCount; i++) {
            if (signalData === undefined) {
              console.error('singalData is undefiend!');
              return;
            }
            signalData.forEach((sample) => {
              let channel = channelInfos[i];
              if(samples.has(channel)){
                if(samples.get(channel)?.length >= window) {
                  samples.get(channel)?.pop();
                }
                samples.get(channel)?.push(sample.Samples[i] * 1e3)
              } else {
                samples.set(channel, [sample.Samples[i] * 1e3])
              }
            })
            if (resistData !== undefined && resistData.length > 0) {
              resistTmp = resistData.pop();
            }
          }
          // if (signalData !== undefined && signalData.length > 0 && signalData[0].Samples !== undefined && signalData[0].Samples.length > 0) {
          //   let v = signalData[0];
          //   let s = v.Samples;
          //   setLastSignalData(`${v.PackNum}: [`
          //     + s.map((v, i) => `\n  ${channelInfos[i]}: ${v}`)
          //     +`\n]`
          //   );
          // }
        }
        NEEGControllerInstance.startSignalResist();
      }
    
      function stopSignal() {
        setIsSignal(!isSignal)
        NEEGControllerInstance.signalResistReceivedCallback = undefined
        NEEGControllerInstance.stopSignalResist()
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
                setCurrentChannelId(index)
        		console.log('selected ch: ', selectedItem, index)
        	}}
        	buttonTextAfterSelection={(selectedItem, index) => {
        		return selectedItem
        	}}
        	rowTextForSelection={(item, index) => {
        		return item
        	}}
        />
      <Text style={[styles.blackText]}>{
        `resist data for: ${currentShowChannel} {`
        + `\n  A1: ${resist?.A1}`
        + `\n  A2: ${resist?.A2}`
        + `\n  Bias: ${resist?.Bias}`
        + `\n  Resist: ${resist?.Values[currentChannelId]}`
        + `\n}`
      }</Text>
      <Text style={[styles.blackText]}>{`${lastSignalData}`}</Text>
      <LineChart
        style={{ flex: 1 }}
        gridMin={-1e3}
        gridMax={1e3}
        data={chartData}
        svg={{ stroke: 'rgb(134, 65, 244)' }}
        animate={false}
        contentInset={{ top: 20, bottom: 20 }}>
        <Grid /></LineChart> 

    </View>
  );
}

const styles = StyleSheet.create({
  blackText: {
    color: 'black',
  }
});