import * as React from 'react';
import { SideNav } from 'app/layout/SideNav';
import { TopBar } from 'app/layout/TopBar';
import css from 'style/cssClasses';
import { useAppContext } from 'app/AppContext';
import { Routes, Route } from 'react-router-dom';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// Info regarding react-rounter
// - <Rout path="/" exact  -> without exact the path matches with a startwith logic. So normally, path="/" is used with exact.
// - <Switch - ensures only one component (the first one matching the path ) is returned

export const MainContainer = (): React.ReactElement<Props> => {
    const app = useAppContext();
    const navModel = app.getNavModel();
    return (
        <>
            <TopBar />
            <SideNav />
            <div className={css('mainContent', 'sideNavSpace',  'p2')}>
                <div>
                    <Routes>
                        {navModel.getRoutes().map((route, i) => {
                            const r = route.getRoute();
                            const compFunc = route.getComponentFunc();
                            return <Route key={i} path={r} element={compFunc()} />;
                        })}
                    </Routes>
                </div>
            </div>
        </>
    );
};

export default MainContainer as React.FC;

/*

    const navModel = nav.getNavigation(userStateRef.value);
 <Switch>
                        {navModel.getRoutes().map((route, index) => (
                            <Route
                                key={index}
                                path={route.getRoute()}
                                exact={route.exact}
                                component={route.getComponent()}
                            />
                        ))}
                    </Switch>
*/
/*
Config.loadConfigFile(true)
    .then((r) => {
        // Info regarding react-rounter
        // - <Rout path="/" exact  -> without exact the path matches with a startwith logic. So normally, path="/" is used with exact.
        // - <Switch - ensures only one component (the first one matching the path ) is returned
        ReactDOM.render(
            ,
            document.getElementById('root'),
        );
    })
    .catch((ex) => {
        alert('Error loading configuration: ' + ex.message);
        console.error('Error loading config: ' + ex.message);
    });
*/
