﻿using PadelInfo.Enums;
using System.Collections.Generic;

namespace PadelInfo.Models
{
    public class TranslationModel
    {
        public TranslationModel(TranslationEnum Id, string englishText, string spanishText)
        {
            this.Id = Id;
            this.Values = new List<string>();
            this.Values.Add(englishText);
            this.Values.Add(spanishText);
        }
        public TranslationEnum Id { get; set; }
        public List<string> Values { get; set; }
        
    }
}
