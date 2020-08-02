import { createState, useState, State, StateMethodsDestroy } from '@hookstate/core';
import { SetStateAction } from 'react';

export class BaseState<T> {
    constructor(initialState: T) {
        this.globalState = createState(initialState);
    }
    protected globalState: State<T> & StateMethodsDestroy;
    protected wrapState = (s: State<T>) => ({
        get: () => s.value,
        set: (newValue: SetStateAction<T>) => {
            s.set(newValue);
        },
    });
    public accessGlobalState = () => this.wrapState(this.globalState);
    public useGlobalState = () => this.wrapState(useState(this.globalState));

    public destroy(): void {
        this.globalState.destroy(); // object will no longer work correctly
    }
}

/* example using a simple state (extending wrap function, which is not always needed)
class NumberState extends BaseState<number>{

    constructor(n : number){
        super(n);
    }
    // overwrite wrapState to offer functions (often not needed, set/get are enough)
    protected wrapState = (s: State<number>) => ({
            get: () => s[self].value,
            set: (newValue: SetStateAction<number>) => {s[self].set(newValue)},
            increment: () => s[self].set(p => p + 1)
    })
}
export const globalNumber : NumberState = new NumberState(0);
*/

/* example using functions https://hookstate.js.org/docs/exporting-state
const globalState = createState(0);
const wrapState = (s: State<number>) => ({
    get: () =>  s[self].value,
    set: (newValue: SetStateAction<number>) => {s[self].set(newValue)},
    increment: () => s[self].set(p => p + 1)
})
// The following 2 functions can be exported now:
export const accessGlobalState = () => wrapState(globalState)
export const useGlobalState = () => wrapState(useState(globalState))
*/
