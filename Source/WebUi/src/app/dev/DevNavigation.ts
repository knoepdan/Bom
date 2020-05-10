import * as React from 'react';
import { MenuItem, RouteInfo } from 'app/common/NavigationState';
import { DeveloperPage } from './DeveloperPage';
//import { UsersPage } from './UsersPage';

export function addDevMenues<T>(topMenu: MenuItem): void {
    // developer
    let devs = new MenuItem(topMenu, 'Devs', new RouteInfo('/dev/developers', React.createElement(DeveloperPage)));
    topMenu.children.push(devs);
}
