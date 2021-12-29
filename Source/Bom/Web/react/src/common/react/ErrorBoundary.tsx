import React, { Component, ErrorInfo, ReactNode } from 'react';

// inspired by https://react-typescript-cheatsheet.netlify.app/docs/basic/getting-started/error_boundaries/

interface Props {
    children: ReactNode;
    fallbackComponent?: JSX.Element;
    onError?: (error: Error, errorInfo: ErrorInfo) => any;
}

interface State {
    hasError: boolean;
}

class ErrorBoundary extends Component<Props, State> {
    public state: State = {
        hasError: false,
    };

    public static getDerivedStateFromError(_: Error): State {
        // Update state so the next render will show the fallback UI.
        return { hasError: true };
    }

    public componentDidCatch(error: Error, errorInfo: ErrorInfo) {
        if (this.props.onError) {
            try {
                this.props.onError(error, errorInfo);
            } catch (e: unknown) {
                console.warn('Error in errorhandling: ' + (e as any).message); // swallow error in logging/error handling
            }
        } else {
            console.error('Uncaught error:', error, errorInfo);
        }
    }

    public render() {
        if (this.state.hasError) {
            if (this.props.fallbackComponent) {
                return this.props.fallbackComponent;
            } else {
                // const s = {
                //     color: 'black',
                //     padding: '20px',
                // };
                return <h1 style={{ padding: '35px' }}>Ups... that should not have happened. We are sorry.</h1>;
            }
        }
        return this.props.children;
    }
}

export default ErrorBoundary;
