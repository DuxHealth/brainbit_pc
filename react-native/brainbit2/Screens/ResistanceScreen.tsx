import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Button, FlatList, Text, View } from 'react-native';
import BBControllerInstance from '../NeuroImpl/BrainBitController';
import { ResistRefChannelsData } from 'react-native-neurosdk2';

export default function ResistanceScreen({navigation} : NativeStackScreenProps<any>) {

  React.useEffect(() => {
    const unsubscribe = navigation.addListener('beforeRemove', () => {
      stopResist()
    });

    return unsubscribe;
  }, [navigation]);

  const [isResist, setIsResist] = useState(false)
  const [resist, setResist] = useState<ResistRefChannelsData>()

  function startResist() {
    setIsResist(!isResist)
    BBControllerInstance.resistReceivedCallback = (data)=>{
      setResist(data[0])
      console.log(data)
    }
    BBControllerInstance.startResist();
  }

  function stopResist() {
    setIsResist(!isResist)
    BBControllerInstance.signalReceivedCallback = undefined
    BBControllerInstance.stopResist()
  }
  
  return (
    <View style={{ marginTop: 10 }}>
      <Button title={isResist ? 'Stop' : 'Start'}
        onPress={() => { isResist ? stopResist() : startResist() }} />
      <Text>{`PackNum: ${resist?.PackNum}`}</Text>
      <Text>{`Samples: ${resist?.Samples.map((sample, i) => {
              return `\n      ${i}: ${sample} `
            })}`}</Text>
      <Text>{`Referents: ${resist?.Referents.map((ref, i) => {
              return `\n      ${i}: ${ref} `
            })}`}</Text>
    </View>
  );
}