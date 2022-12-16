import React, { useState } from 'react';
import { View, Text, FlatList, StyleSheet ,SafeAreaView,StatusBar, Alert} from 'react-native';
import FlatButton from '../UIComponents/FlatButton.js';
import DeviceListItem from '../UIComponents/DeviceListItem'

class DeviceSearchScreen extends React.Component
{ 
  constructor(props) 
  {
    super(props);
    this.sensorManager = props.route.params.sensorManager;
    this.state = {searchButtonText: "Start search", deviceList: null, selectedId: null};
    this.renderItem = this.renderItem.bind(this);
    this.searchButtonClickHandler = this.searchButtonClickHandler.bind(this);
  }
  componentDidMount(){
    
  }
  componentWillUnmount()
  {
    this.sensorManager.StopSearch();
  }
  searchButtonClickHandler()
  {
    if(this.state.searchButtonText == "Start search"){
      this.sensorManager.StartSearch((deviceList => {
        this.setState({deviceList: deviceList});
      }));
      this.setState({searchButtonText: "Stop search"});
    }
    else
    {
      this.sensorManager.StopSearch();
      this.setState({searchButtonText: "Start search"});
      this.setState({deviceList: null});
    }
  }
 
  renderItem({item})
  {
    const backgroundColor = "#f9c2ff";
    const color = 'white';
    return (
      <DeviceListItem
        item={item}
        onPress={() => {
          console.log(item);
          this.sensorManager.Connect(item).then((r)=>{
            if(r) 
              this.props.navigation.goBack();
            else
              Alert.alert("Connect oops..");
          }).catch((e)=>{
            Alert.alert(e);
            console.log(e);
          });
        }}
        backgroundColor={{ backgroundColor }}
        textColor={{ color }}
      />
    );
  };
  render(){
    return(
    <SafeAreaView style={styles.container}>
        <View style={styles.button}>
          <FlatButton
            key="searchButton"
            text={this.state.searchButtonText}
            onPress={() => {
              this.searchButtonClickHandler();
            }}
          />
        </View>
        <FlatList 
          style={styles.list}
          data={this.state.deviceList}
          renderItem={(item) => this.renderItem(item, this)}
          keyExtractor={(item) => item.Address}
          extraData={this.state.selectedId}
        />
      </SafeAreaView>
    );
  }
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: 'column',
    marginTop: StatusBar.currentHeight || 10,
  },
  button:{
    flex: .1,
    marginHorizontal: 20
  },
  list:{
    flex: 0.5
  },
  item: {
    borderRadius: 8,
    padding: 20,
    marginVertical: 8,
    marginHorizontal: 16,
  }
});
export default DeviceSearchScreen;