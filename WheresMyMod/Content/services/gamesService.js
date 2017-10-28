(function() {
    "use strict";
    angular
        .module('wmmApp')
        .factory("gamesService", gamesService);

    gamesService.$inject = ['$http', '$q'];

    function gamesService($http, $q) {

        return {
            getAll: _getAll,
            scrape: _scrape,
            getById: _getById
        };

        function _getAll() {
            var settings = {
                url: '/api/games',
                method: 'GET'
            };
            return $http(settings)
                .then(_getAllSuccess, _getAllFailed);

            function _getAllSuccess(response) {
                return response.data;
            }

            function _getAllFailed(error) {
                return $q.reject(error);
            }
        }

        function _scrape() {
            var settings = {
                url: '/api/games/scrape',
                method: 'GET'
            };
            return $http(settings)
                .then(_scrapeSuccess, _scrapeFailed);

            function _scrapeSuccess(response) {
                return response.data;
            }

            function _scrapeFailed(error) {
                return $q.reject(error);
            }
        }

        function _getById(id) {
            var settings = {
                url: '/api/games/' + id,
                method: 'GET'
            };
            return $http(settings)
                .then(_getByIdSuccess, _getByIdFailed);

            function _getByIdSuccess(response) {
                return response.data;
            }

            function _getByIdFailed(error) {
                return $q.reject(error);
            }


        }
    }

})();