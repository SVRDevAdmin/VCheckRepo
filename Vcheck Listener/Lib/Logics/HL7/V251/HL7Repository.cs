using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheck.Interface.API;
using Vcheck_Listener.Lib.Models;
using Vcheck_Listener.Lib.PMS;
using static Vcheck_Listener.Views.ConfigurationWindow;

namespace Vcheck_Listener.Lib.Logics.HL7.V251
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
        public async static Task<Boolean> ProcessMessage(NHapi.Base.Model.IMessage sIMessage, String sSystemName)
        {
            Boolean isSuccess = false;

            try
            {
                NHapi.Model.V251.Message.OUL_R22 sRU_R01 = (NHapi.Model.V251.Message.OUL_R22)sIMessage;
                String? sResultTestCode = "";
                String? sResultTestType = "";
                String? sOperatorID = "";
                String? sPatientID = "";
                String? sPatientName = "";
                String? sSerialNo = "";
                String? sTestResultStatus = "";
                Boolean isRangeReference = false;
                String? strResultObservStatus = "";
                String? sDoctorName = "";
                Decimal iResultValue = 0;
                DateTime dAnalysisDateTime = DateTime.MinValue;
                string sSpecies = "General";

                if (sRU_R01.PATIENT.PID.PatientIdentifierListRepetitionsUsed > 0)
                {
                    var PatientID = sRU_R01.PATIENT.PID.PatientID.IDNumber.Value;
                    var PatientIdentifierList = (sRU_R01.PATIENT.PID.GetPatientIdentifierList().Length > 0) ?
                                                 sRU_R01.PATIENT.PID.GetPatientIdentifierList().FirstOrDefault().IDNumber.ToString() : null;


                    if (String.IsNullOrEmpty(PatientID) && !String.IsNullOrEmpty(PatientIdentifierList))
                    {
                        sPatientID = PatientIdentifierList;
                    }
                    else
                    {
                        sPatientID = PatientID;
                    }
                }


                if (sRU_R01.PATIENT.PID.PatientNameRepetitionsUsed > 0)
                {
                    var sNameObj = sRU_R01.PATIENT.PID.GetPatientName().FirstOrDefault();

                    if (sNameObj != null)
                    {
                        sPatientName = sNameObj.FamilyName.Surname.Value + " " + sNameObj.GivenName.Value;
                    }
                }

                if (sRU_R01.VISIT != null)
                {
                    var sPatientVisitation1 = sRU_R01.VISIT.PV1;

                    var AttendingDoctor = (sPatientVisitation1.GetAttendingDoctor().Length > 0) ?
                                              sPatientVisitation1.GetAttendingDoctor().FirstOrDefault().IDNumber.Value : null;

                    sDoctorName = AttendingDoctor;
                }


                //----------------- Observation Request ----------------------//
                var sTestResultLst = new List<txn_testresults>();
                var sTestResultDetails = new List<txn_testresults_details>();
                foreach (var observation in sRU_R01.SPECIMENs.FirstOrDefault().ORDERs)
                {
                    sResultTestCode = observation.OBR.UniversalServiceIdentifier.Identifier.Value;
                    sResultTestType = observation.OBR.UniversalServiceIdentifier.Text.Value;

                    // --------------- Observation Results ----------------//
                    foreach (var observationDetail in observation.RESULTs)
                    {
                        String sInterpretation = "";
                        String? sObservValue = "";
                        if (observationDetail.OBX.GetObservationValue().Count() > 0)
                        {
                            if (observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V251.Datatype.NA))
                            {
                                var sNAObject = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data;

                                int iTotalComponent = sNAObject.ExtraComponents.NumComponents();
                                List<String> sVal = new List<String>();

                                // --- Get from Component ------ //
                                PropertyInfo[] props = sNAObject.GetType().GetProperties();
                                foreach (PropertyInfo p in props)
                                {
                                    if (p.PropertyType == typeof(NHapi.Model.V251.Datatype.NM))
                                    {
                                        sVal.Add(p.GetValue(sNAObject, null).ToString());
                                    }                                    
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
                                if (observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V251.Datatype.CWE))
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

                        }
                        else
                        {

                        }

                        
                        if (observationDetail.OBX.GetResponsibleObserver().Length > 0)
                        {
                            sOperatorID = observationDetail.OBX.GetResponsibleObserver().FirstOrDefault().IDNumber.Value;
                        }
                        else
                        {
                            sOperatorID = null;
                        }

                        if (observationDetail.OBX.GetEquipmentInstanceIdentifier().Length > 0)
                        {
                            sSerialNo = observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().NamespaceID.Value;
                        }

                        if (!String.IsNullOrEmpty(observation.OBR.ObservationDateTime.Time.ToString()))
                        {
                            if (observation.OBR.ObservationDateTime.Time.Value.ToString().Length == 14)
                            {
                                dAnalysisDateTime = DateTime.ParseExact(observation.OBR.ObservationDateTime.Time.Value.ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else if (observation.OBR.ObservationDateTime.Time.ToString().Length == 22)
                            {
                                dAnalysisDateTime = DateTime.ParseExact(observation.OBR.ObservationDateTime.Time.Value.ToString(), "yyyyMMdd HH:mm:ssK", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                dAnalysisDateTime = DateTime.ParseExact(observation.OBR.ObservationDateTime.Time.Value.ToString(), "yyyyMMddHHmmss-ffff", System.Globalization.CultureInfo.InvariantCulture);
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

                        if (!String.IsNullOrEmpty(observationDetail.OBX.ReferencesRange.Value))
                        {
                            isRangeReference = true;
                        }


                        if (!(observationDetail.OBX.ObservationIdentifier.Text.Value.ToLower() == "age") &&
                            !(observationDetail.OBX.ObservationIdentifier.Text.Value.ToLower() == "weight"))
                        {
                            sObservValue = sObservValue.Replace("*", "").Replace(" ", "");

                            String sStatus = General.ProcessObservationResultStatusValue(isRangeReference, sObservValue, observationDetail.OBX.ReferencesRange.Value, iResultValue);

                            sTestResultDetails.Add(new txn_testresults_details
                            {
                                TestParameter = observationDetail.OBX.ObservationIdentifier.Text.Value.Replace("?", ""),
                                SubID = observationDetail.OBX.ObservationSubID.Value,
                                ProceduralControl = strResultObservStatus,
                                TestResultStatus = sStatus,
                                TestResultValue = sObservValue,
                                TestResultUnit = observationDetail.OBX.Units.Identifier.Value,
                                ReferenceRange = observationDetail.OBX.ReferencesRange.Value,
                                Interpretation = sInterpretation
                            });
                        }

                    }
                }

                String sOverallStatus = "Normal";
                if (sTestResultDetails != null)
                {
                    if ((sTestResultDetails.Where(x => x.TestResultStatus == "Positive").Count() > 0) ||
                        (sTestResultDetails.Where(x => x.TestResultStatus == "Invalid").Count() > 0))
                    {
                        sOverallStatus = "Abnormal";
                    }
                }

                sSerialNo = string.IsNullOrEmpty(sSerialNo) ? "VCheck C10" : sSerialNo;

                txn_testresults sTestResultObj = new txn_testresults();
                sTestResultObj.TestResultDateTime = dAnalysisDateTime;
                sTestResultObj.TestResultType = sResultTestType;
                sTestResultObj.OperatorID = sOperatorID;
                sTestResultObj.PatientID = sPatientID;
                sTestResultObj.InchargePerson = sDoctorName;
                sTestResultObj.OverallStatus = sOverallStatus;
                sTestResultObj.CreatedDate = DateTime.Now;
                sTestResultObj.CreatedBy = sSystemName;
                sTestResultObj.DeviceSerialNo = sSerialNo;
                sTestResultObj.PMSFunction = "Visible";
                sTestResultObj.PatientName = sPatientName;

                VCheckAPI VcheckAPI = new VCheckAPI();
                var listenerConfig = App.db.GetCollection<ListenerConfig>("ListenerConfig");
                var result = listenerConfig.FindOne(x => x.Id == 1);

                ScheduledTestModel? sScheduledTestObj = new ScheduledTestModel();
                var clinic = result != null && !string.IsNullOrEmpty(result.ClinicID) ? result.ClinicID : "";
                var PMS = result != null && !string.IsNullOrEmpty(result.ConversionChart) ? result.ConversionChart : "";
                if (!string.IsNullOrEmpty(clinic) && !string.IsNullOrEmpty(PMS) && PMS == "Greywind")
                {
                    var scheduleString = await VcheckAPI.GetSchedule(clinic, sTestResultObj.PatientID, sResultTestType);
                    sScheduledTestObj = string.IsNullOrEmpty(scheduleString) ? null : JsonConvert.DeserializeObject<ScheduledTestModel>(scheduleString);

                    if (sScheduledTestObj != null)
                    {
                        sTestResultObj.PatientName = string.IsNullOrEmpty(sPatientName) ? sScheduledTestObj.PatientName : sPatientName;
                    }
                }

                if (sScheduledTestObj != null && sScheduledTestObj.ID != 0)
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

                    }
                }
                else if (!string.IsNullOrEmpty(PMS))
                {

                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                log.Error("ProcessMessage >>> V251 >>> " + ex.ToString());

                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
