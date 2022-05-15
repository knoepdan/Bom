import * as React from 'react';
import { MenuItem } from 'app/nav/MenuItem';
import { RouteInfo } from 'app/nav/RouteInfo';
import { Right } from 'app/Right';
const DeveloperPage = React.lazy(() => import('./DeveloperPage')); // import { DeveloperPage } from './DeveloperPage';
const ExamplesPage = React.lazy(() => import('./ExamplesPage'));
const ExamplesBomApiPage = React.lazy(() => import('./ExampleBomApiPage'));
import { useAppContext } from 'app/AppContext';
import loc from 'app/Localizer';

export function getDevMenu(): MenuItem | null {
    const app = useAppContext();
    const user = app.user;
    let topMenu: MenuItem | null = null;
    if (user.hasRight(Right.DevArea)) {
        topMenu = new MenuItem(null, loc.localize('Special_DevArea', 'Developer-Area'));

        // submenues (no localization needed)
        const devs = new MenuItem(
            topMenu,
            loc.fixed('Devs'),
            new RouteInfo('dev/developers/*', React.createElement(DeveloperPage)),
        );
        topMenu.children.push(devs);

        const ex = new MenuItem(
            topMenu,
            loc.fixed('Examples'),
            new RouteInfo('dev/examples/*', React.createElement(ExamplesPage)),
        );
        topMenu.children.push(ex);

        const exApi = new MenuItem(
            topMenu,
            loc.fixed('Examples API'),
            new RouteInfo('dev/examplesapi/*', React.createElement(ExamplesBomApiPage)),
        );
        topMenu.children.push(exApi);
    }
    return topMenu;
}
