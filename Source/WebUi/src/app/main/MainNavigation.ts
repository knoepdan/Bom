import * as React from 'react';
import { MenuItem, RouteInfo } from 'app/common/NavigationState';
import { EntryPage } from './EntryPage';
//import { UsersPage } from './UsersPage';

export function addMainMenues<T>(topMenu: MenuItem): void {
    let entry = new MenuItem(topMenu, 'Entry', new RouteInfo('/main/entry', React.createElement(EntryPage)));
    topMenu.children.push(entry);
}
