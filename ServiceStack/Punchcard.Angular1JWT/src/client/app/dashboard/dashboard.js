(function() {
    'use strict';

    angular
        .module('app.dashboard')
        .controller('Dashboard', Dashboard);

    Dashboard.$inject = ['$q', 'dataservice', 'logger'];

    function Dashboard($q, dataservice, logger) {

        /*jshint validthis: true */
        var vm = this;

      
        vm.title = 'Dashboard';

        init();

        function init() {
            var promises = [];
//            
            return $q.all(promises).then(function() {
                logger.info('Activated Dashboard View');
            });
        }

      
    }
})();
