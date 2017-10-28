(function () {
    "use strict";

    angular
        .module('wmmApp')
        .factory("modsService", modsService);

    modsService.$Inject = ['$http', '$q'];

    function modsService($http, $q) {

        return {
            getAll: _getAll,
            getByGameId: _getByGameId,
            getById: _getById,
            post: _post,
            put: _put,
            delete: _delete,
            scrape: _scrape,
            scrapeByGameId: _scrapeByGameId
        };

        function _getAll() {
            var settings = {
                url: '/api/mods',
                method: 'GET',
                cache: false
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

        function _getByGameId(gameId) {
            var settings = {
                url: '/api/mods/game/' + gameId,
                method: 'GET',
                cache: false
            };
            return $http(settings)
                .then(_getByGameIdSuccess, _getByGameIdFailed);

            function _getByGameIdSuccess(response) {
                return response.data;
            }

            function _getByGameIdFailed(error) {
                return $q.reject(error);
            }
        }

        function _getById(id) {
            var settings = {
                url: '/api/mods/' + id,
                method: 'GET',
                cache: false
            };
            return $http(settings)
                .then(_getByIdSuccess, _getByIdFailed);

            function _getByIdSuccess(response) {
                return response.data;
            }

            function _getByIdFailed() {
                return $q.reject(error);
            }
        }


        function _post(data) {
            var settings = {
                url: '/api/mods',
                method: 'POST',
                cache: false,
                data: JSON.stringify(data)
            };
            return $http(settings)
                .then(_postSuccess, _postFailed);

            function _postSuccess(response) {
                console.log("post success");
                return response.data;
            }

            function _postFailed(error) {
                console.log(error);
                return $q.reject(error);
            }

        }

        function _put(data) {
            var settings = {
                url: '/api/mods/' + data.id,
                method: 'PUT',
                cache: false,
                data: JSON.stringify(data)
            };
            return $http(settings)
                .then(_putSuccess, _putFailed);

            function _putSuccess(response) {
                return response.data;
            }

            function _putFailed(error) {
                return $q.reject(error);
            }

        }

        function _delete() {

        }


        function _scrape() {
            var settings = {
                url: '/api/mods/scrape',
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

        function _scrapeByGameId(data) {
            var settings = {
                url: '/api/mods/scrape/' + data.gameId,
                method: 'POST',
                cache: false,
                data: data
            };
            return $http(settings)
                .then(_scrapeSuccess, _scrapeError);

            function _scrapeSuccess(response) {
                return response.data;
            }

            function _scrapeError(error) {
                return $q.reject(error);
            }
        }
    }






})();