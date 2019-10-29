
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
    }
}
