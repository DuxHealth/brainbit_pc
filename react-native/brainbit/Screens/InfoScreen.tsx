import React from 'react';

import { Text, View, ScrollView } from 'react-native';
import BBControllerInstance from '../NeuroImpl/BrainBitController';

export default function InfoScreen() {
  return (
    <ScrollView>
      <Text style={{color: 'black'}}>{BBControllerInstance.info}</Text>
    </ScrollView>
  );
}