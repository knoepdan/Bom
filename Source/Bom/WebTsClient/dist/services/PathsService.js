import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class PathsService {
    static pathsGetPaths() {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Paths',
        });
    }
    static pathsGetPath(id) {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Paths/{id}',
            path: {
                'id': id,
            },
        });
    }
    static pathsPutPath(id, requestBody) {
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
//# sourceMappingURL=PathsService.js.map