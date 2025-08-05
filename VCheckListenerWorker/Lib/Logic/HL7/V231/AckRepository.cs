using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheckListenerWorker.Lib.Object;

namespace VCheckListenerWorker.Lib.Logic.HL7.V231
{
    public class AckRepository
    {
        /// <summary>
        /// Generate Ack Message for ORU_R01
        /// </summary>
        /// <param name="sIMessage"></param>
        /// <returns></returns>
        public static String GenerateAcknowlegeMessageORU(NHapi.Base.Model.IMessage sIMessage)
        {
            try
            {
                NHapi.Model.V231.Message.ORU_R01 sORU_R01 = (NHapi.Model.V231.Message.ORU_R01)sIMessage;

                var SoftwareVersion = (NHapi.Base.Model.Varies)sORU_R01.MSH.GetField(21)[0];
                var CharacterSet = sORU_R01.MSH.GetField(18)[0].ToString();

                StringBuilder frame = new StringBuilder();
                frame.Append((char)0x0B);
                Message response = new Message();

                // ------------- Message Header ------------//
                Segment msh = new Segment("MSH");
                msh.Field(1, sORU_R01.MSH.FieldSeparator.Value);
                msh.Field(2, sORU_R01.MSH.EncodingCharacters.Value);
                msh.Field(3, "VCheck Listener");
                msh.Field(4, "LIS");
                msh.Field(5, sORU_R01.MSH.SendingApplication.NamespaceID.Value);
                msh.Field(6, sORU_R01.MSH.SendingFacility.NamespaceID.Value);
                msh.Field(7, DateTime.Now.ToString("yyyyMMddhhmmss"));
                msh.Field(9, "ACK^R01");
                msh.Field(10, sORU_R01.MSH.MessageControlID.Value.ToString());
                msh.Field(11, sORU_R01.MSH.ProcessingID.ProcessingID.Value);
                msh.Field(12, sORU_R01.MSH.VersionID.VersionID.Value);
                msh.Field(16, "0");
                msh.Field(18, "UTF-8");
                response.Add(msh);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                // ------------- Message Acknowledgement (No error) ---------------------//
                response = new Message();
                Segment msa = new Segment("MSA");
                msa.Field(1, NHapi.Base.AcknowledgmentCode.AA.ToString());
                msa.Field(2, sORU_R01.MSH.MessageControlID.Value.ToString());
                msa.Field(6, "0");
                response.Add(msa);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                //// ------------- Message Acknowledgement (error) ---------------------//
                //response = new Message();
                //Segment msa = new Segment("MSA");
                //msa.Field(1, NHapi.Base.AcknowledgmentCode.AE.ToString());
                //msa.Field(2, sORU_R01.MSH.MessageControlID.Value.ToString());
                //msa.Field(3, "Error message");
                //msa.Field(6, "100");
                //response.Add(msa);
                //frame.Append(response.SerializeMessage());
                //frame.Append((char)0x0d);

                frame.Append((char)0x1C);
                frame.Append((char)0x0D);

                var ackMessage = frame.ToString();

                return ackMessage;

            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public static String GenerateAcknowlegeMessageQRY(NHapi.Base.Model.IMessage sIMessage)
        {
            try
            {
                NHapi.Model.V231.Message.QRY_Q02 sQRY_Q02 = (NHapi.Model.V231.Message.QRY_Q02)sIMessage;

                //var SoftwareVersion = (NHapi.Base.Model.Varies)sORU_R01.MSH.GetField(21)[0];
                //var CharacterSet = sORU_R01.MSH.GetField(18)[0].ToString();

                StringBuilder frame = new StringBuilder();
                frame.Append((char)0x0B);
                Message response = new Message();

                // DSR message start //
                // ------------- Message Header ------------//
                Segment msh = new Segment("MSH");
                msh.Field(1, sQRY_Q02.MSH.FieldSeparator.Value);
                msh.Field(2, sQRY_Q02.MSH.EncodingCharacters.Value);
                msh.Field(3, "VCheck Listener");
                msh.Field(4, "LIS");
                msh.Field(5, sQRY_Q02.MSH.SendingApplication.NamespaceID.Value);
                msh.Field(6, sQRY_Q02.MSH.SendingFacility.NamespaceID.Value);
                msh.Field(7, DateTime.Now.ToString("yyyyMMddhhmmss"));
                msh.Field(9, "DSR^Q03");
                msh.Field(10, sQRY_Q02.MSH.MessageControlID.Value.ToString());
                msh.Field(11, sQRY_Q02.MSH.ProcessingID.ProcessingID.Value);
                msh.Field(12, sQRY_Q02.MSH.VersionID.VersionID.Value);
                msh.Field(16, "0");
                msh.Field(18, "UTF-8");
                response.Add(msh);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                int count = 1;
                // DSP //
                response = new Message();
                Segment dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "325643");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "maomao");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "F");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "zhuren^shouyi");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "12345678901");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "9527");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "2");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "M");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "hospital1");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "24.3");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "1");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "2341");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "FullBlood");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "www");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "CBC^CBC^1");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "DIFF^DIFF^1");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                dsp = new Segment("DSP");
                dsp.Field(1, count++.ToString());
                dsp.Field(3, "RET^RET^1");
                response.Add(dsp);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                response = new Message();
                var dsc = new Segment("DSC");
                dsc.Field(1, "1");
                response.Add(dsc);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                frame.Append((char)0x1C);
                frame.Append((char)0x0D);
                // DSR message end //

                frame.Append((char)0x0B);

                //ACK message
                // ------------- Message Header ------------//
                response = new Message();
                msh = new Segment("MSH");
                msh.Field(1, sQRY_Q02.MSH.FieldSeparator.Value);
                msh.Field(2, sQRY_Q02.MSH.EncodingCharacters.Value);
                msh.Field(3, "VCheck Listener");
                msh.Field(4, "LIS");
                msh.Field(5, sQRY_Q02.MSH.SendingApplication.NamespaceID.Value);
                msh.Field(6, sQRY_Q02.MSH.SendingFacility.NamespaceID.Value);
                msh.Field(7, DateTime.Now.ToString("yyyyMMddhhmmss"));
                msh.Field(9, "QCK^Q02");
                msh.Field(10, sQRY_Q02.MSH.MessageControlID.Value.ToString());
                msh.Field(11, sQRY_Q02.MSH.ProcessingID.ProcessingID.Value);
                msh.Field(12, sQRY_Q02.MSH.VersionID.VersionID.Value);
                msh.Field(16, "0");
                msh.Field(18, "UTF-8");
                response.Add(msh);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                // ------------- Message Acknowledgement (No error) ---------------------//
                response = new Message();
                Segment msa = new Segment("MSA");
                msa.Field(1, NHapi.Base.AcknowledgmentCode.AA.ToString());
                msa.Field(2, sQRY_Q02.MSH.MessageControlID.Value.ToString());
                msa.Field(3, "Message accepted");
                msa.Field(6, "0");
                response.Add(msa);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);

                //// ------------- Message Acknowledgement (error) ---------------------//
                //response = new Message();
                //Segment msa = new Segment("MSA");
                //msa.Field(1, NHapi.Base.AcknowledgmentCode.AE.ToString());
                //msa.Field(2, sQRY_Q02.MSH.MessageControlID.Value.ToString());
                //msa.Field(3, "Error message");
                //msa.Field(6, "100");
                //response.Add(msa);
                //frame.Append(response.SerializeMessage());
                //frame.Append((char)0x0d);

                frame.Append((char)0x1C);
                frame.Append((char)0x0D);

                var ackMessage = frame.ToString();

                return ackMessage;

            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
    }
}
