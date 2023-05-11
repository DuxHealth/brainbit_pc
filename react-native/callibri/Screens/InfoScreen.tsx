import React from 'react';

import { Text, View, ScrollView } from 'react-native';
import CallibriControllerInstance from '../NeuroImpl/CallibriController';

export default function InfoScreen() {
  return (
    <ScrollView>
      <Text>{CallibriControllerInstance.info}</Text>
    </ScrollView>
  );
}