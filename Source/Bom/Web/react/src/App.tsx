// app
import * as React from 'react';
import { useState } from 'react';
import { BrowserRouter } from 'react-router-dom';

// main stuff
import { MainContainer } from 'app/layout/MainContainer';
import { IAppContext, AppContext, useAppContext } from 'app/AppContext';
import { INotificationMessage, IUiContext, UiContext } from 'app/UiContext';

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
                    <MainContainer />
                </UiContext.Provider>
            </BrowserRouter>
        </AppContext.Provider>
    );
};

export default App as React.FC;
