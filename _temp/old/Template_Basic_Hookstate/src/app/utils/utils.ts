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
