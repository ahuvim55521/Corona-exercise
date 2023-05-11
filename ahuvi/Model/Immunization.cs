using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ahuvi.Model
{
    public class Immunization
    {
        public int ImmunizationId { get; set; }
        public int InsuredId { get; set; }
        public DateTime ImmunizationDate { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }

    }
}
