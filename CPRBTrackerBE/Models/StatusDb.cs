using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPRBTrackerBE.Models
{
    public class StatusDb
    {
        public int CaseId { get; set; }
        public int RecommendationId { get; set; }
        public int RationaleId { get; set; }
        public int BoardHearingId { get; set; }
        public string AsideText { get; set; }
        public string Ruling { get; set; }
        public string NameOne { get; set; }
        public string NameTwo { get; set; }
    }
}