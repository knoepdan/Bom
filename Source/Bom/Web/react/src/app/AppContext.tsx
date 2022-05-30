import * as React from 'react';
import { AppUser } from './AppUser';
import * as Config from './Config';
import * as Nav from 'app/nav/NavigationModel';
import * as Api from 'api-clients';

//import { ExosPermission } from 'util-components';

//import * as xx from 'a'
//export { ExosPermission };

export interface IAppContext {
    user: AppUser;

    config: Config.IConfig;

    getNavModel(): Nav.NavigationModel;

    getRouterBase(): string;
}

export class AppContextImpl implements IAppContext {
    // constructor(login: API.CurrentUserInfo | null, webapps: Array<API.WebAppOutput> | null) {
    //     this.user = login;
    //     this.webapps = webapps;
    //     if(!this.webapps){
    //         this.webapps = new Array<API.WebAppOutput>();
    //     }
    // }
    constructor(user: AppUser) {
        this.user = user;
        this.config = Config.getConfig();
        this.initApi();
    }
    public user: AppUser;

    public readonly config: Config.IConfig;

    private initApi(): void {
        Api.OpenAPI.BASE = this.config.APIUrl;
        Api.OpenAPI.TOKEN = ''; // reset to be on the safe side
        if (this.user.token) {
            Api.OpenAPI.TOKEN = this.user.token;
        }
    }

    public getNavModel(): Nav.NavigationModel {
        const navModel = Nav.getNavigation(this.user);
        return navModel;
    }

    public getRouterBase(): string {
        // Ensure navigation works (and references etc. in third party libs too, config call etc.)
        // -> we set basename in BrowserRouter. However, this depends on the environment.
        //    in deployed to application that is accesible via app name (localhost/appName) then it must be set
        //    However document.getElementsByTagName('base')[0].href; still returns full url even though we just want whats in base tag
        // see also: https://stackoverflow.com/questions/13832690/get-base-in-html-after-it-has-been-set-but-not-using-page-url
        let baseHref: string = '';
        const baseTag = document.getElementsByTagName('base');
        if (baseTag.length > 0) {
            baseHref = baseTag[0].dataset.href as string; // example base tag: <base href="/app" data-href="/app" />    (for dev "/" is normally ok)
        }
        return baseHref;
        /*
        const n = this.info.routerBase as string;
        // all
        if (n === 'undefined') {
            return undefined; // rely on base functionality of router
        }
        if (!n || n == '' || n.startsWith('[')) {
            // apply default logic (with some possiblity to influence it for absolute corner cases)
            const parsedData = window.location.pathname.split('/');
            let i = 1;
            if (n && n.startsWith('[0')) {
                i = 0;
            } else if (n && n.startsWith('[2')) {
                i = 2;
            }
            return '/' + parsedData[i];
        }
        return n; // rely on server side config
        */
    }

    // private readonly authMap = new Map<string, API.UserPermission>(); // use map to avoid having to iterate through array multiple times

    // public readonly user: API.CurrentUserInfo | null;

    // public readonly webapps: Array<API.WebAppOutput>;

    // public getUrl(app: API.ExosWebAppEnum) : string | null{
    //     const webapp = this.webapps.find(a => a.Id == app);
    //     if(webapp){
    //         return webapp.Url;
    //     }
    //     return null;
    // }

    // public hasPermission(authId: ExosPermission, permission?: API.Permission): boolean {
    //     const perm = this.getPermission(authId as string);
    //     if (perm) {
    //         if (permission == API.Permission.Readonly) {
    //             return true; // since we only have readonly and full, it can only be true
    //         }
    //         let l = perm.PermissionLevel as any; // support enum and inner value
    //         if (l == 2 || l == API.Permission.Full) {
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    // private getPermission(authId: string) : API.UserPermission | null {
    //     let p : API.UserPermission = this.authMap.get(authId);
    //     if (!p && authId && this.user && this.user.Permissions) {
    //         // search in user permission and store it in map for faster access
    //         p = this.user.Permissions.find(p => p.AuthId == authId);
    //         if(!p){
    //             p = { AuthId: authId, PermissionLevel: API.Permission.None } // not found but still store it in map
    //         }
    //         this.authMap.set(authId, p);
    //     }
    //     const l = p.PermissionLevel as any;
    //     if (p && l != API.Permission.None && l != 0) {
    //          return p;
    //     }
    //     return null;
    // }
}

function getAppContextDefault(): IAppContext {
    const info = Config.getUserInfo();
    const user = new AppUser(info.Username, info.Token);
    const c = new AppContextImpl(user);
    return c;
}

export const AppContext = React.createContext<IAppContext>(getAppContextDefault());

export function useAppContext(): IAppContext {
    const context: IAppContext = React.useContext(AppContext);
    return context;
}

export default useAppContext;
