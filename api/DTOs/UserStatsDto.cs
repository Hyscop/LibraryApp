using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class UserStatsDto
    {
        public int UserStatsId { get; set; }
        public int TotalBooksRead { get; set; }
        public int TotalPagesRead { get; set; }
    }
}