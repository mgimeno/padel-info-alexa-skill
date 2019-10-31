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
        private const string LTAPadelNextTournamentUrl = "https://www.britishpadel.org.uk/";
        private const int LTAPadelFirstTableRowThatContainsAPlayerIndex = 4;
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
        public ActionResult<string> GetNextTournament([FromQuery] int rankingTypeId)
        {

            var ranking = DataInMemory.Rankings.FirstOrDefault(r => r.Type == (RankingTypeEnum)rankingTypeId);

            if (ranking == null)
            {
                return NoDataIsAvailableMessage;
            }

            if (string.IsNullOrWhiteSpace(ranking.NextTournament.Name))
            {
                return NoDataIsAvailableMessage;
            }

            return $"The next tournament is {ranking.NextTournament.Name}, " +
                $"on {GetFormattedTextDate(ranking.NextTournament.Date)}";

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
                return $"My records were last updated on {GetFormattedTextDate(DataInMemory.LastUpdateDate.Value)}";
            }

        }

        [HttpGet]
        public async Task<ActionResult<string>> UpdateData()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await UpdateLTARankings();
                await UpdateLTANextTournament();
                DataInMemory.LastUpdateDate = DateTime.Now;

                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateData. OK (executing for {elapsedSeconds} seconds). {DataInMemory.Rankings.Count} rankings. {DataInMemory.Rankings.SelectMany(c => c.Categories).SelectMany(r => r.Players).Count()} total number of players.";

            }
            catch (Exception ex)
            {
                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"UpdateData. Error occurred (executing for {elapsedSeconds} seconds): " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message;
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

        [Produces("application/json")]
        public IActionResult Info() {
            return Ok(DataInMemory);
        }

        private string GetFormattedTextDate(DateTime date)
        {
            return $"{ date.ToString("dddd")}, the { date.Day.Ordinal()} of { date.ToString("MMMM")}, { date.Year}, at {date.ToString(("h" + (date.Minute != 0 ? ":m" : "") + " tt"))}";
        }

        private async Task<HtmlDocument> GetHtmlDocument(string url)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            return doc;
        }

        private async Task UpdateLTARankings()
        {

            var doc = await GetHtmlDocument(LTAPadelRankingsUrl);

            var categoriesCount = DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories.Count;

            var tableNodes = doc.DocumentNode.SelectNodes("//table").Take(categoriesCount);

            if (tableNodes.Any())
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
        }

        private async Task UpdateLTANextTournament()
        {
            var doc = await GetHtmlDocument(LTAPadelNextTournamentUrl);

            var h3 = doc.DocumentNode.SelectNodes("//h3")[0];
            var link = h3.SelectSingleNode(".//a");
            var dateObject = doc.DocumentNode.SelectNodes("//*[@data-year]")[0];

            if (h3 != null && link != null && dateObject != null)
            {
                var name = link.InnerText.Replace("Upcoming: ", null).Replace("&#8211;", null).Trim();

                var yearValue = dateObject.Attributes.FirstOrDefault(a => a.Name == "data-year")?.Value;
                var monthValue = dateObject.Attributes.FirstOrDefault(a => a.Name == "data-month")?.Value;
                var dayValue = dateObject.Attributes.FirstOrDefault(a => a.Name == "data-day")?.Value;
                var hoursValue = dateObject.Attributes.FirstOrDefault(a => a.Name == "data-hour")?.Value;
                var minutesValue = dateObject.Attributes.FirstOrDefault(a => a.Name == "data-minute")?.Value;

                if (!string.IsNullOrWhiteSpace(name))
                {
                    DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].NextTournament = new TournamentModel();
                    DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].NextTournament.Name = name;

                    var date = new DateTime(int.Parse(yearValue), (int.Parse(monthValue) + 1), int.Parse(dayValue), int.Parse(hoursValue), int.Parse(minutesValue), 0);
                    DataInMemory.Rankings[(int)RankingTypeEnum.LTA_PADEL].NextTournament.Date = date;
                }

            }

        }

        private void StoreWorldPadelTourCategory(HtmlNode categoryBlockNode, int rankingCategoryTypeId)
        {
            if (categoryBlockNode != null)
            {
                var playersNodes = categoryBlockNode.SelectNodes(".//*[@class='c-player-card__item']");

                if (playersNodes != null)
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

        private void UpdateWorldPadelTourRanking(string html)
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(html);

            var mensBlockNode = doc.DocumentNode.SelectNodes("//*[@class='c-ranking__block']")[0];
            StoreWorldPadelTourCategory(mensBlockNode, (int)RankingCategoryTypeEnum.Men);

            var ladiesBlockNode = doc.DocumentNode.SelectNodes("//*[@class='c-ranking__block']")[1];
            StoreWorldPadelTourCategory(ladiesBlockNode, (int)RankingCategoryTypeEnum.Ladies);

            DataInMemory.Rankings[(int)RankingTypeEnum.WORLD_PADEL_TOUR].LastUpdateDate = DateTime.Now;
            DataInMemory.LastUpdateDate = DateTime.Now;
        }




    }

}