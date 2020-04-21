# Frontend Boilerplate for React and Typescript

The project does not include **Server-Side Rendering**, **Testing Frameworks** (Jest etc.), **State management** (Redux, Mobx etc.)

Other interesting templates:

-   https://github.com/rokoroku/react-mobx-typescript-boilerplate (is partially based on this)
-   https://scalable-react-ts-boilerplate.herokuapp.com/ (old but maybe good)
-   https://www.npmjs.com/package/react-webpack-typescript-starter (probably good)

### Build tools

-   [x] [Webpack](https://webpack.github.io) 4
    -   [x] [Tree Shaking](https://medium.com/@Rich_Harris/tree-shaking-versus-dead-code-elimination-d3765df85c80)
    -   [x] [Webpack Dev Server](https://github.com/webpack/webpack-dev-server)
-   [x] [Typescript Loader](https://github.com/TypeStrong/ts-loader)
-   [x] [PostCSS Loader](https://github.com/postcss/postcss-loader)
    -   [x] [PostCSS Preset Env](https://preset-env.cssdb.org/)
    -   [x] [CSS modules](https://github.com/css-modules/css-modules)
-   [x] [React Hot Loader](https://github.com/gaearon/react-hot-loader)
-   [x] [Mini CSS Extract Plugin](https://github.com/webpack-contrib/mini-css-extract-plugin)
-   [x] [HTML Webpack Plugin](https://github.com/ampedandwired/html-webpack-plugin)
-   [x] [Testing]
    -   [x] [Jest](https://jestjs.io/)
    -   [x] [Enzyme](https://enzymejs.github.io/enzyme/)

## Installation

```
$ npm ci
```

## Running

```
$ npm start
```

## Build

```
$ npm run build
```

## Deploy (to the [GitHub Pages](https://pages.github.com/))

```
$ npm run deploy
```

## Format code (using [Prettier] or [ESLINT](https://github.com/prettier/prettier))

```
$ npm run prettier
$ npm run lintfix
$ npm run lint  (dry run, will not change anything)
```

## Test

```
$ npm run test
$ npm run test:watch
```

# License

MIT
