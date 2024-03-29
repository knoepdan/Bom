import * as React from 'react';

import css from 'style/cssClasses';
import samplePng from './img/samplePng.png';
import sampleJpg from './img/sampleJpg.jpg';
import sampleGif from './img/sampleGif.gif';
import sampleSvg from './img/sampleSvg.svg';

const imgStyle = {
    maxHeight: '40px',
    border: '5px solid pink',
};

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const ImageExample = (): React.ReactElement<Props> => {
    return (
        <div>
            <div className={css('dottedBox') + ' ' + css('p5')}>
                <img src={samplePng} alt="samplePng" />
                <img src={sampleJpg} alt="sampleJpg" />
                <img src={sampleGif} alt="sampleGif" style={{ maxHeight: 50 }} />
                <img src={sampleSvg} alt="sampleSvg" style={imgStyle} />
            </div>
            <div className="dottedBoxBlaWillNotWorkBecauseCssIsCompiledAndNamesAreChanged"></div>
        </div>
    );
};
