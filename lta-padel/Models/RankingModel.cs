using lta_padel.Enums;
using System.Collections.Generic;

namespace lta_padel.Models
{
    public class RankingModel
    {
        public RankingModel() {
            this.Players = new List<PlayerModel>();
        }
        public List<PlayerModel> Players { get; set; }
        public RankingTypeEnum Type { get; set; }
    }
}
