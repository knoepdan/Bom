import * as React from 'react';
import { MenuItem, RouteInfo } from 'app/common/NavigationState';
import { EntryPage } from './EntryPage';
import { PublicPage } from './PublicPage';

export function addMainMenues<T>(topMenu: MenuItem): void {
    const entry = new MenuItem(topMenu, 'Entry', new RouteInfo('/main/entry', React.createElement(EntryPage)));
    topMenu.children.push(entry);

    topMenu.children.push(
        new MenuItem(topMenu, 'Public', new RouteInfo('/main/public', React.createElement(PublicPage))),
    );
}
