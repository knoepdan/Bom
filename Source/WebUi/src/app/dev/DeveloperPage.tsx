import * as React from 'react';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const DeveloperPage = (): React.ReactElement<Props> => {
    return (
        <div>
            Developer page
            <div>
                <div>
                    Routing and loading does not yet work. <br />- with baseUrl: navigation routes probably have to be
                    without "/" (check for side effects) <br />- clicking on same route should not lead to a route that
                    gets bigger and bigger!!!
                    <br />- clicking on browser link should not kill the base part: http://localhost/BomUi/ url should
                    NOT become: http://localhost/main (but http://localhost/BomUi/main)
                    <br />
                    <br />
                    Tests with base url set ("../image.png" or "images/bla.png" are relative. urls starting with slashes
                    are absolute paths)
                    <ul>
                        <li>
                            router url: "main/entry" url becomes bigger with each click :
                            "http://localhost/BomUi/main/main/main/main/main/entry"
                        </li>
                        <li>
                            router url: "./main/entry" url becomes bigger with each click :
                            "http://localhost/BomUi/main/main/main/main/main/entry"
                        </li>
                        <li>
                            router url: "/main/entry" url removes base tag part : "http://localhost/main/entry" (but
                            does not become bigger) (without BomUI from base tag)
                        </li>
                    </ul>
                </div>
                Steps to do:
                <ul>
                    <li>
                        Have 3 areas: main (welcom, and home, tree search), admin (just a stub), dev (move example code)
                        (lazy load per area)
                    </li>
                    <li>
                        externalize routing (similar to profile tool). Allow: dynamic, right checks, add sub navi per
                        area (sub navi is only defined)
                    </li>
                    <li>
                        File structure something like this:
                        <br />
                        area: subfolder utils (maybe also further subdivision possible)
                        <br />
                        utils (anything that has no business logic at all, could be reused in other projects
                        <br />
                        common: subdivision: api (generated, caching, maybe utils also goes there...
                        <br />
                        <br />
                        (maybe we have to rearrange a bit after.. but initially we should start with division by area)
                    </li>
                    <li>implement basic navigation (fill with stubs)</li>
                    <li>
                        make navigation responsive and also have some basic css for responsive desing (maybe also use
                        css flexbox and cols..)
                    </li>
                    <li>
                        decide next steps.. (maybe include api, wrap icons (not scatter code with dependencies to font
                        awesome) etc etc.)
                    </li>
                </ul>
            </div>
        </div>
    );
};

export default DeveloperPage as React.FC;
