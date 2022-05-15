/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Path } from '../models/Path';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class PathsService {

    /**
     * @returns Path 
     * @throws ApiError
     */
    public static pathsGetPaths(): CancelablePromise<Array<Path>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Paths',
        });
    }

    /**
     * @param id 
     * @returns Path 
     * @throws ApiError
     */
    public static pathsGetPath(
id: number,
): CancelablePromise<Path> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Paths/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * @param id 
     * @param requestBody 
     * @returns binary 
     * @throws ApiError
     */
    public static pathsPutPath(
id: number,
requestBody: Path,
): CancelablePromise<Blob> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Paths/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}