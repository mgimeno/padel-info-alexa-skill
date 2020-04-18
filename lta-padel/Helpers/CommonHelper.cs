
using lta_padel.Constants;
using lta_padel.Enums;
using lta_padel.Models;
using System;
using System.Globalization;
using System.Linq;

namespace lta_padel.Helpers
{
    public static class CommonHelper
    {
        public static string Ordinal(this int number)
        {
            var work = number.ToString();
            if ((number % 100) == 11 || (number % 100) == 12 || (number % 100) == 13)
                return work + "th";
            switch (number % 10)
            {
                case 1: work += "st"; break;
                case 2: work += "nd"; break;
                case 3: work += "rd"; break;
                default: work += "th"; break;
            }
            return work;
        }

        public static bool IsToday(DateTime date)
        {

            var now = DateTime.Now;

            return (date.Year == now.Year && date.Month == now.Month && date.Day == now.Day);

        }

        public static DateStartEndModel ParseLTATournamentDates(string text)
        {
            var result = new DateStartEndModel();

            var yearNumber = DateTime.Now.Year;

            text = text
                .ToLower()
                .Replace("th", "")
                .Replace("st", "")
                .Replace("nd", "")
                .Replace("rd", "")
                .Replace("&#038;", "&")
                .Replace("&#8211;", "&")
                .Trim();

            var dates = text.Split("-");
            if (dates.Length == 1)
            {
                dates = text.Split("&");
            }

            var startDateText = dates[0].Trim();
            var endDateText = dates[1].Trim();

            var startDateMonthText = startDateText.Substring(0, 3);

            var startDateMonthNumber = GetMonthNumber(startDateMonthText);

            startDateText = startDateText.Substring(3).Trim(); // remove start date month 

            var startDateDayNumber = int.Parse(startDateText);

            result.StartDate = new DateTime(yearNumber, startDateMonthNumber, startDateDayNumber, 0, 0, 0, 0);


            if (endDateText.Length <= 2)
            {
                //Just day, so month is the same.
                result.EndDate = new DateTime(yearNumber, startDateMonthNumber, int.Parse(endDateText), 23, 59, 59, 999);
            }
            else
            {
                //contains month
                endDateText = endDateText.Substring(3).Trim(); // remove end date month 
                result.EndDate = new DateTime(yearNumber, (startDateMonthNumber + 1), int.Parse(endDateText), 23, 59, 59, 999);
            }

            return result;

        }

        public static DateStartEndModel ParseWorldPadelTourTournamentDates(string text)
        {
            var result = new DateStartEndModel();

            text = text
                .ToLower()
                .Replace("del ", "")
                .Trim();

            var dates = text.Split(" al ");

            var startDateText = dates[0];
            var endDateText = dates[1];

            var startDateParts = startDateText.Split("/");
            var endDateParts = endDateText.Split("/");

            result.StartDate = new DateTime(int.Parse(startDateParts[2]), int.Parse(startDateParts[1]), int.Parse(startDateParts[0]), 0, 0, 0, 0);
            result.EndDate = new DateTime(int.Parse(endDateParts[2]), int.Parse(endDateParts[1]), int.Parse(endDateParts[0]), 23, 59, 59, 999);

            return result;
        }
        private static int GetMonthNumber(string text)
        {
            text = text.ToLower();

            switch (text)
            {
                case "jan":
                    return 1;
                    break;
                case "feb":
                    return 2;
                    break;
                case "mar":
                    return 3;
                    break;
                case "apr":
                    return 4;
                    break;
                case "may":
                    return 5;
                    break;
                case "jun":
                    return 6;
                    break;
                case "jul":
                    return 7;
                    break;
                case "aug":
                    return 8;
                    break;
                case "sep":
                    return 9;
                    break;
                case "oct":
                    return 10;
                    break;
                case "nov":
                    return 11;
                    break;
                case "dec":
                    return 12;
                    break;
            }

            return -1;

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
                return $"{Translations.Get(TranslationEnum.NO_PLAYER_IS_IN_NUMBER, languageId)} {position}. ";
            }

            var result = string.Empty;

            if (playersAtPosition.Count == 1)
            {
                var countryText = (!string.IsNullOrWhiteSpace(playersAtPosition[0].Country) ? " " + Translations.Get(TranslationEnum.FROM_LOCATION, languageId) + " " + TranslateLocation(languageId, playersAtPosition[0].Country) : "");

                result = $"{Translations.Get(TranslationEnum.NUMBER, languageId)} {position} " +
                    $"{Translations.Get(TranslationEnum.IS, languageId)} {playersAtPosition[0].FullName}{countryText}, {Translations.Get(TranslationEnum.WITH, languageId)} {playersAtPosition[0].Points} {Translations.Get(TranslationEnum.POINTS, languageId)}. ";
            }
            else
            {
                result = $"{playersAtPosition.Count} {Translations.Get(TranslationEnum.PLAYERS_AT_NUMBER, languageId)} {position}";

                foreach (var playerAtPosition in playersAtPosition)
                {
                    var countryText = (!string.IsNullOrWhiteSpace(playerAtPosition.Country) ? $" {Translations.Get(TranslationEnum.FROM_LOCATION, languageId)} " + TranslateLocation(languageId, playerAtPosition.Country) : "");
                    result += $". {playerAtPosition.FullName}{countryText}";
                }

                result += $". {Translations.Get(TranslationEnum.ALL_OF_THEM_WITH, languageId)} {playersAtPosition[0].Points} {Translations.Get(TranslationEnum.POINTS, languageId)}. ";
            }

            return result;
        }

        public static string TranslateLocation(int languageId, string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return string.Empty;
            }
            else
            {
                location = location.ToLower().Trim();

                if (languageId == (int)LanguageEnum.ENGLISH)
                {
                    return location;
                }
                else
                {
                    //spanish

                    var result = location;

                    // replace tournament locations that have these as part of the location.
                    result = result.Replace("england", "inglaterra");
                    result = result.Replace("edinburgh", "edimburgo");
                    result = result.Replace("london", "londres");

                    switch (location)
                    {

                        case "british":
                            result = "Reino Unido";
                            break;
                        case "spanish":
                            result = "España";
                            break;
                        case "portuguese":
                            result = "Portugal";
                            break;
                        case "new zealander":
                            result = "Nueva Zelanda";
                            break;
                        case "italian":
                            result = "Italia";
                            break;
                        case "paraguayan":
                            result = "Paraguay";
                            break;
                        case "argentinian":
                            result = "Argentina";
                            break;



                        case "european union":
                            result = "la Unión Europea";
                            break;
                        case "northern ireland":
                            result = "irlanda del norte";
                            break;
                        case "scotland":
                            result = "escocia";
                            break;
                        case "wales":
                            result = "gales";
                            break;
                        case "argentina":
                            result = "la argentina";
                            break;
                        case "austria":
                            result = "austria";
                            break;
                        case "australia":
                            result = "australia";
                            break;
                        case "belgium":
                            result = "bélgica";
                            break;
                        case "bulgaria":
                            result = "bulgaria";
                            break;
                        case "brazil":
                            result = "brasil";
                            break;
                        case "switzerland":
                            result = "suiza";
                            break;
                        case "chile":
                            result = "chile";
                            break;
                        case "germany":
                            result = "alemania";
                            break;
                        case "denmark":
                            result = "dinamarca";
                            break;
                        case "spain":
                            result = "españa";
                            break;
                        case "finland":
                            result = "finlandia";
                            break;
                        case "france":
                            result = "francia";
                            break;
                        case "united kingdom":
                            result = "reino unido";
                            break;
                        case "ireland":
                            result = "irlanda";
                            break;
                        case "italy":
                            result = "italia";
                            break;
                        case "lithuania":
                            result = "lituania";
                            break;
                        case "mexico":
                            result = "méxico";
                            break;
                        case "the netherlands":
                            result = "los países bajos";
                            break;
                        case "norway":
                            result = "noruega";
                            break;
                        case "poland":
                            result = "polonia";
                            break;
                        case "portugal":
                            result = "portugal";
                            break;
                        case "paraguay":
                            result = "paraguay";
                            break;
                        case "qatar":
                            result = "catar";
                            break;
                        case "russia":
                            result = "rusia";
                            break;
                        case "sweden":
                            result = "suecia";
                            break;
                        case "united states of america":
                            result = "los estados unidos de américa";
                            break;
                        case "uruguay":
                            result = "uruguay";
                            break;
                        case "venezuela":
                            result = "venezuela";
                            break;

                    }

                    return result;
                }

            }
        }

        public static string GetCountryFromNationality(string nationality)
        {
            nationality = nationality.ToLower().Trim();

            var result = nationality;

            switch (nationality)
            {
                case "british":
                    result = "United Kingdom";
                    break;
                case "spanish":
                    result = "Spain";
                    break;
                case "portuguese":
                    result = "Portugal";
                    break;
                case "new zealander":
                    result = "New Zealand";
                    break;
                case "italian":
                    result = "Italy";
                    break;
                case "paraguayan":
                    result = "Paraguay";
                    break;
                case "argentinian":
                    result = "Argentina";
                    break;
            }

            return result;
        }

        public static string GetCleanedUpText(string text)
        {

            //todo strip html from playername
            //todo replace enie con algo que suene igual?
            //Quizas guardar nombre en ingles y en espaniol if country is spain

            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            return text
                .ToLower()
                .Replace("Mª", "Maria")
                .Replace("-", " ")
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u")
                .Replace("ü", "u")
                //.Replace("ñ", "n")
                .Trim()
                ;
        }

        public static string GetEnglishCountryNameFromFlagName(string flagName)
        {
            //todo mirar cuales paises hay jugadores y me he dejado

            switch (flagName)
            {
                case "_ASEAN":
                    return "";
                    break;
                case "_African Union(OAS)":
                    return "";
                    break;
                case "_Arab League":
                    return "";
                    break;
                case "_CARICOM":
                    return "";
                    break;
                case "_CIS":
                    return "";
                    break;
                case "_Commonwealth":
                    return "";
                    break;
                case "_England":
                    return "England";
                    break;
                case "_European Union":
                    return "";
                    break;
                case "_Islamic Conference":
                    return "";
                    break;
                case "_Kosovo":
                    return "";
                    break;
                case "_NATO":
                    return "";
                    break;
                case "_Northern Cyprus":
                    return "";
                    break;
                case "_Northern Ireland":
                    return "Northern Ireland";
                    break;
                case "_OPEC":
                    return "";
                    break;
                case "_Olimpic Movement":
                    return "";
                    break;
                case "_Red Cross":
                    return "";
                    break;
                case "_Scotland":
                    return "Scotland";
                    break;
                case "_Somaliland":
                    return "";
                    break;
                case "_United Nations":
                    return "";
                    break;
                case "_Wales":
                    return "Wales";
                    break;
                case "ad":
                    return "";
                    break;
                case "ae":
                    return "";
                    break;
                case "af":
                    return "";
                    break;
                case "ag":
                    return "";
                    break;
                case "ai":
                    return "";
                    break;
                case "al":
                    return "";
                    break;
                case "am":
                    return "";
                    break;
                case "an":
                    return "";
                    break;
                case "ao":
                    return "";
                    break;
                case "aq":
                    return "";
                    break;
                case "ar":
                    return "Argentina";
                    break;
                case "as":
                    return "";
                    break;
                case "at":
                    return "Austria";
                    break;
                case "au":
                    return "Australia";
                    break;
                case "aw":
                    return "";
                    break;
                case "az":
                    return "";
                    break;
                case "ba":
                    return "";
                    break;
                case "bb":
                    return "";
                    break;
                case "bd":
                    return "";
                    break;
                case "be":
                    return "Belgium";
                    break;
                case "bf":
                    return "";
                    break;
                case "bg":
                    return "Bulgaria";
                    break;
                case "bh":
                    return "";
                    break;
                case "bi":
                    return "";
                    break;
                case "bj":
                    return "";
                    break;
                case "bm":
                    return "";
                    break;
                case "bn":
                    return "";
                    break;
                case "bo":
                    return "";
                    break;
                case "br":
                    return "Brazil";
                    break;
                case "bs":
                    return "";
                    break;
                case "bt":
                    return "";
                    break;
                case "bw":
                    return "";
                    break;
                case "by":
                    return "";
                    break;
                case "bz":
                    return "";
                    break;
                case "ca":
                    return "";
                    break;
                case "cd":
                    return "";
                    break;
                case "cf":
                    return "";
                    break;
                case "cg":
                    return "";
                    break;
                case "ch":
                    return "Switzerland";
                    break;
                case "ci":
                    return "";
                    break;
                case "ck":
                    return "";
                    break;
                case "cl":
                    return "Chile";
                    break;
                case "cm":
                    return "";
                    break;
                case "cn":
                    return "";
                    break;
                case "co":
                    return "";
                    break;
                case "cr":
                    return "";
                    break;
                case "cu":
                    return "";
                    break;
                case "cv":
                    return "";
                    break;
                case "cy":
                    return "";
                    break;
                case "cz":
                    return "";
                    break;
                case "de":
                    return "Germany";
                    break;
                case "dj":
                    return "";
                    break;
                case "dk":
                    return "Denmark";
                    break;
                case "dm":
                    return "";
                    break;
                case "do":
                    return "";
                    break;
                case "dz":
                    return "";
                    break;
                case "ec":
                    return "";
                    break;
                case "ee":
                    return "";
                    break;
                case "eg":
                    return "";
                    break;
                case "eh":
                    return "";
                    break;
                case "er":
                    return "";
                    break;
                case "es":
                    return "Spain";
                    break;
                case "et":
                    return "";
                    break;
                case "fi":
                    return "Finland";
                    break;
                case "fj":
                    return "";
                    break;
                case "fm":
                    return "";
                    break;
                case "fo":
                    return "";
                    break;
                case "fr":
                    return "France";
                    break;
                case "ga":
                    return "";
                    break;
                case "gb":
                    return "United Kingdom";
                    break;
                case "gd":
                    return "";
                    break;
                case "ge":
                    return "";
                    break;
                case "gg":
                    return "";
                    break;
                case "gh":
                    return "";
                    break;
                case "gi":
                    return "";
                    break;
                case "gl":
                    return "";
                    break;
                case "gm":
                    return "";
                    break;
                case "gn":
                    return "";
                    break;
                case "gp":
                    return "";
                    break;
                case "gq":
                    return "";
                    break;
                case "gr":
                    return "";
                    break;
                case "gt":
                    return "";
                    break;
                case "gu":
                    return "";
                    break;
                case "gw":
                    return "";
                    break;
                case "gy":
                    return "";
                    break;
                case "hk":
                    return "";
                    break;
                case "hn":
                    return "";
                    break;
                case "hr":
                    return "";
                    break;
                case "ht":
                    return "";
                    break;
                case "hu":
                    return "";
                    break;
                case "id":
                    return "";
                    break;
                case "ie":
                    return "Ireland";
                    break;
                case "il":
                    return "";
                    break;
                case "im":
                    return "";
                    break;
                case "in":
                    return "";
                    break;
                case "iq":
                    return "";
                    break;
                case "ir":
                    return "";
                    break;
                case "is":
                    return "";
                    break;
                case "it":
                    return "Italy";
                    break;
                case "je":
                    return "";
                    break;
                case "jm":
                    return "";
                    break;
                case "jo":
                    return "";
                    break;
                case "jp":
                    return "";
                    break;
                case "ke":
                    return "";
                    break;
                case "kg":
                    return "";
                    break;
                case "kh":
                    return "";
                    break;
                case "ki":
                    return "";
                    break;
                case "km":
                    return "";
                    break;
                case "kn":
                    return "";
                    break;
                case "kp":
                    return "";
                    break;
                case "kr":
                    return "";
                    break;
                case "kw":
                    return "";
                    break;
                case "ky":
                    return "";
                    break;
                case "kz":
                    return "";
                    break;
                case "la":
                    return "";
                    break;
                case "lb":
                    return "";
                    break;
                case "lc":
                    return "";
                    break;
                case "li":
                    return "";
                    break;
                case "lk":
                    return "";
                    break;
                case "lr":
                    return "";
                    break;
                case "ls":
                    return "";
                    break;
                case "lt":
                    return "Lithuania";
                    break;
                case "lu":
                    return "";
                    break;
                case "lv":
                    return "";
                    break;
                case "ly":
                    return "";
                    break;
                case "ma":
                    return "";
                    break;
                case "mc":
                    return "";
                    break;
                case "md":
                    return "";
                    break;
                case "me":
                    return "";
                    break;
                case "mg":
                    return "";
                    break;
                case "mh":
                    return "";
                    break;
                case "mk":
                    return "";
                    break;
                case "ml":
                    return "";
                    break;
                case "mm":
                    return "";
                    break;
                case "mn":
                    return "";
                    break;
                case "mo":
                    return "";
                    break;
                case "mq":
                    return "";
                    break;
                case "mr":
                    return "";
                    break;
                case "ms":
                    return "";
                    break;
                case "mt":
                    return "";
                    break;
                case "mu":
                    return "";
                    break;
                case "mv":
                    return "";
                    break;
                case "mw":
                    return "";
                    break;
                case "mx":
                    return "Mexico";
                    break;
                case "my":
                    return "";
                    break;
                case "mz":
                    return "";
                    break;
                case "na":
                    return "";
                    break;
                case "nc":
                    return "";
                    break;
                case "ne":
                    return "";
                    break;
                case "ng":
                    return "";
                    break;
                case "ni":
                    return "";
                    break;
                case "nl":
                    return "The Netherlands";
                    break;
                case "no":
                    return "Norway";
                    break;
                case "np":
                    return "";
                    break;
                case "nr":
                    return "";
                    break;
                case "nz":
                    return "";
                    break;
                case "om":
                    return "";
                    break;
                case "pa":
                    return "";
                    break;
                case "pe":
                    return "";
                    break;
                case "pf":
                    return "";
                    break;
                case "pg":
                    return "";
                    break;
                case "ph":
                    return "";
                    break;
                case "pk":
                    return "";
                    break;
                case "pl":
                    return "Poland";
                    break;
                case "pr":
                    return "";
                    break;
                case "ps":
                    return "";
                    break;
                case "pt":
                    return "Portugal";
                    break;
                case "pw":
                    return "";
                    break;
                case "py":
                    return "Paraguay";
                    break;
                case "qa":
                    return "Qatar";
                    break;
                case "re":
                    return "";
                    break;
                case "ro":
                    return "";
                    break;
                case "rs":
                    return "";
                    break;
                case "ru":
                    return "Russia";
                    break;
                case "rw":
                    return "";
                    break;
                case "sa":
                    return "";
                    break;
                case "sb":
                    return "";
                    break;
                case "sc":
                    return "";
                    break;
                case "sd":
                    return "";
                    break;
                case "se":
                    return "Sweden";
                    break;
                case "sg":
                    return "";
                    break;
                case "si":
                    return "";
                    break;
                case "sk":
                    return "";
                    break;
                case "sl":
                    return "";
                    break;
                case "sm":
                    return "";
                    break;
                case "sn":
                    return "";
                    break;
                case "so":
                    return "";
                    break;
                case "sr":
                    return "";

                    break;

                case "st":
                    return "";
                    break;
                case "sv":
                    return "";
                    break;
                case "sy":
                    return "";
                    break;
                case "sz":
                    return "";
                    break;
                case "tc":
                    return "";
                    break;
                case "td":
                    return "";
                    break;
                case "tg":
                    return "";
                    break;
                case "th":
                    return "";
                    break;
                case "tj":
                    return "";
                    break;
                case "tl":
                    return "";
                    break;
                case "tm":
                    return "";
                    break;

                case "tn":
                    return "";
                    break;
                case "to":
                    return "";
                    break;
                case "tr":
                    return "";
                    break;
                case "tt":
                    return "";
                    break;
                case "tv":
                    return "";
                    break;
                case "tw":
                    return "";
                    break;
                case "tz":
                    return "";
                    break;
                case "ua":
                    return "";
                    break;
                case "ug":
                    return "";
                    break;
                case "us":
                    return "United States of America";
                    break;
                case "uy":
                    return "Uruguay";
                    break;
                case "uz":
                    return "";
                    break;
                case "va":
                    return "";
                    break;
                case "vc":
                    return "";
                    break;
                case "ve":
                    return "Venezuela";
                    break;
                case "vg":
                    return "";
                    break;
                case "vi":
                    return "";
                    break;
                case "vn":
                    return "";
                    break;
                case "vu":
                    return "";
                    break;
                case "ws":
                    return "";
                    break;
                case "ye":
                    return "";
                    break;
                case "za":
                    return "";
                    break;
                case "zm":
                    return "";
                    break;
                case "zw":
                    return "";
                    break;
            }

            return string.Empty;
        }
    }
}
