import * as React from 'react';
import { MenuItem } from 'app/nav/MenuItem';
import { RouteInfo } from 'app/nav/RouteInfo';
import { Right } from 'app/Right';
import { DeveloperPage } from './DeveloperPage';
import { ExamplesPage } from './ExamplesPage';
//import { UsersPage } from './UsersPage';
import { useAppContext } from 'app/AppContext';

export function getDevMenu(): MenuItem | null {
    const app = useAppContext();
    const user = app.user;
    let topMenu: MenuItem | null = null;
    if (user.hasRight(Right.DevArea)) {
        topMenu = new MenuItem(null, 'Developer-Area');

        // submenues
        const devs = new MenuItem(
            topMenu,
            'Devs',
            new RouteInfo('dev/developers/*', React.createElement(DeveloperPage)),
        );
        topMenu.children.push(devs);

        const ex = new MenuItem(
            topMenu,
            'Examples',
            new RouteInfo('dev/examples/*', React.createElement(ExamplesPage)),
        );
        topMenu.children.push(ex);
    }
    return topMenu;
}
