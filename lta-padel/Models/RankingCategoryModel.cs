using lta_padel.Enums;
using System.Collections.Generic;

namespace lta_padel.Models
{
    public class RankingCategoryModel
    {
        public RankingCategoryModel() {
            this.Players = new List<PlayerModel>();
        }

        public RankingCategoryTypeEnum Type { get; set; }
        public List<PlayerModel> Players { get; set; }
    }
}
