using HtmlAgilityPack;
using lta_padel.Enums;
using lta_padel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using lta_padel.Helpers;

namespace lta_padel.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private const string RankingsUrl = "https://www.britishpadel.org.uk/rankings/";
        private const string NextTournamentUrl = "https://www.britishpadel.org.uk/";
        private const int FirstTableRowThatContainsAPlayerIndex = 4;
        private const string ErrorWhenNoDataIsAvailable = "Sorry, data is not available at the moment. Try again after a few minutes.";
        private static DataModel DataInMemory = new DataModel();

        [HttpGet]
        public ActionResult<string> GetTopPlayers([FromQuery] int topNumberOfPlayers, int rankingTypeId)
        {

            if (!DataInMemory.Rankings.Any())
            {
                return ErrorWhenNoDataIsAvailable;
            }

            var ranking = DataInMemory.Rankings.FirstOrDefault(r => r.Type == (RankingTypeEnum)rankingTypeId);

            if (ranking == null || ranking.Players.Count == 0)
            {
                return ErrorWhenNoDataIsAvailable;
            }

            var result = "The number one is ";

            foreach (var player in ranking.Players.Where(p => p.Position <= topNumberOfPlayers).OrderBy(p => p.Position))
            {

                if (player.Position > 1)
                {
                    result += "Number " + (player.Position) + " is ";
                }

                result += (player.Name + " " + player.Surname + " from " + player.Country + ", with " + player.Points + " points. ");

            }


            return result;
        }

        [HttpGet]
        public ActionResult<string> GetPlayerAtPosition([FromQuery] int position, int rankingTypeId)
        {

            if (!DataInMemory.Rankings.Any())
            {
                return ErrorWhenNoDataIsAvailable;
            }

            var ranking = DataInMemory.Rankings.FirstOrDefault(r => r.Type == (RankingTypeEnum)rankingTypeId);

            if (ranking == null || ranking.Players.Count == 0)
            {
                return ErrorWhenNoDataIsAvailable;
            }


            var playersAtPosition = ranking.Players.Where(p => p.Position == position).ToList();

            if (!playersAtPosition.Any())
            {
                return "No player is in number " + position;
            }

            var result = "Number " + position + " is ";

            foreach (var player in ranking.Players.Where(p => p.Position == position))
            {
                result += (player.Name + " " + player.Surname + " from " + player.Country + ", with " + player.Points + " points. ");
            }


            return result;
        }

        [HttpGet]
        public ActionResult<string> GetNextTournament()
        {

            if (string.IsNullOrWhiteSpace(DataInMemory.NextTournament.Name))
            {
                return ErrorWhenNoDataIsAvailable;
            }

            return $"The next tournament is {DataInMemory.NextTournament.Name}, " +
                $"on {GetTextDate(DataInMemory.NextTournament.Date)}";

        }

        [HttpGet]
        public ActionResult<string> GetLastUpdatedDate()
        {

            if (DataInMemory.LastUpdateDate == null)
            {
                return ErrorWhenNoDataIsAvailable;
            }
            else
            {
                return $"My records were last updated on {GetTextDate(DataInMemory.LastUpdateDate.Value)}";
            }

        }

        [HttpGet]
        public async Task<ActionResult<string>> UpdateData()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await UpdateRankings();
                await UpdateNextTournament();
                DataInMemory.LastUpdateDate = DateTime.Now;

                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"OK (executing for {elapsedSeconds} seconds). {DataInMemory.Rankings.Count} rankings. {DataInMemory.Rankings.SelectMany(r => r.Players).Count()} players. Next Tournament is {DataInMemory.NextTournament.Name} at {DataInMemory.NextTournament.Date.ToLongDateString()}";

            }
            catch (Exception ex)
            {
                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000;

                return $"Error occurred (executing for {elapsedSeconds} seconds): " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message;
            }

        }

        private string GetTextDate(DateTime date)
        {
            return $"{ date.ToString("dddd")}, the { date.Day.Ordinal()} of { date.ToString("MMMM")}, { date.Year}, at { date.ToString("h:m tt")}";
        }

        private async Task<HtmlDocument> GetHtmlDocument(string url)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            return doc;
        }

        private async Task UpdateRankings()
        {
            var doc = await GetHtmlDocument(RankingsUrl);

            var numberOfRankings = Enum.GetValues(typeof(RankingTypeEnum)).Length;

            var tables = doc.DocumentNode.SelectNodes("//table").Take(numberOfRankings);

            if (tables.Any())
            {
                DataInMemory.Rankings.Clear();

                var rankingTypeId = 0;

                foreach (var table in tables)
                {
                    var ranking = new RankingModel
                    {
                        Type = (RankingTypeEnum)rankingTypeId
                    };

                    StoreRankingData(table, ranking);

                    rankingTypeId++;
                }
            }
        }

        private async Task UpdateNextTournament()
        {
            var doc = await GetHtmlDocument(NextTournamentUrl);

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
                    DataInMemory.NextTournament = new TournamentModel();
                    DataInMemory.NextTournament.Name = name;

                    var date = new DateTime(int.Parse(yearValue), (int.Parse(monthValue) + 1), int.Parse(dayValue), int.Parse(hoursValue), int.Parse(minutesValue), 0);
                    DataInMemory.NextTournament.Date = date;
                }

            }

        }



        private void StoreRankingData(HtmlNode tableNode, RankingModel ranking)
        {
            var playersNodes = tableNode.SelectNodes(".//tbody/tr");

            var nodeCount = 0;

            foreach (var playerNode in playersNodes)
            {
                if (nodeCount >= FirstTableRowThatContainsAPlayerIndex)
                {
                    var positionNode = playerNode.SelectSingleNode(".//td[1]");
                    var nameNode = playerNode.SelectSingleNode(".//td[2]");
                    var surnameNode = playerNode.SelectSingleNode(".//td[3]");
                    var countryNode = playerNode.SelectSingleNode(".//td[4]");
                    var pointsNode = playerNode.SelectSingleNode(".//td[5]");

                    if (positionNode != null && int.TryParse(positionNode.InnerText, out int result) && nameNode != null && pointsNode != null)
                    {

                        ranking.Players.Add(new PlayerModel
                        {
                            Position = int.Parse(positionNode.InnerText),
                            Name = nameNode.InnerText,
                            Surname = surnameNode?.InnerText,
                            Country = countryNode?.InnerText,
                            Points = pointsNode.InnerText
                        });

                    }

                }

                nodeCount++;
            }

            if (ranking.Players.Any())
            {
                DataInMemory.Rankings.Add(ranking);
            }
        }


    }
}
