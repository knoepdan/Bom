import * as React from 'react';

import macroCss from 'app/style/global.macros.module.css';
import samplePng from 'app/img/samplePng.png';
import sampleJpg from 'app/img/sampleJpg.jpg';
import sampleGif from 'app/img/sampleGif.gif';
import sampleSvg from 'app/img/sampleSvg.svg';

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
            <div className={macroCss.dottedBox + ' ' + macroCss.p5}>
                <img src={samplePng} alt="samplePng" />
                <img src={sampleJpg} alt="sampleJpg" />
                <img src={sampleGif} alt="sampleGif" style={{ maxHeight: 50 }} />
                <img src={sampleSvg} alt="sampleSvg" style={imgStyle} />
            </div>
            <div className="dottedBoxBlaWillNotWorkBecauseCssIsCompiledAndNamesAreChanged"></div>
        </div>
    );
};
