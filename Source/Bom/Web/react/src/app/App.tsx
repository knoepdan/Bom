import css from 'style/cssClasses';

// app
import * as React from 'react';
import { useState } from 'react';
import { BrowserRouter } from 'react-router-dom';
//import { BrowserRouter, Route, Switch } from 'react-router-dom';

// main stuff
import { TopBar } from 'app/layout/TopBar';
import { IAppContext, AppContext, useAppContext } from 'app/AppContext';
import { INotificationMessage, IUiContext, UiContext } from 'app/UiContext';
//import { LoginPage } from 'app/LoginPage';
//import Config from 'app/Config';
//import { Right } from 'app/common/Right';

//import * as userState from 'app/common/UserState';
//import * as nav from 'app/common/NavigationState';
import { SideNav } from './layout/SideNav';
//import { useState as globalState } from '@hookstate/core';

// Info regarding react-rounter
// - <Rout path="/" exact  -> without exact the path matches with a startwith logic. So normally, path="/" is used with exact.
// - <Switch - ensures only one component (the first one matching the path ) is returned
export const App = (): React.ReactElement => {
    let tempAppCon: IAppContext = useAppContext();
    const [appContext] = useState(tempAppCon);

    console.log('App rendered');
    //   const userStateRef = globalState(userState.userStateRef);

    /*
    // const handleLoginClick = (): void => {
    userStateRef.set((userModel) => {
        userModel.logIn(Config.Username);
        if (true) {
            userModel.permissions.push(Right.AdminArea);
            userModel.permissions.push(Right.DevArea);
        }
        return userModel;
    });
    */
    // };

    /*
    const navModel = nav.getNavigation(userStateRef.value);
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
*/

    // AppContext

    // UiContext
    const uiContext: IUiContext = {
        // TODO -> improve

        showNotification: (msg: INotificationMessage) => {
            if (msg && msg.text && msg.text != '') {
                alert(msg.text);
            }
        },
        showError: (e: unknown) => {
            alert('ERROR: ' + e);
        },
    };

    const routerBase = appContext.getRouterBase();
    return (
        <AppContext.Provider value={appContext}>
            <BrowserRouter basename={routerBase}>
                <UiContext.Provider value={uiContext}>
                    <TopBar />
                    <SideNav />
                    <div className={css('mainContent')}>
                        <div className={css('p2')}></div>
                    </div>
                </UiContext.Provider>
            </BrowserRouter>
        </AppContext.Provider>
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
