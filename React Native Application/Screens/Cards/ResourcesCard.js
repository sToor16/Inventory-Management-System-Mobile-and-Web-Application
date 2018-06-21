import React, { Component } from 'react';
import { TextInput } from 'react-native';
import { Button, Container, Content, Card, CardItem, Text, Right, Icon } from 'native-base';
import { TouchableOpacity } from 'react-native';
//import ResourcesCard from './Cards/ResourcesCard'

export default class ResourcesCard extends Component {
    constructor(props) {
        super(props);
        this.state = { ResourceName: '' };
    }
    componentWillMount() {
    }
    render() {

        const { ResourceId, ResourceName, Size, Color, Quantity, Description } = this.props.facility;
        const { resourcesMainCardStyle } = styles;
        const { textBorderStyle } = styles;
        return (
                    <TouchableOpacity button onPress={() => {
                    this.props.navigation.navigate('SelectedResourceCard', {
                        ResourceId: ResourceId,
                        ResourceName: ResourceName,
                        Size: Size,
                        Color: Color,
                        Quantity: Quantity,
                        Description: Description
                    });
                }
                }>
                    <Card style={resourcesMainCardStyle}>
                        <Text style={textBorderStyle}>
                            {ResourceName}
                        </Text>
                    </Card>
                </TouchableOpacity>
        );
    }
};
const styles = {
    facilityMainCardStyle: {
        marginTop: 20,
        marginBottom: 20,
        marginLeft: 10,
        marginRight: 10,
        padding: 20
    },
    textBorderStyle: {
        marginRight: 40,
        marginLeft: 40,
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
};