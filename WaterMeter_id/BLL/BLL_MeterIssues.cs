using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_MeterIssues
    {
        public int      Meter_ID;
        public string   Meter_PK;
        public int      Meter_MeterTypeID;
        public int      Meter_MeterManfID;
        public string   Meter_CertMeterManf_TO_Meter="";
        public int      Meter_WaterCompID;
        public string   Meter_CertWaterComp_TO_Meter="";
        public string   Meter_SerialNumber="";
        public DateTime Meter_IssueDate;
        public int      Meter_MeterNum;
        public int      Meter_Diameter;
        public int      Meter_Origin;
        public string   Meter_Model="";
        public string   Meter_Man="";
        public int      Meter_ChargeMode;
        public int      Meter_Satatus;
        
    }
}
