using PadelInfo.Enums;
using System.Collections.Generic;

namespace PadelInfo.Models
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
