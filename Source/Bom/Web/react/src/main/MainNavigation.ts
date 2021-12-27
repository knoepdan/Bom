import * as React from 'react';
import { MenuItem } from 'app/nav/MenuItem';
import { RouteInfo } from 'app/nav/RouteInfo';
import { EntryPage } from './EntryPage';
import { PublicPage } from './PublicPage';

export function getMainMenu(): MenuItem {
    const topMenu = new MenuItem(null, 'Main-Area');
    const entry = new MenuItem(topMenu, 'Entry', new RouteInfo('/main/entry', React.createElement(EntryPage)));
    topMenu.children.push(entry);

    topMenu.children.push(
        new MenuItem(topMenu, 'Public', new RouteInfo('/main/public', React.createElement(PublicPage))),
    );
    return topMenu;
}
