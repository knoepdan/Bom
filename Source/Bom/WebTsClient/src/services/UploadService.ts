/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class UploadService {

    /**
     * @returns binary 
     * @throws ApiError
     */
    public static uploadUpload(): CancelablePromise<Blob> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Upload',
        });
    }

    /**
     * @param fileIdentifier 
     * @returns binary 
     * @throws ApiError
     */
    public static uploadTempUpload(
fileIdentifier?: string | null,
): CancelablePromise<Blob> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Upload',
            query: {
                'fileIdentifier': fileIdentifier,
            },
        });
    }

}