using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_SchedulerMeter
    {
        public int SchedularMeter_ID { get; set; }
        public int SchedularMeter_MeterID { get; set; }
        public int SchedularMeter_AggregationID { get; set; }
        public string SchedularMeter_PayLoad { get; set; }
        public DateTime SchedularMeter_TimeIssue { get; set; }
        public DateTime SchedularMeter_TimeLastReading { get; set; }
        public int SchedularMeter_Status { get; set; }
        public int SchedularMeter_EnableMeter { get; set; }
        public string SchedularMeter_SmartType { get; set; }
        public int MeterNum { get; set; }

        public int AggregationNum { get; set; }

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

        public byte[] SchedularMeter_Payloadarray()
        {
            // Convert hex string SchedularMeter_PayLoad to array of bytes
            return HexStringToByteArray(SchedularMeter_PayLoad);
        }

        public void SchedularMeter_Payloadarray(byte[] array)
        {
            // Convert array to hex string and save in member SchedularMeter_PayLoad
            SchedularMeter_PayLoad = ByteArrayToHexString(array);
        }
    }
}
