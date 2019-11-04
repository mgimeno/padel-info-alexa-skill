using lta_padel.Enums;
using System;
using System.Collections.Generic;

namespace lta_padel.Models
{
    public class RankingModel
    {
        public string Name { get; set; }
        public RankingTypeEnum Type { get; set; }
        public DateTime? LastUpdateDate { get; set; } = null;
        public List<RankingCategoryModel> Categories { get; set; }  = new List<RankingCategoryModel>();

        public List<TournamentModel> Tournaments { get; set; } = new List<TournamentModel>();
    }
}
