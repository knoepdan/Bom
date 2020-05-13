import * as state from '@hookstate/core';
// Wrapper around hookstate

export declare type SetStateAction<S> = (S | Promise<S>) | ((prevState: S) => S | Promise<S>);
export class State<S> {
    innerState: state.StateLink<S>;

    constructor(s: state.StateLink<S>) {
        this.innerState = s;
    }

    set(newState: SetStateAction<S>): void {
        this.innerState.set(newState);
    }

    get value(): S {
        return this.innerState.get();
    }
}

export class StateRef<S> {
    stateRef: state.StateInf<state.StateLink<S>>;

    constructor(initialVal: S) {
        this.stateRef = state.createStateLink(initialVal);
    }

    useState(isMounted = true): State<S> {
        if (isMounted) {
            const s = state.useStateLink(this.stateRef);
            return new State(s);
        } else {
            const us = state.useStateLinkUnmounted(this.stateRef);
            return new State(us);
        }
    }
}
