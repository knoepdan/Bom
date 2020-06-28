import * as React from 'react';
import css from './TopBarAccountInfo.module.css';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const TopBarAccountInfo = (): React.ReactElement<Props> => {
    return <span className={css.account}>Account</span>;
};

export default TopBarAccountInfo as React.FC;
