using lta_padel.Enums;
using lta_padel.Models;
using System.Collections.Generic;
using System.Linq;

namespace lta_padel.Constants
{
    public static class Translations
    {
        public static string Get(TranslationEnum translationId, int languageId) {

            //todo move this outside or somwhere el else

            List<TranslationModel> List = new List<TranslationModel> {

            new TranslationModel(TranslationEnum.NO_DATA_AVAILABLE,
                "Sorry, there was a problem. Please try again in a few minutes",
                "Lo siento, ha habido un problema. Prueba de nuevo dentro de unos minutos"),

            new TranslationModel(TranslationEnum.TOURNAMENTS_ON_THE,
                "Tournaments on the",
                "Torneos del"),

             new TranslationModel(TranslationEnum.CURRENTLY_BEING_PLAYED,
                "Currently being played",
                "Actualmente en juego"),

             new TranslationModel(TranslationEnum.NEXT_TOURNAMENTS,
                "Next tournaments",
                "Proximos torneos"),

             new TranslationModel(TranslationEnum.RECORDS_LAST_UPDATED_ON,
                "My records were last updated on",
                "Mis datos fueron actualizados el"),

             new TranslationModel(TranslationEnum.NO_PLAYER_IS_IN_NUMBER,
                "No player is in number",
                "Ningun jugador esta en el numero"),

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
                "jugadores en el numero"),

             new TranslationModel(TranslationEnum.NUMBER,
                "number",
                "numero"),

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
                "al")

            };

        var translation = List.Where(t => t.Id == translationId).FirstOrDefault();

            return translation.Values[languageId];
        
        }

    }
}

