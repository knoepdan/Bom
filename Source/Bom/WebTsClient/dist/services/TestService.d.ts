import type { CancelablePromise } from '../core/CancelablePromise';
export declare class TestService {
    static testClearDatabase(): CancelablePromise<Blob>;
    static testClearAndFillDatabase(typeOfData?: string | null): CancelablePromise<Blob>;
}
