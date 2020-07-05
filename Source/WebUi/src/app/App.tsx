// include css in top component in correct order (to mimick a global css)
// styles have to be applied value. Example:  className={macroCss.pt2}
import css from 'app/style/global.module.css';
import macroCss from 'app/style/global.macros.module.css';

// app
import * as React from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';

// main stuff
import { TopBar } from 'app/layout/TopBar';
//import { LoginPage } from 'app/LoginPage';
import * as userState from 'app/common/UserState';
import * as nav from 'app/common/NavigationState';
import { SideNav } from './layout/SideNav';

// Info regarding react-rounter
// - <Rout path="/" exact  -> without exact the path matches with a startwith logic. So normally, path="/" is used with exact.
// - <Switch - ensures only one component (the first one matching the path ) is returned
export const App = (): React.ReactElement => {
    console.log('App rendered');
    const userStateRef = userState.userStateRef.useState();
    const navModel = nav.getNavigation(userStateRef.value);

    // Ensure navigation works (and references etc. in third party libs too, config call etc.)
    // -> we set basename in BrowserRouter. However, this depends on the environment.
    //    in deployed to application that is accesible via app name (localhost/appName) then it must be set
    //    However document.getElementsByTagName('base')[0].href; still returns full url even though we just want whats in base tag
    // see also: https://stackoverflow.com/questions/13832690/get-base-in-html-after-it-has-been-set-but-not-using-page-url
    let baseHref: string | undefined = undefined;
    const baseTag = document.getElementsByTagName('base');
    if (baseTag.length > 0) {
        baseHref = baseTag[0].dataset.href; // example base tag: <base href="/app" data-href="/app" />    (for dev "/" is normally ok)
    }
    return (
        <BrowserRouter basename={baseHref}>
            <TopBar />
            <SideNav />
            <div className={css.mainContent}>
                <div className={macroCss.p2}>
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
        </BrowserRouter>
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
