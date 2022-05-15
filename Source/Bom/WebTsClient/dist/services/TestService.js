import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class TestService {
    static testClearDatabase() {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/test/clearDatabase',
        });
    }
    static testClearAndFillDatabase(typeOfData) {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/test/clearAndFillDatabase',
            query: {
                'typeOfData': typeOfData,
            },
        });
    }
}
//# sourceMappingURL=TestService.js.map