import * as React from 'react';
import * as style from './Hello.css';

//import ss from 'src/style.css';
//import styles from './../style.css';

interface RProps {
    compiler: string;
    framework: string;
    bundler: string;
}

export class Hello extends React.Component<RProps, {}> {
    /*
    componentDidMount(): void {
        const d = new TestClass();
        d.callAnything('test call in componentDidMount');
    }
*/

    render(): JSX.Element {
        return (
            <div className={style.hello}>
                <h1>
                    This is a {this.props.framework} application using {this.props.compiler} with {this.props.bundler}{' '}
                    (process.env.NODE_ENV: {process.env.NODE_ENV})
                </h1>
            </div>
        );
    }
}
