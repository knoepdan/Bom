module.exports = {
    roots: ['<rootDir>/src'],
    modulePaths: ['<rootDir>', '/node_modules'],
    transform: {
        '^.+\\.tsx?$': 'ts-jest',
    },
    testRegex: '(/__tests__/.*|(\\.|/)(test|spec))\\.tsx?$',
    moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx', 'json', 'node'],

    // Setup Enzyme  (see src/setupEnzyme.ts)
    snapshotSerializers: ['enzyme-to-json/serializer'],
    setupFilesAfterEnv: ['<rootDir>/src/setupEnzyme.ts'],
    // "setupTestFrameworkScriptFile": "<rootDir>/src/setupEnzyme.ts", depreceated in favor of setupFilesAfterEnv
};

/*

https://github.com/cedrickchee/react-typescript-jest-enzyme-testing
...
The testRegex tells Jest to look for tests in any __tests__ folder AND also any files anywhere that use the (.test|.spec).(ts|tsx) extension e.g. checkbox.test.tsx etc.
..
*/
