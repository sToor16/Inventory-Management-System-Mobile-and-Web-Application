import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { StackNavigator } from 'react-navigation';

import LoginScreen from './Screens/LoginScreen';
import FacilitiesScreen from './Screens/FacilitiesScreen';
import ResourcesScreen from './Screens/ResourcesScreen';
import SelectedResourceCard from './Screens/Cards/SelectedResourceCard';

export default class App extends React.Component {
  render() {
    return (
      <AppNavigator />
    );
  }
}

const AppNavigator = StackNavigator({
  LoginScreen: {
    screen: LoginScreen,
    navigationOptions: {
      title: 'Inventory Management System',
       
    }
  },
  FacilitiesScreen: {
    screen: FacilitiesScreen,
    navigationOptions: {
      title: 'Facilities',
      headerLeft: null
    }
  },
  ResourcesScreen: {
    screen: ResourcesScreen,
    navigationOptions: {
      title: 'Resouces'
    }
  },
  SelectedResourceCard: { screen: SelectedResourceCard,
    navigationOptions: {
      title: 'Resource Details'
    }}
})

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
