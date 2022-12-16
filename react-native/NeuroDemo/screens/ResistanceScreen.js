import React from "react";
import { StyleSheet, View, Dimensions, Text} from "react-native";
import { START_RESIST, STOP_RESIST} from "react-native-neurosdk2";
import {SignalLabel} from '../UIComponents/SignalLabel'; 

export default class ResistanceScreen extends React.Component {
  constructor(props){
    super(props);
    this.values = [];
    this.state = {data: [100,200,300],text1: 0, text2: 0, text3: 0,text4: 0};
    console.log("ResistanceScreen constructor")
    this.SensorManager = props.route.params.sensorManager;
    this.SensorManager.sensor.ExecuteCommand(START_RESIST);
    this.SensorManager.sensor.AddBrainBitResistanceChanged((resistanceData)=>{
      this.setState({text1: resistanceData.O1});
      this.setState({text2: resistanceData.O2});
      this.setState({text3: resistanceData.T3});
      this.setState({text4: resistanceData.T4});
    });
  }
  componentWillUnmount()
  {
    this.SensorManager.sensor.RemoveBrainBitResistanceChanged();
    this.SensorManager.sensor.ExecuteCommand(STOP_RESIST);
  }
  render() {
    return (
      <View style={styles.container}>
        <SignalLabel channelName={"O1"} value={this.state.text1}></SignalLabel>
        <SignalLabel channelName={"O2"} value={this.state.text2}></SignalLabel>
        <SignalLabel channelName={"T3"} value={this.state.text3}></SignalLabel>
        <SignalLabel channelName={"T4"} value={this.state.text4}></SignalLabel>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "#f5fcff"
  },
  text: {
    color: 'black',
    fontWeight: 'bold',
    fontSize: 16,
    textAlign: 'center',
  }
});