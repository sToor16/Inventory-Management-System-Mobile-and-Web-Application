import React, { Component, StyleSheet } from 'react';
import { Button, Text, Card, CardItem } from 'native-base';

export default class LogoutButton extends Component {
    logoutFunction = () => {
        this.props.navigation.navigate('LoginScreen')
    }
    componentWillMount() {
    }
    render() {
        const { logoutCard } = styles;
        return (
            <Card>
                <CardItem>
                    <Button rounded danger
                        style={logoutCard}
                        onPress={this.logoutFunction} >
                        <Text>LOGOUT</Text>
                    </Button>
                </CardItem>
            </Card>
        );
    }
};

const styles = {
    logoutCard: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center'
    }
}
