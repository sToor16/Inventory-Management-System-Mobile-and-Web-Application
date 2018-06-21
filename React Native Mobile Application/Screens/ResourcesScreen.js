import React, { Component } from 'react';
import axios from "axios";
import { ScrollView, Text } from "react-native";
import ResourcesCard from './Cards/ResourcesCard';
import LogoutButton from './Button/LogoutButton';



class ResourcesScreen extends Component {
    constructor(props) {
        super(props);
        this.state = { Facility_FacilityId: this.props.navigation.state.params.Facility_FacilityId, facilities: []};
    }
        componentWillMount(){
            axios.post('https://dewmdy2eph.execute-api.us-east-1.amazonaws.com/latest/getResources',{
                FacilityId: this.state.Facility_FacilityId,
              })
                .then(response => {
                this.setState({facilities: response.data.recordset})
                if(response.data.rowsAffected[0] == 0) {
                    alert("No Resource has been assigned to this facility");
                }
                });
        }
        fetchResources() {
            
                return this.state.facilities.map(facility => 
                    <ResourcesCard key={facility.ResourceId} facility={ facility } navigation={this.props.navigation}/>
                );
    }
    
    render() {
        return (
            <ScrollView>
                <LogoutButton navigation={this.props.navigation}/>
                {this.fetchResources()}         
            </ScrollView>
        );
    }
}
export default ResourcesScreen;