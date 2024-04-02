import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Button, View, Text, FlatList } from 'react-native';
import BBControllerInstance from '../NeuroImpl/BrainBitController';
import { SensorState } from 'react-native-neurosdk2';

function MainScreen({ navigation }: NativeStackScreenProps<any>) {

  const [connected, setConnected] = useState(SensorState.OutOfRange)
  const [power, setPower] = useState(0)

  BBControllerInstance.connectionChangedCallback = (state)=>{
    setConnected(state)
  }

  BBControllerInstance.batteryCallback = (battery)=>{
    setPower(battery)
  }

  return (
    <View style={{ marginTop: 10, flex:1, justifyContent:'space-between'}}>
      <Button
              title={'Search'}
              onPress={() => navigation.navigate('Search')}
            />
            <View style={{height:10}}/>
      <FlatList
        ItemSeparatorComponent={() => <View style={{ height: 10 }} />}
        data={[
          { key: 'Info' },
          { key: 'Signal' },
          { key: 'Resistance' },
        ]}
        renderItem={
          ({ item }) =>
            <Button
              disabled={connected === SensorState.OutOfRange}
              title={item.key}
              onPress={() => navigation.navigate(item.key)}
            />
        } />
        <Text style={{color: 'black'}}>{`Connection state: ${SensorState[connected]} Battery:${power}`}</Text>
    </View>
  );
}

export default MainScreen;