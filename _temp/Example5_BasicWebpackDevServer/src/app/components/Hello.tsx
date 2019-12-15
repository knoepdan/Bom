import * as React from 'react';

import {TestClass } from './../utils/test'

interface IProps {
    compiler: string,
    framework: string,
    bundler: string
}

export class Hello extends React.Component<IProps, {}> {
    
    componentDidMount(){
        let d = new TestClass();
        d.callAnything('test call in componentDidMount');
      }
    
    render() {
        return <h1>This is a {this.props.framework} application using {this.props.compiler} with {this.props.bundler} (process.env.NODE_ENV: {process.env.NODE_ENV})</h1>
    }
}