using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_ConfigCard
	{												
	public int         ConfigCard_ID				   { get; set; }
	 public int         ConfigCard_CardID			   { get; set; }
	 public DateTime    ConfigCard_IssueDate		   { get; set; }
	 public int         ConfigCard_MeterAction		   { get; set; }
	 public DateTime    ConfigCard_RestDate			   { get; set; }
	 public int         ConfigCard_TimeEffective	   { get; set; }
	 public int         ConfigCard_StartConsumerID	   { get; set; }
	 public int         ConfigCard_EndConsumerID       { get; set; }

	public ushort CountList { get; set; } = 0;

	}
}
