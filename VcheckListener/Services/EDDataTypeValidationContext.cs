using NHapi.Base.Model;
using NHapi.Base.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheckListener.Services
{
    class EDDataTypeValidationContext : IMessageRule
    {
        public string Description => "Allows OBX-5 fields longer than 200 characters";
        public string SectionReference => "Custom Rule";

        public ValidationException[] test(IMessage msg)
        {
            return new ValidationException[0];
        }

        public ValidationException[] Test(IMessage msg)
        {
            return new ValidationException[0];
        }
    }
}
