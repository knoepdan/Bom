import * as state from 'app/utils/State';

import * as React from 'react';
import { UserModel } from 'app/common/UserState';
import { LoginPage } from 'app/LoginPage';
import { Right } from 'app/common/Right';
import * as mainNav from 'app/main/MainNavigation';
import * as adminNav from 'app/admin/AdminNavigation';
import * as devNav from 'app/dev/DevNavigation';

export interface IRoute {
    getRoute(): string;
    getComponent(): () => JSX.Element; // JSX.Element; // React.ComponentType<any>; //React.ReactElement;
    exact?: boolean | undefined;
}

export class RouteInfo implements IRoute {
    public route: string;
    public component: JSX.Element;
    public exact?: boolean | undefined = false;

    constructor(route: string, comp: JSX.Element, exact = false) {
        this.route = route;
        this.component = comp;
        this.exact = exact;
    }

    public getRoute(): string {
        return this.route;
    }
    public getComponent(): () => JSX.Element {
        return () => {
            return this.component;
        };
    }
}

export class Routes {
    public static readonly LoginRoute = new RouteInfo('/login', React.createElement(LoginPage));

    // just the routes
    public static readonly PublicMainTo = '/main/entry';
}

export function getNavigation(user: UserModel): NavigationModel {
    const navModel = new NavigationModel();

    // login
    const loginMenu = new MenuItem(null, 'Login');
    loginMenu.route = Routes.LoginRoute;
    navModel.topMenues.push(loginMenu);

    // main
    const m = new MenuItem(null, 'Main-Area');
    navModel.topMenues.push(m);
    mainNav.addMainMenues(m);

    if (user && user.isLoggedIn) {
        // admin
        if (user.hasRight(Right.AdminArea)) {
            const a = new MenuItem(null, 'Admin-Area');
            navModel.topMenues.push(a);
            adminNav.addAdminMenues(a);
        }

        // dev
        if (user.hasRight(Right.DevArea)) {
            const d = new MenuItem(null, 'Developer-Area');
            navModel.topMenues.push(d);
            devNav.addDevMenues(d);
        }
    }
    return navModel;
}

export class NavigationModel {
    public topMenues: Array<MenuItem> = new Array<MenuItem>();

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
}

export class MenuItem {
    constructor(parent: MenuItem | null, label: string, route: IRoute | null = null) {
        this.parent = parent;
        this.label = label;
        this.route = route;
        /*if (this.parent) { // must be done outsice
            this.parent.children.push(this);
        }*/
    }

    public parent: MenuItem | null = null;
    public children: Array<MenuItem> = new Array<MenuItem>();
    public label = '';
    public route: IRoute | null = null;
}

export const navStateRef = new state.StateRef(new NavigationModel());
