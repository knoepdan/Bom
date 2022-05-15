/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Node } from './Node';

export type Path = {
    pathId?: number;
    level?: number;
    nodePathString?: string;
    nodeId?: number;
    node?: Node;
    allNodeIds?: Array<number>;
};