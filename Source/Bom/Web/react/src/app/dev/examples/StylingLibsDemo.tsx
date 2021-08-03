import * as React from 'react';
//import 'font-awesome/css/font';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCoffee } from '@fortawesome/free-solid-svg-icons';

export const StylingLibsDemo: React.FC = () => {
    return (
        <div>
            <div>
                <h2>Font awesome</h2>
                <div>
                    <FontAwesomeIcon icon={faCoffee} />
                </div>
                <div>
                    we use wrapper FontAwesomeIcon instead of accessesing css directly! (3 installs necessary)
                    <br />
                    <mark>
                        npm i --save @fortawesome/fontawesome-svg-core <br />
                        npm i --save @fortawesome/free-solid-svg-icons <br />
                        npm i --save @fortawesome/react-fontawesome <br />
                    </mark>
                    see: https://github.com/FortAwesome/react-fontawesome
                </div>
            </div>
        </div>
    );
};

/*
Other approaches: 
- using react-fontawesome: 
    <FontAwesomeIcon icon="coffee" />  (should be possible to make something like this work)

- without using react-fontawesome 
-- include via css modules (should work but we would have to change the of the classes with '-'. 2.5.2020 tried only very quickly and didnt get it to work§1    nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnadfs************-----------)
 <i className={awCss.fa + ' ' + awCss.faSpinner + ' ' + awCss.faSpin}>no spinner but why</i> <br >
-- reference via normal string: should work but then a css reference is necessary (not possible with css modules)
<i className="fa fa-spinner fa-spin">no spinner but why</i> <br />

*/
