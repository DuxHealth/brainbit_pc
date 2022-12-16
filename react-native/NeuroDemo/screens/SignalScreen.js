import React from 'react';
import {StyleSheet, View, StatusBar, Text} from 'react-native';
import {ChannelType} from '../neuro_impl/SensorImpl';
import {VictoryChart, VictoryLine} from 'victory-native';
import RNPickerSelect from 'react-native-picker-select';
import { color } from 'react-native-elements/dist/helpers';

export default class SignalScreen extends React.Component {
  constructor(props) {
    super(props);
    this.sensorManager = props.route.params.sensorManager;
    this.values = [];
    this.state = {
      data: [],
      pickerSelectedItem: this.sensorManager.Channels()[0],
    };
    this.maxPoints = 150;
    console.log('SignalScreen constructor');
    this.pickerItems = this.sensorManager.Channels().map(element => {
      return {label: ChannelType[element], value: element};
    });

    this.timerId = setInterval(() => {
      this.setState({data: this.values});
    }, 200);

    this.buffer = [];
    this.signalCount = 0;
    this.sensorManager.StartSignal(signalData => {
      let signal = signalData.filter(
        v => v.type == this.state.pickerSelectedItem,
      )[0];
      this.signalCount += signal.signals.length;
      if (this.buffer.length >= 1000) {
        this.buffer = this.buffer.slice(
          signal.signals.length,
          this.buffer.length,
        );
      }
      this.buffer = this.buffer.concat(signal.signals);
      if (this.buffer.length > this.maxPoints) {
        const k = Math.ceil(this.buffer.length / this.maxPoints);
        this.values = this.buffer.filter((d, i) => i % k === 0);
      }
    });
  }
  componentWillUnmount() {
    this.sensorManager.StopSignal();
    clearInterval(this.timerId);
  }
  render() {
    const placeholder = {
      label: 'Select a channel...',
      value: this.state.pickerSelectedItem,
      color: '#9EA0A4',
    };
    return (
      <View style={styles.container}>
        <View style={styles.picker}>
          <Text style={{color: 'red',justifyContent: 'center'}}>Сhannel: </Text>
          <RNPickerSelect
            style={pickerSelectStyles}
            placeholder={placeholder}
            useNativeAndroidPickerStyle ={false}
            onValueChange={item => this.setState({pickerSelectedItem: item})}
            items={this.pickerItems}
          />
        </View>
        <VictoryChart domainPadding={10} style={styles.chart}>
          <VictoryLine
            style={{data: {stroke: '#34d5eb', strokeWidth: 1}}}
            data={this.values}
          />
        </VictoryChart>
      </View>
    );
  }
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'flex-start',
    flexDirection: 'column',
    marginHorizontal: 0,
    marginTop: StatusBar.currentHeight || 10,
  },
  chart: {
    flex: 1,
    justifyContent: 'space-between',
  },
  picker: {
    flex: 0.3,
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center'
  },
});
const pickerSelectStyles = StyleSheet.create({
  inputIOS: {
    fontSize: 14,
    paddingVertical: 10,
    paddingHorizontal: 12,
    borderWidth: 1,
    borderColor: 'green',
    borderRadius: 8,
    color: 'black',
  },
  inputAndroid: {
    fontSize: 14,
    paddingHorizontal: 10,
    paddingVertical: 5,
    borderWidth: 1,
    borderColor: 'blue',
    borderRadius: 8,
    color: 'black',
  },
});
