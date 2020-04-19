using PadelInfo.Enums;
using PadelInfo.Models;
using System.Collections.Generic;
using System.Linq;

namespace PadelInfo.Helpers
{
    public static class TranslationHelper
    {

        public static string Translate(TranslationEnum translationId, int languageId)
        {
            //todo this is not a constant file.
            //todo move this outside or somwhere el else

            var list = new List<TranslationModel> {

            new TranslationModel(TranslationEnum.RANKING_DOES_NOT_EXIST,
                "This ranking does not exist",
                "Este ranking no existe"),

            new TranslationModel(TranslationEnum.TOURNAMENTS_ON_THE,
                "Tournaments on the",
                "Torneos del"),

             new TranslationModel(TranslationEnum.CURRENTLY_BEING_PLAYED,
                "Currently being played",
                "Actualmente en juego"),

             new TranslationModel(TranslationEnum.NEXT_TOURNAMENTS,
                "Next tournaments",
                "Próximos torneos"),

             new TranslationModel(TranslationEnum.RECORDS_LAST_UPDATED_ON,
                "My records were last updated on",
                "Mis datos fueron actualizados el"),

             new TranslationModel(TranslationEnum.NO_PLAYER_IS_IN_NUMBER,
                "No player is in number",
                "Ningún jugador está en el número"),

             new TranslationModel(TranslationEnum.ALL_OF_THEM_WITH,
                "All of them with",
                "Todos con"),

             new TranslationModel(TranslationEnum.POINTS,
                "points",
                "puntos"),

             new TranslationModel(TranslationEnum.FROM_LOCATION,
                "from",
                "de"),

             new TranslationModel(TranslationEnum.PLAYERS_AT_NUMBER,
                "players at number",
                "jugadores en el número"),

             new TranslationModel(TranslationEnum.NUMBER,
                "number",
                "número"),

             new TranslationModel(TranslationEnum.IS,
                "is",
                "es"),

             new TranslationModel(TranslationEnum.WITH,
                "with",
                "con"),

             new TranslationModel(TranslationEnum.IN,
                "in",
                "en"),

             new TranslationModel(TranslationEnum.FROM_TIME,
                "from",
                "del"),

             new TranslationModel(TranslationEnum.TO_TIME,
                "to",
                "al"),

              new TranslationModel(TranslationEnum.NO_NEXT_TOURNAMENTS,
                "There are no tournaments in the near future.",
                "No hay torneos próximamente."),

              new TranslationModel(TranslationEnum.RANKING_DOES_NOT_HAVE_CATEGORY,
                "does not have a ranking for this category.",
                "no tiene un ranking para esta categoría."),

              new TranslationModel(TranslationEnum.CATEGORY_DOES_NOT_HAVE_PLAYERS,
                "Currently there is no players in this category.",
                "Actualmente no hay jugadores en esta categoría."),

               new TranslationModel(TranslationEnum.NO_DATA_AVAILABLE,
                "Data currently not available, please try again in a few minutes.",
                "Datos actualmente no disponibles, por favor pruebe de nuevo en unos minutos.")

            };

            var translation = list.Where(t => t.Id == translationId).FirstOrDefault();

            if (translation != null)
            {
                return translation.Values[languageId];
            }
            else
            {
                return translationId.ToString();
            }


        }
        public static string TranslateLocation(string location, int languageId)
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
                else if (languageId == (int)LanguageEnum.SPANISH)
                {

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


                return location;

            }
        }
    }
}
