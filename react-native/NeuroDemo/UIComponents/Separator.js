import React from 'react';
import { StyleSheet, TouchableOpacity, Text, View } from 'react-native';

export default function Separator() {
  return (
      <View style={styles.separator}/>
  );
}

const styles = StyleSheet.create({
  separator: {
   margin: 10
  }
});