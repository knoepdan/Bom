/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Path } from './Path';

export type Node = {
    nodeId?: number;
    title?: string;
    mainPathId?: number | null;
    mainPath?: Path | null;
};