
// https://dev.to/robertcoopercode/using-eslint-and-prettier-in-a-typescript-project-53jb

/*
// ***** NPM install statementes
npm install prettier  --save-dev
npm install eslint-config-prettier --save-dev
npm install eslint-plugin-prettier --save-dev


// ***** update .eslintrc.js 
'prettier/@typescript-eslint',  // Uses eslint-config-prettier to disable ESLint rules from @typescript-eslint/eslint-plugin that would conflict with prettier
    'plugin:prettier/recommended',  // Enables eslint-plugin-prettier and displays prettier errors as ESLint errors. Make sure this is always the last configuration in the extends array.

*/



module.exports =  {
  semi:  true,
  trailingComma:  'all',
  singleQuote:  true,
  printWidth:  120,
  tabWidth:  4,
};