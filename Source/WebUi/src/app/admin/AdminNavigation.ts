import * as React from 'react';
import { MenuItem, RouteInfo } from 'app/common/NavigationState';
import { StatisticsPage } from './StatisticsPage';
import { UsersPage } from './UsersPage';

export function addAdminMenues<T>(topMenu: MenuItem): void {
    // statistics

    //  // equals: <StatisticsPage> in jsx https://reactjs.org/docs/jsx-in-depth.html

    let statistic = new MenuItem(topMenu, 'Statistics');
    statistic.route = new RouteInfo('/admin/statistics', React.createElement(StatisticsPage)); //// equals: <StatisticsPage> in jsx https://reactjs.org/docs/jsx-in-depth.html

    topMenu.children.push(statistic);

    // users
    let users = new MenuItem(topMenu, 'Users', new RouteInfo('/admin/users', React.createElement(UsersPage)));
    topMenu.children.push(users);
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
