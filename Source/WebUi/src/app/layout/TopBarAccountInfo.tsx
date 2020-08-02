import * as React from 'react';
import * as state from 'app/common/UserState';
import * as nav from 'app/common/NavigationState';
import { Link, useHistory } from 'react-router-dom';
import css from './TopBarAccountInfo.module.css';
import { useState as globalState } from '@hookstate/core';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const TopBarAccountInfo = (): React.ReactElement<Props> => {
    const userState = globalState(state.userStateRef);
    const history = useHistory(); // https://stackoverflow.com/questions/31079081/programmatically-navigate-using-react-router

    const handleLogoutClick = (): void => {
        state.userStateRef.set((userModel) => {
            userModel.logOut();
            history.push(nav.Routes.PublicMainTo);
            return userModel;
        });
    };

    let loginInfo;
    if (!userState.value.isLoggedIn) {
        loginInfo = <Link to={nav.Routes.LoginRoute.getRoute()}>Login</Link>;
    } else {
        loginInfo = (
            <a onClick={handleLogoutClick}>
                Logout {"'"}
                {userState.value.username}
                {"'"}
            </a>
        );
    }
    return <span className={css.account}>{loginInfo}</span>;
};

export default TopBarAccountInfo as React.FC;
