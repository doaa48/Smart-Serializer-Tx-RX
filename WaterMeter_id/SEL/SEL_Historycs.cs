using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_Historycs
    {
        DAL_History dAL_History = new DAL_History();

        public DataTable getTable(string StartDateDataTimePiker, string EndDateDataTimePicker)
        {
            return dAL_History.GetHistory(StartDateDataTimePiker,EndDateDataTimePicker);
        }

    }
}
