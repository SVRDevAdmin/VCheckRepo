using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheckListenerWorker.Lib.Logic.HL7
{
    public class General
    {
        /// <summary>
        /// Process Observation Results Status
        /// </summary>
        /// <param name="isRangeReference"></param>
        /// <param name="sResultValue"></param>
        /// <param name="sReferenceRange"></param>
        /// <param name="dResultValue"></param>
        /// <returns></returns>
        public static String ProcessObservationResultStatusValue(Boolean isRangeReference, String sResultValue, String sReferenceRange, Decimal dResultValue)
        {
            String sRetStatus = "";

            if (isRangeReference)
            {
                Decimal dTargetValue = 0;
                Decimal dMinusOne = Convert.ToDecimal("0.01");
                if (!String.IsNullOrEmpty(sResultValue))
                {
                    sResultValue = sResultValue.Replace("<", "").Replace("nan", "");
                    Decimal.TryParse(sResultValue, out dTargetValue);
                    dTargetValue = dTargetValue - dMinusOne;
                }

                Decimal dRangeA = 0;
                Decimal dRangeB = 0;

                if (!String.IsNullOrEmpty(sReferenceRange))
                {
                    String[] strRange = (sReferenceRange.Replace("[", "").Replace("]", "")).Split(";");
                    if (strRange.Length > 1)
                    {
                        Decimal.TryParse(strRange[0], out dRangeA);
                        Decimal.TryParse(strRange[1], out dRangeB);
                    }

                    if (dRangeA < dTargetValue && dTargetValue < dRangeB)
                    {
                        sRetStatus = "Positive";
                    }
                    else
                    {
                        sRetStatus = "Negative";
                    }
                }
                else
                {
                    sRetStatus = "Negative";
                }
            }
            else
            {
                if (dResultValue >= 1)
                {
                    sRetStatus = "Positive";
                }
                else
                {
                    sRetStatus = "Negative";
                }
            }

            return sRetStatus;
        }
    }
}
