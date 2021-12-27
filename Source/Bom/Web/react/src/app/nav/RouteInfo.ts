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
        return (): JSX.Element => {
            return this.component;
        };
    }
}
