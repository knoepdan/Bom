// include css in top component in correct order (to mimick a global css)
// styles have to be applied value. Example:  className={macroCss.pt2}
import normCss from 'app/style/global.normalize.module.css';
import macroCss from 'app/style/global.macros.module.css';
import defaultCss from 'app/style/global.module.css';

// app
import { Config } from 'app/utils/Config';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { BrowserRouter, Route, Link, Switch } from 'react-router-dom';

import { RouterDemo } from 'app/components/RouterDemo';
import { TemplNotes } from './app/components/TemplNotes';
import { ImageExample } from 'app/components/ImageExample';
import { StylingLibsDemo } from 'app/components/StylingLibsDemo';
import { PrimeReactDemo } from 'app/components/PrimeReactDemo';
/*
import { ReactHooksExample } from 'app/components/ReactHooksExample';
import { ReactWrapperExample } from 'app/components/ReactWrapperExample';
import { AsyncLoadEx } from './app/components/AsyncLoadEx'; 
*/

// hack to ensure css that is to be global is loaded and no type errors
if (false && normCss && macroCss && defaultCss) {
    console.log('');
}

export const RouterDemoWrapper: React.FC = () => {
    // to get the typings right with react-router and props (here framework/bundler/we) it is probably easiest to just write a wrapper
    return <RouterDemo framework="react" bundler="webpack" compiler="typescript"></RouterDemo>;
};

Config.loadConfigFile(true)
    .then((r) => {
        // Info regarding react-rounter
        // - <Rout path="/" exact  -> without exact the path matches with a startwith logic. So normally, path="/" is used with exact.
        // - <Switch - ensures only one component (the first one matching the path ) is returned
        ReactDOM.render(
            <div className={macroCss.p2}>
                <BrowserRouter>
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
            </div>,
            document.getElementById('root'),
        );
    })
    .catch((ex) => {
        alert('Error loading configuration: ' + ex.message);
        console.error('Error loading config: ' + ex.message);
    });
