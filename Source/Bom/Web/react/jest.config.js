module.exports = {
    roots: ['<rootDir>/src'],
    modulePaths: ['<rootDir>/src', '/node_modules'],
    transform: {
        '^.+\\.tsx?$': 'ts-jest',
    },
    testRegex: '(/__tests__/.*|(\\.|/)(test|spec))\\.tsx?$',
    moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx', 'json', 'node'],
    moduleNameMapper: {
        '\\.(css|less|scss|sss|styl)$': '<rootDir>/node_modules/jest-css-modules',
    }, // ignore css module imports.: https://www.npmjs.com/package/jest-css-modules

    // Setup jest for testing-library (        "@testing-library/jest-dom": "^5.16.1",     "@testing-library/react": "^12.1.2",)
    //setupFilesAfterEnv: ['@testing-library/jest-dom/extend-expect'],

    // Example for setting up enzyme  (also see src/setupEnzyme.ts)   (remark: discarded in favor of testing-library)
    // snapshotSerializers: ['enzyme-to-json/serializer'],
    // setupFilesAfterEnv: ['<rootDir>/src/setupEnzyme.ts'],
    // "setupTestFrameworkScriptFile": "<rootDir>/src/setupEnzyme.ts", depreceated in favor of setupFilesAfterEnv
};

/*

https://github.com/cedrickchee/react-typescript-jest-enzyme-testing
...
The testRegex tells Jest to look for tests in any __tests__ folder AND also any files anywhere that use the (.test|.spec).(ts|tsx) extension e.g. checkbox.test.tsx etc.
..
*/
