import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { App } from './App';
import ErrorBoundary from 'common/react/ErrorBoundary';

ReactDOM.render(
    <ErrorBoundary>
        <App />
    </ErrorBoundary>,
    document.getElementById('root'),
);
