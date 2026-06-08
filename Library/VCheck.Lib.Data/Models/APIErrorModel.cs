using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class APIErrorModel
    {
        [Key]
        public int ID { get; set; }
        public String? ClinicID { get; set; }
        public String? ErrorMessage { get; set; }
        public String? ScheduleData { get; set; }
        public int Sync { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
