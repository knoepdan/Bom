import * as state from '@hookstate/core';
import { Right } from 'app/common/Right';

export interface UserState {
    isLoggedIn: boolean;
    username: string;

    // more props
    // - loggedInDate: Date;
    // - permissions etc.
}

export class UserModel {
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

export const userStateRef = state.createState(new UserModel());

/*
function getInitialGlobalState(): GlobalState {
    const g = {
        tempArray: [],
        boolVal: false,
        stringVal: 'values',
        someNr: 1,
        otherNr: 100,
        subOb: {
            subtitle: 'jjjjjuu',
            subArray: [],
        },
    };
    return g as GlobalState;
}

export const globalStateRef = new state.StateRef(getInitialGlobalState());

export const uiStateRef = new state.StateRef({ username: 'testuser', loggedInDate: new Date() });

export interface ArrayContent {
    title: string;
    nr: number;
}

export interface SubOb {
    subtitle: string;
    subArray: Array<string>;
}

export interface GlobalState {
    tempArray: Array<ArrayContent>;
    boolVal: boolean;
    stringVal: string;
    someNr: number;
    otherNr: number;
    subOb: SubOb;
}

export interface GlobalUiState {
    username: string;
    loggedInDate: Date;
}

*/
