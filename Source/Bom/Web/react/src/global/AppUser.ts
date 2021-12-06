export class AppUser {
    isLoggedIn = false;
    username = '';
    permissions = new Array<Right>();

    public hasRight(right: Right): boolean {
        return this.permissions.some((r) => r == right);
    }

    public logIn(user: string): void {
        this.username = user;
        this.isLoggedIn = true;
    }
    public logOut(): void {
        this.username = '';
        this.isLoggedIn = false;
    }
}

export enum Right {
    AdminArea = 'AdminArea',
    DevArea = 'DevArea',
}