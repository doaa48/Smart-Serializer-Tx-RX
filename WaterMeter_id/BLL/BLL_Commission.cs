using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_Commission
    {
       public int Commission_ID{ get; set; }
       public int Client_NUM{ get; set; }
       public int Subscriber_ID{ get; set; }
       public string UnitTypeName{ get; set; }
       public string UnitAddress { get; set; } = "";
       public string UnitDesc { get; set; } = "";
       public int Category{ get; set; }
       public int Activity{ get; set; }
       public int UnitNum{ get; set; }
       public string PriceScheduler_Name{ get; set; }
       public string OFFTime_Name{ get; set; }
       public int Meter_Num{ get; set; }
       public int SWGService{ get; set; }
       public int DeductionMonthFee{ get; set; }
       public int DeductionMonthNum{ get; set; }
       public DateTime DeductionAPPDate { get; set; }







    }
}
