import * as React from 'react';
import { ExampleClass } from '../utils/example';

interface RProps {
    compiler: string;
    framework: string;
    bundler: string;
}

export class Hello extends React.Component<RProps, {}> {
    componentDidMount(): void {
        const d = new ExampleClass();
        d.callAnything('test call in componentDidMount');
    }

    render(): JSX.Element {
        return (
            <h1>
                This is a {this.props.framework} application using {this.props.compiler} with {this.props.bundler}{' '}
                (process.env.NODE_ENV: {process.env.NODE_ENV})
            </h1>
        );
    }
}
