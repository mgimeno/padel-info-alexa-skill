using lta_padel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lta_padel.Helpers
{
    public static class ParseHelper
    {

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

        public static string GetCountryNameFromFlagName(string flagName)
        {
            //todo mirar cuales paises hay jugadores y me he dejado

            var result = string.Empty;

            switch (flagName)
            {
                case "_ASEAN":
                    result = "";
                    break;
                case "_African Union(OAS)":
                    result = "";
                    break;
                case "_Arab League":
                    result = "";
                    break;
                case "_CARICOM":
                    result = "";
                    break;
                case "_CIS":
                    result = "";
                    break;
                case "_Commonwealth":
                    result = "";
                    break;
                case "_England":
                    result = "England";
                    break;
                case "_European Union":
                    result = "";
                    break;
                case "_Islamic Conference":
                    result = "";
                    break;
                case "_Kosovo":
                    result = "";
                    break;
                case "_NATO":
                    result = "";
                    break;
                case "_Northern Cyprus":
                    result = "";
                    break;
                case "_Northern Ireland":
                    result = "Northern Ireland";
                    break;
                case "_OPEC":
                    result = "";
                    break;
                case "_Olimpic Movement":
                    result = "";
                    break;
                case "_Red Cross":
                    result = "";
                    break;
                case "_Scotland":
                    result = "Scotland";
                    break;
                case "_Somaliland":
                    result = "";
                    break;
                case "_United Nations":
                    result = "";
                    break;
                case "_Wales":
                    result = "Wales";
                    break;
                case "ad":
                    result = "";
                    break;
                case "ae":
                    result = "";
                    break;
                case "af":
                    result = "";
                    break;
                case "ag":
                    result = "";
                    break;
                case "ai":
                    result = "";
                    break;
                case "al":
                    result = "";
                    break;
                case "am":
                    result = "";
                    break;
                case "an":
                    result = "";
                    break;
                case "ao":
                    result = "";
                    break;
                case "aq":
                    result = "";
                    break;
                case "ar":
                    result = "Argentina";
                    break;
                case "as":
                    result = "";
                    break;
                case "at":
                    result = "Austria";
                    break;
                case "au":
                    result = "Australia";
                    break;
                case "aw":
                    result = "";
                    break;
                case "az":
                    result = "";
                    break;
                case "ba":
                    result = "";
                    break;
                case "bb":
                    result = "";
                    break;
                case "bd":
                    result = "";
                    break;
                case "be":
                    result = "Belgium";
                    break;
                case "bf":
                    result = "";
                    break;
                case "bg":
                    result = "Bulgaria";
                    break;
                case "bh":
                    result = "";
                    break;
                case "bi":
                    result = "";
                    break;
                case "bj":
                    result = "";
                    break;
                case "bm":
                    result = "";
                    break;
                case "bn":
                    result = "";
                    break;
                case "bo":
                    result = "";
                    break;
                case "br":
                    result = "Brazil";
                    break;
                case "bs":
                    result = "";
                    break;
                case "bt":
                    result = "";
                    break;
                case "bw":
                    result = "";
                    break;
                case "by":
                    result = "";
                    break;
                case "bz":
                    result = "";
                    break;
                case "ca":
                    result = "";
                    break;
                case "cd":
                    result = "";
                    break;
                case "cf":
                    result = "";
                    break;
                case "cg":
                    result = "";
                    break;
                case "ch":
                    result = "Switzerland";
                    break;
                case "ci":
                    result = "";
                    break;
                case "ck":
                    result = "";
                    break;
                case "cl":
                    result = "Chile";
                    break;
                case "cm":
                    result = "";
                    break;
                case "cn":
                    result = "";
                    break;
                case "co":
                    result = "";
                    break;
                case "cr":
                    result = "";
                    break;
                case "cu":
                    result = "";
                    break;
                case "cv":
                    result = "";
                    break;
                case "cy":
                    result = "";
                    break;
                case "cz":
                    result = "";
                    break;
                case "de":
                    result = "Germany";
                    break;
                case "dj":
                    result = "";
                    break;
                case "dk":
                    result = "Denmark";
                    break;
                case "dm":
                    result = "";
                    break;
                case "do":
                    result = "";
                    break;
                case "dz":
                    result = "";
                    break;
                case "ec":
                    result = "";
                    break;
                case "ee":
                    result = "";
                    break;
                case "eg":
                    result = "";
                    break;
                case "eh":
                    result = "";
                    break;
                case "er":
                    result = "";
                    break;
                case "es":
                    result = "Spain";
                    break;
                case "et":
                    result = "";
                    break;
                case "fi":
                    result = "Finland";
                    break;
                case "fj":
                    result = "";
                    break;
                case "fm":
                    result = "";
                    break;
                case "fo":
                    result = "";
                    break;
                case "fr":
                    result = "France";
                    break;
                case "ga":
                    result = "";
                    break;
                case "gb":
                    result = "United Kingdom";
                    break;
                case "gd":
                    result = "";
                    break;
                case "ge":
                    result = "";
                    break;
                case "gg":
                    result = "";
                    break;
                case "gh":
                    result = "";
                    break;
                case "gi":
                    result = "";
                    break;
                case "gl":
                    result = "";
                    break;
                case "gm":
                    result = "";
                    break;
                case "gn":
                    result = "";
                    break;
                case "gp":
                    result = "";
                    break;
                case "gq":
                    result = "";
                    break;
                case "gr":
                    result = "";
                    break;
                case "gt":
                    result = "";
                    break;
                case "gu":
                    result = "";
                    break;
                case "gw":
                    result = "";
                    break;
                case "gy":
                    result = "";
                    break;
                case "hk":
                    result = "";
                    break;
                case "hn":
                    result = "";
                    break;
                case "hr":
                    result = "";
                    break;
                case "ht":
                    result = "";
                    break;
                case "hu":
                    result = "";
                    break;
                case "id":
                    result = "";
                    break;
                case "ie":
                    result = "Ireland";
                    break;
                case "il":
                    result = "";
                    break;
                case "im":
                    result = "";
                    break;
                case "in":
                    result = "";
                    break;
                case "iq":
                    result = "";
                    break;
                case "ir":
                    result = "";
                    break;
                case "is":
                    result = "";
                    break;
                case "it":
                    result = "Italy";
                    break;
                case "je":
                    result = "";
                    break;
                case "jm":
                    result = "";
                    break;
                case "jo":
                    result = "";
                    break;
                case "jp":
                    result = "";
                    break;
                case "ke":
                    result = "";
                    break;
                case "kg":
                    result = "";
                    break;
                case "kh":
                    result = "";
                    break;
                case "ki":
                    result = "";
                    break;
                case "km":
                    result = "";
                    break;
                case "kn":
                    result = "";
                    break;
                case "kp":
                    result = "";
                    break;
                case "kr":
                    result = "";
                    break;
                case "kw":
                    result = "";
                    break;
                case "ky":
                    result = "";
                    break;
                case "kz":
                    result = "";
                    break;
                case "la":
                    result = "";
                    break;
                case "lb":
                    result = "";
                    break;
                case "lc":
                    result = "";
                    break;
                case "li":
                    result = "";
                    break;
                case "lk":
                    result = "";
                    break;
                case "lr":
                    result = "";
                    break;
                case "ls":
                    result = "";
                    break;
                case "lt":
                    result = "Lithuania";
                    break;
                case "lu":
                    result = "";
                    break;
                case "lv":
                    result = "";
                    break;
                case "ly":
                    result = "";
                    break;
                case "ma":
                    result = "";
                    break;
                case "mc":
                    result = "";
                    break;
                case "md":
                    result = "";
                    break;
                case "me":
                    result = "";
                    break;
                case "mg":
                    result = "";
                    break;
                case "mh":
                    result = "";
                    break;
                case "mk":
                    result = "";
                    break;
                case "ml":
                    result = "";
                    break;
                case "mm":
                    result = "";
                    break;
                case "mn":
                    result = "";
                    break;
                case "mo":
                    result = "";
                    break;
                case "mq":
                    result = "";
                    break;
                case "mr":
                    result = "";
                    break;
                case "ms":
                    result = "";
                    break;
                case "mt":
                    result = "";
                    break;
                case "mu":
                    result = "";
                    break;
                case "mv":
                    result = "";
                    break;
                case "mw":
                    result = "";
                    break;
                case "mx":
                    result = "Mexico";
                    break;
                case "my":
                    result = "";
                    break;
                case "mz":
                    result = "";
                    break;
                case "na":
                    result = "";
                    break;
                case "nc":
                    result = "";
                    break;
                case "ne":
                    result = "";
                    break;
                case "ng":
                    result = "";
                    break;
                case "ni":
                    result = "";
                    break;
                case "nl":
                    result = "The Netherlands";
                    break;
                case "no":
                    result = "Norway";
                    break;
                case "np":
                    result = "";
                    break;
                case "nr":
                    result = "";
                    break;
                case "nz":
                    result = "";
                    break;
                case "om":
                    result = "";
                    break;
                case "pa":
                    result = "";
                    break;
                case "pe":
                    result = "";
                    break;
                case "pf":
                    result = "";
                    break;
                case "pg":
                    result = "";
                    break;
                case "ph":
                    result = "";
                    break;
                case "pk":
                    result = "";
                    break;
                case "pl":
                    result = "Poland";
                    break;
                case "pr":
                    result = "";
                    break;
                case "ps":
                    result = "";
                    break;
                case "pt":
                    result = "Portugal";
                    break;
                case "pw":
                    result = "";
                    break;
                case "py":
                    result = "Paraguay";
                    break;
                case "qa":
                    result = "Qatar";
                    break;
                case "re":
                    result = "";
                    break;
                case "ro":
                    result = "";
                    break;
                case "rs":
                    result = "";
                    break;
                case "ru":
                    result = "Russia";
                    break;
                case "rw":
                    result = "";
                    break;
                case "sa":
                    result = "";
                    break;
                case "sb":
                    result = "";
                    break;
                case "sc":
                    result = "";
                    break;
                case "sd":
                    result = "";
                    break;
                case "se":
                    result = "Sweden";
                    break;
                case "sg":
                    result = "";
                    break;
                case "si":
                    result = "";
                    break;
                case "sk":
                    result = "";
                    break;
                case "sl":
                    result = "";
                    break;
                case "sm":
                    result = "";
                    break;
                case "sn":
                    result = "";
                    break;
                case "so":
                    result = "";
                    break;
                case "sr":
                    result = "";

                    break;

                case "st":
                    result = "";
                    break;
                case "sv":
                    result = "";
                    break;
                case "sy":
                    result = "";
                    break;
                case "sz":
                    result = "";
                    break;
                case "tc":
                    result = "";
                    break;
                case "td":
                    result = "";
                    break;
                case "tg":
                    result = "";
                    break;
                case "th":
                    result = "";
                    break;
                case "tj":
                    result = "";
                    break;
                case "tl":
                    result = "";
                    break;
                case "tm":
                    result = "";
                    break;

                case "tn":
                    result = "";
                    break;
                case "to":
                    result = "";
                    break;
                case "tr":
                    result = "";
                    break;
                case "tt":
                    result = "";
                    break;
                case "tv":
                    result = "";
                    break;
                case "tw":
                    result = "";
                    break;
                case "tz":
                    result = "";
                    break;
                case "ua":
                    result = "";
                    break;
                case "ug":
                    result = "";
                    break;
                case "us":
                    result = "United States of America";
                    break;
                case "uy":
                    result = "Uruguay";
                    break;
                case "uz":
                    result = "";
                    break;
                case "va":
                    result = "";
                    break;
                case "vc":
                    result = "";
                    break;
                case "ve":
                    result = "Venezuela";
                    break;
                case "vg":
                    result = "";
                    break;
                case "vi":
                    result = "";
                    break;
                case "vn":
                    result = "";
                    break;
                case "vu":
                    result = "";
                    break;
                case "ws":
                    result = "";
                    break;
                case "ye":
                    result = "";
                    break;
                case "za":
                    result = "";
                    break;
                case "zm":
                    result = "";
                    break;
                case "zw":
                    result = "";
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

        private static int GetMonthNumber(string text)
        {
            text = text.ToLower();

            var result = -1;

            switch (text)
            {
                case "jan":
                    result = 1;
                    break;
                case "feb":
                    result = 2;
                    break;
                case "mar":
                    result = 3;
                    break;
                case "apr":
                    result = 4;
                    break;
                case "may":
                    result = 5;
                    break;
                case "jun":
                    result = 6;
                    break;
                case "jul":
                    result = 7;
                    break;
                case "aug":
                    result = 8;
                    break;
                case "sep":
                    result = 9;
                    break;
                case "oct":
                    result = 10;
                    break;
                case "nov":
                    result = 11;
                    break;
                case "dec":
                    result = 12;
                    break;
            }

            return result;

        }
    }
}
