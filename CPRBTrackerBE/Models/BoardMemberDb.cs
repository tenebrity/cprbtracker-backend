using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPRBTrackerBE.Models
{
    public class BoardMemberDb
    {
        public int BoardMemberId { get; set; }
        public string BoardMemberFirstName { get; set; }
        public string BoardMemberLastName { get; set; }
        public DateTime DateServedStart { get; set; }
        public DateTime DateServedEnd { get; set; }
    }
}