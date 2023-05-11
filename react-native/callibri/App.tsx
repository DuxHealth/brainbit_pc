import React from 'react';
import {NavigationContainer} from '@react-navigation/native';
import {createNativeStackNavigator} from '@react-navigation/native-stack';
import MainScreen from './Screens/MainScreen';
import InfoScreen from './Screens/InfoScreen';
import SignalScreen from './Screens/SignalScreen';
import SearchScreen from './Screens/SearchScreen';
import EnvelopeScreen from './Screens/EnvelopeScreen';

const Stack = createNativeStackNavigator();

function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator initialRouteName="Menu">
      <Stack.Screen name="Menu" component={MainScreen} />
      <Stack.Screen name="Info" component={InfoScreen} />
      <Stack.Screen name="Search" component={SearchScreen} />
      <Stack.Screen name="Signal" component={SignalScreen} />
      <Stack.Screen name="Envelope" component={EnvelopeScreen} />
      </Stack.Navigator>
    </NavigationContainer>
  );
}

export default App;
