import React from 'react';
import { MenuItem } from 'app/nav/MenuItem';
import { IRoute, RouteInfo } from 'app/nav/RouteInfo';
import { AppUser } from 'app/AppUser';

import { LoginPage } from 'app/layout/LoginPage';

// navigations
import * as mainNav from './../../main/MainNavigation';
import * as adminNav from './../../admin/AdminNavigation';
import * as devNav from './../../dev/DevNavigation';

class Routes {
    public readonly LoginRoute = new RouteInfo('login', React.createElement(LoginPage));

    public readonly PublicMainTo = 'main/entry';
}
const routes = new Routes();

export class NavigationModel {
    public topMenues: Array<MenuItem> = new Array<MenuItem>();

    public definedRoutes(): Routes {
        return routes;
    }
    /*
    private _selected: MenuItem | null = null;
    get selected(): MenuItem | null {
        return this._selected;
    }
    set selected(value: MenuItem | null) {
        this._selected = value;
    }
    */

    private static getRoutesRec(a: Array<IRoute>, menu: MenuItem): void {
        if (menu.route) {
            a.push(menu.route);
        }
        for (const child of menu.children) {
            NavigationModel.getRoutesRec(a, child);
        }
    }

    public getRoutes(): Array<IRoute> {
        const a = new Array<IRoute>();
        for (const m of this.topMenues) {
            NavigationModel.getRoutesRec(a, m);
        }
        return a;
    }

    public addMenu(menu: MenuItem | null) {
        if (menu) {
            this.topMenues.push(menu);
        }
    }
}

export function getNavigation(user: AppUser): NavigationModel {
    const navModel = new NavigationModel();

    // main
    const m = mainNav.getMainMenu();
    navModel.addMenu(m);

    if (user && user.isLoggedIn()) {
        // admin
        const adm = adminNav.getAdminMenu();
        navModel.addMenu(adm);

        // dev
        const devM = devNav.getDevMenu();
        navModel.addMenu(devM);
    }
    return navModel;
}
