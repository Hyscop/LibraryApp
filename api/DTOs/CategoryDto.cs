using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class CategoryForCreationDto
    {
        public string CategoryName { get; set; }
    }

    public class CategoryForUpdateDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

}