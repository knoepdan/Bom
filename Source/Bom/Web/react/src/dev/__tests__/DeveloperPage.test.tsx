import * as React from 'react';
import { render, RenderResult } from '@testing-library/react';
import DeveloperPage from './../DeveloperPage';

let documentBody: RenderResult;
describe('<DeveloperPage /> testing-library example test', () => {
    beforeEach(() => {
        documentBody = render(<DeveloperPage />);
    });
    it('shows developer page', () => {
        expect(documentBody.getByText('Developer page')).toBeInTheDocument();
    });
});
