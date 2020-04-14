export class TestClass {
    public async callAnything(s: string): Promise<void> {
        try {
            await fetch('wwwbla');
            console.log('success ' + s);
        } catch (e) {
            // will always fail
            console.log('error ' + e + '     ' + s);
        }

    }
}
