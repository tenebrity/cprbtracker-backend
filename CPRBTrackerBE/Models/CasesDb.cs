using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPRBTrackerBE.Models
{
    public class CasesDb
    {
        public int CaseId { get; set; }
        public string CaseNo { get; set; }
        public string Summary { get; set; }
        public DateTime IssuedDate { get; set; }
        public bool IsResolved { get; set; }
    }
}