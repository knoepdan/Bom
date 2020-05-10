// include css in top component in correct order (to mimick a global css)
// styles have to be applied value. Example:  className={macroCss.pt2}
import macroCss from 'app/style/global.macros.module.css';
//import defaultCss from 'app/style/global.module.css';

// app
import * as React from 'react';
import { BrowserRouter, Route, Link, Switch } from 'react-router-dom';

// main stuff
import { Login } from 'app/Login';
import * as userState from 'app/common/UserState';
import * as nav from 'app/common/NavigationState';

// example stauff (to be moved at some point)
import { RouterDemo } from 'app/dev/components/RouterDemo';
import { TemplNotes } from 'app/dev/components/TemplNotes';
import { ImageExample } from 'app/dev/components/ImageExample';
import { StylingLibsDemo } from 'app/dev/components/StylingLibsDemo';
import { PrimeReactDemo } from 'app/dev/components/PrimeReactDemo';
/*
import { ReactHooksExample } from 'app/components/ReactHooksExample';
import { ReactWrapperExample } from 'app/components/ReactWrapperExample';
import { AsyncLoadEx } from './app/components/AsyncLoadEx'; 
*/

export const RouterDemoWrapper: React.FC = () => {
    // to get the typings right with react-router and props (here framework/bundler/we) it is probably easiest to just write a wrapper
    return <RouterDemo framework="react" bundler="webpack" compiler="typescript"></RouterDemo>;
};

// Info regarding react-rounter
// - <Rout path="/" exact  -> without exact the path matches with a startwith logic. So normally, path="/" is used with exact.
// - <Switch - ensures only one component (the first one matching the path ) is returned
export const App = (): React.ReactElement => {
    console.log('App rendered');
    const userStateRef = userState.userStateRef.useState();
    const navModel = nav.getNavigation(userStateRef.value);

    //------------

    const areaNav = (topNav: nav.MenuItem): React.ReactElement => {
        let subLink = (subNav: nav.MenuItem): React.ReactElement => {
            if (subNav.route && subNav.route.getRoute()) {
                return (
                    <li>
                        <Link to={subNav.route.getRoute()}>
                            {subNav.label} ( {subNav.route.getRoute()} )
                        </Link>
                    </li>
                );
            } else {
                return <li>{subNav.label}</li>;
            }
        };
        let childNav = (
            <ul>
                {topNav.children.map((childMenu, index) => (
                    <li>{subLink(childMenu)}</li>
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

    return (
        <div className={macroCss.p2}>
            <div>
                <Login></Login>
            </div>

            <div>
                <BrowserRouter>
                    <nav>
                        <ul>
                            {navModel.topMenues.map((topNav, index) => (
                                <li>{areaNav(topNav)}</li>
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

                    <nav>
                        <ul>
                            <li>
                                <Link to="/router">Hello React-Router</Link>
                            </li>
                            <li>
                                <Link to="/images">Images</Link>
                            </li>
                            <li>
                                <Link to="/styling">StylingLibsDemo</Link>
                            </li>
                            <li>
                                <Link to="/primeReact">PrimeReact</Link>
                            </li>
                            <li>
                                <Link to="/">Notes</Link>
                            </li>
                        </ul>
                    </nav>
                    <div className={macroCss.solidBox + ' ' + macroCss.p10}>
                        <Switch>
                            <Route path="/router" component={RouterDemoWrapper}></Route>
                            <Route path="/images" component={ImageExample}></Route>
                            <Route path="/styling" component={StylingLibsDemo}></Route>
                            <Route path="/primeReact" component={PrimeReactDemo}></Route>
                            <Route path="/" exact component={TemplNotes}></Route>
                        </Switch>
                    </div>
                </BrowserRouter>
            </div>

            <div>
                Steps to do:
                <ul>
                    <li>
                        Have 3 areas: main (welcom, and home, tree search), admin (just a stub), dev (move example code)
                        (lazy load per area)
                    </li>
                    <li>
                        externalize routing (similar to profile tool). Allow: dynamic, right checks, add sub navi per
                        area (sub navi is only defined)
                    </li>
                    <li>
                        File structure something like this:
                        <br />
                        area -> subfolder utils (maybe also further subdivision possible)
                        <br />
                        utils (anything that has no business logic at all, could be reused in other projects
                        <br />
                        common: subdivision: api (generated, caching, maybe utils also goes there...
                        <br />
                        <br />
                        (maybe we have to rearrange a bit after.. but initially we should start with division by area)
                    </li>
                    <li>implement basic navigation (fill with stubs)</li>
                    <li>
                        make navigation responsive and also have some basic css for responsive desing (maybe also use
                        css flexbox and cols..)
                    </li>
                    <li>
                        decide next steps.. (maybe include api, wrap icons (not scatter code with dependencies to font
                        awesome) etc etc.)
                    </li>
                </ul>
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
