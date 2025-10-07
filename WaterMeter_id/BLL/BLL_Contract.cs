using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id.BLL
{
    internal class BLL_Contract
    {
        public int Contract_ID { get; set; }
        public int Contract_ClientInfoID { get; set; }
        public DateTime Contract_StartDate { get; set; }
        public DateTime Contract_EndDate { get; set; }
        public string Contract_Activity { get; set; }
        public double Contract_TotalPrice { get; set; }
        public int Contract_MeterID { get; set; }
        public int Contract_CardID { get; set; }
        public int Contract_OperatorID { get; set; }
    }

    /*
      CREATE TABLE [dbo].[Contract](
	[Contract_ID] [int] IDENTITY(1,1) NOT NULL,
	[Contract_ClientID] [int] NULL,
	[Contract_StartDate] [date] NULL,
	[Contract_EndDate] [date] NULL,
	[Contract_Activity] [varchar](255) NULL,
	[Contract_TotalPrice] [decimal](18, 2) NULL,
	[Contract_MeterID] [int] NULL,
	[Contract_CardID] [int] NULL,
	[Contract_OperatorID] [int] NULL,
     
     */
}
