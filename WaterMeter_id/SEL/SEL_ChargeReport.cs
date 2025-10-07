using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterMeter_id.DAL;

namespace WaterMeter_id.SEL
{
    public class SEL_ChargeReport
    {
        DAL_ChargeBasicInf ChargeBasicInfObj = new DAL_ChargeBasicInf();
        public DataTable GetSpecificChargeTable(string StartDateDataTimePiker, string EndDateDataTimePicker)
        {
            return ChargeBasicInfObj.SelectSpecificTable (StartDateDataTimePiker, EndDateDataTimePicker);
        }
    }
}
