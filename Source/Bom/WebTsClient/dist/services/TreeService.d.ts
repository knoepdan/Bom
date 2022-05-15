import type { AnswerOfNodeVm } from '../models/AnswerOfNodeVm';
import type { ListAnswerOfNodeVm } from '../models/ListAnswerOfNodeVm';
import type { TreeFilterInput } from '../models/TreeFilterInput';
import type { CancelablePromise } from '../core/CancelablePromise';
export declare class TreeService {
    static treeGetRootNodes(): CancelablePromise<ListAnswerOfNodeVm>;
    static treeGetNodes(requestBody: TreeFilterInput): CancelablePromise<ListAnswerOfNodeVm>;
    static treeGetNodeByPathId(pathId: number): CancelablePromise<AnswerOfNodeVm>;
    static treeGetNodeByNodeId(nodeId: number): CancelablePromise<AnswerOfNodeVm>;
}
