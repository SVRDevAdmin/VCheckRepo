using System;
using HL7Listener;
using System.Text;

namespace ListenerSampleProgra_
{
	public class Utils
	{
		public Utils()
		{
		}

        public static String CreateResponseMessage(String messageControlID)
        {
            try
            {
                Message response = new Message();

                Segment msh = new Segment("MSH");
                msh.Field(2, "^~\\&");
                msh.Field(7, DateTime.Now.ToString("yyyyMMddhhmmsszzz"));
                msh.Field(9, "ACK");
                msh.Field(10, Guid.NewGuid().ToString());
                msh.Field(11, "P");
                msh.Field(12, "2.5.1");
                response.Add(msh);

                Segment msa = new Segment("MSA");
                msa.Field(1, "AA");
                msa.Field(2, messageControlID);
                response.Add(msa);

                StringBuilder frame = new StringBuilder();
                frame.Append((char)0x0b);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x1c);
                frame.Append((char)0x0d);

                return frame.ToString();
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
    }
}

