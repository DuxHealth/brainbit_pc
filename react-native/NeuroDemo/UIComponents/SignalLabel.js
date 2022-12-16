import React from 'react';
import { StyleSheet, Text, View } from 'react-native';

 function SignalLabel({channelName, value}) {
  return (
    <View style={styles.statusBar}>
        <Text style={styles.labelText}>{channelName} </Text>
        <Text style={styles.valueText}>{value}</Text>
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
export {SignalLabel}