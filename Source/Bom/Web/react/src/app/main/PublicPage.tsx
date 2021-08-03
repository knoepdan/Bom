import * as React from 'react';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const PublicPage = (): React.ReactElement<Props> => {
    return <div>Some Public Page</div>;
};

export default PublicPage as React.FC;
