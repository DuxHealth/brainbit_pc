import * as React from 'react';
import {Platform, PermissionsAndroid} from 'react-native';
import {NavigationContainer} from '@react-navigation/native';
import {createNativeStackNavigator} from '@react-navigation/native-stack';

import SensorManager from './neuro_impl/SensorManager';
import {SensorState} from 'react-native-neurosdk2/Types';

//-----Screens-----//
import HomeScreen from './screens/HomeScreen';
import DeviceSearchScreen from './screens/DeviceSearchScreen';
import SignalScreen from './screens/SignalScreen';
import InfoScreen from './screens/InfoScreen';

//-----UIComponents-----//
import {DeviceStatusBar} from './UIComponents/DeviceStatusBar';

const Stack = createNativeStackNavigator();

export async function requestPermissionAndroid() {
  try {
    const granted = await PermissionsAndroid.requestMultiple([
      PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION,
      PermissionsAndroid.PERMISSIONS.BLUETOOTH_CONNECT,
    ]);
    if (
      granted[PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION] ===
      PermissionsAndroid.RESULTS.GRANTED
    ) {
      console.log('permission access');
    } else {
      console.log('permission denied');
      requestPermissionAndroid();
    }
    if (
      granted[PermissionsAndroid.PERMISSIONS.BLUETOOTH_CONNECT] ===
      PermissionsAndroid.RESULTS.GRANTED
    ) {
      console.log('permission access');
    } else {
      console.log('permission denied');
      //requestPermissionAndroid();
    }
  } catch (err) {
    console.warn(err);
  }
}
class App extends React.Component {
  constructor(props) {
    super(props);
    if (Platform.OS === 'android') {
      requestPermissionAndroid();
    }
    this.SensorManager = new SensorManager();
    this.state = {sensorState: 'Disconnect', powerProcent: '-'};
    this.SensorManager.connectionStateChanged(sensorState => {
      console.log('Sensor state: ' + SensorState[sensorState]);
      this.setState({sensorState: sensorState ? 'Disconnect' : 'Connect'});
      if (SensorState.OutRange === sensorState) {
        this.setState({powerProcent: '-'});
      }
    });
    this.SensorManager.batteryChanged(powerProcent => {
      this.setState({powerProcent: powerProcent});
    });
  }
  componentDidMount() {}

  render() {
    return (
      <NavigationContainer>
        <Stack.Navigator
          initialRouteName="Home"
          screenOptions={{
            title: 'NeuroSDK2 Demo',
            headerStyle: {
              backgroundColor: '#ffffff',
            },
            headerTintColor: '#000000',
            headerTitleStyle: {
              fontWeight: 'bold',
            },
          }}>
          <Stack.Screen
            name="Home"
            initialParams={{sensorManager: this.SensorManager}}
            component={HomeScreen}
          />
          <Stack.Screen
            name="DeviceSearch"
            initialParams={{sensorManager: this.SensorManager}}
            component={DeviceSearchScreen}
          />
          <Stack.Screen
            name="Signal"
            initialParams={{sensorManager: this.SensorManager}}
            component={SignalScreen}
          />
          <Stack.Screen
            name="Info"
            initialParams={{sensorManager: this.SensorManager}}
            component={InfoScreen}
          />
        </Stack.Navigator>
        <DeviceStatusBar
          sensorState={this.state.sensorState}
          powerProcent={this.state.powerProcent}
        />
      </NavigationContainer>
    );
  }
}

export default App;
