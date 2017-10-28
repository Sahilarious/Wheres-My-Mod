(function () {
    "use strict";

    angular
        .module('wmmApp')
        .controller('gamesController', GamesController);

    GamesController.$Inject = ['gamesService'];

    function GamesController(gamesService, $window) {
        var exGame = this;
        exGame.gamesService = gamesService;

        exGame.gamePage = _gamePage;
        exGame.scrape = _scrape;

        exGame.$onInit = _init;

        function _init() {
            
            exGame.gamesService.getAll()
                .then(_getAllSuccess, _getAllError);

            function _getAllSuccess(data) {
                exGame.games = data;

                if (exGame.games === [] || exGame.games === null) {
                    _scrape();
                }
            }

            function _getAllError(error) {
                
            }
        }

        function _scrape() {
            exGame.gamesService.scrape()
                .then(_scrapeSuccess, _scrapeError);

            function _scrapeSuccess(data) {
                console.log(data);
                exGame.games = data;
            }

            function _scrapeError(error) {

            }
        }

        function _gamePage(game) {
            //$window.open(game.pageUrl + "?tb=mod&pUp=1");

            $window.open("GameModsPage.html?#id=" + game.id);
        }
    }

})();