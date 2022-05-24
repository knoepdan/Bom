import { BaseState } from './utils/BaseState';
import { State } from '@hookstate/core';
import { SetStateAction } from 'react';

export class GlobalStateWrapper extends BaseState<GlobalState> {
    constructor(s: GlobalState) {
        super(s);
    }
    // overwrite wrapState to offer functions (often not needed, set/get are enough)
    // often not needed, but then one would have to also set the basic methods like get/set again
    protected wrapState = (s: State<GlobalState>) => ({
        get: () => s.value,
        set: (newValue: SetStateAction<GlobalState>) => {
            s.set(newValue);
        },
    });
}

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

export const globalStateRef = new GlobalStateWrapper(getInitialGlobalState());

export const uiStateRef = new BaseState<GlobalUiState>({ username: 'testuser', loggedInDate: new Date() });

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
