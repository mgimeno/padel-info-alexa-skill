using PadelInfo.Enums;
using System;
using System.Collections.Generic;

namespace PadelInfo.Models
{
    public class RankingModel
    {
        public string Name { get; set; }
        public RankingTypeEnum Type { get; set; }
        public DateTime? RankingLastUpdate { get; set; } = null;
        public DateTime? TournamentsLastUpdate { get; set; } = null;
        public List<RankingCategoryModel> Categories { get; set; }  = new List<RankingCategoryModel>();

        public List<TournamentModel> Tournaments { get; set; } = new List<TournamentModel>();
    }
}
