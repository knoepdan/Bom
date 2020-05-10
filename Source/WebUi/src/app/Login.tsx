import React, { useState } from 'react';
import * as state from 'app/common/UserState';
import { Right } from 'app/common/Right';

function handleLoginClick(isAdmin: boolean): void {
    const userState = state.userStateRef.useState(false);
    userState.set((userModel) => {
        userModel.logIn('testuser');
        if (isAdmin) {
            userModel.permissions.push(Right.AdminArea);
            userModel.permissions.push(Right.DevArea);
        }
        return userModel;
    });
}

function handleLogoutClick(): void {
    const userState = state.userStateRef.useState(false);
    userState.set((userModel) => {
        userModel.logOut();
        return userModel;
    });
}

export const Login = (): React.ReactElement => {
    console.log('Login rendered');

    const [isAdmin, setIsAdmin] = useState(false);

    const userState = state.userStateRef.useState();
    let button;
    if (!userState.value.isLoggedIn) {
        button = (
            <form onSubmit={() => handleLoginClick(isAdmin)}>
                <label>
                    <input
                        type="checkbox"
                        value="true"
                        checked={isAdmin}
                        onChange={(e) => {
                            setIsAdmin(!isAdmin);
                        }}
                    />
                    admin
                </label>
                <button onClick={handleLogoutClick}>Login {userState.value.username}</button>
            </form>
        );
    } else {
        button = <button onClick={handleLogoutClick}>Logout ({'' + isAdmin})</button>;
    }

    return <div>{button}</div>;
};
export default Login as React.FC;
