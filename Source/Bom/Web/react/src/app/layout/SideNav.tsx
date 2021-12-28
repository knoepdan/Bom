import * as React from 'react';
import css from './SideNav.module.css';
import c from 'style/cssClasses';
import { MenuItem } from 'app/nav/MenuItem';
import { NavLink } from 'react-router-dom';
import loc from 'app/Localizer';

import { useAppContext } from 'app/AppContext';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

export const SideNavHtmlId = 'sideNav';

export const SideNav = (): React.ReactElement<Props> => {
    const app = useAppContext();
    const navModel = app.getNavModel();

    return (
        <nav className={css.animateFromLeft + ' ' + css.sideNav} id={SideNavHtmlId}>
            <div className={c('pl10')}>
                <h1>{loc.localize('App_Title', 'Birdview')}</h1>
            </div>
            <hr />

            <ul className={c('p10')}>
                {navModel.topMenues.map((topNav, index) => (
                    <li key={index}>
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
    topNav: MenuItem;
}

const AreaNav = (props: AreaNavProps): React.ReactElement<AreaNavProps> => {
    const subLink = (subNav: MenuItem): React.ReactElement => {
        if (subNav.route && subNav.route.getRoute()) {
            let route = subNav.route.getRoute();
            if (route.endsWith('*')) {
                route = route.substring(0, route.length - 1);
            }
            return (
                <NavLink to={route} className={({ isActive }) => '' + (isActive ? ' ' + css.navActive : '')}>
                    {subNav.label}
                </NavLink>
            );
        } else {
            return <span>{subNav.label}</span>;
        }
    };

    const onLiClick = (e: React.MouseEvent<HTMLLIElement, MouseEvent>): boolean => {
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
                {props.topNav.children.map((childMenu, i) => (
                    <li key={i} onClick={onLiClick}>
                        {subLink(childMenu)}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default SideNav as React.FC;
