using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheckListenerWorker.Lib.DBContext;
using VCheckListenerWorker.Lib.Models;

namespace VCheckListenerWorker.Lib.Logic.HL7.V231
{
    public class HL7Repository
    {
        static VCheckListenerWorker.Lib.Util.Logger _logger = new VCheckListenerWorker.Lib.Util.Logger();

        /// <summary>
        /// Process HL7 Message
        /// </summary>
        /// <param name="sIMessage"></param>
        /// <param name="sSystemName"></param>
        /// <returns></returns>
        public static Boolean ProcessMessage(NHapi.Base.Model.IMessage sIMessage, String sSystemName)
        {
            Boolean isSuccess = false;

            try
            {
                NHapi.Model.V231.Message.ORU_R01 sORU_R01 = (NHapi.Model.V231.Message.ORU_R01)sIMessage;
                String sResultRule = "";
                String sResultStatus = "";
                String sResultTestType = "";
                String sOperatorID = "";
                String sPatientID = "";
                String sPatientName = "";
                String strObserveValue = "";
                String sSerialNo = "";
                String sUniversalIdentifier = "";
                String sTestResultStatus = "";
                Boolean isRangeReference = false;
                String strResultObservStatus = "";
                String sDoctorName = "";
                Decimal iResultValue = 0;
                DateTime dAnalysisDateTime = DateTime.MinValue;

                if (sORU_R01.MSH.SendingApplication.NamespaceID != null && sORU_R01.MSH.SendingApplication.NamespaceID.Value != null)
                {
                    sSerialNo = sORU_R01.MSH.SendingApplication.NamespaceID.Value.Trim();
                }

                // --------------- Message Header --------------//
                tbltestanalyze_results_messageheader sMSHObj = new tbltestanalyze_results_messageheader
                {
                    FieldSeparator = sORU_R01.MSH.FieldSeparator.Value,
                    EncodingCharacters = sORU_R01.MSH.EncodingCharacters.Value,
                    SendingApplication = ((sORU_R01.MSH.SendingApplication.NamespaceID.Value != null) ? sORU_R01.MSH.SendingApplication.NamespaceID.Value.Trim() : "") + "^" +
                                         ((sORU_R01.MSH.SendingApplication.UniversalID.Value != null) ? sORU_R01.MSH.SendingApplication.UniversalID.Value.Trim() : "") + "^" +
                                         ((sORU_R01.MSH.SendingApplication.UniversalIDType.Value != null) ? sORU_R01.MSH.SendingApplication.UniversalIDType.Value.Trim() : ""),
                    SendingFacility = sORU_R01.MSH.SendingFacility.NamespaceID.Value,
                    ReceivingApplication = sORU_R01.MSH.ReceivingApplication.NamespaceID.Value,
                    ReceivingFacility = sORU_R01.MSH.ReceivingFacility.NamespaceID.Value,
                    DateTimeMessage = sORU_R01.MSH.DateTimeOfMessage.TimeOfAnEvent.ToString(),
                    MessageType = sORU_R01.MSH.MessageType.MessageType.Value + "^" +
                                  sORU_R01.MSH.MessageType.TriggerEvent.Value + "^" +
                                  sORU_R01.MSH.MessageType.MessageStructure.Value,
                    MessageControlID = sORU_R01.MSH.MessageControlID.Value.ToString(),
                    ProcessingID = sORU_R01.MSH.ProcessingID.ProcessingID.Value,
                    VersionID = sORU_R01.MSH.VersionID.VersionID.Value,
                    AcceptAckmgtType = sORU_R01.MSH.AcceptAcknowledgmentType.Value,
                    AppAckmgtType = sORU_R01.MSH.ApplicationAcknowledgmentType.Value,
                    CountryCode = sORU_R01.MSH.CountryCode.Value,
                    CharacterSet = (sORU_R01.MSH.GetCharacterSet().Length > 0) ? sORU_R01.MSH.GetCharacterSet().FirstOrDefault().Value : null,
                    PrincipalLanguageMsg = sORU_R01.MSH.PrincipalLanguageOfMessage.Identifier.Value + "^" +
                                           sORU_R01.MSH.PrincipalLanguageOfMessage.Text.Value + "^" +
                                           sORU_R01.MSH.PrincipalLanguageOfMessage.NameOfCodingSystem.Value,
                    MessageProfileIdentifier = null
                };

                // ------------ Patient Identification --------------------//
                List<tbltestanalyze_results_patientidentification> sPIDObj = new List<tbltestanalyze_results_patientidentification>();

                tbltestanalyze_results_patientidentification sPID = new tbltestanalyze_results_patientidentification();

                if(sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.PatientID != null)
                {
                    sPID.PatientID = sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.PatientID.ID.Value;
                    sPatientID = sPID.PatientID;
                }

                if(sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.SetIDPID != null)
                {
                    sPID.SetID = sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.SetIDPID.Value;
                }

                if (sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.PatientIdentifierListRepetitionsUsed > 0)
                {                    
                    sPID.AlternatePatientID = (sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetAlternatePatientIDPID().Length > 0) ?
                                               sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetAlternatePatientIDPID().FirstOrDefault().ID.ToString() : null;

                    sPID.PatientIdentifierList = (sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetPatientIdentifierList().Length > 0) ?
                                                 sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetPatientIdentifierList().FirstOrDefault().ID.ToString() : null;


                    if (String.IsNullOrEmpty(sPID.PatientID) && !String.IsNullOrEmpty(sPID.PatientIdentifierList))
                    {
                        sPatientID = sPID.PatientIdentifierList;
                    }
                }


                if (sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.PatientNameRepetitionsUsed > 0)
                {
                    var sNameObj = sORU_R01.PATIENT_RESULTs.FirstOrDefault().PATIENT.PID.GetPatientName().FirstOrDefault();

                    if (sNameObj != null)
                    {
                        sPID.PatientName = sNameObj.FamilyLastName.FamilyName.Value + "^" +
                                           sNameObj.GivenName + "^" +
                                           sNameObj.MiddleInitialOrName + "^" +
                                           sNameObj.SuffixEgJRorIII + "^" +
                                           sNameObj.PrefixEgDR + "^" +
                                           sNameObj.DegreeEgMD + "^" +
                                           sNameObj.NameTypeCode;

                        sPatientName = sNameObj.GivenName.Value;

                        if (sPID.PatientName.Replace("^", "").Length == 0)
                        {
                            sPID.PatientName = "";
                        }
                    }
                    else
                    {
                        using (var ctx = new TestResultDBContext(Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder().Configuration))
                        {
                            var patientInfo = ctx.txn_Testresults.FirstOrDefault(x => x.PatientID == sPatientID);
                            if(patientInfo != null)
                            {
                                sPID.PatientName = patientInfo.PatientName;
                            }
                            else
                            {
                                sPID.PatientName = "";
                            }
                        }
                    }
                }
                sPIDObj.Add(sPID);

                //----------------- Patient Visit Record ---------------------//
                var sPVObj = new tbltestanalyze_results_patientvisit();

                if (sORU_R01.GetPATIENT_RESULT().PATIENT.VISIT.PV1.SetIDPV1.Value != null)
                {
                    var sPatientVisitation1 = sORU_R01.GetPATIENT_RESULT().PATIENT.VISIT.PV1;
                    var sPatientVisitation2 = sORU_R01.GetPATIENT_RESULT().PATIENT.VISIT.PV2;

                    sPVObj.SetID = sPatientVisitation1.SetIDPV1.Value;
                    sPVObj.PatientClass = sPatientVisitation1.PatientClass.Value;
                    sPVObj.AssignedPatientLocation = sPatientVisitation1.AssignedPatientLocation.Room.Value;
                    sPVObj.AdmissionType = sPatientVisitation1.AdmissionType.Value;
                    sPVObj.PreadmitNumber = sPatientVisitation1.PreadmitNumber.ID.Value;
                    sPVObj.PriorPatientLocation = sPatientVisitation1.PriorPatientLocation.LocationDescription.ToString();

                    sPVObj.AttendingDoctor = (sPatientVisitation1.GetAttendingDoctor().Length > 0) ?
                                              sPatientVisitation1.GetAttendingDoctor().FirstOrDefault().IDNumber.Value : null;
                    sPVObj.ReferringDoctor = (sPatientVisitation1.GetReferringDoctor().Length > 0) ?
                                              sPatientVisitation1.GetReferringDoctor().FirstOrDefault().IDNumber.Value : null;
                    sPVObj.ConsultingDoctor = (sPatientVisitation1.GetConsultingDoctor().Length > 0) ?
                                              sPatientVisitation1.GetConsultingDoctor().FirstOrDefault().IDNumber.Value : null;
                    sPVObj.HospitalService = sPatientVisitation1.HospitalService.Value;
                    sPVObj.TemporaryLocation = sPatientVisitation1.TemporaryLocation.LocationDescription.ToString();
                    sPVObj.PreadmitTestIndicator = sPatientVisitation1.PreadmitTestIndicator.Value;
                    sPVObj.ReAdmissionIndicator = sPatientVisitation1.ReAdmissionIndicator.Value;
                    sPVObj.AdmitSource = sPatientVisitation1.AdmitSource.Value;
                    sPVObj.AmbulatoryStatus = (sPatientVisitation1.GetAmbulatoryStatus().Length > 0) ?
                                               sPatientVisitation1.GetAmbulatoryStatus().FirstOrDefault().Value : null;
                    sPVObj.VIPIndicator = sPatientVisitation1.VIPIndicator.Value;
                    sPVObj.AdmittingDoctor = (sPatientVisitation1.GetAdmittingDoctor().Length > 0) ?
                                              sPatientVisitation1.GetAdmittingDoctor().FirstOrDefault().GivenName.Value : null;
                    sPVObj.PatientType = sPatientVisitation1.PatientType.Value;
                    sPVObj.VisitNumber = sPatientVisitation1.VisitNumber.ID.Value;

                    sDoctorName = sPVObj.AttendingDoctor;
                }
                else
                {
                    sPVObj = null;
                }

                //----------------- Observation Request ----------------------//
                var sTestResultLst = new List<txn_testresults>();
                var sTestResultDetails = new List<txn_testresults_details>();
                var sNTEObj = new List<tbltestanalyze_results_notes>();
                var sOBXObjList = new List<tbltestanalyze_results_observationresult>();
                var sOBRObj = new tbltestanalyze_results_observationrequest();
                foreach (var observation in sORU_R01.PATIENT_RESULTs.FirstOrDefault().ORDER_OBSERVATIONs)
                {
                    String sFillerOrdNum = "";
                    sFillerOrdNum = observation.OBR.FillerOrderNumber.EntityIdentifier.Value + "^" +
                                                observation.OBR.FillerOrderNumber.NamespaceID.Value + "^" +
                                                observation.OBR.FillerOrderNumber.UniversalID.Value + "^" +
                                                observation.OBR.FillerOrderNumber.UniversalIDType.Value;

                    if (sFillerOrdNum.Replace("^", "").Length == 0)
                    {
                        sFillerOrdNum = "";
                    }

                    sResultTestType = observation.OBR.UniversalServiceID.Text.Value;
                    sUniversalIdentifier = observation.OBR.UniversalServiceID.Text.Value;

                    sOBRObj.SetID = observation.OBR.SetIDOBR.Value;
                    sOBRObj.PlacerOrderNumber = observation.OBR.PlacerOrderNumber.EntityIdentifier.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.NamespaceID.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.UniversalID.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.UniversalIDType.Value;
                    sOBRObj.FillerOrderNumber = sFillerOrdNum;
                    sOBRObj.UniversalServIdentifier = observation.OBR.UniversalServiceID.Identifier.Value + "^" +
                                                      observation.OBR.UniversalServiceID.Text.Value + "^" +
                                                      observation.OBR.UniversalServiceID.NameOfCodingSystem.Value;
                    sOBRObj.Priority = observation.OBR.PriorityOBR.Value;
                    sOBRObj.RequestedDateTime = (observation.OBR.RequestedDateTime.TimeOfAnEvent.Value != null) ?
                                                 observation.OBR.RequestedDateTime.TimeOfAnEvent.Value.ToString().Trim() : null;
                    sOBRObj.ObservationDateTime = (observation.OBR.ObservationDateTime.TimeOfAnEvent.Value != null) ?
                                                  observation.OBR.ObservationDateTime.TimeOfAnEvent.Value.ToString().Trim() : null;
                    sOBRObj.ObservationEndDateTime = (observation.OBR.ObservationEndDateTime.TimeOfAnEvent.Value != null) ?
                                                      observation.OBR.ObservationEndDateTime.TimeOfAnEvent.Value.ToString().Trim() : null;
                    sOBRObj.CollectVolume = observation.OBR.CollectionVolume.Quantity.Value;
                    sOBRObj.CollectorIdentifier = (observation.OBR.GetCollectorIdentifier().Count() > 0) ?
                                                   observation.OBR.GetCollectorIdentifier().FirstOrDefault().IDNumber.Value : null;
                    sOBRObj.SpecimenActionCode = observation.OBR.SpecimenActionCode.Value;

                    if (observation.NTEs.Count() > 0)
                    {
                        sNTEObj.Add(new tbltestanalyze_results_notes
                        {
                            SetID = (observation.NTEs.Count() > 0) ?
                                    observation.NTEs.FirstOrDefault().SetIDNTE.Value : null,
                            Segment = "OBR",
                            SourceComment = (observation.NTEs.Count() > 0) ?
                                             observation.NTEs.FirstOrDefault().SourceOfComment.Value : null,
                            Comment = GenerateNTEComments(observation.NTEs.ToList())
                        });

                        if (sUniversalIdentifier.ToLower().Contains("babesia") || sUniversalIdentifier.ToLower().Contains("8 panel"))
                        {
                            sTestResultStatus = GenerateNTEComments(observation.NTEs.ToList());
                        }
                    }

                    // --------------- Observation Results ----------------//
                    foreach (var observationDetail in observation.OBSERVATIONs)
                    {                        
                        String sInterpretation = "";
                        String sObservValue = "";
                        strObserveValue = "";
                        if (observationDetail.OBX.GetObservationValue().Count() > 0)
                        {
                            if (observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V231.Datatype.NA))
                            {
                                var sNAObject = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data;

                                int iTotalComponent = sNAObject.ExtraComponents.NumComponents();
                                List<String> sVal = new List<String>();

                                // --- Get from Component ------ //
                                PropertyInfo[] props = sNAObject.GetType().GetProperties();
                                foreach (PropertyInfo p in props)
                                {
                                    if (p.PropertyType == typeof(NHapi.Model.V231.Datatype.NM))
                                    {
                                        sVal.Add(p.GetValue(sNAObject, null).ToString());
                                    };
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
                                    Decimal.TryParse(sObservValue.Replace("<", ""), out iResultValue);
                                }
                            }
                            else
                            {
                                if (observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V231.Datatype.CWE))
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

                            strObserveValue = sObservValue;
                        }
                        else if (observationDetail.OBX.GetProbability().Count() > 0)
                        {
                            sObservValue = observationDetail.OBX.GetProbability()[0].Value;
                            Decimal.TryParse(sObservValue.Replace("<", ""), out iResultValue);
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

                        String sEquipmentIdentifier = "";
                        //if (observationDetail.OBX.GetEquipmentInstanceIdentifier().Length > 0)
                        //{
                        //    sEquipmentIdentifier = observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().EntityIdentifier.Value + "^" +
                        //                           observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().NamespaceID.Value + "^" +
                        //                           observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().UniversalID.Value;
                        //    if (sEquipmentIdentifier.Replace("^", "").Length == 0)
                        //    {
                        //        sEquipmentIdentifier = "";
                        //    }
                        //}

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

                        sOBXObjList.Add(new tbltestanalyze_results_observationresult
                        {
                            SetID = observationDetail.OBX.SetIDOBX.Value,
                            ValueType = observationDetail.OBX.ValueType.Value,
                            ObservationIdentifier = sObservIdentifier,
                            ObservationSubID = observationDetail.OBX.ObservationSubID.Value,
                            ObservationValue = sObservValue,
                            Units = sUnitValue,
                            ReferencesRange = observationDetail.OBX.ReferencesRange.Value,
                            AbnormalFlag = (observationDetail.OBX.GetAbnormalFlags().Length > 0) ?
                                           observationDetail.OBX.GetAbnormalFlags().FirstOrDefault().Value : null,
                            ObservationResultStatus = observationDetail.OBX.ObservationResultStatus.Value,
                            ObservationDateTime = (observationDetail.OBX.DateTimeOfTheObservation.TimeOfAnEvent.Value != null) ?
                                                   observationDetail.OBX.DateTimeOfTheObservation.TimeOfAnEvent.Value.ToString() : null,
                            ProducerID = "",
                            ResponsibleObserver = sOperatorID,
                            ObservationMethod = (observationDetail.OBX.GetObservationMethod().Length > 0) ?
                                                observationDetail.OBX.GetObservationMethod().FirstOrDefault().Text.ToString() : null,
                            EquipmentInstanceIdentifier = sEquipmentIdentifier,
                            AnalysisDateTime = null

                        });

                        // ------------ Notes -------------------//
                        if (observationDetail.NTEs.Count() > 0)
                        {
                            //String sComment = GenerateNTEComments(observationDetail.NTEs.ToList());
                            String sComment = "";

                            sNTEObj.Add(new tbltestanalyze_results_notes
                            {
                                SetID = (observationDetail.NTEs.Count() > 0) ?
                                        observationDetail.NTEs.FirstOrDefault().SetIDNTE.Value.Trim() : null,
                                Segment = "OBX",
                                SourceComment = (observationDetail.NTEs.Count() > 0) ?
                                                 observationDetail.NTEs.FirstOrDefault().SourceOfComment.Value : null,
                                Comment = sComment
                            });

                            if (sComment.ToLower().Contains("cut off index"))
                            {
                                sResultRule = "COI";

                                String sValue = "";
                                String[] strArryValue = sComment.Split(",");
                                if (strArryValue.Length > 0)
                                {
                                    sValue = strArryValue[1].Replace("Value=", "").Trim();
                                }

                                Decimal.TryParse(sValue, out iResultValue);
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

                        if (!String.IsNullOrEmpty(observationDetail.OBX.ReferencesRange.Value))
                        {
                            isRangeReference = true;
                        }
                        else
                        {
                            isRangeReference = false;
                        }


                        if (!(observationDetail.OBX.ObservationIdentifier.Identifier.Value.ToLower() == "age") &&
                            !(observationDetail.OBX.ObservationIdentifier.Identifier.Value.ToLower() == "weight"))
                        {
                            String sStatus = General.ProcessObservationResultStatusValue(isRangeReference, sObservValue, observationDetail.OBX.ReferencesRange.Value, iResultValue);

                            var testParamName = (observationDetail.OBX.ObservationIdentifier.Text.Value != null && observationDetail.OBX.ObservationIdentifier.Text.Value != "") ? observationDetail.OBX.ObservationIdentifier.Text.Value : observationDetail.OBX.ObservationIdentifier.Identifier.Value;

                            sTestResultDetails.Add(new txn_testresults_details
                            {
                                TestParameter = testParamName,
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

                txn_testresults sTestResultObj = new txn_testresults();
                sTestResultObj.TestResultDateTime = dAnalysisDateTime;
                sTestResultObj.TestResultType = sResultTestType;
                sTestResultObj.OperatorID = sOperatorID;
                sTestResultObj.PatientID = sPatientID;
                sTestResultObj.PatientName = sPatientName;
                sTestResultObj.InchargePerson = sDoctorName;
                sTestResultObj.OverallStatus = sOverallStatus;
                sTestResultObj.CreatedDate = DateTime.Now;
                sTestResultObj.CreatedBy = sSystemName;
                sTestResultObj.DeviceSerialNo = sSerialNo.Trim();

                tbltestanalyze_results sResultObj = new tbltestanalyze_results
                {
                    MessageType = sORU_R01.MSH.MessageType.MessageStructure.Value,
                    MessageDateTime = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = sSystemName
                };

                Boolean bResult = TestResultRepository.insertTestObservationMessage(sResultObj, sMSHObj, sPIDObj, sOBRObj, sOBXObjList, sNTEObj, sPVObj);
                //Boolean bResult = true;
                if (bResult)
                {
                    // Insert into Test Result table & create notification 
                    TestResultRepository.createTestResultsMultipleParam(sTestResultObj, sTestResultDetails);

                    NotificationRepository.SendNotification(sTestResultObj.PatientID, sSystemName);
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.Error("ProcessMessage >>> V231 >>> " + ex.ToString());

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
                _logger.Error("GenerateNTEComments >>> V231 >>> " + ex.ToString());
            }

            return nteComment;
        }
    }
}
