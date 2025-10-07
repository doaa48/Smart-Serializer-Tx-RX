using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_Deductions
    {
	    public int        Deductions_ID { get; set; }
		public int         Deductions_ClientInfoID      { get; set; }
	   public DateTime     Deductions_Date		         { get; set; }
	   public double       Deductions_MonthFees	         { get; set; }
	   public DateTime     Deductions_AppDate		    { get; set; }
	   public int          Deductions_Month            { get; set; }

	}
}
