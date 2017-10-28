using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheresMyMod.Models;
using WheresMyMod.Requests;

namespace WheresMyMod.Services
{
    public interface IGamesService
    {
        List<Game> GetAll();
        Game GetById(int id);
        int Post(GameAddRequest model);
        int Put(GameUpdateRequest model);
        int Delete(int id);
        List<Game> ScrapeGames();
    }
}
