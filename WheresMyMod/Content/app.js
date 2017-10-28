(function() {
        "use strict";

        var app = angular.module('wmmApp', ['ui.router']);

        app.config(_configureStates);
       
        _configureStates.$Inject = ['$stateProvider'];

        function _configureStates($stateProvider) {
            $stateProvider
                .state({
                    name: 'gamesIndex',
                    component: 'gamesIndex',
                    url: '/gamesIndex'
                });

            $stateProvider
                .state({
                    name: 'modsIndex',
                    component: 'modsIndex',
                    url: '/modsIndex'
                });

            $stateProvider
                .state({
                    name: 'gameMods',
                    component: 'gameMods',
                    url: '/gameMods/?{id}'
                });
        }
    })();