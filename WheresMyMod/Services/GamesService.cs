using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using HtmlAgilityPack;
using WheresMyMod.Models;
using WheresMyMod.Requests;

namespace WheresMyMod.Services
{
    public class GamesService : IGamesService
    {
        private SqlConnection con()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["WheresMyModDbConnection"]
                .ConnectionString);
        }

        public List<Game> GetAll()
        {

            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Games_GetAll";
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    var results = new List<Game>();
                    while (reader.Read())
                    {
                        results.Add(new Game
                        {
                            Id = (int) reader["Id"],
                            Name = (string) reader["Name"],
                            PageUrl = (string) reader["PageUrl"],
                            PicUrl = (string) reader["PicUrl"]
                        });
                    }

                    return results;
                }
            }
        }

        public Game GetById(int id)
        {
            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Games_GetById";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Id", id));

                using (var reader = cmd.ExecuteReader())
                {
                    var game = new Game();
                    while (reader.Read())
                    {
                        game.Id = (int) reader["Id"];
                        game.Name = (string) reader["Name"];
                        game.PageUrl = (string) reader["PageUrl"];
                        game.PicUrl = (string) reader["PicUrl"];
                    }

                    return game;
                }
            }
        }

        public int Post(GameAddRequest model)
        {

            using (var con = this.con())
            {
                int Id = 0;
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Games_Insert";
                cmd.CommandType = CommandType.StoredProcedure;

                var idParameter = new SqlParameter("@Id", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(idParameter);
                cmd.Parameters.Add(new SqlParameter("@Name", model.Name));
                cmd.Parameters.Add(new SqlParameter("@PageUrl", model.PageUrl));
                cmd.Parameters.Add(new SqlParameter("@PicUrl", model.PicUrl));

                cmd.ExecuteNonQuery();
                Int32.TryParse(cmd.Parameters["@Id"].Value.ToString(), out Id);

                return Id;

            }
        }

        public int Put(GameUpdateRequest model)
        {

            using (var con = this.con())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "dbo.Games_Update";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Id", model.Id));
                cmd.Parameters.Add(new SqlParameter("@Name", model.Name));
                cmd.Parameters.Add(new SqlParameter("@PageUrl", model.PageUrl));
                cmd.Parameters.Add(new SqlParameter("@PicUrl", model.PicUrl));

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
                cmd.CommandText = "dbo.Games_Delete";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Id", id));

                cmd.ExecuteNonQuery();


            }
            return id;
        }

        public List<Game> ScrapeGames()
        {
            using (var con = this.con())
            {

                con.Open();

                //--------------Truncates Games Table-----------------------------
                var tCmd = con.CreateCommand();

                tCmd.CommandText = "dbo.Games_Truncate";
                tCmd.CommandType = CommandType.StoredProcedure;

                tCmd.ExecuteNonQuery();
                //-------------------------------------------

                var webClient = new WebClient();
                var html = webClient.DownloadString("https://www.nexusmods.com/games/?");

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var nodes = htmlDocument.DocumentNode.Descendants()
                    .Where(node =>
                        node.Attributes["class"] != null && node.Attributes["class"].Value.Contains("game-link"));

                var gameList = new List<Game>();

                foreach (var node in nodes)
                {
                    var newGame = new GameAddRequest();

                    var gameName = node.Descendants("div")
                        .Where(node1 => node1.Attributes["class"].Value.Contains("name"))
                        .Select(node1 => node1.InnerText)
                        .FirstOrDefault();

                    var pageUrl = node.Attributes["href"].Value;
                    var picUrl = node.Descendants("img").Select(x => x.Attributes["src"].Value).FirstOrDefault();

                    newGame.Name = gameName;
                    newGame.PageUrl = pageUrl;
                    newGame.PicUrl = picUrl;

                    gameList.Add(new Game
                    {
                        Id = Post(newGame),
                        Name = gameName,
                        PageUrl = pageUrl,
                        PicUrl = picUrl
                    });

                }
                return gameList;
            }
        }
    }
}