(function () {
    "use strict";
    angular
        .module("wmmApp")
        .controller("allModsController", AllModsController);

    AllModsController.$Inject = ['modsService', 'gamesService', '$location', '$window'];

    function AllModsController(modsService, gamesService, $window) {
        var exAllMod = this;

        exAllMod.modsService = modsService;
        exAllMod.gamesService = gamesService;

        exAllMod.postMod = _postMod;
        exAllMod.putMod = _putMod;
        exAllMod.modPage = _modPage;
        exAllMod.scrapeByGameId = _scrapeByGameId;

        exAllMod.scrapePageNum = 1;

        exAllMod.$onInit = _init;


        function _init() {

            //exAllMod.gameId = parseInt($window.location.href.split('=')[1]);
            //console.log(exAllMod.gameId);

            //exAllMod.gameService.getById(exAllMod.gameId)
            //    .then(function (data) {
            //        console.log("get game by Id data");
            //        exAllMod.game = data;


            //        return exAllMod.modService.getAll(exAllMod.gameId);
            //    }).then(_getByGameIdSuccess, _getByGameIdError);


            //function _getByGameIdSuccess(data) {
            //    console.log(data);
            //    exAllMod.mods = data;
            //}

            //function _getByGameIdError(error) {

            //}

            exAllMod.modsService.getAll()
                .then(_getAllSuccess, _getAllError);

            function _getAllSuccess(data) {
                console.log(data);
                exAllMod.mods = data;
            }

            function _getAllError(error) {

            }


            //exAllMod.modService.getById(3)
            //    .then(_getByIdSuccess, _getByIdError);

            //function _getByIdSuccess(data) {
            //    console.log(data);
            //    exAllMod.mod = data;
            //}

            //function _getByIdError(error) {

            //}


            //exAllMod.updateModName = null;


        }


        function _scrapeByGameId() {
            exAllMod.modsService.scrapeByGameId({
                "gameId": exAllMod.gameId,
                "pageNum": 10
            }).then(_scrapeSuccess, _scrapeError);

            function _scrapeSuccess(data) {
                console.log(data);
                exAllMod.mods = data;
            }

            function _scrapeError(error) {

            }
        }


        function _postMod() {

            var newMod = {
                "name": exAllMod.modName,
                "description": "",
                "pageUrl": "",
                "picUrl": "",
                "gameId": 1
            };

            console.log(newMod);

            exAllMod.modsService.post(newMod)
                .then(_postSuccess, _postError);


            function _postSuccess(data) {
                console.log(data);
            }

            function _postError(error) {

            }

        }


        function _putMod() {
            var newModName = exAllMod.updateModName;
            if (newModName === null) {
                newModName = "";
            }
            var newMod = {
                "id": parseInt(exAllMod.updateModId),
                "name": newModName
            };

            exAllMod.modsService.put(newMod)
                .then(_putSuccess, _putError);

            function _putSuccess(data) {
                console.log(data);
            }

            function _putError(error) {
                console.log(error);
            }
        }



        function _modPage(mod) {
            //$window.open(game.pageUrl + "?tb=mod&pUp=1");

            $window.open(mod.pageUrl);
        }

    }
})();


