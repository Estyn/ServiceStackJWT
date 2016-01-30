(function() {
    'use strict';

    angular
        .module('app.claims')
        .run(appRun);

    // appRun.$inject = ['routehelper']

    /* @ngInject */
    function appRun(routehelper) {
        routehelper.configureRoutes(getRoutes());
    }

    function getRoutes() {
        return [
            {
                url: '/claims',
                config: {
                    templateUrl: 'app/claims/claims.html',
                    controller: 'Claims',
                    controllerAs: 'vm',
                    title: 'claims',
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-lock"></i> Claims'
                    }
                }
            }
        ];
    }
})();
