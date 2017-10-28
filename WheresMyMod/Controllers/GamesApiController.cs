using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HtmlAgilityPack;
using WheresMyMod.Models;
using WheresMyMod.Requests;
using WheresMyMod.Services;

namespace WheresMyMod.Controllers
{
    [RoutePrefix("api/games")]
    public class GamesApiController : ApiController
    {
        readonly IGamesService gamesService;

        public GamesApiController(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        [Route()][HttpGet]
        public List<Game> GetAll()
        {
            return gamesService.GetAll();
        }

        [Route("{id:int}")][HttpGet]
        public Game GetById(int id)
        {
            return gamesService.GetById(id);
        }

        [Route()][HttpPost]
        public int Post(GameAddRequest model)
        {
            return gamesService.Post(model);
        }

        [Route("{id:int}")][HttpPut]
        public int Put(GameUpdateRequest model)
        {
            return gamesService.Put(model);
        }

        [Route("{id:int}")][HttpDelete]
        public int Delete(int id)
        {
            return gamesService.Delete(id);
        }

        [Route("scrape")][HttpGet]
        public List<Game> ScrapeGames()
        {
            return gamesService.ScrapeGames();
        }


    }
}
