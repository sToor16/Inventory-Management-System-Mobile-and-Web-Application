import React, { Component } from 'react';
import { View, StyleSheet } from 'react-native';
import { NavigationActions } from 'react-navigation';
import axios from 'axios';
import { Container, Header, Content, Card, CardItem, Button, Text } from 'native-base';
import t from 'tcomb-form-native';
import { KeyboardAwareScrollView } from 'react-native-keyboard-aware-scroll-view';
import LogoutButton from '../Button/LogoutButton';

const Form = t.form.Form;

const ResourceDetails = t.struct({
  Size: t.maybe(t.String),
  Color: t.maybe(t.String),
  Quantity: t.Number,
  Description: t.maybe(t.String),
});

const formStyles = {
  ...Form.stylesheet,
  formGroup: {
    normal: {
      marginBottom: 10
    },
  },
  controlLabel: {
    normal: {
      color: '#909090',
      fontSize: 16,
      marginBottom: 5,
      fontWeight: '500'
    },
    error: {
      color: 'red',
      fontSize: 18,
      marginBottom: 7,
      fontWeight: '600'
    }
  }
}

const options = {
  fields: {
    ResourceName: {
      editable: false,
      label: 'Name'
    },
    Quantity: {
      error: 'Quantity is Required.',
    },
  },
  stylesheet: formStyles,
};

export default class SelectedResourceCard extends Component {
  constructor(props) {
    super(props);
    this.state = {
      ResourceId: this.props.navigation.state.params.ResourceId,
      ResourceDetails: ''
    };
  }
  componentWillMount() {
    axios.post('https://dewmdy2eph.execute-api.us-east-1.amazonaws.com/latest/getSelectedResources', {
      ResourceId: this.state.ResourceId,
    })
      .then(response => {
        this.setState({ ResourceDetails: response.data.recordset[0]});
          console.log(this.state.ResourceDetails.ResourceName);
      });
  }
  updateFunction = () => {
    const value = this._form.getValue();
    if (value) {
      axios.put('https://dewmdy2eph.execute-api.us-east-1.amazonaws.com/latest/editSelectedResource', {
        ResourceId: this.props.navigation.state.params.ResourceId,
        Quantity: value['Quantity'],
        Description: value['Description'],
        Size: value['Size'],
        Color: value['Color']
      })
        .then(() => {
          alert("Details Updated")
        });
    }
  }

  render() {
    var values = {
      ResourceId: this.props.navigation.state.params.ResourceId,
      ResourceName: this.state.ResourceDetails.ResourceName,
      Size: this.state.ResourceDetails.Size,
      Color: this.state.ResourceDetails.Color,
      Quantity: this.state.ResourceDetails.Quantity,
      Description:this.state.ResourceDetails.Description
    };
    const { cardStyle } = styles;
    const { updateButton } = styles;
    const { textBorderStyle } = styles;
    return (
      <KeyboardAwareScrollView>
        <Container>
          <Content>
            <LogoutButton navigation={this.props.navigation} />
            <Card style={cardStyle}>
                <Text style={textBorderStyle}>
                {this.state.ResourceDetails.ResourceName}
                </Text>
              <Form
                ref={c => this._form = c}
                type={ResourceDetails}
                options={options}
                value={values}
              />
              <CardItem>
                <Button success onPress={this.updateFunction}
                  style={updateButton}>
                  <Text>
                    UPDATE
                    </Text>
                </Button>
              </CardItem>
            </Card>
          </Content>
        </Container>
      </KeyboardAwareScrollView>
    );
  }
}

const styles = StyleSheet.create({
  cardStyle: {
    marginLeft: 10,
    marginRight: 10,
    marginBottom: 30,
    padding: 20,
    backgroundColor: '#ffffff'
  },
  updateButton: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center'
  },
  textBorderStyle: {
    marginRight: 20,
    marginLeft: 20,
    marginTop: 10,
    marginBottom: 10,
    paddingTop: 5,
    paddingBottom: 5,
    borderRadius: 10,
    borderWidth: 1,
    borderColor: '#909090',
    color: '#909090',
    textAlign: 'center',
}
});
