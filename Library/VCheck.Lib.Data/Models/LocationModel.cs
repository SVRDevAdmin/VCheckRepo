using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class LocationModel
    {
        [Key]
        public String? ID { get; set; }
        public String? Name { get; set; }
        public String? Address { get; set; }
        public String? ContactName { get; set; }
        public String? PhoneNum { get; set; }
        public String? Email { get; set; }
        public String? Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String? UpdatedBy { get; set; }
    }
}
