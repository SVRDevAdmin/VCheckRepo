using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VCheck.Interface.API;
using Vcheck_Listener.Lib.Models;
using Vcheck_Listener.Lib.PMS;

namespace Vcheck_Listener.Lib.Logics.HL7.V231
{
    public class HL7Repository
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Process HL7 Message
        /// </summary>
        /// <param name="sIMessage"></param>
        /// <param name="sSystemName"></param>
        /// <returns></returns>
        public static async Task<bool> ProcessMessageAsync(NHapi.Base.Model.IMessage sIMessage, String sSystemName)
        {
            Boolean isSuccess = false;

            try
            {
                NHapi.Model.V231.Message.ORU_R01 sORU_R01 = (NHapi.Model.V231.Message.ORU_R01)sIMessage;
                String? sResultTestCode = "";
                String? sResultTestType = "";
                String? sOperatorID = "";
                String? sPatientID = "";
                String? sPatientName = "";
                String? strObserveValue = "";
                String? sSerialNo = "";
                String? sUniversalIdentifier = "";
                String? sTestResultStatus = "";
                Boolean isRangeReference = false;
                String? strResultObservStatus = "";
                String? sDoctorName = "";
                Decimal iResultValue = 0;
                DateTime dAnalysisDateTime = DateTime.MinValue;
                DateTime CurrentDatetime = DateTime.Now;
                string age = "";
                string sSpecies = "General";
                bool UD_Exist = false;
                bool UC_Exist = false;
                var DeviceType = "";
                
                var listenerConfig = App.db.GetCollection<ListenerConfig>("ListenerConfig");
                var result = listenerConfig.FindOne(x => x.Id == 1);
                DeviceType = result != null ? result.AnalyzerType : "";

                if (sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.PatientID != null)
                {
                    sPatientID = sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.PatientID.ID.Value;
                }

                if (sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetRace().FirstOrDefault() != null && !string.IsNullOrEmpty(sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetRace().FirstOrDefault().Text.ToString()))
                {
                    sSpecies = sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetRace().FirstOrDefault().Text.ToString();
                }

                if (sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.PatientIdentifierListRepetitionsUsed > 0)
                {
                    var PatientIdentifierList = (sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetPatientIdentifierList().Length > 0) ?
                                                 sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetPatientIdentifierList().FirstOrDefault().ID.ToString() : null;


                    if (String.IsNullOrEmpty(sPatientID) && !String.IsNullOrEmpty(PatientIdentifierList))
                    {
                        sPatientID = PatientIdentifierList;
                    }
                }

                DateTime birthdate = DateTime.Now;
                var datetimeOfBirth = sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.DateTimeOfBirth.TimeOfAnEvent.ToString();

                if (string.IsNullOrEmpty(datetimeOfBirth))
                {
                    age = "General";
                }
                else if (datetimeOfBirth.Length == 1)
                {
                    if (datetimeOfBirth.ToString() == "C")
                    {
                        age = "Child";
                    }
                    else
                    {
                        age = "General";
                    }
                }
                else if (datetimeOfBirth.Contains("Y") || datetimeOfBirth.Contains("M") || datetimeOfBirth.Contains("D") || datetimeOfBirth.Contains("H"))
                {
                    MatchCollection matches = Regex.Matches(datetimeOfBirth, @"\d+");

                    string ageUnit = datetimeOfBirth.Replace(matches[0].Value, "");

                    if (ageUnit == "Y" && int.Parse(matches[0].Value) > 2) { age = "General"; }
                    else { age = "Child"; }
                }
                else if (datetimeOfBirth.Length > 7)
                {
                    if (datetimeOfBirth.Length == 8)
                    {
                        birthdate = DateTime.ParseExact(datetimeOfBirth, "yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        DateTime.TryParse(datetimeOfBirth, out birthdate);
                    }

                    if ((DateTime.Now.Year - birthdate.Year) > 2) { age = "General"; }
                    else { age = "Child"; }
                }


                if (sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.PatientNameRepetitionsUsed > 0)
                {
                    var sNameObj = sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetPatientName().FirstOrDefault();

                    if (sNameObj != null)
                    {
                        sPatientName = string.IsNullOrEmpty(sNameObj.GivenName.Value) ? sNameObj.FamilyLastName.FamilyName.Value : sNameObj.GivenName.Value;
                    }
                }

                if (sORU_R01.GetPATIENT_RESULT().PATIENT.VISIT.PV1.SetIDPV1.Value != null)
                {
                    var sPatientVisitation1 = sORU_R01.GetPATIENT_RESULT().PATIENT.VISIT.PV1;

                    sDoctorName = (sPatientVisitation1.GetAttendingDoctor().Length > 0) ?
                                              sPatientVisitation1.GetAttendingDoctor().FirstOrDefault().IDNumber.Value : null;
                }

                //----------------- Observation Request ----------------------//
                var sTestResultLst = new List<txn_testresults>();
                var sTestResultDetails = new List<txn_testresults_details>();
                foreach (var observation in sORU_R01.PATIENT_RESULTs.FirstOrDefault().ORDER_OBSERVATIONs)
                {
                    String sFillerOrdNum = "";
                    sFillerOrdNum = observation.OBR.FillerOrderNumber.EntityIdentifier.Value + "^" +
                                                observation.OBR.FillerOrderNumber.NamespaceID.Value + "^" +
                                                observation.OBR.FillerOrderNumber.UniversalID.Value + "^" +
                                                observation.OBR.FillerOrderNumber.UniversalIDType.Value;

                    sPatientID = string.IsNullOrEmpty(sPatientID) ? observation.OBR.FillerOrderNumber.EntityIdentifier.Value : sPatientID;

                    if (sFillerOrdNum.Replace("^", "").Length == 0)
                    {
                        sFillerOrdNum = "";
                    }

                    var extraComp = observation.OBR.PlacerField1.ExtraComponents;
                    int totalExtraComp = extraComp.NumComponents();
                    //sResultTestType = string.IsNullOrEmpty(sResultTestType) ? observation.OBR.UniversalServiceID.Identifier.Value : observation.OBR.UniversalServiceID.Text.Value;
                    sResultTestType = totalExtraComp > 1 ? extraComp.GetComponent(2).Data.ToString() : observation.OBR.SpecimenSource.SpecimenSourceNameOrCode.Identifier.Value;
                    sResultTestCode = totalExtraComp > 1 ? extraComp.GetComponent(1).Data.ToString() : "";
                    sUniversalIdentifier = observation.OBR.UniversalServiceID.Text.Value;
                    sSerialNo = string.IsNullOrEmpty(observation.OBR.UniversalServiceID.Text.Value) ? observation.OBR.UniversalServiceID.Identifier.Value : observation.OBR.UniversalServiceID.Text.Value;

                    if (observation.NTEs.Count() > 0)
                    {
                        if (sUniversalIdentifier.ToLower().Contains("babesia") || sUniversalIdentifier.ToLower().Contains("8 panel"))
                        {
                            sTestResultStatus = GenerateNTEComments(observation.NTEs.ToList());
                        }
                    }

                    // --------------- Observation Results ----------------//
                    foreach (var observationDetail in observation.OBSERVATIONs)
                    {
                        String sInterpretation = "";
                        String? sObservValue = "";
                        strObserveValue = "";
                        if (observationDetail.OBX.GetObservationValue().Count() > 0)
                        {
                            var tempsObservValue = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data;

                            if (tempsObservValue.GetType() == typeof(NHapi.Model.V231.Datatype.NA))
                            {
                                var sNAObject = tempsObservValue;

                                int iTotalComponent = sNAObject.ExtraComponents.NumComponents();
                                List<String> sVal = new List<String>();

                                // --- Get from Component ------ //
                                PropertyInfo[] props = sNAObject.GetType().GetProperties();
                                foreach (PropertyInfo p in props)
                                {
                                    if (p.PropertyType == typeof(NHapi.Model.V231.Datatype.NM))
                                    {
                                        sVal.Add(p.GetValue(sNAObject, null).ToString());
                                    }
                                    ;
                                }

                                // -- Get From Extra Component -----//
                                for (int i = 0; i < iTotalComponent; i++)
                                {
                                    if (sNAObject.ExtraComponents.GetComponent(i).Data.ToString() != null)
                                    {
                                        sVal.Add(sNAObject.ExtraComponents.GetComponent(i).Data.ToString());
                                    }
                                }

                                if (sVal.Count() > 0)
                                {
                                    sObservValue = String.Join("^", sVal);
                                    Decimal.TryParse(sObservValue.Replace("<", ""), CultureInfo.InvariantCulture, out iResultValue);
                                }
                            }
                            else
                            {
                                if (tempsObservValue.GetType() == typeof(NHapi.Model.V231.Datatype.CWE))
                                {
                                    var sCWEObject = tempsObservValue;

                                    List<String> sCWEVal = new List<String>();
                                    PropertyInfo[] propCWE = sCWEObject.GetType().GetProperties();
                                    foreach (PropertyInfo c in propCWE)
                                    {
                                        if (c.PropertyType == typeof(NHapi.Base.Model.IType[]))
                                        {
                                            NHapi.Base.Model.IType[] iTypeObj = (NHapi.Base.Model.IType[])c.GetValue(sCWEObject, null);
                                            for (int i = 0; i < 4; i++)
                                            {
                                                sCWEVal.Add(iTypeObj[i].ToString());
                                            }
                                        }
                                    }

                                    if (sCWEVal.Count() > 0)
                                    {
                                        sObservValue = String.Join("^", sCWEVal);
                                    }
                                }
                                else if (tempsObservValue.GetType() == typeof(NHapi.Model.V231.Datatype.ED))
                                {
                                    continue;
                                }
                                else
                                {
                                    sObservValue = tempsObservValue.ToString();
                                }

                            }

                            strObserveValue = sObservValue;
                        }
                        else if (observationDetail.OBX.GetProbability().Count() > 0)
                        {
                            sObservValue = observationDetail.OBX.GetProbability()[0].Value;
                            Decimal.TryParse(sObservValue.Replace("<", ""), CultureInfo.InvariantCulture, out iResultValue);
                        }
                        else
                        {
                            if (observationDetail.OBX.ObservationResultStatus != null)
                            {
                                if (observationDetail.OBX.ObservationResultStatus.Value == "F")
                                {
                                    strObserveValue = "Valid";
                                }
                                if (observationDetail.OBX.ObservationResultStatus.Value == "X")
                                {
                                    strObserveValue = "Invalid";
                                }
                            }
                        }

                        String sUnitValue = "";
                        sUnitValue = observationDetail.OBX.Units.Identifier.Value + "^" +
                                observationDetail.OBX.Units.Text.Value + "^" +
                                observationDetail.OBX.Units.NameOfCodingSystem;

                        if (sUnitValue.Replace("^", "").Length == 0)
                        {
                            sUnitValue = "";
                        }

                        String sObservIdentifier = "";
                        sObservIdentifier = observationDetail.OBX.ObservationIdentifier.Identifier.Value + "^" +
                                            observationDetail.OBX.ObservationIdentifier.Text.Value + "^" +
                                            observationDetail.OBX.ObservationIdentifier.NameOfCodingSystem.Value + "^" +
                                            observationDetail.OBX.ObservationIdentifier.AlternateIdentifier.Value;
                        if (sObservIdentifier.Replace("^", "").Length == 0)
                        {
                            sObservIdentifier = "";
                        }

                        if (observationDetail.OBX.GetResponsibleObserver().Length > 0)
                        {
                            sOperatorID = observationDetail.OBX.GetResponsibleObserver().FirstOrDefault().IDNumber.Value;
                        }
                        else
                        {
                            sOperatorID = null;
                        }

                        if (!String.IsNullOrEmpty(observation.OBR.ObservationDateTime.TimeOfAnEvent.ToString()))
                        {
                            if (observation.OBR.ObservationDateTime.TimeOfAnEvent.Value.ToString().Length == 14)
                            {
                                dAnalysisDateTime = DateTime.ParseExact(observation.OBR.ObservationDateTime.TimeOfAnEvent.Value.ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else if (observation.OBR.ObservationDateTime.TimeOfAnEvent.ToString().Length == 22)
                            {
                                dAnalysisDateTime = DateTime.ParseExact(observation.OBR.ObservationDateTime.TimeOfAnEvent.Value.ToString(), "yyyyMMdd HH:mm:ssK", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                dAnalysisDateTime = DateTime.ParseExact(observation.OBR.ObservationDateTime.TimeOfAnEvent.Value.ToString(), "yyyyMMddHHmmss-ffff", System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }

                        // ------------ Notes -------------------//
                        if (observationDetail.NTEs.Count() > 0)
                        {
                            String sComment = "";

                            if (sComment.ToLower().Contains("cut off index"))
                            {
                                String sValue = "";
                                String[] strArryValue = sComment.Split(",");
                                if (strArryValue.Length > 0)
                                {
                                    sValue = strArryValue[1].Replace("Value=", "").Trim();
                                }

                                Decimal.TryParse(sValue, CultureInfo.InvariantCulture, out iResultValue);
                                sObservValue = sValue;
                            }

                            if (sComment.ToLower().Contains("interpretation"))
                            {
                                //sInterpretation
                                String[] strArryValue = sComment.Split("=");
                                if (strArryValue.Length > 0)
                                {
                                    sInterpretation = strArryValue[1].Trim();
                                }
                            }
                        }


                        if (observationDetail.OBX.ObservationResultStatus.Value == "F")
                        {
                            strResultObservStatus = "Valid";
                        }
                        else if (observationDetail.OBX.ObservationResultStatus.Value == "X")
                        {
                            strResultObservStatus = "Invalid";
                        }

                        isRangeReference = !String.IsNullOrEmpty(observationDetail.OBX.ReferencesRange.Value);

                        if (!(observationDetail.OBX.ObservationIdentifier.Identifier.Value.ToLower() == "age") &&
                            !(observationDetail.OBX.ObservationIdentifier.Identifier.Value.ToLower() == "weight") &&
                            !(observationDetail.OBX.ObservationIdentifier.Identifier.Value.ToLower().Contains("title")))
                        {
                            String sStatus = "";
                            string referenceRange = observationDetail.OBX.ReferencesRange.Value;
                            string measuringRange = "";
                            var testParamName = (observationDetail.OBX.ObservationIdentifier.Text.Value != null && observationDetail.OBX.ObservationIdentifier.Text.Value != "") ? observationDetail.OBX.ObservationIdentifier.Text.Value : observationDetail.OBX.ObservationIdentifier.Identifier.Value;

                            if(!UD_Exist && testParamName.Contains("UD_")) { UD_Exist = true; }
                            if (!UC_Exist && testParamName.Contains("UC_")) { UC_Exist = true; }

                            testParamName = testParamName.Replace("UC_", "").Replace("UD_", "");

                            if (testParamName == "WBC") { testParamName = "Leukocytes"; }
                            else if (testParamName == "KET") { testParamName = "Ketones"; }
                            else if (testParamName == "NIT") { testParamName = "Nitrite"; }
                            else if (testParamName == "URO") { testParamName = "Urobilinogen"; }
                            else if (testParamName == "BIL") { testParamName = "Bilirubin"; }
                            else if (testParamName == "GLU") { testParamName = "Glucose"; }
                            else if (testParamName == "PRO") { testParamName = "Protein"; }
                            else if (testParamName == "SG") { testParamName = "Specific Gravity"; }
                            else if (testParamName == "PH") { testParamName = "pH"; }
                            else if (testParamName == "BLD") { testParamName = "Blood"; }
                            else if (testParamName == "VC") { testParamName = "Ascorbic Acid"; }
                            else if (testParamName == "MA") { testParamName = "Microalbumin"; }
                            else if (testParamName == "CA") { testParamName = "Calcium"; }
                            else if (testParamName == "CR") { testParamName = "Creatinine"; }
                            else if (testParamName == "PCR") { testParamName = "UPC"; }
                            else if (testParamName == "DRBC-Other") { testParamName = "DRBC"; }
                            else if (testParamName == "REP") { testParamName = "RTEP"; }
                            else if (testParamName == "UNCC") { testParamName = "PAT"; }
                            else if (testParamName == "BACT_R") { testParamName = "BACT"; }
                            else if (testParamName == "IMPURITY") { testParamName = "OTHER"; }
                            else if (testParamName == "Transparency") { testParamName = "TUR"; }
                            else if (testParamName == "R_TATE") { testParamName = "R-RATE"; }
                            else if (testParamName == "MCV_CV") { testParamName = "MCV-CV"; }

                            if (DeviceType == "U3") { sStatus = General.ProcessObservationResultStatusValueU3(isRangeReference, sObservValue, observationDetail.OBX.ReferencesRange.Value, iResultValue); }
                            else
                            {
                                sStatus = General.ProcessObservationResultStatusValue(isRangeReference, sObservValue, observationDetail.OBX.ReferencesRange.Value, iResultValue);
                            }

                            int testUnit = 0;
                            var unitExtended = int.TryParse(observationDetail.OBX.Units.Identifier.Value, out testUnit) ? observationDetail.OBX.Units.Identifier.Value + "^" + observationDetail.OBX.Units.Text.Value : observationDetail.OBX.Units.Identifier.Value;

                            sTestResultDetails.Add(new txn_testresults_details
                            {
                                TestParameter = testParamName.Replace("?", ""),
                                SubID = observationDetail.OBX.ObservationSubID.Value,
                                ProceduralControl = strResultObservStatus,
                                TestResultStatus = sStatus,
                                TestResultValue = sObservValue,
                                TestResultUnit = unitExtended,
                                ReferenceRange = string.IsNullOrEmpty(referenceRange) ? observationDetail.OBX.ReferencesRange.Value : referenceRange,
                                MeasuringRange = measuringRange,
                                Interpretation = sInterpretation
                            });
                        }

                    }

                }

                String sOverallStatus = "Normal";
                if (sTestResultDetails != null)
                {
                    if ((sTestResultDetails.Where(x => x.TestResultStatus == "Positive").Count() > 0) ||
                        (sTestResultDetails.Where(x => x.TestResultStatus == "Invalid").Count() > 0) ||
                        (sTestResultDetails.Where(x => x.TestResultStatus == "Abnormal").Count() > 0))
                    {
                        sOverallStatus = "Abnormal";
                    }
                }

                var U3TestType = "";

                if (UD_Exist && !UC_Exist)
                {
                    U3TestType = "Vcheck U Sediment 16 Plus";
                }
                else if (UD_Exist && UC_Exist)
                {
                    U3TestType = "Vcheck U Both 16 Plus";
                }
                else if (!UD_Exist && UC_Exist)
                {
                    U3TestType = "Vcheck U UC Vet 13 Plus";
                }

                txn_testresults sTestResultObj = new txn_testresults();
                sTestResultObj.TestResultDateTime = dAnalysisDateTime;
                sTestResultObj.TestResultType = string.IsNullOrEmpty(U3TestType) ? sResultTestType : U3TestType;
                sTestResultObj.OperatorID = sOperatorID;
                sTestResultObj.PatientID = sPatientID;
                sTestResultObj.PatientName = sPatientName;
                sTestResultObj.InchargePerson = sDoctorName;
                sTestResultObj.OverallStatus = sOverallStatus;
                sTestResultObj.CreatedDate = CurrentDatetime;
                sTestResultObj.CreatedBy = sSystemName;
                sTestResultObj.DeviceSerialNo = sSerialNo;
                sTestResultObj.PMSFunction = "Visible";


                VCheckAPI VcheckAPI = new VCheckAPI();

                ScheduledTestModel? sScheduledTestObj = new ScheduledTestModel();
                var clinic = result != null && !string.IsNullOrEmpty(result.ClinicID) ? result.ClinicID : "";
                var PMS = result != null && !string.IsNullOrEmpty(result.ConversionChart) ? result.ConversionChart : "";
                if (!string.IsNullOrEmpty(clinic) && !string.IsNullOrEmpty(PMS))
                {
                    var scheduleString = await VcheckAPI.GetSchedule(clinic, sTestResultObj.PatientID, sResultTestType);
                    sScheduledTestObj = string.IsNullOrEmpty(scheduleString) ? null : JsonConvert.DeserializeObject<ScheduledTestModel>(scheduleString);

                    if (sScheduledTestObj != null)
                    {
                        sTestResultObj.PatientName = sScheduledTestObj.PatientName;
                    }
                }

                if (sScheduledTestObj != null && !string.IsNullOrEmpty(sScheduledTestObj.ScheduleUniqueID))
                {
                    if (string.IsNullOrEmpty(sResultTestCode) || double.TryParse(sResultTestCode, CultureInfo.InvariantCulture, out _))
                    {
                        var testResponseString = await VcheckAPI.GetTestByNameOrCode(sTestResultObj.TestResultType, null);
                        sResultTestCode = string.IsNullOrEmpty(testResponseString) ? "VCheck" : JsonConvert.DeserializeObject<TestDataObject>(testResponseString).testid;
                    }

                    PMSHandler pmsHandler = new PMSHandler();
                    var success = false;
                    success = await pmsHandler.SendToPMS(sTestResultObj, sTestResultDetails, sScheduledTestObj, sResultTestCode);

                    await VcheckAPI.UpdateScheduleStatus(sScheduledTestObj.LocationID, sScheduledTestObj.PatientID, sScheduledTestObj.ScheduleUniqueID.Split("-")[1], sScheduledTestObj.CreatedBy, success ? 4 : 3, sTestResultObj.TestResultType);

                    if (!success)
                    {
                        System.Windows.Forms.MessageBox.Show("VCheck failed to send result to PIMS.");
                    }
                }
                else if (!string.IsNullOrEmpty(PMS))
                {

                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                log.Error("ProcessMessage >>> V231 >>> " + ex.ToString());

                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get NTE Comment for HL7
        /// </summary>
        /// <param name="nte"></param>
        /// <returns></returns>
        public static String GenerateNTEComments(List<NHapi.Model.V231.Segment.NTE> nte)
        {
            string nteComment = "";
            try
            {
                for (int i = 0; i <= nte.Count() - 1; i++)
                {
                    if (nte[i].GetComment() != null && nte[i].GetComment().FirstOrDefault() != null && nte[i].GetComment().FirstOrDefault().Value != null)
                    {
                        if (nteComment != "")
                        {
                            nteComment += ", ";

                        }
                        nteComment += nte[i].GetComment().FirstOrDefault().Value;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("GenerateNTEComments >>> V231 >>> " + ex.ToString());
            }

            return nteComment;
        }
    }
}
