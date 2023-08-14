import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Button, View, Text, FlatList } from 'react-native';
import { SensorState } from 'react-native-neurosdk2';
import CallibriControllerInstance from '../NeuroImpl/CallibriController';

function MainScreen({ navigation }: NativeStackScreenProps<any>) {

  const [connected, setConnected] = useState(SensorState.OutRange)
  const [power, setPower] = useState(0)

  CallibriControllerInstance.connectionChangedCallback = (state) => {
    setConnected(state)
  }

  CallibriControllerInstance.batteryCallback = (battery) => {
    setPower(battery)
  }

  return (
    <View style={{ marginTop: 10, flex: 1, justifyContent: 'space-between' }}>
      <Button
        title={'Search'}
        onPress={() => {console.log("search tapped"); navigation.navigate('Search')}}
      />
      <View style={{ height: 10 }} />
      <FlatList
        ItemSeparatorComponent={() => <View style={{ height: 10 }} />}
        data={[
          { key: 'Info' },
          { key: 'Signal' },
          { key: 'Envelope' },
          { key: 'ECG' },
        ]}
        renderItem={
          ({ item }) =>
            <Button
              disabled={connected === SensorState.OutRange}
              title={item.key}
              onPress={() => navigation.navigate(item.key)}
            />
        } />
        
      <Text>{`Connection state: ${SensorState[connected]} Battery:${power}`}</Text>
    </View>
  );
}

export default MainScreen;