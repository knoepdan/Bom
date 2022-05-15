/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { AnswerOfNodeVm } from '../models/AnswerOfNodeVm';
import type { ListAnswerOfNodeVm } from '../models/ListAnswerOfNodeVm';
import type { TreeFilterInput } from '../models/TreeFilterInput';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class TreeService {

    /**
     * @returns ListAnswerOfNodeVm 
     * @throws ApiError
     */
    public static treeGetRootNodes(): CancelablePromise<ListAnswerOfNodeVm> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/main/root',
        });
    }

    /**
     * @param requestBody 
     * @returns ListAnswerOfNodeVm 
     * @throws ApiError
     */
    public static treeGetNodes(
requestBody: TreeFilterInput,
): CancelablePromise<ListAnswerOfNodeVm> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/main/nodes',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * @param pathId 
     * @returns AnswerOfNodeVm 
     * @throws ApiError
     */
    public static treeGetNodeByPathId(
pathId: number,
): CancelablePromise<AnswerOfNodeVm> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/main/nodeByPath/{pathId}',
            path: {
                'pathId': pathId,
            },
        });
    }

    /**
     * @param nodeId 
     * @returns AnswerOfNodeVm 
     * @throws ApiError
     */
    public static treeGetNodeByNodeId(
nodeId: number,
): CancelablePromise<AnswerOfNodeVm> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/main/nodeById/{nodeId}',
            path: {
                'nodeId': nodeId,
            },
        });
    }

}