using System.Collections.Generic;
using WheresMyMod.Models;
using WheresMyMod.Requests;

namespace WheresMyMod.Services
{
    public interface IModsService
    {
        List<Mod> GetAll();
        List<Mod> GetByGameId(int id); 
        Mod GetById(int id);
        int Post(ModAddRequest model);
        int Put(ModUpdateRequest model);
        int Delete(int id);
        List<Mod> Scrape();
        List<Mod> ScrapeByGameId(ModScrape model);
    }
}