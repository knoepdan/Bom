import * as React from 'react';
import * as Api from 'api-clients';

interface Props {}

export const ExamplesBomApiPage = (): React.ReactElement<Props> => {
    const [rootNodes, setRootNodes] = React.useState<Api.ListAnswerOfNodeVm>({});

    const loadSomething = async () => {
        try {
            Api.OpenAPI.BASE = 'BomApi';
            let result = await Api.TreeService.treeGetRootNodes();
            setRootNodes(result);
        } catch (e: unknown) {
            console.warn('Error when calling api: ' + e);
        }
    };

    React.useEffect(() => {
        loadSomething();
    }, []);

    let topNodes = new Array<Api.NodeVm>();
    if (rootNodes?.value) {
        topNodes = rootNodes.value;
    }

    return (
        <div>
            Examples Bom API <br />
            Nof root nodes: {topNodes.length}
            <ul>
                {topNodes.map(function (node: Api.NodeVm, index) {
                    return <li key={index}>{node.title}</li>;
                })}
            </ul>
        </div>
    );
};

export default ExamplesBomApiPage as React.FC;
