// include css in top component in correct order (to mimick a global css)
// styles have to be applied value. Example:  className={macroCss.pt2}
import macroCss from 'app/style/global.macros.module.css';

// app
import * as React from 'react';
import { BrowserRouter, Route, Link, Switch } from 'react-router-dom';

// main stuff
import { Login } from 'app/Login';
import * as userState from 'app/common/UserState';
import * as nav from 'app/common/NavigationState';

// Info regarding react-rounter
// - <Rout path="/" exact  -> without exact the path matches with a startwith logic. So normally, path="/" is used with exact.
// - <Switch - ensures only one component (the first one matching the path ) is returned
export const App = (): React.ReactElement => {
    console.log('App rendered');
    const userStateRef = userState.userStateRef.useState();
    const navModel = nav.getNavigation(userStateRef.value);

    const areaNav = (topNav: nav.MenuItem): React.ReactElement => {
        let subLink = (subNav: nav.MenuItem): React.ReactElement => {
            if (subNav.route && subNav.route.getRoute()) {
                return (
                    <Link to={subNav.route.getRoute()}>
                        {subNav.label} ( {subNav.route.getRoute()} )
                    </Link>
                );
            } else {
                return <span>{subNav.label}</span>;
            }
        };
        let childNav = (
            <ul>
                {topNav.children.map((childMenu, index) => (
                    <li key={index}>{subLink(childMenu)}</li>
                ))}
            </ul>
        );
        //   <Route key={index} path={topNav.path} exact={route.exact} component={route.main} />
        return (
            <span>
                <h5>{topNav.label}</h5>
                {childNav}
            </span>
        );
    };

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
        <div className={macroCss.p2}>
            <div>
                <Login></Login>
            </div>

            <div>
                <BrowserRouter basename={baseHref}>
                    <nav>
                        <ul>
                            {navModel.topMenues.map((topNav, index) => (
                                <li key={index}>{areaNav(topNav)}</li>
                            ))}
                        </ul>
                    </nav>

                    <div>
                        <h5>Content-Area</h5>
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
                </BrowserRouter>
            </div>
        </div>
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
