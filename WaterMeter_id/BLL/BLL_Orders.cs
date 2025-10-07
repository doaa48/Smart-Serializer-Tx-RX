using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_Orders
    {
        public int Orders_ID { get; set; }
        public int Orders_CommandType { get; set; }
        public int Orders_GetwayID { get; set; }
        public int Orders_SchedularID { get; set; }
        public string Orders_Payload { get; set; }
        public int Orders_TimeOut { get; set; }
        public int Orders_RetransmitNo { get; set; }
        public int Orders_Status { get; set; }
        public string Orders_ErrorMessege { get; set; }
        public DateTime Orders_IssueDate { get; set; }

        public int Meter_Number { get; set; }
        public int Aggergation_Number { get; set; }
        public byte[] HexStringToByteArray(string hex)
        {
            // Remove any spaces in the hex string
            hex = hex.Replace(" ", "");

            // Calculate the length of the byte array
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];

            // Convert each pair of characters to a byte
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public byte[] Orders_Payloadarray()
        {
            // Convert hex string SchedularMeter_PayLoad to array of bytes
            return HexStringToByteArray(Orders_Payload);
        }

        public void Orders_Payloadarray(byte[] array)
        {
            // Convert array to hex string and save in member SchedularMeter_PayLoad
            Orders_Payload = ByteArrayToHexString(array);
        }

       

    }
}
