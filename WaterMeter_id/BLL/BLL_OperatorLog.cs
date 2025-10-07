using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id.BLL
{
    internal class BLL_OperatorLog
    {
        public int OperatorLog_ID { get; set; }
        public int OperatorLog_OperatorID { get; set; }
        public string OperatorLog_TableName { get; set; }
        public int OperatorLog_TableNameID { get; set; }
        public string OperatorLog_Action { get; set; }
        public string OperatorLog_Action_Disc { get; set; }
        public DateTime OperatorLog_Date { get; set; }
    }
}
