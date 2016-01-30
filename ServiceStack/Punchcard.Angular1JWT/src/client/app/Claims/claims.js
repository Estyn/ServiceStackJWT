(function() {
    'use strict';

    angular
        .module('app.claims')
        .controller('Claims', Claims);

    /* @ngInject */
    function Claims(dataservice, logger, authService) {
        /*jshint validthis: true */
        var vm = this;
        vm.claims = [];
        vm.title = 'Claims';

        activate();
        vm.access_token = authService.OidcTokenManager().access_token;
        vm.id_token = authService.OidcTokenManager().id_token;
        vm.id_content = authService.OidcTokenManager().profile;
        parseAccessToken(authService.OidcTokenManager().access_token);
        function activate() {
//            Using a resolver on all routes or dataservice.ready in every controller
//            var promises = [getAvengers()];
//            return dataservice.ready(promises).then(function(){
            return getClaims().then(function() {
                logger.info('Activated Claims View');
                
            });
            
        }

        function getClaims() {
            return dataservice.getHelloAuth().then(function(data) {
                vm.claims = data.Claims;
                return vm.claims;
            });
        }

        function parseAccessToken(token) {
            var parts = token.split(".");
            vm.access_token_header = JSON.parse(decode(parts[0]).result);
            vm.access_token_content = JSON.parse(decode(parts[1]).result);
           
        }

        function url_base64_decode(str) {
            var output = str.replace(/-/g, '+').replace(/_/g, '/');
            switch (output.length % 4) {
                case 0:
                    break;
                case 2:
                    output += '==';
                    break;
                case 3:
                    output += '=';
                    break;
                default:
                    throw 'Illegal base64url string!';
            }
            var result = window.atob(output); //polifyll https://github.com/davidchambers/Base64.js
            try {
                return decodeURIComponent(escape(result));
            } catch (err) {
                return result;
            }
        }

         function decode(base64json) {
            var json = null, error = null;
            try {
                json = url_base64_decode(base64json);
                json = JSON.stringify(JSON.parse(json), undefined, 2);
            } catch (e) {
                error = e;
            }
            return { result: json, error: error };
        };
    }
})();
