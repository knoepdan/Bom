export interface IRoute {
    getRoute(): string;
    getComponentFunc(): () => JSX.Element; // JSX.Element; // React.ComponentType<any>; //React.ReactElement;
    exact?: boolean | undefined;
}

export class RouteInfo implements IRoute {
    public route: string;
    public component: JSX.Element;

    constructor(route: string, comp: JSX.Element) {
        this.route = route;
        this.component = comp;
    }

    public getRoute(): string {
        return this.route;
    }
    public getComponentFunc(): () => JSX.Element {
        return (): JSX.Element => {
            return this.component;
        };
    }
}
