import React from 'react';
import { StyleSheet, Text, View } from 'react-native';

import Separator from '../UIComponents/Separator.js'; 

 function DeviceStatusBar({ sensorState, powerProcent}) {
  return (
    <View style={styles.statusBar}>
        <Text style={styles.labelText}>State: </Text>
        <Text style={styles.valueText}>{sensorState}</Text>
        <Separator/>
        <Text style={styles.labelText}>Power: </Text>
        <Text style={styles.valueText}>{powerProcent} %</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  statusBar: {
    padding: 5,
    justifyContent: 'flex-start',
    flexDirection: 'row',
    backgroundColor: '#46474C',
  },
  labelText: {
    color: 'white',
    fontWeight: 'bold',
    fontSize: 14,
    textAlign: 'center',
    color: "#FFFFFF"
  },
  valueText: {
    color: 'white',
    fontWeight: 'bold',
    fontSize: 14,
    textAlign: 'center',
    color: "#09D70A"
  }
});
export {DeviceStatusBar}