import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Hello } from 'src/app/components/Hello';
import { ReactHooksExample } from 'src/app/components/ReactHooksExample';
import { ReactWrapperExample } from './components/ReactWrapperExample';
declare let module: any;

// <Hello compiler="Typescript" framework="React..." bundler="Webpack" />,
ReactDOM.render(
    <div>
    <Hello compiler="Typescript" framework="React..." bundler="Webpack" />
    <ReactHooksExample />
    <br/>
    <ReactWrapperExample />
    </div>,
    document.getElementById('root'),
);

if (module.hot) {
    module.hot.accept();
}