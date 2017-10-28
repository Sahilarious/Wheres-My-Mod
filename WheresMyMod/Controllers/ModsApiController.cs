using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;
using WheresMyMod.Models;
using WheresMyMod.Requests;
using WheresMyMod.Services;

namespace WheresMyMod.Controllers
{
    [RoutePrefix("api/mods")]
    public class ModsApiController : ApiController
    {
        readonly IModsService modsService;
        
        public ModsApiController(IModsService modsService)
        {
            this.modsService = modsService;
        }

        // GET ALL REQUEST
        [Route()][HttpGet]
        public List<Mod> GetAll()
        {
            return modsService.GetAll();
        }

        [Route("game/{gameId:int}")][HttpGet]
        public List<Mod> GetByGameId(int gameId)
        {
            return modsService.GetByGameId(gameId);
        }

        // GET BY ID REQUEST
        [Route("{id:int}")][HttpGet]
        public Mod GetById(int id)
        {
            return modsService.GetById(id);
        }

        [Route()][HttpPost]
        public int Post(ModAddRequest model)
        {
            return modsService.Post(model);
        }


        [Route("{id:int}")][HttpPut]
        public int Put(ModUpdateRequest model)
        {
            return modsService.Put(model);
        }

        [Route("{id:int}")][HttpDelete]
        public int Delete(int id)
        {
            return modsService.Delete(id);
        }

        [Route("scrape")][HttpGet]
        public List<Mod> Scrape()
        {
            return modsService.Scrape();
        }

        [Route("scrape/{id:int}")][HttpPost]
        public List<Mod> ScrapeByGameId(ModScrape model)
        {
            return modsService.ScrapeByGameId(model);
        }

    }


}
