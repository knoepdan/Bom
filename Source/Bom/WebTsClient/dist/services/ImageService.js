import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class ImageService {
    static imageOrig(uid) {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Image',
            query: {
                'uid': uid,
            },
        });
    }
}
//# sourceMappingURL=ImageService.js.map