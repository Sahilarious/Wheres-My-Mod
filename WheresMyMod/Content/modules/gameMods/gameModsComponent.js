(function () {
    "use strict";

    angular
        .module('wmmApp')
        .component('gameMods', {
            templateUrl: '/Content/modules/gameMods/gameMods.html',
            controller: 'gameModsController',
            controllerAs: 'exMod'
        });
})();