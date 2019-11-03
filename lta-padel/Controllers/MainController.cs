using HtmlAgilityPack;
using lta_padel.Enums;
using lta_padel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using lta_padel.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace lta_padel.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private const string LTAPadelRankingsUrl = "https://www.britishpadel.org.uk/rankings/";
        private const string LTAPadelTournamentsUrl = "https://www.britishpadel.org.uk/tournaments/";
        private const int LTAPadelFirstTableRowThatContainsAPlayerIndex = 4;

        private const string WorldPadelTourTournamentsUrl = "https://www.worldpadeltour.com/torneos/";

        private const string NoDataIsAvailableMessage = "Sorry, data is not available at the moment. Try again after a few minutes.";
        private const string DataIsEmptyMessage = "Data is empty";
        private static DataModel DataInMemory = new DataModel();

        [HttpGet]
        public ActionResult<string> GetPlayersAtPositions([FromQuery] int rankingTypeId, int rankingCategoryId, int numberOfTopPositions)
        {
            var result = string.Empty;

            for (int position = 1; position <= numberOfTopPositions; position++)
            {

                result += GetPlayersAtPosition(rankingTypeId, rankingCategoryId, position);

            }

            return result;

        }

        [HttpGet]
        public ActionResult<string> GetPlayersAtPosition([FromQuery] int rankingTypeId, int rankingCategoryId, int position)
        {
            var ranking = DataInMemory.Rankings.FirstOrDefault(r => r.Type == (RankingTypeEnum)rankingTypeId);

            if (ranking == null)
            {
                return NoDataIsAvailableMessage;
            }

            var rankingCategory = ranking.Categories.FirstOrDefault(c => c.Type == (RankingCategoryTypeEnum)rankingCategoryId);

            if (rankingCategory == null || !rankingCategory.Players.Any())
            {
                return NoDataIsAvailableMessage;
            }

            return CommonHelper.GetPlayersAtPositionText(rankingCategory, position);
        }

        [HttpGet]
        public ActionResult<string> GetTournaments([FromQuery] int rankingTypeId)
        {

            var ranking = DataInMemory.Rankings.FirstOrDefault(r => r.Type == (RankingTypeEnum)rankingTypeId);

            if (ranking == null)
            {
                return NoDataIsAvailableMessage;
            }

            if (!ranking.Tournaments.Any())
            {
                return NoDataIsAvailableMessage;
            }

            var now = DateTime.Now;

            var result = string.Empty;

            //todo improve this check
            var currentTournaments = ranking.Tournaments.Where(t => t.StartDate <= now && t.EndDate >= now).ToList();

            if (currentTournaments.Any())
            {

                result += "Playing now";

                foreach (var currentTournament in currentTournaments)
                {
                    result += $". {currentTournament.Name} {(!string.IsNullOrWhiteSpace(currentTournament.Location) ? $"in {currentTournament.Location}" : "")} from {CommonHelper.GetFormattedTextDate(currentTournament.StartDate)} to {CommonHelper.GetFormattedTextDate(currentTournament.EndDate)}";
                }
            }

            //todo improve this check
            var futureTournaments = ranking.Tournaments.Where(t => t.StartDate >= now && !currentTournaments.Contains(t)).ToList();

            if (futureTournaments.Any())
            {

                result += "Next tournaments";

                foreach (var futureTournament in futureTournaments)
                {
                    result += $". {futureTournament.Name} {(!string.IsNullOrWhiteSpace(futureTournament.Location) ? $"in {futureTournament.Location}" : "")} from {CommonHelper.GetFormattedTextDate(futureTournament.StartDate)} to {CommonHelper.GetFormattedTextDate(futureTournament.EndDate)}";
                }
            }

            return result;

        }

        [HttpGet]
        public ActionResult<string> GetLastUpdatedDate()
        {

            if (DataInMemory.LastUpdateDate == null)
            {
                return NoDataIsAvailableMessage;
            }
            else
            {
                return $"My records were last updated on {CommonHelper.GetFormattedTextDate(DataInMemory.LastUpdateDate.Value)}";
            }

        }

        [HttpGet]
        public async Task<ActionResult<string>> UpdateLTAPadelTournaments()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            long elapsedSeconds;
            HtmlNode debugNode = null; //todo delete

            try
            {
                var doc = await GetHtmlDocument(LTAPadelTournamentsUrl);


                var tournamentCardsNodes = doc.DocumentNode.SelectNodes("//*[@id='av_section_2']//section");

                if (tournamentCardsNodes == null || !tournamentCardsNodes.Any())
                {
                    watch.Stop();
                    elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                    return $"UpdateLTAPadelTournaments. NO TOURNAMENTS FOUND (executing for {elapsedSeconds} seconds).";
                }

                DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].Tournaments = new List<TournamentModel>();
                var now = DateTime.Now;

                foreach (var tournamentCardNote in tournamentCardsNodes)
                {
                    debugNode = tournamentCardNote;
                    if (debugNode.Line == 325)
                    {
                        var debugThisOne = true;
                    }

                    //todo some of them contain h3 instead... 
                    var h4Nodes = tournamentCardNote.SelectNodes(".//h4");
                    var pNodes = tournamentCardNote.SelectNodes(".//p");

                    if (h4Nodes != null && h4Nodes.Count == 2 && pNodes != null && pNodes.Count == 2)
                    {
                        var nameNode = tournamentCardNote.SelectNodes(".//h4")[0];
                        var locationNode = tournamentCardNote.SelectNodes(".//h4")[1];
                        var dateNode = tournamentCardNote.SelectNodes(".//p")[1];

                        if (nameNode != null && !string.IsNullOrWhiteSpace(nameNode.InnerHtml) && dateNode != null)
                        {

                            var dateModel = CommonHelper.ParseLTATournamentDates(dateNode.InnerText);

                            if (dateModel.StartDate != null && dateModel.EndDate != null)
                            {
                                //todo improve this check
                                if (dateModel.StartDate >= now || (dateModel.EndDate != null && dateModel.EndDate <= now))
                                {
                                    var tournament = new TournamentModel();
                                    tournament.Name = nameNode.InnerHtml.Trim();
                                    tournament.Location = (locationNode != null && !string.IsNullOrWhiteSpace(locationNode.InnerHtml) ? locationNode.InnerHtml.Trim() : null);
                                    tournament.StartDate = dateModel.StartDate.Value;
                                    tournament.EndDate = dateModel.EndDate.Value;

                                    DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].Tournaments.Add(tournament);

                                }
                            }

                        }
                    }

                }


                DataInMemory.LastUpdateDate = DateTime.Now;

                watch.Stop();
                elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateLTAPadelTournaments. OK (executing for {elapsedSeconds} seconds). {DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].Tournaments.Count} tournaments.";

            }
            catch (Exception ex)
            {
                watch.Stop();
                elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateLTAPadelTournaments. Error occurred (executing for {elapsedSeconds} seconds): " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message;
            }

        }

        [HttpGet]
        public async Task<ActionResult<string>> UpdateLTAPadelRanking()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var doc = await GetHtmlDocument(LTAPadelRankingsUrl);

                var categoriesCount = DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories.Count;

                var tableNodes = doc.DocumentNode.SelectNodes("//table").Take(categoriesCount);

                if (tableNodes != null && tableNodes.Any())
                {
                    var rankingCategoryTypeId = 0;

                    foreach (var tableNode in tableNodes)
                    {
                        StoreLTAPAdelCategory(tableNode, rankingCategoryTypeId);

                        rankingCategoryTypeId++;
                    }

                    DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].LastUpdateDate = DateTime.Now;
                    DataInMemory.LastUpdateDate = DateTime.Now;
                }


                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateLTAPadelRanking. OK (executing for {elapsedSeconds} seconds). {DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories.Count} categories. {DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories.SelectMany(r => r.Players).Count()} total number of players.";

            }
            catch (Exception ex)
            {
                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateLTAPadelRanking. Error occurred (executing for {elapsedSeconds} seconds): " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message;
            }

        }

        [HttpGet]
        public string UpdateWorldPadelTourRankingFromLocalFile()
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var fileReader = System.IO.File.OpenText("C:\\dev\\world-padel-tour-html.txt");
                var html = fileReader.ReadToEnd();

                UpdateWorldPadelTourRanking(html);

                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateWorldPadelTourRanking. OK (executing for {elapsedSeconds} seconds). {DataInMemory.Rankings.Count} rankings. {DataInMemory.Rankings.SelectMany(c => c.Categories).SelectMany(r => r.Players).Count()} total number of players.";

            }
            catch (Exception ex)
            {
                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateWorldPadelTourRanking. Error occurred (executing for {elapsedSeconds} seconds): " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message;
            }
        }

        [HttpPost]
        public string UpdateWorldPadelTourRankingFromFileUpload()
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                if (!HttpContext.Request.Form.Files.Any())
                {
                    return DataIsEmptyMessage;
                }

                var file = HttpContext.Request.Form.Files[0] as IFormFile;

                if (file == null || file.Length == 0)
                {
                    return DataIsEmptyMessage;
                }

                var html = "";
                using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                {
                    html = reader.ReadToEnd();
                }

                UpdateWorldPadelTourRanking(html);

                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateWorldPadelTourRanking. OK (executing for {elapsedSeconds} seconds). {DataInMemory.Rankings.Count} rankings. {DataInMemory.Rankings.SelectMany(c => c.Categories).SelectMany(r => r.Players).Count()} total number of players.";

            }
            catch (Exception ex)
            {
                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateWorldPadelTourRanking. Error occurred (executing for {elapsedSeconds} seconds): " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message;
            }

        }


        [HttpGet]
        public async Task<ActionResult<string>> UpdateWorldPadelTourTournaments()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            long elapsedSeconds;
            HtmlNode debugNode = null; //todo delete

            try
            {
                var doc = await GetHtmlDocument(WorldPadelTourTournamentsUrl);


                var tournamentCardsNodes = doc.DocumentNode.SelectNodes("//*[@class='c-tournaments__container']");

                if (tournamentCardsNodes == null || !tournamentCardsNodes.Any())
                {
                    watch.Stop();
                    elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                    return $"UpdateWorldPadelTourTournaments. NO TOURNAMENTS FOUND (executing for {elapsedSeconds} seconds).";
                }

                DataInMemory.Rankings[(int)RankingTypeEnum.WORLD_PADEL_TOUR].Tournaments = new List<TournamentModel>();
                var now = DateTime.Now;

                foreach (var tournamentCardNote in tournamentCardsNodes)
                {
                    debugNode = tournamentCardNote;
                    if (debugNode.Line == 325)
                    {
                        var debugThisOne = true;
                    }

                    var h3Nodes = tournamentCardNote.SelectNodes(".//h3");
                    var pNodes = tournamentCardNote.SelectNodes(".//p");

                    if (h3Nodes != null && h3Nodes.Count == 1 && pNodes != null && pNodes.Count == 2)
                    {
                        var nameNode = tournamentCardNote.SelectNodes(".//h3")[0];
                        var dateNode = tournamentCardNote.SelectNodes(".//p")[0];

                        if (nameNode != null && !string.IsNullOrWhiteSpace(nameNode.InnerHtml) && dateNode != null)
                        {

                            var dateModel = CommonHelper.ParseWorldPadelTourTournamentDates(dateNode.InnerText);

                            if (dateModel.StartDate != null && dateModel.EndDate != null)
                            {
                                //todo improve this check
                                if (dateModel.StartDate >= now || (dateModel.EndDate != null && dateModel.EndDate <= now))
                                {
                                    var tournament = new TournamentModel();
                                    tournament.Name = nameNode.InnerHtml.Trim();
                                    tournament.Location = null;
                                    tournament.StartDate = dateModel.StartDate.Value;
                                    tournament.EndDate = dateModel.EndDate.Value;

                                    DataInMemory.Rankings[(int)RankingTypeEnum.WORLD_PADEL_TOUR].Tournaments.Add(tournament);

                                }
                            }

                        }
                    }

                }


                DataInMemory.LastUpdateDate = DateTime.Now;

                watch.Stop();
                elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateWorldPadelTourTournaments. OK (executing for {elapsedSeconds} seconds). {DataInMemory.Rankings[(int)RankingTypeEnum.WORLD_PADEL_TOUR].Tournaments.Count} tournaments.";

            }
            catch (Exception ex)
            {
                watch.Stop();
                elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateWorldPadelTourTournaments. Error occurred (executing for {elapsedSeconds} seconds): " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message;
            }

        }

        [Produces("application/json")]
        public IActionResult Info()
        {
            return Ok(DataInMemory);
        }



        private async Task<HtmlDocument> GetHtmlDocument(string url)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            return doc;
        }

        private void StoreWorldPadelTourCategory(HtmlNode categoryBlockNode, int rankingCategoryTypeId)
        {
            if (categoryBlockNode != null)
            {
                var playersNodes = categoryBlockNode.SelectNodes(".//*[@class='c-player-card__item']");

                if (playersNodes != null && playersNodes.Any())
                {
                    var players = new List<PlayerModel>();

                    foreach (var playerNode in playersNodes)
                    {
                        var positionNode = playerNode.SelectSingleNode(".//*[@class='c-player-card__position']");
                        var nameNode = playerNode.SelectSingleNode(".//*[@class='c-player-card__name']");
                        var flagNode = playerNode.SelectSingleNode(".//*[@class='c-player-card__flag']");
                        var pointsNode = playerNode.SelectSingleNode(".//*[@class='c-player-card__score']");

                        if (positionNode != null
                            && !string.IsNullOrWhiteSpace(positionNode.InnerText)
                            && int.TryParse(positionNode.InnerText, out int result)
                            && nameNode != null && !string.IsNullOrWhiteSpace(nameNode.InnerText)
                            && pointsNode != null && !string.IsNullOrWhiteSpace(pointsNode.InnerText))
                        {
                            var flagCode = flagNode.Attributes.FirstOrDefault(a => a.Name == "src")?.Value.Replace("https://www.worldpadeltour.com/media/images/flags/", "").Replace(".png", "");

                            var country = CommonHelper.GetCountryNameFromFlagName(flagCode);

                            players.Add(new PlayerModel
                            {
                                Position = int.Parse(positionNode.InnerText),
                                FullName = CommonHelper.GetCleanedUpText(nameNode.InnerHtml.Replace("<br>", " ")),
                                Country = CommonHelper.GetCleanedUpText(country),
                                Points = pointsNode.InnerText
                            });

                        }

                    }

                    DataInMemory.Rankings[(int)RankingTypeEnum.WORLD_PADEL_TOUR].Categories[rankingCategoryTypeId].Players = players;
                }
            }
        }

        private void StoreLTAPAdelCategory(HtmlNode categoryTableNode, int rankingCategoryTypeId)
        {
            var playersNodes = categoryTableNode.SelectNodes(".//tbody/tr");

            if (playersNodes != null && playersNodes.Any())
            {

                var players = new List<PlayerModel>();

                var nodeCount = 0;

                foreach (var playerNode in playersNodes)
                {
                    if (nodeCount >= LTAPadelFirstTableRowThatContainsAPlayerIndex)
                    {
                        var positionNode = playerNode.SelectSingleNode(".//td[1]");
                        var nameNode = playerNode.SelectSingleNode(".//td[2]");
                        var surnameNode = playerNode.SelectSingleNode(".//td[3]");
                        var countryNode = playerNode.SelectSingleNode(".//td[4]");
                        var pointsNode = playerNode.SelectSingleNode(".//td[5]");

                        if (positionNode != null
                                && !string.IsNullOrWhiteSpace(positionNode.InnerText)
                                && int.TryParse(positionNode.InnerText, out int result)
                                && nameNode != null && !string.IsNullOrWhiteSpace(nameNode.InnerText)
                                && pointsNode != null && !string.IsNullOrWhiteSpace(pointsNode.InnerText))
                        {

                            players.Add(new PlayerModel
                            {
                                Position = int.Parse(positionNode.InnerText),
                                FullName = nameNode.InnerText + (surnameNode != null && string.IsNullOrWhiteSpace(surnameNode.InnerText) ? " " + CommonHelper.GetCleanedUpText(surnameNode.InnerText) : ""),
                                Country = (countryNode != null ? CommonHelper.GetCleanedUpText(countryNode.InnerText) : ""),
                                Points = pointsNode.InnerText
                            });

                        }

                    }

                    nodeCount++;
                }


                DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories[rankingCategoryTypeId].Players = players;

            }
        }

        private void UpdateWorldPadelTourRanking(string html)
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(html);

            var mensBlockNode = doc.DocumentNode.SelectNodes("//*[@class='c-ranking__block']")[0];
            var ladiesBlockNode = doc.DocumentNode.SelectNodes("//*[@class='c-ranking__block']")[1];

            if (mensBlockNode != null && ladiesBlockNode != null)
            {
                StoreWorldPadelTourCategory(mensBlockNode, (int)RankingCategoryTypeEnum.Men);
                StoreWorldPadelTourCategory(ladiesBlockNode, (int)RankingCategoryTypeEnum.Ladies);

                DataInMemory.Rankings[(int)RankingTypeEnum.WORLD_PADEL_TOUR].LastUpdateDate = DateTime.Now;
                DataInMemory.LastUpdateDate = DateTime.Now;
            }

        }

    }

}