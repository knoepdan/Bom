import * as React from 'react';
import css from './SideNav.module.css';
import macroCss from 'app/style/global.macros.module.css';
import * as userState from 'app/common/UserState';
import * as nav from 'app/common/NavigationState';
import { Link } from 'react-router-dom';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

export const SideNavHtmlId = 'sideNav';

// more or less random notes about webpack setup
export const SideNav = (): React.ReactElement<Props> => {
    const userStateRef = userState.userStateRef.useState();
    const navModel = nav.getNavigation(userStateRef.value);

    return (
        <nav className={css.animateFromLeft + ' ' + css.sideNav} id={SideNavHtmlId}>
            <div className={macroCss.pl5}>
                <h5>Birdview</h5>
            </div>
            <hr />

            <ul>
                {navModel.topMenues.map((topNav, index) => (
                    <AreaNav topNav={topNav}></AreaNav>
                ))}
            </ul>
        </nav>
    );

    /* ## will look something like this ##
            <div className={macroCss.pl5}>
                <h5>Birdview</h5>
            </div>
            <hr />

            <h5 className={macroCss.pl5}>Main</h5>
            <ul>
                <li>Menu one</li>
                <li>Menu two</li>
                <li>Menu three</li>
            </ul>
*/
};

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface AreaNavProps {
    topNav: nav.MenuItem;
}

const AreaNav = (props: AreaNavProps): React.ReactElement<AreaNavProps> => {
    let subLink = (subNav: nav.MenuItem): React.ReactElement => {
        if (subNav.route && subNav.route.getRoute()) {
            return (
                <Link to={subNav.route.getRoute()}>
                    {subNav.label} ( {subNav.route.getRoute()} )
                </Link>
            );
        } else {
            return <span>{subNav.label}</span>;
        }
    };

    return (
        <div>
            <h5 className={macroCss.pl5}>{props.topNav.label}</h5>
            <ul>
                {props.topNav.children.map((childMenu, index) => (
                    <li key={index}>{subLink(childMenu)}</li>
                ))}
            </ul>
        </div>
    );
};

export default SideNav as React.FC;
