import * as React from 'react';
import { useParams } from 'react-router-dom';

export const RouterDemoDetails: React.FC = (props) => {
    let { testid } = useParams();
    return (
        <div>
            <h2>
                Details {'"'}
                {testid}
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
