import * as React from 'react';

//import * as state from 'app/common/UserState';
//import * as nav from 'app/common/NavigationState';
import { Link } from 'react-router-dom'; // useHistory
import css from './TopBarAccountInfo.module.css';

import { useAppContext } from 'app/AppContext';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const TopBarAccountInfo = (): React.ReactElement<Props> => {
    const app = useAppContext();
    const nav = app.getNavModel();
    const user = app.user;

    //const history = useHistory(); // https://stackoverflow.com/questions/31079081/programmatically-navigate-using-react-router

    // TODO this is a hack
    const handleLogoutClick = async (): Promise<void> => {
        const url = 'en/account/logout';
        try {
            const response = await fetch(url, {
                method: 'POST',
                cache: 'no-cache',
            });
            console.log('finished calling logut' + response);
            if (response) {
                console.log('finished calling logut' + response.status + '          ' + response.statusText);
            }
            if (response && response.status >= 200 && response.status <= 400) {
                // reset local state
                user.logOut();
                // state.userStateRef.set((userModel) => {
                //     userModel.logOut();
                //     return userModel;
                // });
            }
        } catch (e: any) {
            debugger;
            console.log('error: ' + e.message);
        }
        window.document.location.reload();
        /*
        state.userStateRef.set((userModel) => {
            userModel.logOut();
            history.push(nav.Routes.PublicMainTo);
            return userModel;
        });
        */
    };

    let loginInfo;
    if (!user.isLoggedIn()) {
        const login = nav.definedRoutes().LoginRoute;
        loginInfo = <Link to={login.getRoute()}>Login</Link>;
    } else {
        loginInfo = (
            <a onClick={handleLogoutClick}>
                Logout {"'"}
                {user.username}
                {"'"}
            </a>
        );
    }
    return <div className={css.account}>{loginInfo}</div>;
};

export default TopBarAccountInfo as React.FC;
