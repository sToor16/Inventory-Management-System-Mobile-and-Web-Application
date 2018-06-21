import React, { Component } from "react";
import { StyleSheet } from "react-native";
import t from 'tcomb-form-native';
import axios from "axios";
import { Card, Container, Content, CardItem, Button, Text } from 'native-base';
import { KeyboardAwareScrollView } from 'react-native-keyboard-aware-scroll-view';

const Form = t.form.Form;

const Resource = t.struct({
    USERNAME: t.String,
    PASSWORD: (t.String)
});

const formStyles = {
    ...Form.stylesheet,
    formGroup: {
        normal: {
            marginTop: 10,
            marginBottom: 10
        },
    },
    controlLabel: {
        normal: {
            color: '#909090',
            fontSize: 20,
            marginTop: 5,
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
        PASSWORD: {
            error: 'PASSWORD is Required.',
        },
    },
    stylesheet: formStyles,
};

export default class LoginScreen extends Component {
    loginFunction = () => {
        const value = this._form.getValue();
        if (value) {
            axios.post('https://dewmdy2eph.execute-api.us-east-1.amazonaws.com/latest/getPassword', {
                USERNAME: value['USERNAME'],
                PASSWORD: value['PASSWORD']
            })
                .then(response => {
                    try {
                        const Id = response.data.fetchId.recordset[0].Id;
                        if (Id) {
                            this.props.navigation.navigate('FacilitiesScreen', {
                                Id: Id
                            });
                        }
                    }
                    catch (err) {
                        alert("Wrong Credentials");
                    }
                })
                .catch(function (error) {
                    console.log(error);
                });
        }

    }
    constructor(props) {
        super(props);
        this.state = { USERNAME: 'Toornative@gmail.com', PASSWORD: 'password' };
    }

    render() {
        var values = {
            USERNAME: this.state.USERNAME,
            PASSWORD: this.state.PASSWORD
        };
        var { navigate } = this.props.navigation;
        const { loginCard } = styles;
        const { loginButton } = styles;
        return (
            <KeyboardAwareScrollView>
                <Container>
                    <Content>
                        <Card style={loginCard}>
                            <Form
                                ref={c => this._form = c}
                                type={Resource}
                                options={options}
                                value={values}
                            />
                            <CardItem>
                                <Button success onPress={this.loginFunction}
                                    style={loginButton}>
                                    <Text>
                                        LOGIN
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

const styles = {
    loginCard: {
        justifyContent: 'center',
        padding: 10,
        marginTop: 20,
        marginBottom: 20,
        marginLeft: 10,
        marginRight: 10,
    },
    loginButton: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center'
    }
}
