import React from "react";
import { StyleSheet, View, Dimensions, Text, SafeAreaView,ScrollView} from "react-native";
import { LineChart } from "react-native-chart-kit";
import {SensorCommand, SensorState, SensorExternalSwitchInput, SensorParameter, SensorFamily, SensorFeature, SensorParamAccess} from "react-native-neurosdk2/Types";

import {SignalLabel} from '../UIComponents/SignalLabel'; 

export default class InfoScreen extends React.Component {
  constructor(props){
    super(props);
    this.values = [];
    this.state = {InfoText: 0};
    console.log("InfoScreen constructor")
    this.sensorManager = props.route.params.sensorManager;
  }
  componentDidMount(){
    this.setState({InfoText: this.sensorManager.Info()});
  }
  componentWillUnmount()
  {
  }
  render() {
    return (
      <SafeAreaView style={styles.container}>
        <ScrollView>
          <Text style={styles.text}>{this.state.InfoText}</Text>
        </ScrollView>
      </SafeAreaView>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "space-between",
    alignItems: "center",
    backgroundColor: "#f5fcff"
  },
  text: {
    color: 'black',
    fontWeight: 'bold',
    fontSize: 12,
    textAlign: 'left',
  }
});