import * as React from 'react';
import macroCss from 'app/style/global.macros.module.css';
import { Route, Link, Switch } from 'react-router-dom';

// example stauff (to be moved at some point)
import { RouterDemo } from 'app/dev/examples/RouterDemo';
import { TemplNotes } from 'app/dev/examples/TemplNotes';
import { ImageExample } from 'app/dev/examples/ImageExample';
import { StylingLibsDemo } from 'app/dev/examples/StylingLibsDemo';
import { PrimeReactDemo } from 'app/dev/examples/PrimeReactDemo';

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
                </ul>
            </nav>
            <div className={macroCss.solidBox + ' ' + macroCss.p10}>
                <Switch>
                    <Route path={preRoute + 'router'} component={RouterDemoWrapper}></Route>
                    <Route path={preRoute + 'images'} component={ImageExample}></Route>
                    <Route path={preRoute + 'styling'} component={StylingLibsDemo}></Route>
                    <Route path={preRoute + 'primeReact'} component={PrimeReactDemo}></Route>
                    <Route path={preRoute + 'notes'} exact component={TemplNotes}></Route>
                </Switch>
            </div>
        </div>
    );
};

export default ExamplesPage as React.FC;
