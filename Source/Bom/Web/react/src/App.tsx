import css from 'app/style/cssClasses';

// app
import * as React from 'react';
//import { BrowserRouter, Route, Switch } from 'react-router-dom';

import { Route, Switch } from 'react-router-dom';

// main stuff
import { TopBar } from 'app/layout/TopBar';
//import { LoginPage } from 'app/LoginPage';
import Config from 'app/Config';
import { Right } from 'app/common/Right';

import * as userState from 'app/common/UserState';
import * as nav from 'app/common/NavigationState';
import { SideNav } from 'app/layout/SideNav';
import { useState as globalState } from '@hookstate/core';

// Info regarding react-rounter
// - <Rout path="/" exact  -> without exact the path matches with a startwith logic. So normally, path="/" is used with exact.
// - <Switch - ensures only one component (the first one matching the path ) is returned
export const App = (): React.ReactElement => {
    console.log('App rendered');
    const userStateRef = globalState(userState.userStateRef);

    // const handleLoginClick = (): void => {
    userStateRef.set((userModel) => {
        userModel.logIn(Config.Username);
        if (true) {
            userModel.permissions.push(Right.AdminArea);
            userModel.permissions.push(Right.DevArea);
        }
        return userModel;
    });
    // };

    const navModel = nav.getNavigation(userStateRef.value);
    return (<>
            <TopBar />
            <SideNav />
            <div className={css('mainContent')}>
                <div className={css('p2')}>
                    <Switch>
                        {navModel.getRoutes().map((route, index) => (
                            <Route
                                key={index}
                                path={route.getRoute()}
                                exact={route.exact}
                                component={route.getComponent()}
                            />
                        ))}
                    </Switch>
                </div>
            </div>
</>
    );
};

export default App as React.FC;

/*
Config.loadConfigFile(true)
    .then((r) => {
        // Info regarding react-rounter
        // - <Rout path="/" exact  -> without exact the path matches with a startwith logic. So normally, path="/" is used with exact.
        // - <Switch - ensures only one component (the first one matching the path ) is returned
        ReactDOM.render(
            ,
            document.getElementById('root'),
        );
    })
    .catch((ex) => {
        alert('Error loading configuration: ' + ex.message);
        console.error('Error loading config: ' + ex.message);
    });
*/
