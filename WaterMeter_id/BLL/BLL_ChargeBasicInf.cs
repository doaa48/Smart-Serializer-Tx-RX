using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_ChargeBasicInf
	{													   
	public int         ChargeBasicInf_ID 				   { get; set; }
	public int         ChargeBasicInf_ClientInfoID        { get; set; }
	public DateTime    ChargeBasicInf_Date				   { get; set; }
	public double      ChargeBasicInf_ChargeAmount		   { get; set; }
	public int         ChargeBasicInf_ChargeNo			   { get; set; }
	public int         ChargeBasicInf_CutoffWarningLimit   { get; set; }
	public int         ChargeBasicInf_MaxOverdraftCredit   { get; set; }
	public DateTime    ChargeBasicInf_ChargeDate		   { get; set; }
	public int         ChargeBasicInf_EnabledValvePeriod   { get; set; }
    public int         ChargeBasicInf_MeterAction { get; set; }
	}
}
