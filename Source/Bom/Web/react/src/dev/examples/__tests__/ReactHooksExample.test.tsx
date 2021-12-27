import * as React from 'react';
import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import { ReactHooksExample } from 'dev/examples/ReactHooksExample';

describe('ReactHooksExample', () => {
    test('renders without crashing given the required props', () => {
        const wrapper = shallow(<ReactHooksExample />);
        expect(toJson(wrapper)).toMatchSnapshot();
    });
});
