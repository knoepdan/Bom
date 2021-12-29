import * as React from 'react';
import { MenuItem } from 'app/nav/MenuItem';
import { RouteInfo } from 'app/nav/RouteInfo';
import { Right } from 'app/Right';
import { useAppContext } from 'app/AppContext';
const StatisticsPage = React.lazy(() => import('./StatisticsPage')); // import { StatisticsPage } from './StatisticsPage';
const UsersPage = React.lazy(() => import('./UsersPage')); // import { UsersPage } from './UsersPage';

import loc from 'app/Localizer';

export function getAdminMenu(): MenuItem | null {
    const app = useAppContext();
    const user = app.user;
    let topMenu: MenuItem | null = null;
    // admin
    if (user.hasRight(Right.AdminArea)) {
        topMenu = new MenuItem(null, loc.localize('Admin_AdminArea', 'Admin-Area'));

        //  // equals: <StatisticsPage> in jsx https://reactjs.org/docs/jsx-in-depth.html

        const statistic = new MenuItem(topMenu, loc.localize('Admin_NavStatistics', 'Statistics'));
        statistic.route = new RouteInfo('admin/statistics/*', React.createElement(StatisticsPage)); //// equals: <StatisticsPage> in jsx https://reactjs.org/docs/jsx-in-depth.html

        topMenu.children.push(statistic);

        // users
        const users = new MenuItem(
            topMenu,
            loc.localize('Admin_NavUsers', 'Users'),
            new RouteInfo('admin/users/*', React.createElement(UsersPage)),
        );
        topMenu.children.push(users);
    }
    return topMenu;
}

/*
export function getRoutes<T>(): Array<IRoute> {
    let a = new Array<IRoute>();

    a.push({
        getRoute: () => {
            return 'admin/statistics';
        },
        getComponent: () => {
            return React.createElement(StatisticsPage); // equals: <StatisticsPage> in jsx https://reactjs.org/docs/jsx-in-depth.html
        },
    });

    a.push({
        getRoute: () => {
            return 'admin/users';
        },
        getComponent: () => {
            return React.createElement(UsersPage);
        },
    });

    return a;
}
*/
