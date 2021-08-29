// app
//import { Config } from 'app/Config';

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { App } from 'app/App';

ReactDOM.render(<App />, document.getElementById('root'));
/*
Config.loadConfigFile(true)
    .then(() => {
        ReactDOM.render(<App />, document.getElementById('root'));
    })
    .catch((ex: any) => {
        alert('Error loading configuration: ' + ex.message);
        console.error('Error loading config: ' + ex.message);
    });
    */
