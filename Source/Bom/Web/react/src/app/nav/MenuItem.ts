import { IRoute } from 'app/nav/RouteInfo';

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
