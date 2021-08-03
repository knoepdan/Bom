import * as enzyme from 'enzyme';
import * as Adapter from 'enzyme-adapter-react-16';

enzyme.configure({ adapter: new Adapter() });

/*
import { configure } from 'enzyme';
import EnzymeAdapter from 'enzyme-adapter-react-16';


configure({ adapter: new EnzymeAdapter() });
*/
