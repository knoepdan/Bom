import * as React from 'react';
import { Suspense } from 'react';
import { SideNav } from 'app/layout/SideNav';
import { TopBar } from 'app/layout/TopBar';
import css from 'style/cssClasses';
import { useAppContext } from 'app/AppContext';
import { Routes, Route } from 'react-router-dom';
import loc from 'app/Localizer';
import ErrorBoundary from 'common/react/ErrorBoundary';

interface SuspenseContainerProps {
    element: JSX.Element;
}

const SuspenseContainer: React.FC<SuspenseContainerProps> = (props: SuspenseContainerProps) => {
    return <Suspense fallback={<div>{loc.localize('Common_Loading', 'Loading...')}</div>}>{props.element}</Suspense>;
};

const ErrorDisplay = (): React.ReactElement => {
    return (
        <div className={css('p10', 'm10')}>
            <br />
            <br />
            <br />
            {loc.localize('Main_errorMsg', 'Oh no there was an error. That should not have happened.')}
            <br />
            {loc.localize('Main_errorMsgApology', 'We are sorry for the inconvenience')}
            <br />
            {loc.localize('Main_errorMsgRecommendation', 'It might be necessary to refresh the browser.')}
        </div>
    );
};

export const MainContainer = (): React.ReactElement => {
    const app = useAppContext();
    const navModel = app.getNavModel();
    return (
        <>
            <TopBar />
            <SideNav />
            <div className={css('mainContent', 'sideNavSpace', 'p2')}>
                <div>
                    <ErrorBoundary fallbackComponent={<ErrorDisplay></ErrorDisplay>}>
                        <Routes>
                            {navModel.getRoutes().map((route, i) => {
                                const r = route.getRoute();
                                const compFunc = route.getComponentFunc();
                                return <Route key={i} path={r} element={<SuspenseContainer element={compFunc()} />} />;
                            })}
                        </Routes>
                    </ErrorBoundary>
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
