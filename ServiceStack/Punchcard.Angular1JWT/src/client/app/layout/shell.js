(function() {
    'use strict';

    angular
        .module('app.layout')
        .controller('Shell', Shell);

    Shell.$inject = ['$timeout', 'config', 'logger', 'authService'];

    function Shell($timeout, config, logger, authService) {
        /*jshint validthis: true */
        var vm = this;

        vm.title = config.appTitle;
        vm.busyMessage = 'Please wait ...';
        vm.isBusy = true;
        vm.showSplash = true;
        //vm.userName = authService.OidcTokenManager().profile.name;

        activate();

        function activate() {
            logger.success(config.appTitle + ' loaded!', null);
//            Using a resolver on all routes or dataservice.ready in every controller
//            dataservice.ready().then(function(){
//                hideSplash();
            //            });
            vm.mgr = authService.OidcTokenManager();
            if (vm.mgr.expired) {
                vm.mgr.redirectForToken();
            }

            hideSplash();
        }

        function hideSplash() {
            //Force a 1 second delay so we can see the splash.
            $timeout(function() {
                vm.showSplash = false;
            }, 1000);
        }
    }
})();
