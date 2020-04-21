import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Hello } from 'app/components/Hello';
import { ReactHooksExample } from 'app/components/ReactHooksExample';
import { ReactWrapperExample } from 'app/components/ReactWrapperExample';

import samplePng from 'app/img/samplePng.png';
import sampleJpg from 'app/img/sampleJpg.jpg';
import sampleGif from 'app/img/sampleGif.gif';
import sampleSvg from 'app/img/sampleSvg.svg';

const imgStyle = {
    maxHeight: '40px',
    border: '5px solid pink',
};

ReactDOM.render(
    <div>
        <Hello compiler="Typescript" framework="React..." bundler="Webpack" />
        <ReactHooksExample />
        <br />
        <ReactWrapperExample />
        <br />
        <br />
        <div>
            <img src={samplePng} alt="samplePng" />
            <img src={sampleJpg} alt="sampleJpg" />
            <img src={sampleGif} alt="sampleGif" style={{ maxHeight: 50 }} />
            <img src={sampleSvg} alt="sampleSvg" style={imgStyle} />
        </div>
    </div>,
    document.getElementById('root'),
);
