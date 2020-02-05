import * as React from 'react';
import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import { Hello } from 'src/app/components/Hello';

describe('App', () => {
    test('renders without crashing given the required props', () => {
        const props = {
            compiler: 'testc-ompiler',
            framework: 'test-framework',
            bundler: 'test-bundler',
        };
        const wrapper = shallow(<Hello {...props} />);
        expect(toJson(wrapper)).toMatchSnapshot();
    });
});

/*
-- some other test
describe('My Test Suite', () => {
  it('My Test Case', () => {
    expect(true).toEqual(true);
  });
}); 

-- other test (not 100% sure if it is with same frameworks)
import React from 'react';
import { render } from '@testing-library/react';
import App from './App';

test('renders learn react link', () => {
  const { getByText } = render(<App />);
  const linkElement = getByText(/learn react/i);
  expect(linkElement).toBeInTheDocument();
});
*/
