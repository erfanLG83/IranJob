using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IranJob.WebApi.Models
{
    public class JobsFilterModel
    {
        public int? ProvinceId { get; set; }
        public int? CategoryId { get; set; }
        public string Search { get; set; }
    }
}
