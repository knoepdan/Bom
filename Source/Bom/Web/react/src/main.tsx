// include css in top component in correct order (to mimick a global css)
// styles have to be applied value. Example:  className={macroCss.pt2}
import normCss from 'app/style/global.normalize.module.css';
import macroCss from 'app/style/global.macros.module.css';
import defaultCss from 'app/style/global.module.css';

// app
import { Config } from 'app/Config';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { App } from 'app/App';

// hack to ensure css that is to be global is loaded and no type errors
if (false && normCss && macroCss && defaultCss) {
    console.log('');
}
Config.loadConfigFile(true)
    .then(() => {
        ReactDOM.render(<App />, document.getElementById('root'));
    })
    .catch((ex) => {
        alert('Error loading configuration: ' + ex.message);
        console.error('Error loading config: ' + ex.message);
    });
