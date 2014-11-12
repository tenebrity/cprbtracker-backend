using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPRBTrackerBE.Models
{
    public class RecommendationDb
    {
        public int RecommendationId { get; set; }
        public string RecommendationText { get; set; }
        public bool RecommendationComplete { get; set; }
    }
}