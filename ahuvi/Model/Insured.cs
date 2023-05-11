using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ahuvi.Model
{
    public class Insured
    {
        public int InsuredId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string IDnumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int? NumHome { get; set; }
        public DateTime DateBirthDay { get; set; }
        public string? Telephone { get; set; }
        public string Pelephone { get; set; }
        public string? PicturePath { get; set; }
        public List<Immunization> Immunizations { get; set; }
        public List<Disease> Diseases { get; set; }

    }
}
