using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_MeterList
    {
        public int MeterList_ID { get; set; }
        public int MeterList_MeterID { get; set; }
        public int MeterList_AggregationID { get; set; }
        public int MeterList_SecuirityEnable { get; set; }
        public int MeterList_Status { get; set; }
        public DateTime MeterList_TimeIssue { get; set; }
        public DateTime MeterList_LastTimeReading { get; set; }
    }
}
