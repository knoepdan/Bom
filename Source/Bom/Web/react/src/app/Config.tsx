export interface IConfig {
    readonly APIUrl: string;
    readonly TestEntry: string;
}

interface IPassedUser {
    readonly Username: string;
    readonly Token: string;
    readonly Lang: string;
}

function getBom(): any {
    const w = window as any;
    if (w.bom) {
        return w.bom;
    }
    return {};
}

export function getUserInfo(): IPassedUser {
    const bom = getBom();
    if (bom.User) {
        return bom.User as IPassedUser;
    }
    console.warn('user not found' + bom);
    return {} as IPassedUser;
}

export function getConfig(): IConfig {
    const bom = getBom();
    if (bom.Info) {
        return bom.Info as IConfig;
    }
    console.warn('config not found' + bom);
    return {} as IConfig;
}

const inst: IConfig = getConfig();
export default inst;
// const defaultConfig: IConfig = {
//     APIUrl: '',
//     TestEntry: 'Default-Testentry',
// };
// const confKey = 'confKey';
// export class Config {
//     public static async loadConfigFile(forceFetch = false): Promise<void> {
//         if (!forceFetch) {
//             if (sessionStorage.getItem(confKey)) {
//                 return;
//             }
//         }
//         /*
//             // Idea for dev mode only (process.env.NODE_ENV == 'development')
//             // have another hardoded url to config (example: 'http:localhost/blabla.json')
//             // that is used for a developers that wants to test against some other api,
//             // but is reluctant to always change config that is tracked by source control
//         */

//         // we rely that <base href="/PUBLIC_URL%/" /> is set correctly in index file (in dev "/" is normally correct in prod -> depends on environment)
//         const result = await fetch(document.baseURI + 'externalConfig.json');
//         if (!result.ok) {
//             throw new Error('Error loading config'); // improve: more info about error
//         }
//         const conf = await result.json();
//         sessionStorage.setItem(confKey, JSON.stringify(conf));
//     }

//     static get isLoaded(): boolean {
//         const val = sessionStorage.getItem(confKey);
//         if (val) {
//             return true;
//         }
//         return false;
//     }

//     private static getConfig(): IConfig {
//         const val = sessionStorage.getItem(confKey);
//         if (val) {
//             return JSON.parse(val);
//         }
//         if (process.env.NODE_ENV == 'development') {
//             Config.loadConfigFile(true); // in dev: make sure config is always/regularly refreshed
//         }
//         throw new Error('Config was not in session. Means it was not loaded before');
//     }

//     static get APIUrl(): string {
//         const configFile = Config.getConfig();
//         return configFile.APIUrl || defaultConfig.APIUrl;
//     }

//     static get TestEntry(): string {
//         const configFile = Config.getConfig();
//         return configFile.TestEntry || defaultConfig.TestEntry;
//     }
// }
