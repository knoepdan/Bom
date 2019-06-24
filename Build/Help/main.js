var DK = DK || {};
DK.helpFiles = DK.helpFiles || [];

DK.options = {};
DK.options.title = 'Bom API Definition';


DK.swaggerOptions = {}; // for more options see https://github.com/swagger-api/swagger-ui/blob/master/docs/usage/configuration.md
DK.swaggerOptions.layout = 'StandaloneLayout';
DK.swaggerOptions.Wrapper = 'div';
DK.swaggerOptions.WrapperId = 'dk-swagger-ui';
DK.swaggerOptions.docExpansion = "none"; // String=["list", "full", "none"]
DK.swaggerOptions.deepLinking = true; // Boolean=[true, false]
DK.swaggerOptions.showExtensions = true;
DK.swaggerOptions.validatorUrl = null; //
DK.swaggerOptions.supportedSubmitMethods = ['get', 'post', 'put', 'delete']; // Array=["get", "put", "post", "delete", "options", "head", "patch", "trace"]

var setupSwaggerUI = function (tagName, id) {
    var element = document.createElement(tagName);
    element.id = id;
    document.body.appendChild(element);
}(DK.swaggerOptions.Wrapper, DK.swaggerOptions.WrapperId);


/* IE Polyfills START */
(function () {
    if (window.NodeList && !NodeList.prototype.forEach) {
        NodeList.prototype.forEach = function (callback, thisArg) {
            thisArg = thisArg || window;
            for (var i = 0; i < this.length; i++) {
                callback.call(thisArg, this[i], i, this);
            }
        };
    }
})();

(function (arr) {
    arr.forEach(function (item) {
        if (item.hasOwnProperty('remove')) {
            return;
        }
        Object.defineProperty(item, 'remove', {
            configurable: true,
            enumerable: true,
            writable: true,
            value: function remove() {
                if (this.parentNode !== null)
                    this.parentNode.removeChild(this);
            }
        });
    });
})([Element.prototype, CharacterData.prototype, DocumentType.prototype]);
/* IE Polyfills END */


window.onload = function () {
    var ui = SwaggerUIBundle({
        urls: DK.helpFiles,
        dom_id: '#' + DK.swaggerOptions.WrapperId,
        deepLinking: DK.swaggerOptions.deepLinking,
        validatorUrl: DK.swaggerOptions.validatorUrl,
        presets: [
            SwaggerUIBundle.presets.apis,
            SwaggerUIStandalonePreset
        ],
        plugins: [
            SwaggerUIBundle.plugins.DownloadUrl
        ],
        layout: DK.swaggerOptions.layout,
        showExtensions: DK.swaggerOptions.showExtensions,
        supportedSubmitMethods: DK.swaggerOptions.supportedSubmitMethods,
        docExpansion: DK.swaggerOptions.docExpansion,
        onComplete: function () {
			console.log('loading swagger complete');
			window.apiDefLoaded = true;
        },
		
		requestInterceptor: function (request) {
            console.log('request is intercepted!');
			if(window.apiDefLoaded){
				var txtBaseUrl = document.getElementById('txtBaseUrl');
				if(txtBaseUrl && txtBaseUrl.value && txtBaseUrl.value != ''){
					// http://localhost/api/Values should be converted to https://localhost:44373/api/Values
					// baseUrl would behttps://localhost:44373/  (very hacky approach but will do for now)
					if(request.url.startsWith('https://')){
					request.url = request.url.replace('https://localhost', txtBaseUrl.value);						
					}else{
						request.url = request.url.replace('http://localhost', txtBaseUrl.value);
					}
				}
			}
            return request;
        },
		responseInterceptor: function (response) {
            console.log('response is intercepted!');
            return response;
        },
		
		
    });
    window.ui = ui;
	window.apiDefLoaded = false;
    document.querySelector('.topbar-wrapper > a.link span').innerHTML = DK.options.title;
};