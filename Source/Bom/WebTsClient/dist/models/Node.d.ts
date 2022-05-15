import type { Path } from './Path';
export declare type Node = {
    nodeId?: number;
    title?: string;
    mainPathId?: number | null;
    mainPath?: Path | null;
};
