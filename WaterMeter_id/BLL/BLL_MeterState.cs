using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_MeterState
    {
        public int MeterState_ID { get; set; }
        public int MeterState_ClientInfoID { get; set; }
        public DateTime MeterState_Date { get; set; }
        public string MeterState_ReadingBy { get; set; }
        public int MeterState_Malfun1_Month { get; set; }
        public int MeterState_Malfun1_Day { get; set; }
        public int MeterState_Malfun1_Count { get; set; }
        public int MeterState_Malfun2_Month { get; set; }
        public int MeterState_Malfun2_Day { get; set; }
        public int MeterState_Malfun2_Count { get; set; }
        public int MeterState_Malfun3_Month { get; set; }
        public int MeterState_Malfun3_Day { get; set; }
        public int MeterState_Malfun3_Count { get; set; }
        public int MeterState_Malfun4_Month { get; set; }
        public int MeterState_Malfun4_Day { get; set; }
        public int MeterState_Malfun4_Count { get; set; }
        public int MeterState_Malfun5_Month { get; set; }
        public int MeterState_Malfun5_Day { get; set; }
        public int MeterState_Malfun5_Count { get; set; }
        public DateTime MeterState_MeterStateDate1 { get; set; }
        public DateTime MeterState_MeterStateDate2 { get; set; }
        public DateTime MeterState_MeterStateDate3 { get; set; }
        public DateTime MeterState_MeterStateDate4 { get; set; }
        public int MeterState_MeterErrors { get; set; }

    }
}
