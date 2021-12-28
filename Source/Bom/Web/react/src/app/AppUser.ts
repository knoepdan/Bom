import { Right } from 'app/Right';
export class AppUser {
    username = '';
    token: string | null = '';
    lang: string = 'en';
    permissions = new Array<Right>();

    constructor(username: string, token: string) {
        this.username = username;
        this.token = token;

        // TODO  remove.. this is just dummy
        this.permissions.push(Right.AdminArea, Right.DevArea);
    }

    public isLoggedIn(): boolean {
        return !!this.token;
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
