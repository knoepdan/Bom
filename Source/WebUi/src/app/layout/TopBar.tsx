import * as React from 'react';
import { TopBarAccountInfo } from './TopBarAccountInfo';
import { HamburgerMenu } from './HamburgerMenu';
import css from './TopBar.module.css';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const TopBar = (): React.ReactElement<Props> => {
    return (
        <header className={css.topBar}>
            <HamburgerMenu />
            <TopBarAccountInfo />
        </header>
    );
};

export default TopBar as React.FC;
