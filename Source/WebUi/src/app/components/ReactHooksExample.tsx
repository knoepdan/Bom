import * as React from 'react';
import { createStateLink, useStateLink, useStateLinkUnmounted } from '@hookstate/core';
import * as utils from 'app/utils/utils';

const stateRef = createStateLink(0);

const stateRef2 = createStateLink(77);

setInterval(() => {
    const useableState = useStateLinkUnmounted(stateRef);
    useableState.set((p) => p + 1);
}, 3000);

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

export const ReactHooksExample = (): React.ReactElement<Props> => {
    const state = useStateLink(stateRef);

    const state2 = useStateLink(stateRef2);

    return (
        <div>
            <div>
                <b>1. Counter value: {state.value}</b> (watch +1 every 3 seconds){' '}
                <button
                    onClick={(): void => {
                        console.log('simple increment clicked');
                        state.set((p) => p + 1);
                    }}
                >
                    Increment
                </button>
            </div>

            <div>
                <b>2. Counter value: {state2.value}</b>
                <button
                    onClick={async (): Promise<void> => {
                        await utils.delay((): void => console.log('increment by 2 clicked'), 2000);
                        state2.set((p) => p + 1);
                        state2.set((p) => {
                            p = p + 1;
                            return p;
                        });
                    }}

                    /*  compiling and no runtime error but no delay (obviously, missing delay)
                    onClick={(): void =>
                        state2.set(p => {
                            utils.delay((): void => console.log('xxx'), 2000);
                            p = p + 1;
                            return p;
                        })
                    }*/
                    /*  compiling but error at runtime
                    onClick={(): void =>
                        state2.set(async p => {
                            await utils.delay((): void => console.log('xxx'), 2000);
                            p = p + 1;
                            return p;
                        })
                    }*/
                >
                    Increment by 2 with delay
                </button>
            </div>
        </div>
    );
};
