using System.Text.RegularExpressions;

namespace VCheckListener.Lib.Logics.HL7
{
    class General
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
                if (sResultValue.ToLower() != "invalid")
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
                        String[] strRange = [];
                        if (sReferenceRange.Contains(";"))
                        {
                            strRange = (sReferenceRange.Replace("[", "").Replace("]", "")).Split(";");
                        }
                        else if (sReferenceRange.Contains("-"))
                        {
                            strRange = sReferenceRange.Split("-");
                        }

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
                    sRetStatus = "Invalid";
                }
            }
            else
            {
                if (sResultValue.ToLower() == "invalid")
                {
                    sRetStatus = "Invalid";
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
            }

            return sRetStatus;
        }

        /// <summary>
        /// Process Observation Results Status for U3
        /// </summary>
        /// <param name="isRangeReference"></param>
        /// <param name="sResultValue"></param>
        /// <param name="sReferenceRange"></param>
        /// <param name="dResultValue"></param>
        /// <returns></returns>
        public static String ProcessObservationResultStatusValueU3(Boolean isRangeReference, String sResultValue, String sReferenceRange, Decimal dResultValue)
        {
            String sRetStatus = "";

            if (isRangeReference)
            {
                if (!Regex.IsMatch(sReferenceRange, "[A-Za-z]"))
                {
                    Decimal dTargetValue = 0;
                    var referenceRangeTemp = sReferenceRange;
                    if (!String.IsNullOrEmpty(sResultValue))
                    {
                        if (sResultValue.Contains("-") && sResultValue.Length > 1)
                        {
                            var resultTemp = sResultValue.Split("-");
                            sResultValue = resultTemp[0];
                        }

                        sResultValue = sResultValue.Replace("<", "").Replace("=", "").Replace("nan", "");
                        Decimal.TryParse(sResultValue, out dTargetValue);
                    }

                    if (sReferenceRange.Contains("<"))
                    {
                        var tempReferenceRange = referenceRangeTemp.Replace("<", "").Replace("=", "");
                        sRetStatus = dTargetValue <= Decimal.Parse(tempReferenceRange) ? "Normal" : "Abnormal";
                    }
                    else
                    {
                        Decimal dRangeA = 0;
                        Decimal dRangeB = 0;

                        String[] strRange = [];
                        if (sReferenceRange.Contains(";"))
                        {
                            strRange = (sReferenceRange.Replace("[", "").Replace("]", "")).Split(";");
                        }
                        else if (sReferenceRange.Contains("-"))
                        {
                            strRange = sReferenceRange.Split("-");
                        }

                        if (strRange.Length > 1)
                        {
                            Decimal.TryParse(strRange[0], out dRangeA);
                            Decimal.TryParse(strRange[1], out dRangeB);


                            if (dRangeA <= dTargetValue && dTargetValue <= dRangeB)
                            {
                                sRetStatus = "Normal";
                            }
                            else
                            {
                                sRetStatus = "Abnormal";
                            }
                        }
                        else
                        {
                            if (dTargetValue == Decimal.Parse(sReferenceRange))
                            {
                                sRetStatus = "Normal";
                            }
                            else
                            {
                                sRetStatus = "Abnormal";
                            }
                        }
                    }
                }
                else
                {
                    sRetStatus = "Normal";
                }
            }
            else
            {
                sRetStatus = "Normal";
            }

            return sRetStatus;
        }
    }
}
