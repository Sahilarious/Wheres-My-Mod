using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.ModelBinding.Binders;
using WheresMyMod.Models;
using WheresMyMod.Requests;
using WheresMyMod.Services;

namespace WheresMyMod.Services
{
    public class ModsService : IModsService
    {

        private SqlConnection con()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["WheresMyModDbConnection"]
                .ConnectionString);
        }


        // GET ALL - LOGIC that is then called in the Api controller
        public List<Mod> GetAll()
        {
            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Mods_SelectAll";
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    var mods = new List<Mod>();

                    while (reader.Read())
                    {
                        mods.Add(new Mod()
                        {
                            Id = (int) reader["id"],
                            Name = (string) reader["name"],
                            PageUrl = (string) reader["pageUrl"],
                            PicUrl = (string) reader["picUrl"],
                            GameId = (int) reader["gameId"]
                        });
                    }

                    return mods;
                }
            }
        }

        public List<Mod> GetByGameId(int gameId)
        {
            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Mods_SelectByGameId";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@GameId", gameId));

                using (var reader = cmd.ExecuteReader())
                {
                    List<Mod> mods = new List<Mod>();
                    while (reader.Read())
                    {
                        mods.Add(new Mod
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            PageUrl = (string)reader["PageUrl"],
                            PicUrl = (string)reader["PicUrl"],
                            GameId = (int)reader["GameId"]
                        });
                    }
                    return mods;
                }
            }
        }

        public Mod GetById(int id)
        {
            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Mods_SelectById";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Id", id));

                using (var reader = cmd.ExecuteReader())
                {
                    Mod mod = null;
                    while (reader.Read())
                    {
                        mod = new Mod()
                        {
                            Id = (int) reader["id"],
                            Name = (string) reader["name"],
                            PageUrl = (string) reader["pageUrl"],
                            PicUrl = (string) reader["picUrl"],
                            GameId = (int) reader["gameId"]

                        };
                    }
                    return mod;
                }
            }
        }

        public int Post(ModAddRequest model)
        {
            using (var con = this.con())
            {
                int Id = 0;
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Mods_Insert";
                cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters.Add(new SqlParameter("@Id", id));
                SqlParameter idParameter = new SqlParameter("@Id", SqlDbType.Int);
                idParameter.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(idParameter);
                cmd.Parameters.Add(new SqlParameter("@Name", model.Name));
                cmd.Parameters.Add(new SqlParameter("@PageUrl", model.PageUrl));
                cmd.Parameters.Add(new SqlParameter("@PicUrl", model.PicUrl));
                cmd.Parameters.Add(new SqlParameter("@GameId", model.GameId));

                cmd.ExecuteNonQuery();

                Int32.TryParse(cmd.Parameters["@Id"].Value.ToString(), out Id);

                return Id;
            }
        }

        public int Put(ModUpdateRequest model)
        {
            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Mods_Update";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Id", model.Id));
                cmd.Parameters.Add(new SqlParameter("@Name", model.Name));
                cmd.Parameters.Add(new SqlParameter("@PageUrl", model.PageUrl));
                cmd.Parameters.Add(new SqlParameter("@PicUrl", model.PicUrl));
                cmd.Parameters.Add(new SqlParameter("@GameId", model.GameId));

                cmd.ExecuteNonQuery();
            }

            return model.Id;
        }

        public int Delete(int id)
        {
            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Mods_Delete";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Id", id));

                cmd.ExecuteNonQuery();
            }

            return id;
        }


        // SCRAPE FOR ALL MODS
        public List<Mod> Scrape()
        {

            GamesService gs = new GamesService();

            var gamesArray = gs.GetAll();

            Dictionary<string, int> games = new Dictionary<string, int>();

            foreach (var game in gamesArray)
            {
                games.Add(game.Name, game.Id);
            }


            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Mods_Truncate";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();
            }



            var webClient = new WebClient();

            List<ModAddRequest> mods = new List<ModAddRequest>();
            List<Mod> modList = new List<Mod>();

            for (var i = 1; i < 15; i++)
            {
                var html = webClient.DownloadString(
                    "https://www.nexusmods.com/games/mods/searchresults/?src_order=2&src_sort=0&src_view=1&src_tab=1&src_language=0&page=" +
                    i + "&pUp=1");

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var nodes = htmlDocument.DocumentNode.Descendants()
                    .Where(node =>
                        node.Attributes["class"] != null && node.Attributes["class"].Value.Contains("block-list"))
                    .FirstOrDefault()
                    .Descendants()
                    .Where(node =>
                        node.Attributes["class"] != null && node.Attributes["class"].Value.Contains("popbox"));


                foreach (var node in nodes)
                {
                    var name = HttpUtility.HtmlDecode(node.Descendants("a")
                        .Select(node1 => node1.InnerText)
                        .LastOrDefault());

                    if (games.ContainsKey(name))
                    {
                        var text = node.Descendants("div")
                            .Where(node1 => node1.Attributes["class"].Value.Contains("bubble-collapse"))
                            .FirstOrDefault()
                            .Descendants("div")
                            .Where(node1 => node1.Attributes["class"].Value.Contains(""))
                            .Select(node1 => node1.InnerText)
                            .FirstOrDefault().Split(new string[] { "Released" }, StringSplitOptions.None)[0];

                        var newText = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                        newText = newText.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().Distinct().ToArray();

                        var modName = HttpUtility.HtmlDecode(newText[0]);

                        var pageUrl = node.Descendants("a")
                            .Select(node1 => node1.Attributes["href"].Value)
                            .FirstOrDefault();

                        var picUrl = node.Descendants("img")
                            .FirstOrDefault()
                            .Attributes["src"].Value;


                        mods.Add(new ModAddRequest
                        {
                            Name = modName,
                            PageUrl = pageUrl,
                            PicUrl = picUrl,
                            GameId = games[name]
                        });
                    }
                }
            }

            foreach (var mod in mods)
            {

                modList.Add(new Mod()
                {
                    Id = Post(mod),
                    Name = mod.Name,
                    PageUrl = mod.PageUrl,
                    PicUrl = mod.PicUrl,
                    GameId = mod.GameId
                });
            }
            return modList;
        }

        // SCRAPE BY GAME ID
        public List<Mod> ScrapeByGameId(ModScrape model)
        {

            GamesService gs = new GamesService();

            var gamesArray = gs.GetAll();

            Dictionary<int, string> games = new Dictionary<int, string>();

            foreach (var game in gamesArray)
            {
                games.Add(game.Id, game.Name);
            }

            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Mods_DeleteByGameId";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@GameId", model.GameId));

                cmd.ExecuteNonQuery();
            }



            var webClient = new WebClient();

            List<ModAddRequest> mods = new List<ModAddRequest>();
            List<Mod> modList = new List<Mod>();

            for (var i = 1; i < model.PageNum + 1; i++)
            {
                var html = webClient.DownloadString(
                    "https://www.nexusmods.com/games/mods/searchresults/?src_order=2&src_sort=0&src_view=1&src_tab=1&src_language=0&page=" +
                    i + "&pUp=1");

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var nodes = htmlDocument.DocumentNode.Descendants()
                    .Where(node =>
                        node.Attributes["class"] != null && node.Attributes["class"].Value.Contains("block-list"))
                    .FirstOrDefault()
                    .Descendants()
                    .Where(node =>
                        node.Attributes["class"] != null && node.Attributes["class"].Value.Contains("popbox"));


                foreach (var node in nodes)
                {
                    var name = HttpUtility.HtmlDecode(node.Descendants("a")
                        .Select(node1 => node1.InnerText)
                        .LastOrDefault());

                    if (games[model.GameId] == name)
                    {
                        var text = node.Descendants("div")
                            .Where(node1 => node1.Attributes["class"].Value.Contains("bubble-collapse"))
                            .FirstOrDefault()
                            .Descendants("div")
                            .Where(node1 => node1.Attributes["class"].Value.Contains(""))
                            .Select(node1 => node1.InnerText)
                            .FirstOrDefault().Split(new string[] {"Released"}, StringSplitOptions.None)[0];

                        var newText = text.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

                        newText = newText.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().Distinct().ToArray();

                        var modName = HttpUtility.HtmlDecode(newText[0]);

                        var pageUrl = node.Descendants("a")
                            .Select(node1 => node1.Attributes["href"].Value)
                            .FirstOrDefault();

                        var picUrl = node.Descendants("img")
                            .FirstOrDefault()
                            .Attributes["src"].Value;


                        mods.Add(new ModAddRequest
                        {
                            Name = modName,
                            PageUrl = pageUrl,
                            PicUrl = picUrl,
                            GameId = model.GameId

                        });
                    }
                }
            }

            foreach (var mod in mods)
            {

                modList.Add(new Mod()
                {
                    Id = Post(mod),
                    Name = mod.Name,
                    PageUrl = mod.PageUrl,
                    PicUrl = mod.PicUrl,
                    GameId = model.GameId
                });



            }
            return modList;

        }
    }
}