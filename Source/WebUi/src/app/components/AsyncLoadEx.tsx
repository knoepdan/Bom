import React, { Suspense } from 'react';
import { Config } from 'app/utils/Config';
import macroCss from 'app/style/global.macros.module.css';

// Import component lazy load
// (must have default export defined to work with typescript)
// define react components in typescript:
// - export default SimpleText as React.FC;
// - export default SimpleTextAsClassComponent as React.ComponentType
const SimpleText = React.lazy(() => import('app/components/SimpleText'));

/*
// if there is no default export defined, module can still be imported lazy load
// https://stackoverflow.com/questions/54150819/using-react-lazy-with-typescript
const SimpleText = lazy(() =>
  import('app/components/SimpleText')
    .then(({ SimpleText }) => ({ default: SimpleText })),
);
*/

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

export async function handleButtonClick(s: string): Promise<void> {
    const testutils = await import('app/utils/TestFunc');
    testutils.testAlertBox(s);
}

// more or less random notes about webpack setup
export const AsyncLoadEx = (): React.ReactElement<Props> => {
    return (
        <div>
            <div>AsyncLoadComp :-) ({Config.TestEntry})</div>
            <div>
                <button onClick={async () => await handleButtonClick('click ')}>Click button to load code async</button>
            </div>
            <Suspense fallback={<div>Loading that will never be called</div>}>
                <div className={macroCss.solidBox + ' ' + macroCss.p5}>
                    <SimpleText></SimpleText>
                </div>
            </Suspense>

            <Suspense fallback={<div>Loading that will never be called because lazy load component defined</div>}>
                <div className={macroCss.solidBox + ' ' + macroCss.p5}>in suspense but no lazy load defined</div>
            </Suspense>
        </div>
    );
};

/*

// Route-based code splitting (a simple wrapper would also be possible)
import React, { Suspense, lazy } from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';

const Home = lazy(() => import('./routes/Home'));
const About = lazy(() => import('./routes/About'));

const App = () => (
  <Router>
    <Suspense fallback={<div>Loading...</div>}>
      <Switch>
        <Route exact path="/" component={Home}/>
        <Route path="/about" component={About}/>
      </Switch>
    </Suspense>
  </Router>
);


*/
