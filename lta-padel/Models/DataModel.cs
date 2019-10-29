using lta_padel.Enums;
using System;
using System.Collections.Generic;

namespace lta_padel.Models
{
    public class DataModel
    {
        public DataModel() {

            for (int i = 0; i < Enum.GetValues(typeof(RankingTypeEnum)).Length; i++) {
                this.Rankings.Add(new RankingModel { Type = (RankingTypeEnum)i });
            }
            
        }
        public List<RankingModel> Rankings { get; set; } = new List<RankingModel>();
        public TournamentModel NextTournament { get; set; } = new TournamentModel();

        public DateTime? LastUpdateDate { get; set; } = null;

    }
}
