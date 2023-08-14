import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Button, FlatList, Text, View } from 'react-native';
import BBControllerInstance from '../NeuroImpl/BrainBitController';
import { BrainBitResistData } from 'react-native-neurosdk2';

export default function ResistanceScreen({navigation} : NativeStackScreenProps<any>) {

  React.useEffect(() => {
    const unsubscribe = navigation.addListener('beforeRemove', () => {
      stopResist()
    });

    return unsubscribe;
  }, [navigation]);

  const [isResist, setIsResist] = useState(false)
  const [resist, setResist] = useState<BrainBitResistData>()

  function startResist() {
    setIsResist(!isResist)
    BBControllerInstance.resistReceivedCallback = (data)=>{
      setResist(data)
      console.log("resist:")
      console.info(data)
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
      <Text>{`O1: ${resist?.O1}`}</Text>
      <Text>{`O2: ${resist?.O2}`}</Text>
      <Text>{`T3: ${resist?.T3}`}</Text>
      <Text>{`T4: ${resist?.T4}`}</Text>
    </View>
  );
}