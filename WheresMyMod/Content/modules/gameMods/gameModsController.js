    (function () {
        angular
        .module('wmmApp')
        .controller("gameModsController", GameModsController);

        GameModsController.$Inject = ['modsService', 'gamesService', '$location', '$window', '$stateParams'];

        function GameModsController(modsService, gamesService, $window, $stateParams) {
        var exMod = this;

        exMod.modsService = modsService;
        exMod.gamesService = gamesService;

        exMod.postMod = _postMod;
        exMod.putMod = _putMod;
        exMod.modPage = _modPage;
        exMod.scrapeByGameId = _scrapeByGameId;

        exMod.scrapePageNum = 1;


        exMod.$onInit = _init;


        if ($stateParams.id) {
            exMod.gameId = parseInt($stateParams.id);
        }
        else {
            exMod.gameId = null;
        }
        console.log(exMod.gameId);

        function _init() {

            //exMod.gameId = parseInt($window.location.href.split('=')[1]);

         

            exMod.gamesService.getById(exMod.gameId)
                .then(function (data) {
                    console.log("get game by Id data");
                    exMod.game = data;
        
                    return exMod.modsService.getByGameId(exMod.gameId);
                }).then(_getByGameIdSuccess, _getByGameIdError);


            function _getByGameIdSuccess(data) {
                console.log(data);
                exMod.mods = data;
            }

            function _getByGameIdError(error) {
                
            }

            //exMod.modService.getAll()
            //    .then(_getAllSuccess, _getAllError);

            //function _getAllSuccess(data) {
            //    console.log(data);
            //    exMod.mods = data;
            //}

            //function _getAllError(error) {

            //}


            //exMod.modService.getById(3)
            //    .then(_getByIdSuccess, _getByIdError);

            //function _getByIdSuccess(data) {
            //    console.log(data);
            //    exMod.mod = data;
            //}

            //function _getByIdError(error) {

            //}


            //exMod.updateModName = null;


        }


        function _scrapeByGameId() {
            exMod.modService.scrapeByGameId({
                "gameId": exMod.gameId,
                "pageNum": 10
            }).then(_scrapeSuccess, _scrapeError);

            function _scrapeSuccess(data) {
                console.log(data);
                exMod.mods = data;
            }

            function _scrapeError(error) {

            }
        }


        function _postMod() {
                
            var newMod = {
                "name": exMod.modName,
                "description": "",
                "pageUrl": "",
                "picUrl": "",
                "gameId": 1
            };

            console.log(newMod);

            exMod.modService.post(newMod)
                .then(_postSuccess, _postError);


            function _postSuccess(data) {
                console.log(data);
            }

            function _postError(error) {
                
            }
        }

        function _putMod() {
            var newModName = exMod.updateModName ;
            if (newModName === null) {
                newModName = "";
            }
            var newMod = {
                "id": parseInt(exMod.updateModId),
                "name": newModName
            };

            exMod.modService.put(newMod)
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


