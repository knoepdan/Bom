# react-ts-webpack-boilerplate
This is the ultimate lightweight boilerplate needed for a React application using typescript with webpack

npm -v
5.5.1

node -v
v.9.4

run 'npm install'

no cli needed .




Setup: 
1. basics from: 
https://hackernoon.com/react-with-typescript-and-webpack-654f93f34db6
https://github.com/saurabhpati/react-ts-webpack-boilerplate
2. removed express and installed webpack-dev-server
https://www.npmjs.com/package/webpack-dev-server
3. add environment variables (add scripts in package.json)
https://www.valentinog.com/blog/webpack/
https://www.robinwieruch.de/webpack-advanced-setup-tutorial
4. updated typescript config (strict, cleaned, target changed.. also ensured source maps)
https://www.typescriptlang.org/docs/handbook/compiler-options.html
5. integrate eslint and prettier
https://dev.to/robertcoopercode/using-eslint-and-prettier-in-a-typescript-project-53jb  (tslint is deprecated and replaced by eslint)
https://medium.com/the-node-js-collection/why-and-how-to-use-eslint-in-your-project-742d0bc61ed7  (general)
https://eslint.org/docs/user-guide/getting-started
-> apply Eslint+prettier upon save in VS code: install "ESLint (with 'autoFixOnSave..." + "Prettier - Code formatter"  (will create vscode settings file)
