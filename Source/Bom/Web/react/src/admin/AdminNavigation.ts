import * as React from 'react';
import { MenuItem } from 'app/nav/MenuItem';
import { RouteInfo } from 'app/nav/RouteInfo';
import { Right } from 'app/Right';
import { useAppContext } from 'app/AppContext';
import { StatisticsPage } from './StatisticsPage';
import { UsersPage } from './UsersPage';

export function getAdminMenu(): MenuItem | null {
    const app = useAppContext();
    const user = app.user;
    let topMenu: MenuItem | null = null;
    // admin
    if (user.hasRight(Right.AdminArea)) {
        topMenu = new MenuItem(null, 'Admin-Area');

        //  // equals: <StatisticsPage> in jsx https://reactjs.org/docs/jsx-in-depth.html

        const statistic = new MenuItem(topMenu, 'Statistics');
        statistic.route = new RouteInfo('admin/statistics/*', React.createElement(StatisticsPage)); //// equals: <StatisticsPage> in jsx https://reactjs.org/docs/jsx-in-depth.html

        topMenu.children.push(statistic);

        // users
        const users = new MenuItem(topMenu, 'Users', new RouteInfo('admin/users/*', React.createElement(UsersPage)));
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
