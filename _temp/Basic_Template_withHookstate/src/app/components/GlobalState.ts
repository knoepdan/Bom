import * as state from 'app/utils/CustomState';

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
