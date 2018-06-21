import React, { Component } from 'react';
import { Text } from "react-native";
import { Container, Button, Content, Card, CardItem } from 'native-base';
import { TouchableOpacity } from 'react-native';

export default class FacilityCard extends Component {
    constructor(props) {
        super(props);
    }
    componentWillMount() {
    }
    render() {

        const { FacilityId, Name, FacilityName, Address, Description,
            City, Landmark, State, ZipCode, Facility_FacilityId } = this.props.facility;
        const { facilityMainCardStyle } = styles;
        const { facilityNameStyle } = styles;
        const { textBorderStyle } = styles;
        return (
            <Content>
                <TouchableOpacity button onPress={() => {
                    this.props.navigation.navigate('ResourcesScreen', {
                        Facility_FacilityId: Facility_FacilityId
                    })
                }
                }>
                    <Card style={facilityMainCardStyle}>
                        <Text style={facilityNameStyle}>
                            {FacilityName}
                        </Text>
                        <Text style={textBorderStyle}>
                            ADDRESS: {Address}
                        </Text>
                        <Text style={textBorderStyle}>
                            Description: {Description}
                        </Text>
                        <Text style={textBorderStyle}>
                            City: {City}
                        </Text>
                        <Text style={textBorderStyle}>
                            Landmark: {Landmark}
                        </Text>
                        <Text style={textBorderStyle}>
                            State: {State}
                        </Text>
                        <Text style={textBorderStyle}>
                            ZipCode: {ZipCode} 
                        </Text>
                    </Card>
                </TouchableOpacity>
            </Content>
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
    facilityNameStyle: {
        marginRight: 20,
        marginLeft: 20,
        marginTop: 5,
        paddingTop: 5,
        paddingBottom: 5,
        backgroundColor:'#909090',
        borderRadius: 10,
        borderWidth: 1,
        borderColor: '#909090',
        color: '#ffffff',
        fontSize: 20,
        textAlign: 'center',
    },
    textBorderStyle: {
        marginRight: 40,
        marginLeft: 40,
        marginTop: 10,
        paddingTop: 5,
        paddingBottom: 5,
        borderRadius: 10,
        borderWidth: 1,
        borderColor: '#909090',
        color: '#909090',
        textAlign: 'center',
    }
};