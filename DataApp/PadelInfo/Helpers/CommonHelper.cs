
using PadelInfo.ExtensionsMethods;
using PadelInfo.Enums;
using PadelInfo.Models;
using System;
using System.Globalization;
using System.Linq;

namespace PadelInfo.Helpers
{
    public static class CommonHelper
    {

        public static bool IsToday(DateTime date)
        {

            var now = DateTime.Now;

            return (date.Year == now.Year && date.Month == now.Month && date.Day == now.Day);

        }    

        public static string GetFormattedTextDate(int languageId, DateTime date, bool includeTime = false)
        {
            if (languageId == (int)LanguageEnum.SPANISH)
            {
                var cultureInfo = new CultureInfo("es-ES");
                return $"{ date.ToString("dddd", cultureInfo)}, { date.Day} de { date.ToString("MMMM", cultureInfo)} de { date.Year}{(includeTime ? $", a las {date.ToString(("H" + (date.Minute != 0 ? ":m" : "")))}" : "")}";
            }
            else
            {
                return $"{ date.ToString("dddd")}, the { date.Day.Ordinal()} of { date.ToString("MMMM")}, { date.Year}{(includeTime ? $", at {date.ToString(("h" + (date.Minute != 0 ? ":m" : "") + " tt"))}" : "")}";
            }
        }

        public static string GetPlayersAtPositionText(int languageId, RankingCategoryModel rankingCategory, int position)
        {
            var playersAtPosition = rankingCategory.Players.Where(p => p.Position == position).ToList();

            if (!playersAtPosition.Any())
            {
                return $"{TranslationHelper.Translate(TranslationEnum.NO_PLAYER_IS_IN_NUMBER, languageId)} {position}. ";
            }

            var result = string.Empty;

            if (playersAtPosition.Count == 1)
            {
                var countryText = (!string.IsNullOrWhiteSpace(playersAtPosition[0].Country) ? " " + TranslationHelper.Translate(TranslationEnum.FROM_LOCATION, languageId) + " " + TranslationHelper.TranslateLocation(playersAtPosition[0].Country,languageId) : "");

                result = $"{TranslationHelper.Translate(TranslationEnum.NUMBER, languageId)} {position} " +
                    $"{TranslationHelper.Translate(TranslationEnum.IS, languageId)} {playersAtPosition[0].FullName}{countryText}, {TranslationHelper.Translate(TranslationEnum.WITH, languageId)} {playersAtPosition[0].Points} {TranslationHelper.Translate(TranslationEnum.POINTS, languageId)}. ";
            }
            else
            {
                result = $"{playersAtPosition.Count} {TranslationHelper.Translate(TranslationEnum.PLAYERS_AT_NUMBER, languageId)} {position}";

                foreach (var playerAtPosition in playersAtPosition)
                {
                    var countryText = (!string.IsNullOrWhiteSpace(playerAtPosition.Country) ? $" {TranslationHelper.Translate(TranslationEnum.FROM_LOCATION, languageId)} " + TranslationHelper.TranslateLocation(playerAtPosition.Country,languageId) : "");
                    result += $". {playerAtPosition.FullName}{countryText}";
                }

                result += $". {TranslationHelper.Translate(TranslationEnum.ALL_OF_THEM_WITH, languageId)} {playersAtPosition[0].Points} {TranslationHelper.Translate(TranslationEnum.POINTS, languageId)}. ";
            }

            return result;
        }

    }
}
