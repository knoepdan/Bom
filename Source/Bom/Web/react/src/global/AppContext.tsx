import * as React from 'react';
import {AppUser}  from './AppUser';
//import { ExosPermission } from 'util-components';

//import * as xx from 'a'
//export { ExosPermission };

export interface IAppContext {
    // getUrl(app: API.ExosWebAppEnum) : string | null
    // webapps: Array<API.WebAppOutput>

    user: AppUser
    // hasPermission(authId: ExosPermission, permission?: API.Permission): boolean
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
    }
    user: AppUser

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
const dummyUser = new AppUser();
export const AppContext = React.createContext<IAppContext>(new AppContextImpl(dummyUser));

export function useAppContext(): IAppContext {
    const context: IAppContext = React.useContext(AppContext);
    return context;
}

export default useAppContext;