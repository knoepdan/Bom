import * as React from 'react';
import loc from 'app/Localizer';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const PublicPage = (): React.ReactElement<Props> => {
    return <div>{loc.localizeDynamic('Tmp.SomePage', 'Some page')}</div>;
};

export default PublicPage as React.FC;
