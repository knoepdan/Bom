export class ExampleClass {
    public async callAnything(s: string): Promise<void> {
        console.log('TestClass.callAnything');

        try {
            await fetch('wwwbla');
            console.log('success ' + s);
        } catch (e) {
            // will always fail
            console.log('error ' + e + '     ' + s);
        }
    }
}

export function sum(a: number, b: number): number {
    return a + b;
}
