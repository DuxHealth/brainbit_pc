import React, { useState } from 'react';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { View, FlatList, Button } from 'react-native';

import NEEGControllerInstance from '../NeuroImpl/NeuroEEGController';
import { SensorInfo, SensorState } from 'react-native-neurosdk2';

export default function SearchScreen({ navigation }: NativeStackScreenProps<any>) {

  React.useEffect(() => {
    const unsubscribe = navigation.addListener('beforeRemove', () => {
      stopSearch()
    });

    return unsubscribe;
  }, [navigation]);

  const [searching, setSearching] = useState(false)
  const [sensorList, setSensorList] = useState(Array<SensorInfo>);

  function startSearch() {

    setSearching(!searching)
    NEEGControllerInstance.startSearch((sensors) => {
      console.log(sensors)
      setSensorList(sensors)
    });
  }

  function stopSearch() {
    setSearching(!searching)
    NEEGControllerInstance.stopSearch()
    setSensorList([])
  }

  return (
    <View style={{marginTop:10}}>
      <Button title={searching ? 'Stop' : 'Start'}
        onPress={() => { searching ? stopSearch() : startSearch() }} />

      <FlatList style={{ marginTop: 10 }}
        ItemSeparatorComponent={() => <View style={{ height: 10 }} />}
        data={sensorList}
        renderItem={
          ({ item }) =>
            <Button
              color="orange"
              title={`${item.Name} (${item.SerialNumber})`}
              onPress={async () => {
                let state = await NEEGControllerInstance.createAndConnect(item)
                if (state === SensorState.InRange) {
                  NEEGControllerInstance.stopSearch()
                  navigation.goBack()
                }
              }}
            />
        } />
    </View>
  );
}