import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Hello } from 'app/components/Hello';
import { ReactHooksExample } from 'app/components/ReactHooksExample';
import { ReactWrapperExample } from 'app/components/ReactWrapperExample';

//import { createBrowserHistory } from 'history';
// import { configureStore } from 'app/store';
//import { Router } from 'react-router';
//import { App } from './app';

// prepare store
//const history = createBrowserHistory();
//const store = configureStore();

ReactDOM.render(
    <div>
        <Hello compiler="Typescript" framework="React..." bundler="Webpack" />
        <ReactHooksExample />
        <br />
        <ReactWrapperExample />
    </div>,
    document.getElementById('root'),
);
/*
ReactDOM.render(
  <Provider store={store}>
    <Router history={history}>
      <App />
    </Router>
  </Provider>,
  document.getElementById('root')
);
*/
