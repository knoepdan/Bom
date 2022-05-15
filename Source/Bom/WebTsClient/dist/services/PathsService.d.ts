import type { Path } from '../models/Path';
import type { CancelablePromise } from '../core/CancelablePromise';
export declare class PathsService {
    static pathsGetPaths(): CancelablePromise<Array<Path>>;
    static pathsGetPath(id: number): CancelablePromise<Path>;
    static pathsPutPath(id: number, requestBody: Path): CancelablePromise<Blob>;
}
