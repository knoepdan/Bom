// <reference types="jest" />   -> reference not necessary
import { sum } from '../dev/examples/utils/example';

test('adds 1 + 2 to equal 3', () => {
    expect(sum(1, 2)).toBe(3);
});
