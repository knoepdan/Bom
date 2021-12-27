import * as React from 'react';
import Config from 'app/Config';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const TemplNotes = (): React.ReactElement<Props> => {
    return (
        <div>
            <br />
            <br />
            <h1>Notes about webpack setup, react, this template (a bit random)</h1>
            <h3>Source maps</h3>
            <ul>
                <li>Whether source maps are generated or not depends on webpack config</li>
                <li>Normally different configurations for css sourcemaps and js</li>
                <li>
                    Normally, no source maps in build (production), but sourcemaps during development (can be changed in
                    webpack)
                </li>
                <li>
                    when not working: check webpack.config or
                    https://blog.sentry.io/2018/10/18/4-reasons-why-your-source-maps-are-broken
                </li>
            </ul>
            <h3>CSS</h3>
            <ul>
                <li>Global css can be achieved via css modules</li>
                <li>- global css via css modules: see example in this template</li>
                <li>
                    - almost certainly other ways to have global templates (at some point necessary when integrating
                    third party lib, but for now its good to only have one approach)
                </li>
            </ul>
            <h3>Configuration</h3>
            <ul>
                <li>
                    Approach 1: load configuration via external call. See approach in this template. Corner points and
                    stuff to keep in mind:
                    <ul>
                        <li>
                            Configuration file must be served in dev and prod (for production: file is to be copied)
                        </li>
                        <li>
                            When accessing the configuration, it must be ensured that file is loaded. Can be done prior
                            to startig react or upon usage
                        </li>
                        <li>
                            Can be eaysily saved to session storage, so even when using mulitple tabs, only one call to
                            config is made
                        </li>
                    </ul>
                    Testentry: {Config.TestEntry}
                </li>
                <li>
                    Approach 2: external config file as global variable. See:
                    https://stackoverflow.com/questions/38272128/how-to-load-an-external-config-file-in-a-webpack-react-application-without-bundl
                    Corner points and stuff to keep in mind:
                    <ul>
                        <li>
                            Configuration file must be served in dev and prod (for production: file is to be copied)
                        </li>
                        <li>Html file must load it, will load each time (session will not work then)</li>
                    </ul>
                </li>
                <li>
                    Approach 3: bundle configuration via webpack and environment variable. example:
                    http://www.matjazev.net/blog/2018/03/01/using-vue-spa-created-with-webpack-in-subfolder/
                    Disadvantage: not suited when for every usecase because once built, configuration cannot be adapted
                </li>
            </ul>
            <h3>Scalability</h3>
            <ul>
                <li>
                    Lazy load code (typescript code):
                    <br />
                    https://reactjs.org/docs/code-splitting.html <br />
                    https://stackoverflow.com/questions/54150819/using-react-lazy-with-typescript
                </li>
                <li>
                    External (webpack configuration) Scripts from other sources (example:
                    https://cdnjs.cloudflare.com/someScript.js), use externals:
                    https://symfonycasts.com/screencast/webpack-encore/external-libs
                </li>
                <li>
                    Other ideas (not conclusive): <br />
                    - lazy load css (maybe could just loaded dynamically)
                    <br />
                    - IFrame to lazy load content
                    <br />
                    - render parts on the server (first call)
                    <br />
                    - maybe bundler also offers more options
                    <br />
                    - Caching <br />
                    -- app (with manifest etc.)
                    <br />
                    -- use browser cacher (to store data)
                    <br />
                    - Infrastructre (must sometimes be supported by software)
                    <br />
                    -- multiple servers to server data or scripts
                    <br />
                </li>
            </ul>
            <h3>Global variables (like $ in JQuery)</h3>
            <ul>
                <li>
                    in code. {'"'}import $ from {'"'}jquery{'"'};global.$ = $;{'"'}
                </li>
                <li>see: https://symfonycasts.com/screencast/webpack-encore/external-libs</li>
                <li>probably also possible to just store it to window object or similar (not tested)</li>
                <li>Not recommended.. </li>
            </ul>
        </div>
    );
};
