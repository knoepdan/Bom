import * as React from 'react';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const DeveloperPage = (): React.ReactElement<Props> => {
    return <div>Developer page</div>;
};

export default DeveloperPage as React.FC;
