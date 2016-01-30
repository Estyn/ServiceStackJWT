(function() {
    'use strict';

    angular
        .module('app.core')
        .factory('authService', authService);

    /* @ngInject */
    function authService() {

        var config = {
            authority: "http://localhost:22530/",
            client_id: "js_oidc",
            redirect_uri: window.location.protocol + "//" + window.location.host + "/callback.html",
            post_logout_redirect_uri: window.location.protocol + "//" + window.location.host + "/index.html",

            // these two will be done dynamically from the buttons clicked, but are
            // needed if you want to use the silent_renew
            response_type: "id_token token",
            scope: "openid profile email api1 api2",

            // this will toggle if profile endpoint is used
            load_user_profile: true,

            // silent renew will get a new access_token via an iframe 
            // just prior to the old access_token expiring (60 seconds prior)
            silent_redirect_uri: window.location.protocol + "//" + window.location.host + "/silent_renew.html",
            silent_renew: false,

            // this will allow all the OIDC protocol claims to be visible in the window. normally a client app 
            // wouldn't care about them or want them taking up space
            filter_protocol_claims: false
        };

        var mgr = new OidcTokenManager(config);

        return { OidcTokenManager: function() { return mgr; } }

    }


    angular
       .module('app.core')
       .factory('oidcInterceptor', oidcInterceptor);
    /* @ngInject */
    function oidcInterceptor(globalConfig, authService) {



        return {
            'request': function (config) {
                if (config.url.indexOf(globalConfig.baseUrl) === 0) {
                    config.headers.Authorization = 'Bearer ' + authService.OidcTokenManager().access_token;
                }
                return config;
            }

        }

    }
})();