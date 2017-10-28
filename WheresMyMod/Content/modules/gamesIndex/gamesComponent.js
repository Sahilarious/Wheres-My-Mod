(function() {
    "use strict";
    angular
        .module('wmmApp')
        .component('gamesIndex',
            {
                templateUrl: '/Content/modules/gamesIndex/gamesIndex.html',
                controller: 'gamesController',
                controllerAs: 'exGame'
            });
})();