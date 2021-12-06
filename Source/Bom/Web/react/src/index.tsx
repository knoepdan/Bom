import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { App } from 'app/App';
import { BrowserRouter } from 'react-router-dom';

// Ensure navigation works (and references etc. in third party libs too, config call etc.)
// -> we set basename in BrowserRouter. However, this depends on the environment.
//    in deployed to application that is accesible via app name (localhost/appName) then it must be set
//    However document.getElementsByTagName('base')[0].href; still returns full url even though we just want whats in base tag
// see also: https://stackoverflow.com/questions/13832690/get-base-in-html-after-it-has-been-set-but-not-using-page-url
let baseHref: string | undefined = undefined;
const baseTag = document.getElementsByTagName('base');
if (baseTag.length > 0) {
    baseHref = baseTag[0].dataset.href; // example base tag: <base href="/app" data-href="/app" />    (for dev "/" is normally ok)
}

ReactDOM.render(
    <BrowserRouter basename={baseHref}>
        <App />
    </BrowserRouter>, document.getElementById('root'));
