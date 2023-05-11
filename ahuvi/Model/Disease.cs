using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ahuvi.Model
{
    public class Disease
    {
        public int DiseaseId { get; set; }
        public int InsuredId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Recovery { get; set; }
    }
}
