import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class TreeService {
    static treeGetRootNodes() {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/main/root',
        });
    }
    static treeGetNodes(requestBody) {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/main/nodes',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    static treeGetNodeByPathId(pathId) {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/main/nodeByPath/{pathId}',
            path: {
                'pathId': pathId,
            },
        });
    }
    static treeGetNodeByNodeId(nodeId) {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/main/nodeById/{nodeId}',
            path: {
                'nodeId': nodeId,
            },
        });
    }
}
//# sourceMappingURL=TreeService.js.map