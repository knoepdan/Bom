import * as React from 'react';
import css from 'style/cssClasses';
import { Route, Link, Routes } from 'react-router-dom';

//import { Link } from 'react-router-dom';

// example stauff (to be moved at some point)
import { RouterDemo } from './examples/RouterDemo';
import { TemplNotes } from './examples/TemplNotes';
import { ImageExample } from './examples/ImageExample';
import { StylingLibsDemo } from './examples/StylingLibsDemo';
import { PrimeReactDemo } from './examples/PrimeReactDemo';
import { AsyncLoadEx } from './examples/AsyncLoadEx';
import { ReactHooksExample } from './examples/ReactHooksExample';
import { ReactWrapperExample } from './examples/ReactWrapperExample';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

export const RouterDemoWrapper: React.FC = () => {
    // to get the typings right with react-router and props (here framework/bundler/we) it is probably easiest to just write a wrapper
    return <RouterDemo framework="react" bundler="webpack" compiler="typescript"></RouterDemo>;
};

// more or less random notes about webpack setup
export const ExamplesPage = (): React.ReactElement<Props> => {
    const preRouteRelative = './';
    //const preRoute = '/dev/examples/';
    return (
        <div>
            Examples page <br />
            <nav>
                <ul>
                    <li>
                        <Link to={preRouteRelative + 'router'}>Hello React-Router</Link>
                    </li>
                    <li>
                        <Link to={preRouteRelative + 'images'}>Images</Link>
                    </li>
                    <li>
                        <Link to={preRouteRelative + 'styling'}>StylingLibsDemo</Link>
                    </li>
                    <li>
                        <Link to={preRouteRelative + 'primeReact'}>PrimeReact</Link>
                    </li>
                    <li>
                        <Link to={preRouteRelative + 'notes'}>Notes</Link>
                    </li>

                    <li>
                        <Link to={preRouteRelative + 'async'}>Async Examples</Link>
                    </li>
                    <li>
                        <Link to={preRouteRelative + 'reacthooks'}>React Hooks Examples</Link>
                    </li>
                    <li>
                        <Link to={preRouteRelative + 'reacthooksWrapper'}>REact Hooks with Wrapper Examples</Link>
                    </li>
                </ul>
            </nav>
            <div className={css('solidBox') + ' ' + css('p10')}>
                <Routes>
                    <Route path="/" element={<div>aaaaaaaaaaaaaaaaaaaaaaa</div>}>
                        <Route path="dev" element={<div>bbbbbbbbbbbbbbb</div>}>
                            <Route path="examples" element={<div>ccccccccccc</div>}>
                                <Route path="router" element={<RouterDemoWrapper />}></Route>
                                <Route path="images" element={<ImageExample />} />
                                <Route path="styling" element={<StylingLibsDemo />} />
                                <Route path="primeReact" element={<PrimeReactDemo />}></Route>
                                <Route path="notes" element={<TemplNotes />}></Route>
                                <Route path="async" element={<AsyncLoadEx />}></Route>
                                <Route path="reacthooks" element={<ReactHooksExample />}></Route>
                                <Route path="reacthooksWrapper" element={<ReactWrapperExample />}></Route>
                                <Route path="*" element={<div>NOOO MATCH</div>}></Route>
                            </Route>
                        </Route>
                    </Route>
                </Routes>
            </div>
            <div className={css('solidBox') + ' ' + css('p10')}>
                <Routes>
                    <Route path="/" element={<div>aaaaaaaaaaaaaaaaaaaaaaa2</div>}>
                        <Route path="*" element={<div>path seems to be not ok</div>}></Route>
                        <Route path="dev" element={<div>bbbbbbbbbbbbbbb</div>}>
                            <Route path="examples" element={<div>ccccccccccc</div>}>
                                <Route path="router" element={<RouterDemoWrapper />}></Route>
                                <Route path="images" element={<ImageExample />} />
                                <Route path="styling" element={<StylingLibsDemo />} />
                                <Route path="primeReact" element={<PrimeReactDemo />}></Route>
                                <Route path="notes" element={<TemplNotes />}></Route>
                                <Route path="async" element={<AsyncLoadEx />}></Route>
                                <Route path="reacthooks" element={<ReactHooksExample />}></Route>
                                <Route path="reacthooksWrapper" element={<ReactWrapperExample />}></Route>
                            </Route>
                        </Route>
                    </Route>
                </Routes>
            </div>
            <div className={css('solidBox') + ' ' + css('p10')}>
                <Routes>
                    <Route path="/" element={<div>aaaaaaaaaaaaaaaaaaaaaa333333333</div>}>
                        <Route path="*" element={<div>path seems to be not ok</div>}></Route>
                        <Route path="dev" element={<div>bbbbbbbbbbbbbbb</div>}>
                            <Route path="examples" element={<div>ccccccccccc</div>}>
                                <Route path="router" element={<RouterDemoWrapper />}></Route>
                                <Route path="images" element={<ImageExample />} />
                                <Route path="styling" element={<StylingLibsDemo />} />
                                <Route path="primeReact" element={<PrimeReactDemo />}></Route>
                                <Route path="notes" element={<TemplNotes />}></Route>
                                <Route path="async" element={<AsyncLoadEx />}></Route>
                                <Route path="reacthooks" element={<ReactHooksExample />}></Route>
                                <Route path="reacthooksWrapper" element={<ReactWrapperExample />}></Route>
                            </Route>
                        </Route>
                    </Route>
                </Routes>
            </div>
        </div>
    );
};

export default ExamplesPage as React.FC;
