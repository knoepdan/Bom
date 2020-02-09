# react-ts-webpack-boilerplate

This is the ultimate lightweight boilerplate needed for a React application using typescript with webpack

npm -v
5.5.1

node -v
v.9.4

run 'npm install'

no cli needed .

Setup:

1.  basics from:
    https://hackernoon.com/react-with-typescript-and-webpack-654f93f34db6
    https://github.com/saurabhpati/react-ts-webpack-boilerplate
2.  removed express and installed webpack-dev-server
    https://www.npmjs.com/package/webpack-dev-server
3.  add environment variables (add scripts in package.json)
    https://www.valentinog.com/blog/webpack/
    https://www.robinwieruch.de/webpack-advanced-setup-tutorial
4.  updated typescript config (strict, cleaned, target changed.. also ensured source maps)
    https://www.typescriptlang.org/docs/handbook/compiler-options.html
5.  integrate eslint and prettier
    https://dev.to/robertcoopercode/using-eslint-and-prettier-in-a-typescript-project-53jb (tslint is deprecated and replaced by eslint)
    https://medium.com/the-node-js-collection/why-and-how-to-use-eslint-in-your-project-742d0bc61ed7 (general)
    https://eslint.org/docs/user-guide/getting-started
    -> apply Eslint+prettier upon save in VS code: install "ESLint (with 'autoFixOnSave..." + "Prettier - Code formatter" (will create vscode settings file)
6.  Setup tests
    -   1. JEST: npm i jest @types/jest ts-jest --dev (plus setup test script)
    -                -> add jest.config.js
    -   2. Enzyme: npm i enzyme @types/enzyme enzyme-to-json enzyme-adapter-react-16 ---dev
           -> adapt jest.config.js and add src/setupEnzyme.ts
    -   3. Fix typings for enzymeadapter-react-16
           https://stackoverflow.com/questions/46435558/could-not-find-declaration-file-for-enzyme-adapter-react-16
           (should have typings: npm install --save @types/enzyme-adapter-react-16 but doesnt seem to work https://www.npmjs.com/package/@types/enzyme-adapter-react-16 )

https://github.com/cedrickchee/react-typescript-jest-enzyme-testing (setup according to this one)
https://medium.com/@tejasupmanyu/setting-up-unit-tests-in-react-typescipt-with-jest-and-enzyme-56634e54703
https://jestjs.io/docs/en/getting-started (first tests)

run: npx jest --watch

other links
https://www.robinwieruch.de/react-testing-jest (good)
npm install --save @types/enzyme-adapter-react-16

https://gist.github.com/jackawatts/1c7a8d3c277ccf4e969675002fe35bc9
https://jestjs.io/docs/en/tutorial-react
https://www.robinwieruch.de/react-testing-jest (good)
https://basarat.gitbooks.io/typescript/docs/testing/jest.html (no longer working)
https://www.pluralsight.com/guides/how-to-test-react-components-in-typescript (good but not using enzype but some other library)

7. State
   https://github.com/avkonst/hookstate#quick-start

8. Routing
   https://github.com/ReactTraining/react-router/tree/master/packages/react-router-dom (not react-router)
   https://www.pluralsight.com/guides/react-router-typescript
   npm install --save-dev @types/react-router-dom

9) CSS (SASS)
   TODO

https://medium.com/a-beginners-guide-for-webpack-2/webpack-loaders-css-and-sass-2cc0079b5b3a
https://dev.to/pixelgoo/how-to-configure-webpack-from-scratch-for-a-basic-website-46a5 (includes css/sass)

10. configuration
    idea: simple js/ts module as a wrapper to config

-   config file is a normal js file that sets some values on windows object that the wrapper access (only the config wrapper will access these)
-   if no config file is present: default values are used (maybe different on webpack variables)
-   if at some point a different system is needed: easy to replace as only wrapper accesses config

// ######### varia

@types/node -> typing for node
------------ info------
https://www.robinwieruch.de/react-libraries#react-state-management
