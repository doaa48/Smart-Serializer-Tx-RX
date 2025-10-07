using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public  class BLL_MaintCard
    {
        public int MaintCard_ID { get; set; }
        public int MaintCard_CardNID { get; set; }
        public DateTime MaintCard_IssueDate { get; set; }
        public int MaintCard_MeterAction { get; set; }
        public int MaintCard_ClientCateg { get; set; }
        public DateTime MaintCard_ActionDate { get; set; }
        public int MaintCard_CategToapply { get; set; }
        public int MaintCard_SwegToAPPly { get; set; }
        public int MaintCard_TimeEffective { get; set; }
        public int MaintCard_StartConsumerID { get; set; }
        public int MaintCard_EndConsumerID { get; set; }
        public int MaintCard_OFFTimeNum { get; set; }
        public int MaintCard_PriceScheduleNum { get; set; }

        public string MaintCard_PriceSchdukerName { get; set; }
        public string MaintCard_OfftimeName { get; set; }

        public ushort CountList { get; set; } = 0;


    }
    /*
     CREATE TABLE [dbo].[MaintCard](
	[MaintCard_ID] [int] IDENTITY(1,1) NOT NULL,
	[MaintCard_CardNID] [int] NULL,
	[MaintCard_IssueDate] [date] NULL,
	[MaintCard_MeterAction] [int] NULL,
	[MaintCard_ClientCateg] [int] NULL,
	[MaintCard_ActionDate] [date] NOT NULL,
	[MaintCard_CategToapply] [int] NULL,
	[MaintCard_SwegToAPPly] [int] NULL,
	[MaintCard_TimeEffective] [int] NULL,
	[MaintCard_StartConsumerID] [int] NULL,
	[MaintCard_EndConsumerID] [int] NULL,
	[MaintCard_OFFTimeNum] [int] NULL,
	[MaintCard_PriceScheduleNum] [int] NULL,
     */
}
