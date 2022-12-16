import React from 'react';
import { View, Text , StyleSheet, StatusBar, SafeAreaView} from 'react-native';

import { SensorState } from 'react-native-neurosdk2/Types';
import FlatButton from '../UIComponents/FlatButton';
import Separator from '../UIComponents/Separator';

class HomeScreen extends React.Component 
{ 
  constructor(props) 
  {
    super(props);
    this.state = {disabledButton: true}
    this.sensorManager = props.route.params.sensorManager;
    this.sensorManager.connectionStateChanged((deviceState) => {
     this.setState({disabledButton: (deviceState == SensorState.OutRange)})
    });
  }
  componentDidMount(){
    
  }
  render(){ 
    return <SafeAreaView style={styles.container}>
              <FlatButton
                disabled={false}
                text="Device search"
                onPress={() => this.props.navigation.navigate('DeviceSearch')}
              />
              <Separator/>
              <FlatButton
                disabled={this.state.disabledButton}
                text="Signal"
                onPress={() => this.props.navigation.navigate('Signal')}
              />
              <Separator/>
              <FlatButton
                disabled={this.state.disabledButton}
                text="Info"
                onPress={() => this.props.navigation.navigate('Info')}
              />
            </SafeAreaView>
  }
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: 'column',
    marginHorizontal: 70,
    marginTop: StatusBar.currentHeight || 10,
  },
  button:{
    flex: .1,
  }
});
export default HomeScreen;