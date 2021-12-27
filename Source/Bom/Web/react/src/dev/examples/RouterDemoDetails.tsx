import * as React from 'react';

export const RouterDemoDetails: React.FC = (props) => {
    return (
        <div>
            <h2>
                Details {'"'}
                TODO: not working since upgrading to react-router v6
                {'"'}
            </h2>
            <br />
        </div>
    );
};
/*
import { RouteComponentProps } from 'react-router-dom';

export const RouterDemoDetails: React.FC<RouterDemoDetailProps> = (props) => {
    return (
        <div>
            <h2>
                Details {'"'}
                {props.match.params.testid}
                {'"'}
            </h2>
            <br />
        </div>
    );
};

type RouterDemoDetailProps = RouteComponentProps<{ testid: string }>;
*/
