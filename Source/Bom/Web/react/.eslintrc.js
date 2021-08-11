// https://dev.to/robertcoopercode/using-eslint-and-prettier-in-a-typescript-project-53jb

/*
// ***** NPM install statementes
npm install eslint --save-dev
npm install @typescript-eslint/parser --save-dev
npm install @typescript-eslint/eslint-plugin --save-dev
npm install eslint-plugin-react --save-dev
*/

module.exports = {
    parser: '@typescript-eslint/parser', // Specifies the ESLint parser
    extends: [
        'plugin:react/recommended', // Uses the recommended rules from @eslint-plugin-react
        'plugin:@typescript-eslint/recommended', // Uses the recommended rules from @typescript-eslint/eslint-plugin
        'plugin:prettier/recommended', // Enables eslint-plugin-prettier and displays prettier errors as ESLint errors. Make sure this is always the last configuration in the extends array.
    ],
    parserOptions: {
        ecmaVersion: 2018, // Allows for the parsing of modern ECMAScript features
        sourceType: 'module', // Allows for the use of imports
        ecmaFeatures: {
            jsx: true, // Allows for the parsing of JSX
        },
    },
    rules: {
        // Place to specify ESLint rules. Can be used to overwrite rules specified from the extended configs
        // e.g. "@typescript-eslint/explicit-function-return-type": "off",
        '@typescript-eslint/no-empty-function': 'off',
        '@typescript-eslint/no-empty-interface': 'off',
        '@typescript-eslint/no-explicit-any': 'off',
    },
    overrides: [
        {
            files: ['*RouterDemoDetails.tsx'],
            rules: {
                'react/prop-types': 'off',
            },
        },
        {
            files: ['**/*/GlobalExampleState.ts', '**/*/BaseState.ts'],
            rules: {
                '@typescript-eslint/explicit-module-boundary-types': 'off',
            },
        },
    ],
    settings: {
        react: {
            version: 'detect', // Tells eslint-plugin-react to automatically detect the version of React to use
        },
    },
};

/*
/// ************ config withouth react *********
module.exports =  {
  parser:  '@typescript-eslint/parser',  
  extends:  [
    'plugin:@typescript-eslint/recommended',  
  ],
 parserOptions:  {
    ecmaVersion:  2018, 
    sourceType:  'module',  
  },
  rules:  {
  },
};
*/
