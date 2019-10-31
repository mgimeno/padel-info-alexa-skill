using lta_padel.Enums;
using System;
using System.Collections.Generic;

namespace lta_padel.Models
{
    public class DataModel
    {

        public DataModel(){

            this.Rankings.Add(new RankingModel {
                Type = RankingTypeEnum.WORLD_PADEL_TOUR
            });

            this.Rankings[(int)RankingTypeEnum.WORLD_PADEL_TOUR].Categories.Add(new RankingCategoryModel { Type = RankingCategoryTypeEnum.Men });
            this.Rankings[(int)RankingTypeEnum.WORLD_PADEL_TOUR].Categories.Add(new RankingCategoryModel { Type = RankingCategoryTypeEnum.Ladies });


            this.Rankings.Add(new RankingModel
            {
                Type = RankingTypeEnum.LTA_PADEL
            });

            this.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories.Add(new RankingCategoryModel { Type = RankingCategoryTypeEnum.Men });
            this.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories.Add(new RankingCategoryModel { Type = RankingCategoryTypeEnum.Ladies });
            this.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories.Add(new RankingCategoryModel { Type = RankingCategoryTypeEnum.MenSenior }); 
            this.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories.Add(new RankingCategoryModel { Type = RankingCategoryTypeEnum.LadiesSenior });
            this.Rankings[(int)RankingTypeEnum.LTA_PADEL].Categories.Add(new RankingCategoryModel { Type = RankingCategoryTypeEnum.Junior });

        }

        public List<RankingModel> Rankings { get; set; } = new List<RankingModel>();
        
        public DateTime? LastUpdateDate { get; set; } = null;

    }
}
