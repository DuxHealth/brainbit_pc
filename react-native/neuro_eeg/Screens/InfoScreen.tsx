import React from 'react';

import { StyleSheet, Text, ScrollView } from 'react-native';
import NEEGControllerInstance from '../NeuroImpl/NeuroEEGController';

export default function InfoScreen() {
  return (
    <ScrollView>
      <Text style={{color: 'black'}}>{NEEGControllerInstance.info}</Text>
    </ScrollView>
  );
}