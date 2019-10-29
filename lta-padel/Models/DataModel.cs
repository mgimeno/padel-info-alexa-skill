using System;
using System.Collections.Generic;

namespace lta_padel.Models
{
    public class DataModel
    {
        public List<RankingModel> Rankings { get; set; } = new List<RankingModel>();
        public TournamentModel NextTournament { get; set; } = new TournamentModel();

        public DateTime? LastUpdateDate { get; set; } = null;

    }
}
