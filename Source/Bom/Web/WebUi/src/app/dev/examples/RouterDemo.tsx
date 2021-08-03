import * as React from 'react';
import { Route, Link } from 'react-router-dom';
import { RouterDemoDetails } from 'app/dev/examples/RouterDemoDetails';
import css from './RouterDemo.module.css';

export interface RouterDemoProps {
    compiler: string;
    framework: string;
    bundler: string;
}
export const RouterDemo: React.FC<RouterDemoProps> = (props: RouterDemoProps) => {
    const preRoute = '/dev/examples/router';
    return (
        <div>
            <span className={css.hello}>
                This is a {props.framework} application using {props.compiler} with {props.bundler}{' '}
                (process.env.NODE_ENV: {process.env.NODE_ENV}). And it showcases some basic router behavior.
            </span>
            <div>
                <Route path={preRoute + '/:testid'} exact component={RouterDemoDetails}></Route>

                <ul>
                    <li>
                        <Link to={preRoute + '/3'}>
                            {' '}
                            Details {'"'}3{'"'}
                        </Link>
                    </li>
                    <li>
                        <Link to={preRoute + '/xx'}>
                            {' '}
                            Details {'"'}xx{'"'}
                        </Link>
                    </li>
                    <li>
                        <Link to={preRoute + '/999'}>
                            {' '}
                            Details {'"'}999{'"'}
                        </Link>
                    </li>
                </ul>
            </div>
            <div>
                React router and Typescript (sometimes be painful)
                <br />
                For example: <br />
                <ul>
                    <li>
                        Passing props (rarely needed. Often global state or passing url param is the better options):{' '}
                        <br />
                        Easiest solution I have found: WrapperComponent (as in this example, couldnt get typing to work)
                    </li>
                    <li>Passing url params: see example here or some links</li>
                    <li>
                        <mark>
                            For routing to work, server must always load the react application. (out of the box, this
                            wont work in IIS for example as you might get a 404 because IIS expects a file at that
                            route)
                        </mark>
                    </li>
                    <li>
                        Detail link might not support browser refresh in dev mode (depending on settings)
                        <br />
                        https://stackoverflow.com/questions/29718481/unexpected-token-error-in-react-router-component
                    </li>
                </ul>
                Some react-router links:
                <ul>
                    <li>https://itnext.io/a-react-router-from-scratch-in-typescript-f0eec6ccb293</li>
                    <li>
                        https://medium.com/@robjtede/the-real-routewithprops-react-component-in-typescript-defacde01991
                    </li>
                    <li>https://programming.vip/docs/using-typescript-to-write-react-router-5.html</li>
                    <li>https://stackoverflow.com/questions/44118060/react-router-dom-with-typescript</li>
                    <li>https://www.youtube.com/watch?v=3pS-oRn9LTk</li>
                </ul>
            </div>
        </div>
    );
};

RouterDemo.defaultProps = {
    // if not passed, these values apply
    compiler: 'COMPILER NOT PASSED ON PROPS',
    framework: 'FRAMEWORK NOT PASSED ON PROPS',
    bundler: 'BUNDLER NOT PASSED ON PROPS',
};
