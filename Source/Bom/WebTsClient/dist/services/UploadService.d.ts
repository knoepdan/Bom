import type { CancelablePromise } from '../core/CancelablePromise';
export declare class UploadService {
    static uploadUpload(): CancelablePromise<Blob>;
    static uploadTempUpload(fileIdentifier?: string | null): CancelablePromise<Blob>;
}
