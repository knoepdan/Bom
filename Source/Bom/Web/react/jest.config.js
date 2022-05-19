module.exports = {
    roots: ['<rootDir>/src'],
    modulePaths: ['<rootDir>/src', '/node_modules'],

    testEnvironment: 'jsdom', // set environment to browser (and not node) https://stackoverflow.com/questions/69227566/consider-using-the-jsdom-test-environment

    // Jest transformations -- this adds support for TypeScript using ts-jest
    transform: {
        '^.+\\.tsx?$': 'ts-jest',
    },

    // Runs special logic, such as cleaning up components
    // when using React Testing Library and adds special
    // extended assertions to Jest
    setupFilesAfterEnv: ['@testing-library/jest-dom/extend-expect'],
    // setupFilesAfterEnv: ['./rtl.setup.js'], // could be externalized as well but not necessary as so simple

    // Test spec file resolution pattern
    // Matches parent folder `__tests__` and
    // also files anywhere that use the (.test|.spec).(ts|tsx) extension e.g. checkbox.test.tsx etc.
    testRegex: '(/__tests__/.*|(\\.|/)(test|spec))\\.tsx?$',

    // Module file extensions for importing
    moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx', 'json', 'node'],

    moduleNameMapper: {
        '\\.(css|less|scss|sss|styl)$': '<rootDir>/node_modules/jest-css-modules', // ignore css module imports.: https://www.npmjs.com/package/jest-css-modules
    },
};

/*
#### setting up Jest + React Testing library  (some links, attention, some probably refer to an obsolete version of react-testing library)

https://testing-library.com/docs/  (main page)

https://dev.to/aromanarguello/getting-started-with-jest-react-testing-library-4nga

https://www.pluralsight.com/guides/how-to-test-react-components-in-typescript

https://javascript.plainenglish.io/the-practical-guide-to-start-react-testing-library-with-typescript-d386804a018

 -> also dont forget to install: npm install --save-dev jest-environment-jsdom  (needed for jest from version 28 on)
*/
