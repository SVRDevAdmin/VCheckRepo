using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheckListenerWorker.Lib.Models;

namespace VCheckListenerWorker.Lib.Logic.HL7.V26
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
                NHapi.Model.V26.Message.ORU_R01 sRU_R01 = (NHapi.Model.V26.Message.ORU_R01)sIMessage;

                String sResultRule = "";
                String sResultStatus = "";
                String sResultTestType = "";
                String sOperatorID = "";
                String sPatientID = "";
                String strObserveValue = "";
                String sSerialNo = "";
                String sUniversalIdentifier = "";
                String sTestResultStatus = "";
                String strResultObservStatus = "";
                Decimal iResultValue = 0;
                Boolean isRangeReference = false;
                DateTime dAnalysisDateTime = DateTime.MinValue;

                if (sRU_R01.MSH.SendingApplication.NamespaceID != null && sRU_R01.MSH.SendingApplication.NamespaceID.Value != null)
                {
                    sSerialNo = sRU_R01.MSH.SendingApplication.NamespaceID.Value.Trim();
                }

                // --------------- Message Header --------------//
                tbltestanalyze_results_messageheader sMSHObj = new tbltestanalyze_results_messageheader
                {
                    FieldSeparator = sRU_R01.MSH.FieldSeparator.Value,
                    EncodingCharacters = sRU_R01.MSH.EncodingCharacters.Value,
                    SendingApplication = ((sRU_R01.MSH.SendingApplication.NamespaceID.Value != null) ? sRU_R01.MSH.SendingApplication.NamespaceID.Value.Trim() : "") + "^" +
                                         ((sRU_R01.MSH.SendingApplication.UniversalID.Value != null) ? sRU_R01.MSH.SendingApplication.UniversalID.Value.Trim() : "") + "^" +
                                         ((sRU_R01.MSH.SendingApplication.UniversalIDType.Value != null) ? sRU_R01.MSH.SendingApplication.UniversalIDType.Value.Trim() : ""),
                    SendingFacility = sRU_R01.MSH.SendingFacility.NamespaceID.Value,
                    ReceivingApplication = sRU_R01.MSH.ReceivingApplication.NamespaceID.Value,
                    ReceivingFacility = sRU_R01.MSH.ReceivingFacility.NamespaceID.Value,
                    DateTimeMessage = sRU_R01.MSH.DateTimeOfMessage.Value,
                    MessageType = sRU_R01.MSH.MessageType.MessageCode.Value + "^" +
                                  sRU_R01.MSH.MessageType.TriggerEvent.Value + "^" +
                                  sRU_R01.MSH.MessageType.MessageStructure.Value,
                    MessageControlID = sRU_R01.MSH.MessageControlID.Value.ToString(),
                    ProcessingID = sRU_R01.MSH.ProcessingID.ProcessingID.Value,
                    VersionID = sRU_R01.MSH.VersionID.VersionID.Value,
                    AcceptAckmgtType = sRU_R01.MSH.AcceptAcknowledgmentType.Value,
                    AppAckmgtType = sRU_R01.MSH.ApplicationAcknowledgmentType.Value,
                    CountryCode = sRU_R01.MSH.CountryCode.Value,
                    CharacterSet = (sRU_R01.MSH.GetCharacterSet().Length > 0) ? sRU_R01.MSH.GetCharacterSet().FirstOrDefault().Value : null,
                    PrincipalLanguageMsg = sRU_R01.MSH.PrincipalLanguageOfMessage.Identifier.Value + "^" +
                                           sRU_R01.MSH.PrincipalLanguageOfMessage.Text.Value + "^" +
                                           sRU_R01.MSH.PrincipalLanguageOfMessage.NameOfCodingSystem.Value,
                    MessageProfileIdentifier = (sRU_R01.MSH.GetMessageProfileIdentifier().Length > 0) ?
                                               sRU_R01.MSH.GetMessageProfileIdentifier().FirstOrDefault().EntityIdentifier.Value + "^" +
                                               sRU_R01.MSH.GetMessageProfileIdentifier().FirstOrDefault().NamespaceID.Value + "^" +
                                               sRU_R01.MSH.GetMessageProfileIdentifier().FirstOrDefault().UniversalID.Value + "^" +
                                               sRU_R01.MSH.GetMessageProfileIdentifier().FirstOrDefault().UniversalIDType.Value : null
                };

                // ------------ Patient Identification --------------------//
                List<tbltestanalyze_results_patientidentification> sPIDObj = new List<tbltestanalyze_results_patientidentification>();

                tbltestanalyze_results_patientidentification sPID = new tbltestanalyze_results_patientidentification();
                if (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.PatientIdentifierListRepetitionsUsed > 0)
                {
                    sPID.SetID = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.SetIDPID.Value;
                    sPID.PatientID = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.PatientID.IDNumber.Value;
                    sPID.AlternatePatientID = (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetAlternatePatientIDPID().Length > 0) ?
                                               sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetAlternatePatientIDPID().FirstOrDefault().IDNumber.ToString() : null;

                    sPID.PatientIdentifierList = (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetPatientIdentifierList().Length > 0) ?
                                                 sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetPatientIdentifierList().FirstOrDefault().IDNumber.ToString() : null;


                    if (String.IsNullOrEmpty(sPID.PatientID) && !String.IsNullOrEmpty(sPID.PatientIdentifierList))
                    {
                        sPatientID = sPID.PatientIdentifierList;
                    }
                    else
                    {
                        sPatientID = sPID.PatientID;
                    }
                }

                if (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.PatientNameRepetitionsUsed > 0)
                {
                    var sNameObj = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetPatientName().FirstOrDefault();

                    if (sNameObj != null)
                    {
                        sPID.PatientName = "" + "^" +
                                           sNameObj.GivenName + "^" +
                                           sNameObj.SecondAndFurtherGivenNamesOrInitialsThereof + "^" +
                                           sNameObj.SuffixEgJRorIII + "^" +
                                           sNameObj.PrefixEgDR + "^" +
                                           sNameObj.DegreeEgMD + "^" +
                                           sNameObj.NameTypeCode;

                        if (sPID.PatientName.Replace("^", "").Length == 0)
                        {
                            sPID.PatientName = "";
                        }
                    }
                    else
                    {
                        sPID.PatientName = "";
                    }
                }
                sPIDObj.Add(sPID);

                //----------------- Observation Request ----------------------//
                var sTestResultLst = new List<txn_testresults>();
                var sTestResultDetails = new List<txn_testresults_details>();
                var sNTEObj = new List<tbltestanalyze_results_notes>();
                var sOBXObjList = new List<tbltestanalyze_results_observationresult>();
                var sOBRObj = new tbltestanalyze_results_observationrequest();
                foreach (var observation in sRU_R01.PATIENT_RESULTs.FirstOrDefault().ORDER_OBSERVATIONs)
                {
                    sResultTestType = observation.OBR.UniversalServiceIdentifier.Text.Value;
                    sUniversalIdentifier = observation.OBR.UniversalServiceIdentifier.Text.Value;

                    sOBRObj.SetID = observation.OBR.SetIDOBR.Value;
                    sOBRObj.PlacerOrderNumber = observation.OBR.PlacerOrderNumber.EntityIdentifier.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.NamespaceID.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.UniversalID.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.UniversalIDType.Value;
                    sOBRObj.FillerOrderNumber = observation.OBR.FillerOrderNumber.EntityIdentifier.Value + "^" +
                                                observation.OBR.FillerOrderNumber.NamespaceID.Value + "^" +
                                                observation.OBR.FillerOrderNumber.UniversalID.Value + "^" +
                                                observation.OBR.FillerOrderNumber.UniversalIDType.Value;
                    sOBRObj.UniversalServIdentifier = observation.OBR.UniversalServiceIdentifier.Identifier.Value + "^" +
                                                      observation.OBR.UniversalServiceIdentifier.Text.Value + "^" +
                                                      observation.OBR.UniversalServiceIdentifier.NameOfCodingSystem.Value;
                    sOBRObj.Priority = observation.OBR.Priority.Value;
                    sOBRObj.RequestedDateTime = observation.OBR.RequestedDateTime.Value;
                    sOBRObj.ObservationDateTime = observation.OBR.ObservationDateTime.Value.Trim();
                    sOBRObj.ObservationEndDateTime = observation.OBR.ObservationEndDateTime.Value.Trim();
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
                            ObservationDateTime = observationDetail.OBX.DateTimeOfTheObservation.Value,
                            ProducerID = observationDetail.OBX.ProducerSID.Text.Value,
                            ResponsibleObserver = sOperatorID,
                            ObservationMethod = (observationDetail.OBX.GetObservationMethod().Length > 0) ?
                                                 observationDetail.OBX.GetObservationMethod().FirstOrDefault().Text.ToString() : null,
                            EquipmentInstanceIdentifier = (observationDetail.OBX.GetEquipmentInstanceIdentifier().Length > 0) ?
                                                           observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().EntityIdentifier.Value : null,
                            AnalysisDateTime = observationDetail.OBX.DateTimeOfTheAnalysis.Value

                        });

                        // ------------ Notes -------------------//
                        if (observationDetail.NTEs.Count() > 0)
                        {
                            String sComment = GenerateNTEComments(observationDetail.NTEs.ToList());

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

                                                Decimal.TryParse(sValue, out iResultValue);
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

                                    Decimal.TryParse(sValue, out iResultValue);
                                    sObservValue = sValue;
                                }
                            }

                            if (sComment.ToLower().Contains("interpretation"))
                            {
                                String[] strArryValue = sComment.Split("=");
                                if (strArryValue.Length > 0)
                                {
                                    sInterpretation = strArryValue[1].Trim();
                                    sTestResultStatus = strArryValue[1].Trim();
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


                        if (sResultTestType.ToLower() != "babesia gibsoni/canis" && sResultTestType.ToLower() != "canine diarrhea 8 panel")
                        {
                            if (sResultTestType.ToLower() != "cav ab")
                            {
                                if (sResultRule == "COI")
                                {
                                    sTestResultStatus = General.ProcessObservationResultStatusValue(false, sObservValue, observationDetail.OBX.ReferencesRange.Value, iResultValue);
                                }
                                //sTestResultStatus = General.ProcessObservationResultStatusValue(isRangeReference, sObservValue, observationDetail.OBX.ReferencesRange.Value, iResultValue);
                            }
                            
                            sTestResultDetails.Add(new txn_testresults_details
                            {
                                TestParameter = observationDetail.OBX.ObservationIdentifier.Text.Value,
                                SubID = observationDetail.OBX.ObservationSubID.Value,
                                ProceduralControl = strResultObservStatus,
                                TestResultStatus = sTestResultStatus,
                                TestResultValue = sObservValue,
                                TestResultUnit = (!String.IsNullOrEmpty(observationDetail.OBX.Units.Identifier.Value)) ? observationDetail.OBX.Units.Identifier.Value : sResultRule,
                                ReferenceRange = observationDetail.OBX.ReferencesRange.Value,
                                Interpretation = sInterpretation
                            });
                        }
                    }
                }

                if (sResultTestType.ToLower() == "babesia gibsoni/canis" || sResultTestType.ToLower() == "canine diarrhea 8 panel")
                {
                    sTestResultDetails = ProcessBabesiaGibsoniTestResult(sOBXObjList);
                }

                String sOverallStatus = "Normal";
                if (sTestResultDetails != null)
                {
                    if (sTestResultDetails.Where(x => x.TestParameter.ToLower() == "cav ab").Count() > 0)
                    {
                        sOverallStatus = sTestResultDetails[0].TestResultStatus;
                    }
                    else
                    {
                        if (sTestResultDetails.Where(x => x.TestResultStatus == "Positive").Count() > 0) 
                        {
                            sOverallStatus = "Abnormal";
                        }
                        else if (sTestResultDetails.Where(x => x.TestResultStatus == "Invalid").Count() > 0)
                        {
                            sOverallStatus = "Invalid";
                        }
                        else
                        {
                            sOverallStatus = sTestResultDetails.Where(x => x.TestResultStatus != "").FirstOrDefault().TestResultStatus;
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
                sTestResultObj.DeviceSerialNo = sSerialNo.Trim();

                tbltestanalyze_results sResultObj = new tbltestanalyze_results
                {
                    MessageType = sRU_R01.MSH.MessageType.MessageStructure.Value,
                    MessageDateTime = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = sSystemName
                };

                Boolean bResult = TestResultRepository.insertTestObservationMessage(sResultObj, sMSHObj, sPIDObj, sOBRObj, sOBXObjList, sNTEObj, null, null, null);
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
                _logger.Error("ProcessMessage >>> V26 >>> " + ex.ToString());
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get NTE Comment data 
        /// </summary>
        /// <param name="nte"></param>
        /// <returns></returns>
        public static String GenerateNTEComments(List<NHapi.Model.V26.Segment.NTE> nte)
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
                _logger.Error("GenerateNTEComments >>> V26 >>> " + ex.ToString());
            }

            return nteComment;
        }

        /// <summary>
        /// Process Babesia type observation test result 
        /// </summary>
        /// <param name="sObservationResult"></param>
        /// <returns></returns>
        public static List<txn_testresults_details> ProcessBabesiaGibsoniTestResult(List<tbltestanalyze_results_observationresult> sObservationResult)
        {
            List<txn_testresults_details> sResultDetails = new List<txn_testresults_details>();

            var sObservationGroup = sObservationResult.GroupBy(x => x.ObservationIdentifier).ToList();
            if (sObservationGroup.Count() > 0)
            {
                foreach (var g in sObservationGroup)
                {
                    var sObserv = sObservationResult.Where(x => x.ObservationIdentifier == g.Key).ToList();
                    if (sObserv != null)
                    {
                        String sIdentifier = "";
                        String[] arrayIdentifier = g.Key.Split("^");

                        if (arrayIdentifier.Length > 0)
                        {
                            sIdentifier = arrayIdentifier[1];
                        }

                        String sSetID = sObserv.OrderBy(x => Convert.ToInt32(x.SetID)).FirstOrDefault().SetID;
                        int iNextID = Convert.ToInt32(sSetID) + 2;

                        String sProcControl = "";
                        var s = sObserv.Where(x => x.SetID == sSetID).FirstOrDefault().ObservationResultStatus;
                        if (s == "F")
                        {
                            sProcControl = "Valid";
                        }
                        else
                        {
                            sProcControl = "Invalid";
                        }

                        String sUnitString = "";
                        var unitObj = sObserv.Where(x => x.SetID == sSetID).FirstOrDefault().Units;
                        if (unitObj != null)
                        {
                            String[] arrayUnit = unitObj.Split("^");
                            if (arrayUnit.Length > 0)
                            {
                                sUnitString = arrayUnit[0];
                            }
                        }

                        sResultDetails.Add(new txn_testresults_details
                        {
                            TestParameter = sIdentifier,
                            SubID = sObserv.Where(x => x.SetID == sSetID).FirstOrDefault().ObservationSubID,
                            ProceduralControl = sProcControl,
                            TestResultStatus = sObserv.Where(x => x.SetID == sSetID).FirstOrDefault().ObservationValue,
                            TestResultValue = sObserv.Where(x => x.SetID == iNextID.ToString()).FirstOrDefault().ObservationValue.ToString(),
                            TestResultUnit = sUnitString,
                            ReferenceRange = sObserv.Where(x => x.SetID == sSetID).FirstOrDefault().ReferencesRange,
                            Interpretation = ""
                        });
                    }
                }
            }

            return sResultDetails;
        }
    }
}
