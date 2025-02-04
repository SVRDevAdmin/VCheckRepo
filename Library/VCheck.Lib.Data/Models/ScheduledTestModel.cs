using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VCheck.Lib.Data.Models
{
    public class ScheduledTestModel
    {
        [Key]
        public long ID { get; set; }
        public String? ScheduledTestType { get; set; }
        public DateTime? ScheduledDateTime { get; set; }
        public String? ScheduledBy { get; set; }
        public String? PatientID { get; set; }
        public String? PatientName { get; set; }
        public String? Gender { get; set; }
        public String? Species { get; set; }
        public String? OwnerName { get; set; }
        public String? InchargePerson { get; set; }
        public int? ScheduleTestStatus { get; set; }
        public int? TestCompleted { get; set; }
        public String? ScheduleUniqueID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String? UpdatedBy { get; set; }
        public int? LocationID { get; set; }
    }
}
