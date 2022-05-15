import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class UploadService {
    static uploadUpload() {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Upload',
        });
    }
    static uploadTempUpload(fileIdentifier) {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Upload',
            query: {
                'fileIdentifier': fileIdentifier,
            },
        });
    }
}
//# sourceMappingURL=UploadService.js.map