using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPRBTrackerBE.Models
{
    public class CaseSummary
    {
        public int CaseId { get; set; }
        public string CaseNo { get; set; }
        public DateTime IssuedDate { get; set; }
        public string LatestDisposition { get; set; }
        public DateTime LatestDispositionDate { get; set; }
        public bool IsResolved { get; set; }
    }
}