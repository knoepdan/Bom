/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class TestService {

    /**
     * @returns binary 
     * @throws ApiError
     */
    public static testClearDatabase(): CancelablePromise<Blob> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/test/clearDatabase',
        });
    }

    /**
     * @param typeOfData 
     * @returns binary 
     * @throws ApiError
     */
    public static testClearAndFillDatabase(
typeOfData?: string | null,
): CancelablePromise<Blob> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/test/clearAndFillDatabase',
            query: {
                'typeOfData': typeOfData,
            },
        });
    }

}