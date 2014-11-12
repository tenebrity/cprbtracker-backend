using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPRBTrackerBE.Models
{
    public class BoardHearingDb
    {
        public int BoardHearingId { get; set; }
        public DateTime DateHeld { get; set; }
        public string Notes { get; set; }
        public byte?[] pdf { get; set; }
    }
}