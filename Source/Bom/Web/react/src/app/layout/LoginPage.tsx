import React, { useState } from 'react';
//import { useState as globalState } from '@hookstate/core';
//import * as state from 'app/common/UserState';
//import { Right } from 'app/Right';
import { useAppContext } from 'app/AppContext';
import css from 'style/cssClasses';

// TODO -> page probably to be removed

export const LoginPage = (): React.ReactElement => {
    const app = useAppContext();
    const user = app.user;

    const [isAdmin, setIsAdmin] = useState(false);
    const [username, setUsername] = useState('');

    //  const userState = globalState(state.userStateRef);

    const handleLoginClick = (): void => {
        // state.userStateRef.set((userModel) => {
        //     userModel.logIn(username);
        //     if (isAdmin) {
        //         userModel.permissions.push(Right.AdminArea);
        //         userModel.permissions.push(Right.DevArea);
        //     }
        //     return userModel;
        // });
    };

    const handleLogoutClick = (): void => {
        // state.userStateRef.set((userModel) => {
        //     userModel.logOut();
        //     return userModel;
        // });
    };

    let submitFn = (): void => {};
    let controls;
    if (!user.isLoggedIn()) {
        submitFn = handleLoginClick;

        controls = (
            <div>
                <label>
                    Username
                    <br />
                    <input
                        type="text"
                        maxLength={255}
                        value={username}
                        onChange={(e): void => {
                            setUsername(e.target.value);
                        }}
                    ></input>
                </label>
                <br />
                <br />

                <label>
                    Password
                    <br />
                    <input type="password" maxLength={255}></input>
                </label>
                <br />
                <br />

                <button type="submit">Login </button>
                <label className={css('ml10')}>
                    <input
                        type="checkbox"
                        value="true"
                        checked={isAdmin}
                        onChange={(): void => {
                            setIsAdmin(!isAdmin);
                        }}
                    />{' '}
                    As admin
                </label>
            </div>
        );
    } else {
        controls = (
            <div>
                Your are currently logged in as {"'"}
                {user.username}
                {"'"}. <br />
                <br />
                <button onClick={handleLogoutClick}>Logout</button>
            </div>
        );
    }

    return (
        <div className={css('p10')}>
            <br />
            <br />
            <br />
            <form
                onSubmit={(): void => {
                    submitFn();
                }}
            >
                {controls}
            </form>
        </div>
    );
};
export default LoginPage as React.FC;
