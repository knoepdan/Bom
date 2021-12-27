import * as React from 'react';
import css from 'style/cssClasses';
import { Slider } from 'primereact/slider';

import { ColorPicker } from 'primereact/colorpicker';

import 'primereact/resources/primereact.min.css';
import 'primereact/resources/themes/nova-light/theme.css';
import 'primeicons/primeicons.css';

export const PrimeReactDemo = (): React.ReactElement => {
    return (
        <div>
            <h2>PrimeReact</h2>
            UI-Library: https://www.primefaces.org/primereact/ <br />
            how to get it running:
            <div>
                npm install primeract --save
                <br />
                npm install primeicons --save
                <br />
                npm install classnames --save
                <br /> <br />
                <div>
                    Styles not loaded
                    <ul>
                        <li>
                            It must be ensured that third party styles etc. are loaded: <br />
                            https://medium.com/@marzelin/using-css-modules-with-third-party-stylesheets-c9633ff759c1
                            (affects webpack.config)
                        </li>
                        <li>
                            https://stackoverflow.com/questions/48010671/use-primereact-themes-with-css-modules-enabled-in-react-application
                            (some other info)
                        </li>
                    </ul>
                </div>
            </div>
            <div>
                Examples (just a tiny excerpt): <br />
                <div className={css('p5')}>
                    Slider: <Slider value={50} style={{ maxWidth: '200px' }} />
                </div>
                <div className={css('p5')}>
                    ColorPicker: <ColorPicker />
                </div>
            </div>
        </div>
    );
};

export default PrimeReactDemo as React.FC;
