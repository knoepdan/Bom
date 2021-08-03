import * as React from 'react';
import macroCss from 'app/style/global.macros.module.css';
import { Route, Link, Switch } from 'react-router-dom';

// example stauff (to be moved at some point)
import { RouterDemo } from 'app/dev/examples/RouterDemo';
import { TemplNotes } from 'app/dev/examples/TemplNotes';
import { ImageExample } from 'app/dev/examples/ImageExample';
import { StylingLibsDemo } from 'app/dev/examples/StylingLibsDemo';
import { PrimeReactDemo } from 'app/dev/examples/PrimeReactDemo';
import { AsyncLoadEx } from 'app/dev/examples/AsyncLoadEx';
import { ReactHooksExample } from 'app/dev/examples/ReactHooksExample';
import { ReactWrapperExample } from 'app/dev/examples/ReactWrapperExample';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

export const RouterDemoWrapper: React.FC = () => {
    // to get the typings right with react-router and props (here framework/bundler/we) it is probably easiest to just write a wrapper
    return <RouterDemo framework="react" bundler="webpack" compiler="typescript"></RouterDemo>;
};

// more or less random notes about webpack setup
export const ExamplesPage = (): React.ReactElement<Props> => {
    const preRoute = '/dev/examples/';
    return (
        <div>
            Examples page <br />
            <nav>
                <ul>
                    <li>
                        <Link to={preRoute + 'router'}>Hello React-Router</Link>
                    </li>
                    <li>
                        <Link to={preRoute + 'images'}>Images</Link>
                    </li>
                    <li>
                        <Link to={preRoute + 'styling'}>StylingLibsDemo</Link>
                    </li>
                    <li>
                        <Link to={preRoute + 'primeReact'}>PrimeReact</Link>
                    </li>
                    <li>
                        <Link to={preRoute + 'notes'}>Notes</Link>
                    </li>

                    <li>
                        <Link to={preRoute + 'async'}>Async Examples</Link>
                    </li>
                    <li>
                        <Link to={preRoute + 'reacthooks'}>React Hooks Examples</Link>
                    </li>
                    <li>
                        <Link to={preRoute + 'reacthooksWrapper'}>REact Hooks with Wrapper Examples</Link>
                    </li>
                </ul>
            </nav>
            <div className={macroCss.solidBox + ' ' + macroCss.p10}>
                <Switch>
                    <Route path={preRoute + 'router'} component={RouterDemoWrapper}></Route>
                    <Route path={preRoute + 'images'} component={ImageExample}></Route>
                    <Route path={preRoute + 'styling'} component={StylingLibsDemo}></Route>
                    <Route path={preRoute + 'primeReact'} component={PrimeReactDemo}></Route>
                    <Route path={preRoute + 'notes'} exact component={TemplNotes}></Route>
                    <Route path={preRoute + 'async'} exact component={AsyncLoadEx}></Route>
                    <Route path={preRoute + 'reacthooks'} exact component={ReactHooksExample}></Route>
                    <Route path={preRoute + 'reacthooksWrapper'} exact component={ReactWrapperExample}></Route>
                </Switch>
            </div>
        </div>
    );
};

export default ExamplesPage as React.FC;
