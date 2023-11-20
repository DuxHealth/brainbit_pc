import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Button, View, Text, FlatList } from 'react-native';
import NEEGControllerInstance from '../NeuroImpl/NeuroEEGController';
import { SensorState } from 'react-native-neurosdk2';

function MainScreen({ navigation }: NativeStackScreenProps<any>) {

  const [connected, setConnected] = useState(SensorState.OutOfRange)
  const [power, setPower] = useState(0)

  NEEGControllerInstance.connectionChangedCallback = (state)=>{
    setConnected(state)
  }

  NEEGControllerInstance.batteryCallback = (battery)=>{
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
          { key: 'Signal+Resistance' },
        ]}
        renderItem={
          ({ item }) =>
            <Button
              disabled={connected === SensorState.OutOfRange}
              title={item.key}
              onPress={() => navigation.navigate(item.key)}
            />
        } />
        <Text>{`Connection state: ${SensorState[connected]} Battery:${power}`}</Text>
    </View>
  );
}

export default MainScreen;