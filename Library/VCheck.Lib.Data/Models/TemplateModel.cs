using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class TemplateModel
    {
        public int TemplateID { get; set; }
        public string TemplateType { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateTitle { get; set; }
        public string TemplateContent { get; set; }
    }
}
