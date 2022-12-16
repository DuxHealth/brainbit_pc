import React, { useState } from 'react';
import { View, Text, StyleSheet , TouchableOpacity} from 'react-native';
import { ListItem } from 'react-native-elements'

const styles = StyleSheet.create({
    item: {
      borderRadius: 8,
      padding: 20,
      marginVertical: 8,
      marginHorizontal: 16,
    },
    title: {
      fontSize: 32,
    },
  });
export default function DeviceListItem ({ item, onPress, backgroundColor, textColor }) {
    return(
        <TouchableOpacity onPress={onPress} style={[styles.item, backgroundColor]}>
            <Text style={[styles.title, textColor]}>{item.Name}</Text>
            <Text style={[styles.title, textColor]}>{item.SerialNumber}</Text>
        </TouchableOpacity>
    );
}