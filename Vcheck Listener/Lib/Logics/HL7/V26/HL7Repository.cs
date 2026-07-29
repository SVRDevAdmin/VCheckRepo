using LiteDB;
using Newtonsoft.Json;
using NHapi.Model.V26.Segment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheck.Interface.API;
using Vcheck_Listener.Lib.Models;
using Vcheck_Listener.Lib.PMS;
using static Vcheck_Listener.Views.ConfigurationWindow;

namespace Vcheck_Listener.Lib.Logics.HL7.V26
{
    class HL7Repository
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
            var sTestResultDetails = new List<txn_testresults_details>();

            String? sResultRule = "";
            String? sResultTestType = "";
            String? sResultTestCode = "";
            String? sOperatorID = "";
            String? sPatientID = "";
            String? sSerialNo = "";
            String? sUniversalIdentifier = "";
            String? sTestResultStatus = "";
            String? strResultObservStatus = "";
            Decimal iResultValue = 0;
            DateTime dAnalysisDateTime = DateTime.MinValue;
            string notes = "";
            String? sOverallStatus = "Normal";

            NHapi.Model.V26.Message.ORU_R01 sRU_R01 = new NHapi.Model.V26.Message.ORU_R01();

            try
            {
                sRU_R01 = (NHapi.Model.V26.Message.ORU_R01)sIMessage;

                var StructureList = sRU_R01.Names;
                var OBXStructure = StructureList.FirstOrDefault(x => x == "OBX") != null ? sRU_R01.GetAll("OBX") : null;
                var OBRStructure = StructureList.FirstOrDefault(x => x == "OBR") != null ? sRU_R01.GetAll("OBR") : null;
                var NTEStructure = StructureList.FirstOrDefault(x => x == "NTE") != null ? sRU_R01.GetAll("NTE") : null;


                if (sRU_R01.MSH.SendingApplication.NamespaceID != null && sRU_R01.MSH.SendingApplication.NamespaceID.Value != null)
                {
                    sSerialNo = sRU_R01.MSH.SendingApplication.NamespaceID.Value.Trim();
                }

                if (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.PatientIdentifierListRepetitionsUsed > 0)
                {
                    var PatientID = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.PatientID.IDNumber.Value;
                    var PatientIdentifierList = (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetPatientIdentifierList().Length > 0) ?
                                                 sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetPatientIdentifierList().FirstOrDefault().IDNumber.ToString() : null;


                    if (String.IsNullOrEmpty(PatientID) && !String.IsNullOrEmpty(PatientIdentifierList))
                    {
                        sPatientID = PatientIdentifierList;
                    }
                    else
                    {
                        sPatientID = PatientID;
                    }
                }

                //----------------- Observation Request ----------------------//
                var sTestResultLst = new List<txn_testresults>();

                if (OBRStructure != null)
                {
                    var SubID = "";
                    var ResultStatus = "";
                    var ResultValue = "";
                    var ReferenceRange = "";
                    var ResultUnit = "";
                    var ProceduralControl = "";

                    var obr = (OBR)OBRStructure.FirstOrDefault();
                    sResultTestCode = obr.UniversalServiceIdentifier.Identifier.Value;
                    sResultTestType = obr.UniversalServiceIdentifier.Text.Value;


                    if (!String.IsNullOrEmpty(obr.ObservationEndDateTime.Value))
                    {
                        if (obr.ObservationEndDateTime.Value.Length == 14)
                        {
                            dAnalysisDateTime = DateTime.ParseExact(obr.ObservationEndDateTime.Value, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (obr.ObservationEndDateTime.Value.Length == 22)
                        {
                            dAnalysisDateTime = DateTime.ParseExact(obr.ObservationEndDateTime.Value, "yyyyMMdd HH:mm:ssK", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            dAnalysisDateTime = DateTime.ParseExact(obr.ObservationEndDateTime.Value, "yyyyMMddHHmmss-ffff", System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }

                    List<string> graphDatas = new List<string>();

                    foreach (var obxTemp in OBXStructure)
                    {
                        var obx = (OBX)obxTemp;

                        String? sObservValue = "";

                        if (obx.GetObservationValue().Count() > 0)
                        {
                            var SubIDSplit = obx.ObservationSubID.Value.Split(".");

                            if (SubIDSplit.Count() == 3)
                            {
                                SubID = obx.ObservationSubID.Value;
                                ResultStatus = obx.GetObservationValue().FirstOrDefault().Data.ToString();
                                ReferenceRange = obx.ReferencesRange.Value;
                            }
                            else
                            {
                                if (SubIDSplit[3] == "1")
                                {
                                    if (obx.ObservationResultStatus.Value == "F")
                                    {
                                        strResultObservStatus = "Valid";
                                    }
                                    else if (obx.ObservationResultStatus.Value == "X")
                                    {
                                        strResultObservStatus = "Invalid";
                                    }

                                    ProceduralControl = strResultObservStatus;
                                }
                                else if (SubIDSplit[3] == "2")
                                {
                                    ResultValue = obx.GetObservationValue().FirstOrDefault().Data.ToString();
                                    ResultUnit = obx.Units.Identifier.Value;
                                }
                                else if (SubIDSplit[3] == "3")
                                {
                                    var parameterName = obx.ObservationIdentifier.Text.Value.Replace(sResultTestType, "").TrimStart();

                                    if (string.IsNullOrEmpty(parameterName)) { parameterName = obx.ObservationIdentifier.Text.Value; }

                                    graphDatas.Add(parameterName);

                                    if (obx.ObservationIdentifier.Text.Value.Contains("IC"))
                                    {
                                        parameterName = graphDatas.FirstOrDefault() + " IC";

                                        graphDatas.Clear();

                                        ResultStatus = ResultStatus == "Positive" ? "Valid" : "Invalid";

                                    }

                                    sTestResultDetails.Add(new txn_testresults_details
                                    {
                                        TestParameter = parameterName,
                                        SubID = SubID,
                                        ProceduralControl = ProceduralControl,
                                        TestResultStatus = ResultStatus,
                                        TestResultValue = ResultValue,
                                        TestResultUnit = (!String.IsNullOrEmpty(ResultUnit)) ? ResultUnit : "",
                                        ReferenceRange = ReferenceRange
                                    });


                                }
                            }

                            if (obx.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V26.Datatype.TX))
                            {
                                sTestResultStatus = obx.GetObservationValue().FirstOrDefault().Data.ToString();
                            }

                            if (obx.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V26.Datatype.NA))
                            {
                                var sNAObject = obx.GetObservationValue().FirstOrDefault().Data;

                                int iTotalComponent = sNAObject.ExtraComponents.NumComponents();
                                List<String> sVal = new List<String>();

                                // --- Get from Component ------ //
                                PropertyInfo[] props = sNAObject.GetType().GetProperties();
                                foreach (PropertyInfo p in props)
                                {
                                    if (p.PropertyType == typeof(NHapi.Model.V26.Datatype.NM))
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
                                }
                            }
                            else
                            {
                                if (obx.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V26.Datatype.CWE))
                                {
                                    var sCWEObject = obx.GetObservationValue().FirstOrDefault().Data;

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
                                else
                                {
                                    sObservValue = obx.GetObservationValue().FirstOrDefault().Data.ToString();
                                }

                            }
                        }

                        String sUnitValue = "";
                        sUnitValue = obx.Units.Identifier.Value + "^" +
                                     obx.Units.Text.Value + "^" +
                                     obx.Units.NameOfCodingSystem;

                        if (sUnitValue.Replace("^", "").Length == 0)
                        {
                            sUnitValue = "";
                        }

                        String sObservIdentifier = "";
                        sObservIdentifier = obx.ObservationIdentifier.Identifier.Value + "^" +
                                            obx.ObservationIdentifier.Text.Value + "^" +
                                            obx.ObservationIdentifier.NameOfCodingSystem.Value + "^" +
                                            obx.ObservationIdentifier.AlternateIdentifier.Value;

                        if (sObservIdentifier.Replace("^", "").Length == 0)
                        {
                            sObservIdentifier = "";
                        }
                    }

                    if (NTEStructure != null)
                    {
                        var nteTemp = (NTE)NTEStructure.FirstOrDefault();

                        notes = nteTemp.GetComment(1).Value;
                    }
                }
                else
                {
                    foreach (var observation in sRU_R01.PATIENT_RESULTs.FirstOrDefault().ORDER_OBSERVATIONs)
                    {
                        sResultTestCode = observation.OBR.UniversalServiceIdentifier.Identifier.Value;
                        sResultTestType = observation.OBR.UniversalServiceIdentifier.Text.Value;
                        sUniversalIdentifier = observation.OBR.UniversalServiceIdentifier.Text.Value;

                        if (observation.NTEs.Count() > 0)
                        {
                            if (sUniversalIdentifier.ToLower().Contains("babesia") || sUniversalIdentifier.ToLower().Contains("8 panel"))
                            {
                                sTestResultStatus = GenerateNTEComments(observation.NTEs.ToList());
                                notes = sTestResultStatus;
                            }
                        }

                        // --------------- Observation Results ----------------//
                        foreach (var observationDetail in observation.OBSERVATIONs)
                        {
                            String sInterpretation = "";
                            String? sObservValue = "";

                            if (observationDetail.OBX.GetObservationValue().Count() > 0)
                            {
                                if (observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V26.Datatype.NA))
                                {
                                    var sNAObject = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data;

                                    int iTotalComponent = sNAObject.ExtraComponents.NumComponents();
                                    List<String> sVal = new List<String>();

                                    // --- Get from Component ------ //
                                    PropertyInfo[] props = sNAObject.GetType().GetProperties();
                                    foreach (PropertyInfo p in props)
                                    {
                                        if (p.PropertyType == typeof(NHapi.Model.V26.Datatype.NM))
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
                                    }
                                }
                                else
                                {
                                    if (observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V26.Datatype.CWE))
                                    {
                                        var sCWEObject = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data;

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
                                    else
                                    {
                                        sObservValue = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.ToString();
                                    }

                                }

                                //strObserveValue = sObservValue;
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

                            if (!String.IsNullOrEmpty(observationDetail.OBX.DateTimeOfTheAnalysis.Value))
                            {
                                if (observationDetail.OBX.DateTimeOfTheAnalysis.Value.Length == 14)
                                {
                                    dAnalysisDateTime = DateTime.ParseExact(observationDetail.OBX.DateTimeOfTheAnalysis.Value, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else if (observationDetail.OBX.DateTimeOfTheAnalysis.Value.Length == 22)
                                {
                                    dAnalysisDateTime = DateTime.ParseExact(observationDetail.OBX.DateTimeOfTheAnalysis.Value, "yyyyMMdd HH:mm:ssK", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    dAnalysisDateTime = DateTime.ParseExact(observationDetail.OBX.DateTimeOfTheAnalysis.Value, "yyyyMMddHHmmss-ffff", System.Globalization.CultureInfo.InvariantCulture);
                                }
                            }

                            // ------------ Notes -------------------//
                            if (observationDetail.NTEs.Count() > 0)
                            {
                                String sComment = GenerateNTEComments(observationDetail.NTEs.ToList());

                                if (sComment.ToLower().Contains("cut off index"))
                                {
                                    if (sResultTestType.ToLower() == "cav ab")
                                    {
                                        var sCWEOBXValue = observationDetail.OBX.GetObservationValue();
                                        if (sCWEOBXValue.Length > 0)
                                        {
                                            NHapi.Model.V26.Datatype.CWE sVNData = (NHapi.Model.V26.Datatype.CWE)sCWEOBXValue[0].Data;
                                            if (sVNData != null)
                                            {
                                                String sCWEValue = sVNData.NameOfCodingSystem.Value;
                                                if (sCWEValue.ToLower() == "invalid")
                                                {
                                                    sTestResultStatus = sCWEValue;

                                                    String sValue = "";
                                                    String[] strArryValue = sComment.Split(",");
                                                    if (strArryValue.Length > 0)
                                                    {
                                                        sValue = strArryValue[1].Replace("Value=", "").Trim();
                                                    }

                                                    Decimal.TryParse(sValue, CultureInfo.InvariantCulture, out iResultValue);
                                                    sObservValue = sValue;
                                                }
                                                else
                                                {
                                                    String[] arrayCWE = sCWEValue.Split(",");
                                                    if (arrayCWE.Length > 0)
                                                    {
                                                        sTestResultStatus = arrayCWE[0].Trim();

                                                        String[] arrayValue = arrayCWE[1].Split("VN");
                                                        if (arrayValue.Length > 0)
                                                        {
                                                            sResultRule = "VN";
                                                            sObservValue = arrayValue[1].Trim();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sResultRule = "COI";

                                        String sValue = "";
                                        String[] strArryValue = sComment.Split(",");
                                        if (strArryValue.Length > 0)
                                        {
                                            sValue = strArryValue[1].Replace("Value=", "").Trim();
                                        }

                                        Decimal.TryParse(sValue, CultureInfo.InvariantCulture, out iResultValue);
                                        sObservValue = sValue;
                                    }
                                }

                                if (sComment.ToLower().Contains("interpretation"))
                                {
                                    String[] strArryValue = sComment.Split("=");
                                    if (strArryValue.Length > 0 && strArryValue[1].Trim() != "Not Used")
                                    {
                                        sInterpretation = strArryValue[1].Trim();
                                        sTestResultStatus = strArryValue[1].Trim();
                                    }
                                    else
                                    {
                                        sTestResultStatus = "Normal";
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


                            if (sResultTestType.ToLower() != "babesia gibsoni/canis" && sResultTestType.ToLower() != "canine diarrhea 8 panel")
                            {
                                string referenceRange = observationDetail.OBX.ReferencesRange.Value;
                                string measuringRange = "";

                                if (sResultRule == "COI")
                                {
                                    sTestResultStatus = General.ProcessObservationResultStatusValue(false, sObservValue, observationDetail.OBX.ReferencesRange.Value, iResultValue);
                                }
                                else
                                {
                                    sTestResultStatus = General.ProcessObservationResultStatusValueReferenceRange("V200", sObservValue, sResultTestType, "General", "General", out referenceRange, out measuringRange);
                                }

                                sTestResultDetails.Add(new txn_testresults_details
                                {
                                    TestParameter = observationDetail.OBX.ObservationIdentifier.Text.Value.Replace("?", ""),
                                    SubID = observationDetail.OBX.ObservationSubID.Value,
                                    ProceduralControl = strResultObservStatus,
                                    TestResultStatus = sTestResultStatus,
                                    TestResultValue = sObservValue,
                                    TestResultUnit = (!String.IsNullOrEmpty(observationDetail.OBX.Units.Identifier.Value)) ? observationDetail.OBX.Units.Identifier.Value : sResultRule,
                                    ReferenceRange = referenceRange,
                                    MeasuringRange = measuringRange,
                                    Interpretation = sInterpretation
                                });
                            }
                        }
                    }
                }

                sOverallStatus = "Normal";
                if (sTestResultDetails != null)
                {
                    if (sTestResultDetails.Where(x => x.TestParameter.ToLower() == "cav ab").Count() > 0)
                    {
                        sOverallStatus = sTestResultDetails[0].TestResultStatus;
                    }
                    else
                    {
                        if (sTestResultDetails.Where(x => x.TestResultStatus == "Positive").Count() > 0 && sTestResultDetails.Where(x => x.TestParameter.Contains(" IC")).Count() == 0)
                        {
                            sOverallStatus = "Abnormal";
                        }
                        else if (sTestResultDetails.Where(x => x.TestResultStatus == "Invalid").Count() > 0)
                        {
                            if (sTestResultDetails.Where(x => x.TestParameter.Contains(" IC")).Count() > 0)
                            {
                                sOverallStatus = "IC Invalid";
                            }
                            else
                            {
                                sOverallStatus = "Invalid";
                            }
                        }
                        else if (sTestResultDetails.Where(x => x.TestResultStatus == "Abnormal").Count() > 0)
                        {
                            sOverallStatus = "Abnormal";
                        }
                        else if (sTestResultDetails.Where(x => x.TestResultStatus == "Normal").Count() == sTestResultDetails.Count())
                        {
                            sOverallStatus = "Normal";
                        }
                        else if (sTestResultDetails.Where(x => x.TestParameter.Contains(" IC")).Count() > 1 && sTestResultDetails.Where(x => x.TestResultStatus == "Valid" && x.TestParameter.Contains(" IC")).Count() == sTestResultDetails.Where(x => x.TestParameter.Contains(" IC")).Count())
                        {
                            sOverallStatus = "IC Valid";
                        }
                        else
                        {
                            var sStatus = sTestResultDetails.FirstOrDefault(x => !string.IsNullOrEmpty(x.TestResultStatus));
                            sOverallStatus = sStatus != null ? sStatus.TestResultStatus : "Error";
                        }
                    }

                }

                txn_testresults sTestResultObj = new txn_testresults();
                sTestResultObj.TestResultDateTime = dAnalysisDateTime;
                sTestResultObj.TestResultType = sResultTestType;
                sTestResultObj.OperatorID = sOperatorID;
                sTestResultObj.PatientID = sPatientID;
                sTestResultObj.InchargePerson = "";
                sTestResultObj.OverallStatus = sOverallStatus;
                sTestResultObj.CreatedDate = DateTime.Now;
                sTestResultObj.CreatedBy = sSystemName;
                sTestResultObj.DeviceSerialNo = sSerialNo;
                sTestResultObj.PMSFunction = "Visible";

                var parameters = sTestResultDetails.Select(x => x.TestParameter).ToList();

                VCheckAPI VcheckAPI = new VCheckAPI();
                var listenerConfig = App.db.GetCollection<ListenerConfig>("ListenerConfig");
                var result = listenerConfig.FindOne(x => x.Id == 1);

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
                    success = await pmsHandler.SendToPMS(sTestResultObj, sTestResultDetails, sScheduledTestObj, sResultTestCode, notes);

                    await VcheckAPI.UpdateScheduleStatus(sScheduledTestObj.LocationID, sScheduledTestObj.PatientID, sScheduledTestObj.ScheduleUniqueID.Split("-")[1], sScheduledTestObj.CreatedBy, success ? 4 : 3, sTestResultObj.TestResultType);

                    if (!success)
                    {

                    }
                }
                else if (!string.IsNullOrEmpty(PMS))
                {

                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                log.Error("ProcessMessage >>> V26 >>> ", ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get NTE Comment data 
        /// </summary>
        /// <param name="nte"></param>
        /// <returns></returns>
        public static String GenerateNTEComments(List<NTE> nte)
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
                log.Error("GenerateNTEComments >>> V26 >>> " + ex.ToString());
            }

            return nteComment;
        }
    }
}
