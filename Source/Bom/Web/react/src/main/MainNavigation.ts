import * as React from 'react';
import { MenuItem } from 'app/nav/MenuItem';
import { RouteInfo } from 'app/nav/RouteInfo';
import { EntryPage } from './EntryPage';
import { PublicPage } from './PublicPage';
import loc from 'app/Localizer';

export function getMainMenu(): MenuItem {
    const topMenu = new MenuItem(null, loc.localize('Main_MainArea', 'Main-Area'));
    const entry = new MenuItem(
        topMenu,
        loc.localize('Main_NavEntry', 'Entry'),
        new RouteInfo('main/entry/*', React.createElement(EntryPage)),
    );
    topMenu.children.push(entry);

    topMenu.children.push(
        new MenuItem(
            topMenu,
            loc.localize('Main_NavPublic', 'Public'),
            new RouteInfo('main/public/*', React.createElement(PublicPage)),
        ),
    );
    return topMenu;
}
