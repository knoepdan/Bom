export class AppUser {
    username = '';
    token: string | null = '';
    permissions = new Array<Right>();

    constructor(username: string, token: string) {
        this.username = username;
    }

    public hasRight(right: Right): boolean {
        return this.permissions.some((r) => r == right);
    }

    public logIn(user: string, token: string): void {
        this.username = user;
        this.token = token;
    }
    public logOut(): void {
        this.username = '';
        this.token = null;
    }
}

export enum Right {
    AdminArea = 'AdminArea',
    DevArea = 'DevArea',
}
