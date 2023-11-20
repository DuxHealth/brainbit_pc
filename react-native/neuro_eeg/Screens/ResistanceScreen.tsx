import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { StyleSheet, Button, FlatList, ScrollView, Text, View } from 'react-native';
import NEEGControllerInstance from '../NeuroImpl/NeuroEEGController';
import { ResistChannelsData } from 'react-native-neurosdk2';

var resistUpdater: number
var resistTmp: ResistChannelsData

export default function ResistanceScreen({navigation} : NativeStackScreenProps<any>) {

  const [channelInfos, setChannelInfos] = useState<Array<String>>([])

  React.useEffect(() => {
    const unsubscribe = navigation.addListener('beforeRemove', () => {
      stopResist()
      clearInterval(resistUpdater);
    });

    return unsubscribe;
  }, [navigation]);

  React.useEffect(() => {
    setChannelInfos(NEEGControllerInstance.channelsList())
    resistUpdater = setInterval(() => {
      setResist(resistTmp)
    }, 200);
  }, []);

  const [isResist, setIsResist] = useState(false)
  const [resist, setResist] = useState<ResistChannelsData>()

  function startResist() {
    setIsResist(!isResist)
    NEEGControllerInstance.resistReceivedCallback = (data)=>{
      resistTmp = data[0]
    }
    NEEGControllerInstance.startResist();
  }

  function stopResist() {
    setIsResist(!isResist)
    NEEGControllerInstance.resistReceivedCallback = undefined
    NEEGControllerInstance.stopResist()
  }
  
  return (
    <View style={{ marginTop: 10 }}>
      <Button title={isResist ? 'Stop' : 'Start'}
        onPress={() => { isResist ? stopResist() : startResist() }} />
      <Text style={[styles.blackText]}>{`A1: ${resist?.A1}`}</Text>
      <Text style={[styles.blackText]}>{`A2: ${resist?.A2}`}</Text>
      <Text style={[styles.blackText]}>{`Bias: ${resist?.Bias}`}</Text>
     
     <ScrollView>
      {
      resist?.Values.map((v,i)=>
        <Text style={[styles.blackText]}>{channelInfos[i] + ": " + v}</Text>
      )
     }
     </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  blackText: {
    color: 'black',
  }
});