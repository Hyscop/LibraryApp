using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class UserStats
    {
        public int UserStatsId { get; set; }
        public int UserId { get; set; }
        public int TotalBookRead { get; set; }
        public int TotalPagesRead { get; set; }

        public User User { get; set; }

    }
}