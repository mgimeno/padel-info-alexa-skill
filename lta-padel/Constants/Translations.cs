using lta_padel.Enums;
using lta_padel.Models;
using System.Collections.Generic;
using System.Linq;

namespace lta_padel.Constants
{
    public static class Translations
    {
        public static string Get(TranslationEnum translationId, int languageId)
        {

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
            else {
                return translationId.ToString();
            }
            

        }

    }
}

