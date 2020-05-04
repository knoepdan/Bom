import * as React from 'react';
import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import { RouterDemo } from '../RouterDemo';

describe('RouterDemo', () => {
    it('renders without crashing given the required props', () => {
        const props = {
            compiler: 'test-compiler',
            framework: 'test-framework',
            bundler: 'test-bundler',
        };
        const wrapper = shallow(<RouterDemo {...props} />);
        expect(toJson(wrapper)).toMatchSnapshot();
    });
});

/*
-- original test
describe('App', () => {
  it('renders without crashing given the required props', () => {
    const props = {
      isFetching: false,
      dispatch: jest.fn(),
      selectedSubreddit: 'reactjs',
      posts: []
    }
    const wrapper = shallow(<App {...props} />)
    expect(toJson(wrapper)).toMatchSnapshot()
  })
})


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
