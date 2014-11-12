using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPRBTrackerBE.Models
{
    public class OfficerDb
    {
        public int OfficerId { get; set; }
        public string OfficerBadgeNumber { get; set; }
        public string OfficerFirstName { get; set; }
        public string OfficerLastName { get; set; }
        public string OfficerUniqueId { get; set; }
    }
}