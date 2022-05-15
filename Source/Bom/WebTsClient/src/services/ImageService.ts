/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class ImageService {

    /**
     * @param uid 
     * @returns binary 
     * @throws ApiError
     */
    public static imageOrig(
uid?: string | null,
): CancelablePromise<Blob> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Image',
            query: {
                'uid': uid,
            },
        });
    }

}