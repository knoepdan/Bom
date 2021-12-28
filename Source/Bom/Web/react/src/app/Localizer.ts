import { useAppContext } from 'app/AppContext';

// TODO -> improve typing
export enum CommonResKey {
    Cancel = 'Common_Cancel',
    Ok = 'Common_Ok',
    Close = 'Common_Close',
    Save = 'Common_Save',
}

type Prefix = 'Special' | 'Common' | 'App' | 'Main' | 'Admin';

type TimeKeys =
    | 'Special_TimeFormatJson'
    | 'Special_DateFormatJson'
    | 'Special_DateTimeFormatJson'
    | 'Special_DateFormatCultureOverride';

type OtherKeys = `${Prefix}_${string}`; // {Area/app prefix}_{PageOrControl/second prefix}_{Text}

export type ResKey = TimeKeys | OtherKeys;

export class Localizer {
    private res: any = {}; // dic

    public localizeDynamic(
        id: ResKey | CommonResKey | string,
        defaultMessage: string,
        para1: string | number = '',
        para2: string | number = '',
    ): string {
        const val = this.localize(id as ResKey, defaultMessage, para1, para2);
        return val;
    }

    private getRessources(): any {
        return {}; // TODO -> at the moment no localization.. default value is always used
    }

    public localize(id: ResKey | CommonResKey, defaultMessage: string, ...params: (string | number)[]): string {
        if (!this.res) {
            this.res = this.getRessources();
        }
        if (!id) {
            console.log('Localization key is empty. Fallback is returned: ' + defaultMessage);
            return defaultMessage || '';
        }
        let val = this.res[id];
        if (val === undefined || val === null) {
            //console.error("Localization key '" + id + "' not found. Fallback is returned: " + defaultMessage);
            return defaultMessage || '';
        }
        if (arguments.length > 2) {
            val = this.format(val, params);
        }
        return val;
    }

    public fixed(msg: string, para1: string | number = '', para2: string | number = ''): string {
        return this.localizeDynamic(' ', msg, para1, para2);
    }

    public formatTime(date: Date): string {
        const opt: Intl.DateTimeFormatOptions = this.parseJson(this.localize('Special_TimeFormatJson', ''), {
            timeStyle: 'short',
        });
        return this.formatDateOb(date, opt);
    }

    public formatDate(date: Date): string {
        const opt: Intl.DateTimeFormatOptions = this.parseJson(this.localize('Special_DateFormatJson', ''), {
            dateStyle: 'short',
        });
        return this.formatDateOb(date, opt);
    }

    public formatDateTime(date: Date): string {
        const opt: Intl.DateTimeFormatOptions = this.parseJson(this.localize('Special_DateTimeFormatJson', ''), {
            dateStyle: 'short',
            timeStyle: 'short',
        });
        return this.formatDateOb(date, opt);
    }

    private parseJson<T>(s: string, fallback: T): T {
        if (s && s != '') {
            try {
                return JSON.parse(s) as T;
            } catch (e) {
                console.warn('Error parsing dateFormat-json:' + s);
            }
        }
        return fallback;
    }

    private formatDateOb(date: Date, opt: Intl.DateTimeFormatOptions) {
        if (date) {
            try {
                // define lang to use
                const user = useAppContext().user;
                let locale: string | string[] | undefined = this.localize('Special_DateFormatCultureOverride', '');
                if (!locale || locale == '') {
                    locale = user.lang;
                } else if (locale == '|') {
                    locale = undefined; // will use browser lang
                } else {
                    locale = new Array<string>(locale, user.lang);
                }
                const f = new Intl.DateTimeFormat(locale, opt);
                return f.format(date);
            } catch (e: unknown) {
                console.warn('Ex in formatDateOb: ' + (e as any).message);
                return date.toLocaleDateString() + ' ' + date.toLocaleDateString(); // fallback to show at least something and not crash app
            }
        }
        return '';
    }

    private format(s: string, args: unknown[]) {
        // inspired by: https://coderwall.com/p/flonoa/simple-string-format-in-javascript
        if (!s.match(/^(?:(?:(?:[^{}]|(?:\{\{)|(?:\}\}))+)|(?:\{[0-9]+\}))+$/)) {
            console.log('localize.format: string might not be formated correctly: ' + s);
        }
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        return s.replace(/((?:[^{}]|(?:\{\{)|(?:\}\}))+)|(?:\{([0-9]+)\})/g, (m: string, str: any, index: number) => {
            if (str) {
                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                return str.replace(/(?:{{)|(?:}})/g, (m: any) => m[0]);
            } else {
                if (index >= args.length) {
                    return '{' + index + '}'; // dont change anything
                }
                return args[index];
            }
        });
    }
}

const loc = new Localizer();
export default loc;
