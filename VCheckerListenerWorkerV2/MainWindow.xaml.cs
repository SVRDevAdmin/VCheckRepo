﻿using log4net.Config;
using log4net.Core;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using VCheckerListenerWorkerV2.Lib.Models;
using VCheckerListenerWorkerV2.Lib.Logic;
using Microsoft.Extensions.Configuration;
using VCheckerListenerWorkerV2.Lib.Object;

namespace VCheckerListenerWorkerV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource source = new CancellationTokenSource();
        SynchronizationContext syncContext = SynchronizationContext.Current;
        Task task = null;

        VCheckerListenerWorkerV2.Lib.Util.Logger sLogger;
        System.Net.Sockets.Socket sListener = null;

        public String sSystemName = "VCheckViewer Listener";

        public MainWindow()
        {
            InitializeComponent();

            XmlConfigurator.Configure(log4net.LogManager.GetRepository(Assembly.GetEntryAssembly()),
                                      new FileInfo("log4net.config"));

            sLogger = new VCheckerListenerWorkerV2.Lib.Util.Logger();

            List<ListViewObject> sListingObject = new List<ListViewObject>();
        }

        private void StartListener()
        {
            var configBuilder = Host.CreateApplicationBuilder();

            try
            {
                String sHostIP = configBuilder.Configuration.GetSection("Listener:HostIP").Value;
                int iPortNo = Convert.ToInt32(configBuilder.Configuration.GetSection("Listener:Port").Value);

                System.Net.IPEndPoint sIPEndPoint = System.Net.IPEndPoint.Parse(String.Concat(sHostIP, ":", iPortNo));
                sListener = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                        System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

                sListener.Bind(sIPEndPoint);
                sListener.Listen(3);
                source.Token.Register(sListener.Dispose);

                syncContext.Send(x => lbStatus.Text = "Online", null);
                syncContext.Send(x => lbStatus.Foreground = new BrushConverter().ConvertFrom("Green") as SolidColorBrush, null);

                while (!source.IsCancellationRequested)
                {
                    while (true)
                    {
                        System.Net.Sockets.Socket sClient = sListener.Accept();
                        syncContext.Send(x => txtTest.Text = txtTest.Text = txtTest.Text + "Connection Accepted.", null);

                        byte[] bBuffer = new byte[32768];

                        var childSocket = new Thread(() =>
                        {
                            int s = sClient.Receive(bBuffer);

                            String sData = System.Text.Encoding.ASCII.GetString(bBuffer, 0, s);
                            sData = sData.Replace("\u001c", "")
                                         .Replace("\n", "\r");

                            if (!String.IsNullOrEmpty(sData))
                            {
                                //Console.WriteLine(sData);
                                //txtTest.Text = txtTest.Text + sData;
                                //syncContext.Send(x => lvIncomingMessage.)
                                ListViewObject sItem = new ListViewObject
                                {
                                    transactionDate = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"),
                                    transactionMsgFiltered = sData.Substring(0, 200).Trim(),
                                    transactionMsg = sData
                                };
                                
                                syncContext.Send(x => lvIncomingMessage.Items.Insert(0, sItem), null);
                                syncContext.Send(x => txtTest.Text = txtTest.Text = txtTest.Text + sData, null);

                                NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser();
                                NHapi.Base.Model.IMessage sIMessage = sParser.Parse(sData.Trim());

                                String sXMLMessage = String.Empty;
                                String sAckMessage = String.Empty;
                                if (sIMessage.Version == "2.5.1")
                                {
                                    NHapi.Model.V251.Message.OUL_R22 sRU_R01 = (NHapi.Model.V251.Message.OUL_R22)sIMessage;
                                    NHapi.Base.Parser.XMLParser sXMLParser = new NHapi.Base.Parser.DefaultXMLParser();
                                    //String sXMLMessage = sXMLParser.Encode(sIMessage);
                                    sXMLMessage = sXMLParser.Encode(sIMessage);
                                    Console.WriteLine(sXMLMessage);

                                    //String sAckMessage = Lib.Util.ResponseRepo.CreateResponseMessage2(sRU_R01);
                                    sAckMessage = Lib.Util.ResponseRepo.CreateResponseMessage2(sRU_R01);
                                    var sMessageByte = System.Text.Encoding.UTF8.GetBytes(sAckMessage);
                                    sClient.SendAsync(sMessageByte, System.Net.Sockets.SocketFlags.None);
                                }
                                else
                                {
                                    NHapi.Model.V26.Message.ORU_R01 sRU_R01 = (NHapi.Model.V26.Message.ORU_R01)sIMessage;
                                    NHapi.Base.Parser.XMLParser sXMLParser = new NHapi.Base.Parser.DefaultXMLParser();
                                    sXMLMessage = sXMLParser.Encode(sIMessage);
                                    //String sXMLMessage = sXMLParser.Encode(sIMessage);
                                    Console.WriteLine(sXMLMessage);

                                    // Populate Acknowledge message & Send back
                                    //String sAckMessage = Lib.Util.ResponseRepo.CreateResponseMessage(sRU_R01);
                                    sAckMessage = Lib.Util.ResponseRepo.CreateResponseMessage(sRU_R01);
                                    var sMessageByte = System.Text.Encoding.UTF8.GetBytes(sAckMessage);
                                    sClient.SendAsync(sMessageByte, System.Net.Sockets.SocketFlags.None);
                                }


                                //// Populate Acknowledge message & Send back
                                // String sAckMessage = Lib.Util.ResponseRepo.CreateResponseMessage(sRU_R01);
                                // var sMessageByte = System.Text.Encoding.UTF8.GetBytes(sAckMessage);
                                // sClient.SendAsync(sMessageByte, System.Net.Sockets.SocketFlags.None);

                                String sFileName = "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                                // Ouput Message to file
                                OutputMessage(configBuilder, sFileName, sData, sXMLMessage, sAckMessage);

                                // Save raw data
                                if (sIMessage.Version == "2.5.1")
                                {
                                    ProcessHL7Version251Message(sIMessage);
                                }
                                else
                                {
                                    ProcessHL7Message(sIMessage);
                                }
                            }

                            sClient.Close();
                        });

                        childSocket.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                String abc = "abc";
                syncContext.Send(x => txtTest.Text = txtTest.Text = txtTest.Text + ex.ToString(), null);
            }
            finally
            {
                //sListener.Dispose();
                //sListener.Dispose();
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            task = Task.Factory.StartNew(StartListener, source.Token);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //sListener.Disconnect(true);
            //sListener.Dispose();
            source.Cancel();

            syncContext.Send(x => txtTest.Text = "Connection Closed.", null);
        }

        private void btnStartListener_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                task = Task.Factory.StartNew(StartListener, source.Token);

                syncContext.Send(x => bdStatusLight.BorderBrush = new BrushConverter().ConvertFrom("Green") as SolidColorBrush, null);
                syncContext.Send(x => bdStatusLight.Background = new BrushConverter().ConvertFrom("Green") as SolidColorBrush, null);
                syncContext.Send(x => btnStartListener.IsEnabled = false, null);
                syncContext.Send(x => btnStopListener.IsEnabled = true, null);
            }
            catch (Exception ex)
            {

            }
            
        }

        private async void btnStopListener_Click(object sender, RoutedEventArgs e)
        {
            //task.Dispose();
            sListener.Dispose();
            source.Cancel();

            //syncContext.Send(x => sListener.Disconnect(true), null);
            //syncContext.Send(x => sListener.Dispose(), null);

            syncContext.Send(x => txtTest.Text = "Connection Closed.", null);
            syncContext.Send(x => lbStatus.Text = "Offline", null);
            syncContext.Send(x => lbStatus.Foreground = new BrushConverter().ConvertFrom("Red") as SolidColorBrush, null);

            syncContext.Send(x => bdStatusLight.BorderBrush = new BrushConverter().ConvertFrom("Red") as SolidColorBrush, null);
            syncContext.Send(x => bdStatusLight.Background = new BrushConverter().ConvertFrom("Red") as SolidColorBrush, null);
            syncContext.Send(x => btnStartListener.IsEnabled = true, null);
            syncContext.Send(x => btnStopListener.IsEnabled = false, null);
        }

        /// <summary>
        /// Process data and save to DB
        /// </summary>
        /// <param name="sIMessage"></param>
        public void ProcessHL7Message(NHapi.Base.Model.IMessage sIMessage)
        {
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


                        if (sResultTestType.ToLower() != "babesia gibsoni/canis")
                        {
                            sTestResultStatus = ProcessObservationResultStatusValue(isRangeReference, sObservValue, observationDetail.OBX.ReferencesRange.Value, iResultValue);

                            sTestResultDetails.Add(new txn_testresults_details
                            {
                                TestParameter = observationDetail.OBX.ObservationIdentifier.Text.Value,
                                SubID = observationDetail.OBX.ObservationSubID.Value,
                                //ProceduralControl = strObserveValue,
                                ProceduralControl = strResultObservStatus,
                                TestResultStatus = sTestResultStatus,
                                //TestResultValue = (isRangeReference) ? sObservValue : iResultValue.ToString(),
                                TestResultValue = sObservValue,
                                TestResultUnit = (!String.IsNullOrEmpty(observationDetail.OBX.Units.Identifier.Value)) ? observationDetail.OBX.Units.Identifier.Value : sResultRule,
                                ReferenceRange = observationDetail.OBX.ReferencesRange.Value,
                                Interpretation = sInterpretation
                            });
                        }
                    }
                }

                if (sResultTestType.ToLower() == "babesia gibsoni/canis")
                {
                    sTestResultDetails = ProcessBabesiaGibsoniTestResult(sOBXObjList);
                }

                txn_testresults sTestResultObj = new txn_testresults();
                sTestResultObj.TestResultDateTime = dAnalysisDateTime;
                sTestResultObj.TestResultType = sResultTestType;
                sTestResultObj.OperatorID = sOperatorID;
                sTestResultObj.PatientID = sPatientID;
                sTestResultObj.InchargePerson = "";
                //sTestResultObj.ObservationStatus = strResultObservStatus;
                //sTestResultObj.TestResultStatus = sTestResultStatus;
                //sTestResultObj.TestResultValue = iResultValue; //
                //sTestResultObj.TestResultRules = sResultRule; //
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

                    SendNotification(sTestResultObj.PatientID);
                }
            }
            catch (Exception ex)
            {
                sLogger.Error("Function ProcessHL7Message >>> " + ex.ToString());
            }
        }

        /// <summary>
        /// Process data and save into DB (HL7 version 2.5.1)
        /// </summary>
        /// <param name="sIMessage"></param>
        public void ProcessHL7Version251Message(NHapi.Base.Model.IMessage sIMessage)
        {
            try
            {
                NHapi.Model.V251.Message.OUL_R22 sRU_R01 = (NHapi.Model.V251.Message.OUL_R22)sIMessage;
                String sResultRule = "";
                String sResultStatus = "";
                String sResultTestType = "";
                String sOperatorID = "";
                String sPatientID = "";
                String strObserveValue = "";
                String sSerialNo = "";
                String sUniversalIdentifier = "";
                String sTestResultStatus = "";
                Boolean isRangeReference = false;
                String strResultObservStatus = "";
                String sDoctorName = "";
                Decimal iResultValue = 0;
                DateTime dAnalysisDateTime = DateTime.MinValue;

                var sSPMObj = new tbltestanalyze_results_specimen();
                var sSACObj = new tbltestanalyze_results_specimencontainer();

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
                    DateTimeMessage = sRU_R01.MSH.DateTimeOfMessage.Time.ToString(),
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
                if (sRU_R01.PATIENT.PID.PatientIdentifierListRepetitionsUsed > 0)
                {
                    sPID.SetID = sRU_R01.PATIENT.PID.SetIDPID.Value;
                    sPID.PatientID = sRU_R01.PATIENT.PID.PatientID.IDNumber.Value;
                    sPID.AlternatePatientID = (sRU_R01.PATIENT.PID.GetAlternatePatientIDPID().Length > 0) ?
                                               sRU_R01.PATIENT.PID.GetAlternatePatientIDPID().FirstOrDefault().IDNumber.ToString() : null;

                    sPID.PatientIdentifierList = (sRU_R01.PATIENT.PID.GetPatientIdentifierList().Length > 0) ?
                                                 sRU_R01.PATIENT.PID.GetPatientIdentifierList().FirstOrDefault().IDNumber.ToString() : null;


                    if (String.IsNullOrEmpty(sPID.PatientID) && !String.IsNullOrEmpty(sPID.PatientIdentifierList))
                    {
                        sPatientID = sPID.PatientIdentifierList;
                    }
                    else
                    {
                        sPatientID = sPID.PatientID;
                    }
                }


                if (sRU_R01.PATIENT.PID.PatientNameRepetitionsUsed > 0)
                {
                    var sNameObj = sRU_R01.PATIENT.PID.GetPatientName().FirstOrDefault();

                    if (sNameObj != null)
                    {
                        sPID.PatientName = sNameObj.FamilyName.Surname.Value + "^" +
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

                //----------------- Patient Visit Record ---------------------//
                var sPVObj = new tbltestanalyze_results_patientvisit();

                if (sRU_R01.VISIT != null)
                {
                    var sPatientVisitation1 = sRU_R01.VISIT.PV1;
                    var sPatientVisitation2 = sRU_R01.VISIT.PV2;

                    sPVObj.SetID = sPatientVisitation1.SetIDPV1.Value;
                    sPVObj.PatientClass = sPatientVisitation1.PatientClass.Value;
                    sPVObj.AssignedPatientLocation = sPatientVisitation1.AssignedPatientLocation.Room.Value;
                    sPVObj.AdmissionType = sPatientVisitation1.AdmissionType.Value;
                    sPVObj.PreadmitNumber = sPatientVisitation1.PreadmitNumber.IDNumber.Value;
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
                    sPVObj.VisitNumber = sPatientVisitation1.VisitNumber.IDNumber.Value;

                    sDoctorName = sPVObj.AttendingDoctor;
                }

                // ------ Specimen --------//
                if (sRU_R01.GetSPECIMEN() != null)
                {
                    var sSpecimenObj = sRU_R01.GetSPECIMEN().SPM;

                    String sSpecimentID = "";
                    sSpecimentID = sSpecimenObj.SpecimenID.PlacerAssignedIdentifier.EntityIdentifier.Value + "^" +
                                   sSpecimenObj.SpecimenID.FillerAssignedIdentifier.EntityIdentifier.Value;
                    if (sSpecimentID.Replace("^", "").Length == 0)
                    {
                        sSpecimentID = "";
                    }

                    String sSpecimentParentID = "";
                    if (sSpecimenObj.GetSpecimenParentIDs().Length > 0)
                    {
                        sSpecimentParentID = sSpecimenObj.GetSpecimenParentIDs().FirstOrDefault().PlacerAssignedIdentifier.EntityIdentifier.Value + "^" +
                                             sSpecimenObj.GetSpecimenParentIDs().FirstOrDefault().FillerAssignedIdentifier.EntityIdentifier.Value;
                        if (sSpecimentParentID.Replace("^", "").Length == 0)
                        {
                            sSpecimentParentID = "";
                        }
                    }

                    sSPMObj.SetID = sSpecimenObj.SetIDSPM.Value;
                    sSPMObj.SpecimenID = sSpecimentID;
                    sSPMObj.SpecimentParentID = sSpecimentParentID;
                    sSPMObj.SpecimenType = sSpecimenObj.SpecimenType.Identifier.Value;
                    sSPMObj.SpecimenTypeModifier = (sSpecimenObj.GetSpecimenTypeModifier().Length > 0) ?
                                                    sSpecimenObj.GetSpecimenTypeModifier().FirstOrDefault().Identifier.Value : null;
                    sSPMObj.SpecimenAdditives = (sSpecimenObj.GetSpecimenAdditives().Length > 0) ?
                                                 sSpecimenObj.GetSpecimenAdditives().FirstOrDefault().Identifier.Value : null;
                    sSPMObj.SpecimenCollectionMethod = (sSpecimenObj.SpecimenCollectionMethod.Identifier.Value != null) ?
                                                        sSpecimenObj.SpecimenCollectionMethod.Identifier.Value : null;
                    sSPMObj.SpecimenSourceSite = (sSpecimenObj.SpecimenSourceSite.Identifier.Value != null) ?
                                                  sSpecimenObj.SpecimenSourceSite.Identifier.Value : null;
                    sSPMObj.SpecimenSourceSiteModifier = (sSpecimenObj.GetSpecimenSourceSiteModifier().Length > 0) ?
                                                          sSpecimenObj.GetSpecimenSourceSiteModifier().FirstOrDefault().Identifier.Value : null;
                    sSPMObj.SpecimenCollectionSite = (sSpecimenObj.SpecimenCollectionSite.Identifier.Value != null) ?
                                                      sSpecimenObj.SpecimenCollectionSite.Identifier.Value : null;
                    sSPMObj.SpecimenRole = (sSpecimenObj.GetSpecimenRole().Length > 0) ?
                                            sSpecimenObj.GetSpecimenRole().FirstOrDefault().Identifier.Value : null;
                }

                if (sRU_R01.GetSPECIMEN().CONTAINERs.Count() > 0)
                {
                    var sContainerObj = sRU_R01.GetSPECIMEN().CONTAINERs.FirstOrDefault().SAC;
                    if (sContainerObj != null)
                    {
                        string sContainerStatus = "";
                        sContainerStatus = sContainerObj.ContainerStatus.Identifier.Value + "^" +
                                           sContainerObj.ContainerStatus.Text.Value + "^" +
                                           sContainerObj.ContainerStatus.NameOfCodingSystem;
                        if (sContainerStatus.Replace("^", "").Length == 0)
                        {
                            sContainerStatus = "";
                        }

                        String sCarrierType = "";
                        sCarrierType = sContainerObj.CarrierType.Identifier.Value + "^" +
                                       sContainerObj.CarrierType.Text.Value + "^" +
                                       sContainerObj.CarrierType.NameOfCodingSystem;
                        if (sCarrierType.Replace("^", "").Length == 0)
                        {
                            sCarrierType = "";
                        }

                        String sCarrierIdentifier = "";
                        sCarrierIdentifier = sContainerObj.CarrierIdentifier.EntityIdentifier + "^" +
                                             sContainerObj.CarrierIdentifier.NamespaceID + "^" +
                                             sContainerObj.CarrierIdentifier.UniversalID + "^" +
                                             sContainerObj.CarrierIdentifier.UniversalIDType;
                        if (sCarrierIdentifier.Replace("^", "").Length == 0)
                        {
                            sCarrierIdentifier = "";
                        }

                        String sAdditive = "";
                        if (sContainerObj.GetAdditive().Length > 0)
                        {
                            sAdditive = sContainerObj.GetAdditive().FirstOrDefault().Identifier + "^" +
                                        sContainerObj.GetAdditive().FirstOrDefault().Text.Value + "^" +
                                        sContainerObj.GetAdditive().FirstOrDefault().NameOfCodingSystem.Value;
                            if (sAdditive.Replace("^", "").Length == 0)
                            {
                                sAdditive = "";
                            }
                        }

                        String sCapType = "";
                        sCapType = sContainerObj.CapType.Identifier + "^" +
                                   sContainerObj.CapType.Text.Value + "^" +
                                   sContainerObj.CapType.NameOfCodingSystem.Value;
                        if (sCapType.Replace("^", "").Length == 0)
                        {
                            sCapType = "";
                        }

                        sSACObj.ExternalAccessionIdentifier = sContainerObj.ExternalAccessionIdentifier.EntityIdentifier.Value;
                        sSACObj.AccessionIdentifier = sContainerObj.AccessionIdentifier.EntityIdentifier.Value;
                        sSACObj.ContainerIdentifier = sContainerObj.ContainerIdentifier.EntityIdentifier.Value;
                        sSACObj.PrimaryContainerIdentifier = sContainerObj.PrimaryParentContainerIdentifier.EntityIdentifier.Value;
                        sSACObj.EquipmentContainerIdentifier = sContainerObj.EquipmentContainerIdentifier.EntityIdentifier.Value;
                        sSACObj.SpecimenSource = sContainerObj.SpecimenSource.SpecimenSourceNameOrCode.Identifier.Value;
                        sSACObj.RegistrationDateTime = sContainerObj.RegistrationDateTime.Time.Value;
                        sSACObj.ContainerStatus = sContainerStatus;
                        sSACObj.CarrierType = sCarrierType;
                        sSACObj.CarrierIdentifier = sCarrierIdentifier;
                        sSACObj.PositionInCarrier = sContainerObj.PositionInCarrier.Value1.ToString();
                        sSACObj.TrayTypeSAC = sContainerObj.TrayTypeSAC.Identifier.Value;
                        sSACObj.TrayIdentifier = sContainerObj.TrayIdentifier.EntityIdentifier.Value;
                        sSACObj.PositionInTray = sContainerObj.PositionInTray.Value1.ToString();
                        sSACObj.Location = (sContainerObj.GetLocation().Length > 0) ?
                                            sContainerObj.GetLocation().FirstOrDefault().Identifier.Value : null;
                        sSACObj.ContainerHeight = sContainerObj.ContainerHeight.Value;
                        sSACObj.ContainerDiameter = sContainerObj.ContainerDiameter.Value;
                        sSACObj.BarrierDelta = sContainerObj.BarrierDelta.Value;
                        sSACObj.BottomDelta = sContainerObj.BottomDelta.Value;
                        sSACObj.ContainerHeightDiamtrUnits = sContainerObj.ContainerHeightDiameterDeltaUnits.Text.ToString();
                        sSACObj.ContainerVolume = sContainerObj.ContainerVolume.Value;
                        sSACObj.AvailableSpecimenVolume = sContainerObj.AvailableSpecimenVolume.Value;
                        sSACObj.volumeUnits = sContainerObj.VolumeUnits.Text.ToString();
                        sSACObj.SeparatorType = sContainerObj.SeparatorType.Identifier.Value;
                        sSACObj.CapType = sCapType;
                        sSACObj.Additive = sAdditive;
                        sSACObj.SpecimenComponent = sContainerObj.SpecimenComponent.Identifier.Value;
                        sSACObj.DilutionFactor = sContainerObj.DilutionFactor.Description;
                        sSACObj.Treatment = sContainerObj.Treatment.Identifier.Value;
                        sSACObj.HemolysisIndex = sContainerObj.HemolysisIndex.Value;
                        sSACObj.HemolysisIndexUnits = sContainerObj.HemolysisIndexUnits.Text.ToString();
                        sSACObj.LipemiaIndex = sContainerObj.LipemiaIndex.Value;
                        sSACObj.LipemiaIndexUnits = sContainerObj.LipemiaIndexUnits.Identifier.Value;
                        sSACObj.IcterusIndex = sContainerObj.IcterusIndex.Value;
                        sSACObj.IcterusIndexUnits = sContainerObj.IcterusIndexUnits.Identifier.Value;
                    }

                }


                //----------------- Observation Request ----------------------//
                var sTestResultLst = new List<txn_testresults>();
                var sTestResultDetails = new List<txn_testresults_details>();
                var sNTEObj = new List<tbltestanalyze_results_notes>();
                var sOBXObjList = new List<tbltestanalyze_results_observationresult>();
                var sOBRObj = new tbltestanalyze_results_observationrequest();
                foreach (var observation in sRU_R01.SPECIMENs.FirstOrDefault().ORDERs)
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

                    sResultTestType = observation.OBR.UniversalServiceIdentifier.Text.Value;
                    sUniversalIdentifier = observation.OBR.UniversalServiceIdentifier.Text.Value;

                    sOBRObj.SetID = observation.OBR.SetIDOBR.Value;
                    sOBRObj.PlacerOrderNumber = observation.OBR.PlacerOrderNumber.EntityIdentifier.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.NamespaceID.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.UniversalID.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.UniversalIDType.Value;
                    sOBRObj.FillerOrderNumber = sFillerOrdNum;
                    sOBRObj.UniversalServIdentifier = observation.OBR.UniversalServiceIdentifier.Identifier.Value + "^" +
                                                      observation.OBR.UniversalServiceIdentifier.Text.Value + "^" +
                                                      observation.OBR.UniversalServiceIdentifier.NameOfCodingSystem.Value;
                    sOBRObj.Priority = observation.OBR.PriorityOBR.Value;
                    sOBRObj.RequestedDateTime = (observation.OBR.RequestedDateTime.Time.Value != null) ?
                                                 observation.OBR.RequestedDateTime.Time.Value.ToString().Trim() : null;
                    sOBRObj.ObservationDateTime = (observation.OBR.ObservationDateTime.Time.Value != null) ?
                                                  observation.OBR.ObservationDateTime.Time.Value.ToString().Trim() : null;
                    sOBRObj.ObservationEndDateTime = (observation.OBR.ObservationEndDateTime.Time.Value != null) ?
                                                      observation.OBR.ObservationEndDateTime.Time.Value.ToString().Trim() : null;
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
                            Comment = GenerateNTECommentsV251(observation.NTEs.ToList())
                        });

                        if (sUniversalIdentifier.ToLower().Contains("babesia") || sUniversalIdentifier.ToLower().Contains("8 panel"))
                        {
                            sTestResultStatus = GenerateNTECommentsV251(observation.NTEs.ToList());
                        }
                    }

                    // --------------- Observation Results ----------------//
                    foreach (var observationDetail in observation.RESULTs)
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

                            strObserveValue = sObservValue;
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
                        if (observationDetail.OBX.GetEquipmentInstanceIdentifier().Length > 0)
                        {
                            sEquipmentIdentifier = observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().EntityIdentifier.Value + "^" +
                                                   observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().NamespaceID.Value + "^" +
                                                   observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().UniversalID.Value;
                            if (sEquipmentIdentifier.Replace("^", "").Length == 0)
                            {
                                sEquipmentIdentifier = "";
                            }
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
                            ObservationDateTime = (observationDetail.OBX.DateTimeOfTheObservation.Time.Value != null) ?
                                                   observationDetail.OBX.DateTimeOfTheObservation.Time.Value.ToString() : null,
                            ProducerID = "",
                            ResponsibleObserver = sOperatorID,
                            ObservationMethod = (observationDetail.OBX.GetObservationMethod().Length > 0) ?
                                                observationDetail.OBX.GetObservationMethod().FirstOrDefault().Text.ToString() : null,
                            EquipmentInstanceIdentifier = sEquipmentIdentifier,
                            AnalysisDateTime = (observationDetail.OBX.DateTimeOfTheAnalysis.Time.Value != null) ?
                                                observationDetail.OBX.DateTimeOfTheAnalysis.Time.Value.ToString() : null

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


                        if (!(observationDetail.OBX.ObservationIdentifier.Text.Value.ToLower() == "age") &&
                            !(observationDetail.OBX.ObservationIdentifier.Text.Value.ToLower() == "weight"))
                        {
                            String sStatus = ProcessObservationResultStatusValue(isRangeReference, sObservValue, observationDetail.OBX.ReferencesRange.Value, iResultValue);

                            sTestResultDetails.Add(new txn_testresults_details
                            {
                                TestParameter = observationDetail.OBX.ObservationIdentifier.Text.Value,
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

                txn_testresults sTestResultObj = new txn_testresults();
                sTestResultObj.TestResultDateTime = dAnalysisDateTime;
                sTestResultObj.TestResultType = sResultTestType;
                sTestResultObj.OperatorID = sOperatorID;
                sTestResultObj.PatientID = sPatientID;
                sTestResultObj.InchargePerson = sDoctorName;
                //sTestResultObj.ObservationStatus = strResultObservStatus;
                //sTestResultObj.TestResultStatus = sTestResultStatus;
                //sTestResultObj.TestResultValue = iResultValue;
                //sTestResultObj.TestResultRules = sResultRule;
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

                Boolean bResult = TestResultRepository.insertTestObservationMessage(sResultObj, sMSHObj, sPIDObj, sOBRObj, sOBXObjList, sNTEObj, sPVObj, sSPMObj, sSACObj);
                if (bResult)
                {
                    // Insert into Test Result table & create notification 
                    TestResultRepository.createTestResultsMultipleParam(sTestResultObj, sTestResultDetails);

                    SendNotification(sTestResultObj.PatientID);
                }
            }
            catch (Exception ex)
            {
                sLogger.Error("Function ProcessHL7Version251Message >>> " + ex.ToString());
            }
        }

        /// <summary>
        /// Insert Notification
        /// </summary>
        /// <param name="sResult"></param>
        public void SendNotification(String sPatientID)
        {
            try
            {
                var sConfigurationObj = TestResultRepository.GetConfigurationByKey("SystemSettings_Language");
                String sNotificationContent = "";

                //var sTemplateObj = TestResultRepository.GetNotificationTemplate("TR01");
                var sTemplateObj = TestResultRepository.GetNotificationTemplateByLanguage("TR01", (sConfigurationObj != null) ? sConfigurationObj.ConfigurationValue : "");
                if (sTemplateObj != null)
                {
                    sNotificationContent = sTemplateObj.TemplateContent;
                }

                sNotificationContent = sNotificationContent.Replace("###<patient_id>###", sPatientID);

                txn_notification sNotificationSend = new txn_notification()
                {
                    NotificationType = "Completed Test Results",
                    NotificationTitle = (sTemplateObj != null) ? sTemplateObj.TemplateTitle : "",
                    NotificationContent = sNotificationContent,
                    CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedBy = sSystemName
                };

                TestResultRepository.insertNotification(sNotificationSend);
            }
            catch (Exception ex)
            {
                sLogger.Error("Function SendNotification >>> " + ex.ToString());
            }

        }

        /// <summary>
        /// Output data to file
        /// </summary>
        /// <param name="sBuilder"></param>
        /// <param name="sFileName"></param>
        /// <param name="sData"></param>
        /// <param name="sXMLMessage"></param>
        /// <param name="sAckMessage"></param>
        public void OutputMessage(HostApplicationBuilder sBuilder, String sFileName, String sData, String sXMLMessage, String sAckMessage)
        {
            try
            {
                String sOutputPathHL7 = sBuilder.Configuration.GetSection("FileOutput:HL7").Value;
                String sOutputPathXML = sBuilder.Configuration.GetSection("FileOutput:XML").Value;
                String sOutputPathACK = sBuilder.Configuration.GetSection("FileOutput:ACK").Value;

                if (!String.IsNullOrEmpty(sOutputPathHL7))
                {
                    String outputPathHL7 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathHL7);
                    if (!Directory.Exists(outputPathHL7))
                    {
                        Directory.CreateDirectory(outputPathHL7);
                    }
                    File.WriteAllText(outputPathHL7 + sFileName + ".hl7", sData, System.Text.Encoding.ASCII);
                }

                // Output to XML file
                if (!String.IsNullOrEmpty(sOutputPathXML))
                {
                    String outputPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathXML);
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    File.WriteAllText(outputPath + sFileName + ".xml", sXMLMessage, System.Text.Encoding.ASCII);
                }

                if (!String.IsNullOrEmpty(sOutputPathACK))
                {
                    String outputPathACK = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathACK);
                    if (!Directory.Exists(outputPathACK))
                    {
                        Directory.CreateDirectory(outputPathACK);
                    }
                    File.WriteAllText(outputPathACK + sFileName + ".hl7", sAckMessage, System.Text.Encoding.ASCII);
                }
            }
            catch (Exception ex)
            {
                sLogger.Error("Function OutputMessage >>> " + ex.ToString());
            }

        }

        /// <summary>
        /// Get NTE Comment data 
        /// </summary>
        /// <param name="nte"></param>
        /// <returns></returns>
        public String GenerateNTEComments(List<NHapi.Model.V26.Segment.NTE> nte)
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
                sLogger.Error("GenerateNTEComments >>> " + ex.ToString());
            }

            return nteComment;
        }

        /// <summary>
        /// Get NTE Comment for HL7 version 2.5.1
        /// </summary>
        /// <param name="nte"></param>
        /// <returns></returns>
        public String GenerateNTECommentsV251(List<NHapi.Model.V251.Segment.NTE> nte)
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
                sLogger.Error("GenerateNTECommentsV251 >>> " + ex.ToString());
            }

            return nteComment;
        }

        /// <summary>
        /// Process Observation Results Status
        /// </summary>
        /// <param name="isRangeReference"></param>
        /// <param name="sResultValue"></param>
        /// <param name="sReferenceRange"></param>
        /// <param name="dResultValue"></param>
        /// <returns></returns>
        public String ProcessObservationResultStatusValue(Boolean isRangeReference, String sResultValue, String sReferenceRange, Decimal dResultValue)
        {
            String sRetStatus = "";

            if (isRangeReference)
            {
                Decimal dTargetValue = 0;
                if (!String.IsNullOrEmpty(sResultValue))
                {
                    sResultValue = sResultValue.Replace("<", "").Replace("nan", "");
                    Decimal.TryParse(sResultValue, out dTargetValue);
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
                        sRetStatus = "Normal";
                    }
                    else
                    {
                        sRetStatus = "Abnormal";
                    }
                }
                else
                {
                    sRetStatus = "Abnormal";
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

        /// <summary>
        /// Process Babesia type observation test result 
        /// </summary>
        /// <param name="sObservationResult"></param>
        /// <returns></returns>
        public List<txn_testresults_details> ProcessBabesiaGibsoniTestResult(List<tbltestanalyze_results_observationresult> sObservationResult)
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