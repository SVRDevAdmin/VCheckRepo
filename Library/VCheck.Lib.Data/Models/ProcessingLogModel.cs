using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class ProcessingLogModel
    {
        [Key]
        public long ID { get; set; }
        public String? ProcessingTaskName { get; set; }
        public DateTime? ProcessingStartDate { get; set; }
        public DateTime? ProcessingEndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String? CreatedBy {  get; set; }
    }
}
