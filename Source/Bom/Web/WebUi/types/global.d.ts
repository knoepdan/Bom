/// <reference types="node" />
/// <reference types="react" />
/// <reference types="react-dom" />

declare namespace NodeJS {
    interface ProcessEnv {
        NODE_ENV: 'development' | 'production' | 'test';
        //PUBLIC_URL: string; -> not needed
    }
}

// for style loader
declare module '*.css' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const styles: any;
    export = styles;
}

declare module '*.jpg' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const styles: any;
    export = styles;
}

declare module '*.png' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const styles: any;
    export = styles;
}

declare module '*.svg' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const styles: any;
    export = styles;
}

declare module '*.gif' {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const styles: any;
    export = styles;
}

// Omit type https://github.com/Microsoft/TypeScript/issues/12215#issuecomment-377567046
//type Omit<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>
type PartialPick<T, K extends keyof T> = Partial<T> & Pick<T, K>;

/* other ways of definitions devinitions (used by react-scripts) */
/*
declare module '*.png' {
    const src: string;
    export default src;
  }
  declare module '*.svg' {
    import * as React from 'react';
  
    export const ReactComponent: React.SFC<React.SVGProps<SVGSVGElement>>;
  
    const src: string;
    export default src;
  }
  
  declare module '*.module.css' {
    const classes: { [key: string]: string };
    export default classes;
  }
  */
