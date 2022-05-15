import type { Node } from './Node';
export declare type Path = {
    pathId?: number;
    level?: number;
    nodePathString?: string;
    nodeId?: number;
    node?: Node;
    allNodeIds?: Array<number>;
};
