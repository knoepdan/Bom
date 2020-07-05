/* eslint-disable @typescript-eslint/no-explicit-any */

export async function delay<T>(fn: () => T, ms: number): Promise<T> {
    return new Promise<T>((resolve, reject) => {
        window.setTimeout(() => {
            try {
                const result = fn();
                resolve(result);
            } catch (e) {
                reject(e);
            }
        }, ms);
    });
}

/*
export function omit<T extends object, K extends keyof T>(target: T, ...omitKeys: K[]): Omit<T, K> {
    return (Object.keys(target) as K[]).reduce((res, key) => {
        if (!omitKeys.includes(key)) {
            res[key] = target[key];
        }
        return res;
    }, {} as any);
}
*/
