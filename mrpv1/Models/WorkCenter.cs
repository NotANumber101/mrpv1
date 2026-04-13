using System;
using System.Collections.Generic;
using System.Text;

namespace mrpv1.Models
{
    public class WorkCenter()
    {
        public int Id { get; set; }
        public required string CompanyName { get; set; }
        public required string CurrentStatus { get; set; }
        public DateTime CurrentStatusDate { get; set; }
        public required string JobDescription { get; set; }
    }
}