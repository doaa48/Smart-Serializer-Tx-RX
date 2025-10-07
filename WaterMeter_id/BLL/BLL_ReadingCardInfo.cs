using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_ReadingCardInfo
    {
        public int ClientInfo_ID {  get; set; }
        public int Meter_MeterNum {  get; set; }
        public int Meter_Origin {  get; set; }
        public string Meter_Man {  get; set; }
        public int Meter_ChargeMode { get; set; }
        public double Meter_Diameter { get; set; }
        public int ClientInfo_Activity { get; set; }
        public int ClientInfo_NumOFUnit { get; set; }
        public int ClientInfo_Category { get; set; }
        public int ClientInfo_SwGServices { get; set; }
        public string PriceSchedule_Name { get; set; }
        public string OFFTime_Name { get; set; }
        public string Client_FullName { get; set; }
        public int Client_Number { get; set; }
    }
}
