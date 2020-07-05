import * as React from 'react';
import css from './SideNav.module.css';
import macroCss from 'app/style/global.macros.module.css';
import * as userState from 'app/common/UserState';
import * as nav from 'app/common/NavigationState';
import { NavLink } from 'react-router-dom';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

export const SideNavHtmlId = 'sideNav';

// more or less random notes about webpack setup
export const SideNav = (): React.ReactElement<Props> => {
    const userStateRef = userState.userStateRef.useState();
    const navModel = nav.getNavigation(userStateRef.value);

    return (
        <nav className={css.animateFromLeft + ' ' + css.sideNav} id={SideNavHtmlId}>
            <div className={macroCss.pl10}>
                <h1>Birdview</h1>
            </div>
            <hr />

            <ul>
                {navModel.topMenues.map((topNav, index) => (
                    <li key={index} className={macroCss.p10}>
                        <AreaNav topNav={topNav}></AreaNav>
                    </li>
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
    const subLink = (subNav: nav.MenuItem): React.ReactElement => {
        if (subNav.route && subNav.route.getRoute()) {
            return (
                <NavLink to={subNav.route.getRoute()} activeClassName={css.navActive}>
                    {subNav.label} ( {subNav.route.getRoute()} )
                </NavLink>
            );
        } else {
            return <span>{subNav.label}</span>;
        }
    };

    const onLiClick = (e: React.MouseEvent<HTMLLIElement, MouseEvent>) => {
        e.stopPropagation();
        const el: any = e.target;
        const l: HTMLLinkElement = el.querySelector('a');
        if (l) {
            l.click();
        }
        return false;
    };

    return (
        <div className={css.navArea}>
            <h2>{props.topNav.label}</h2>
            <ul>
                {props.topNav.children.map((childMenu, index) => (
                    <li key={index} onClick={onLiClick}>
                        {subLink(childMenu)}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default SideNav as React.FC;
