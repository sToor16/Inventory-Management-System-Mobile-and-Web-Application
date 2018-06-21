import React, { Component } from 'react';
import axios from "axios";
import { ScrollView } from "react-native";
import { Button, Text } from "native-base";
import FacilityCard from './Cards/FacilityCard';
import LogoutButton from './Button/LogoutButton';

class FacilitiesScreen extends Component {
    constructor(props) {
        super(props);
        this.state = { Id: this.props.navigation.state.params.Id, facilities: [] };
    }
    componentWillMount() {
        axios.post('https://dewmdy2eph.execute-api.us-east-1.amazonaws.com/latest/getFacilities', {
            Employee_Id: this.state.Id
        })
            .then(response => {
                this.setState({ facilities: response.data.recordset})
                if (response.data.rowsAffected[0] == 0) {
                    alert("You have not been assigned any facilities!")
                }
            });
    }
    fetchFacilities() {

        return this.state.facilities.map(facility =>
            <FacilityCard key={facility.Facility_FacilityId} facility={facility} navigation={this.props.navigation} />
        );
    }

    render() {
        navigationOptions: {
            title: 'MyScreen'
        }
        return (
            <ScrollView>
                <LogoutButton navigation={this.props.navigation}/>
                {this.fetchFacilities()}
            </ScrollView>
        );
    }
}
export default FacilitiesScreen;