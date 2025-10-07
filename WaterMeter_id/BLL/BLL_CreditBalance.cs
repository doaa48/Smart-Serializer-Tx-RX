using System;

namespace WaterMeter_id
{
    public class BLL_CreditBalance
    {
        public int CreditBalance_ID { get; set; }
        public int CreditBalance_ClientInfoID { get; set; }
        public DateTime CreditBalance_Date { get; set; }
        public string CreditBalance_ReadingBy { get; set; }
        public double CreditBalance_UsedMonthly1 { get; set; }
        public double CreditBalance_UsedMonthly2 { get; set; }
        public double CreditBalance_UsedMonthly3 { get; set; }
        public double CreditBalance_UsedMonthly4 { get; set; }
        public double CreditBalance_UsedMonthly5 { get; set; }
        public double CreditBalance_UsedMonthly6 { get; set; }
        public double CreditBalance_UsedMonthly7 { get; set; }
        public double CreditBalance_UsedMonthly8 { get; set; }
        public double CreditBalance_UsedMonthly9 { get; set; }
        public double CreditBalance_UsedMonthly10 { get; set; }
        public double CreditBalance_UsedMonthly11 { get; set; }
        public double CreditBalance_UsedMonthly12 { get; set; }
        public double CreditBalance_RemainCredit { get; set; }
        public double CreditBalance_OverdraftCredit { get; set; }
        public double CreditBalance_ConsumedCredit { get; set; }
        public double CreditBalance_CumulativeCharges { get; set; }
        public DateTime CreditBalance_AppDate { get; set; }

        public ushort ReturnMeterAction { get; set; }
    }

    /*
     CREATE TABLE [dbo].[CreditBalance](
	[CreditBalance_ID] [int] IDENTITY(1,1) NOT NULL,
	[CreditBalance_ClientID] [int] NULL,
	[CreditBalance_Date] [date] NULL,
	[CreditBalance_ReadingBy] [varchar](255) NULL,
	[CreditBalance_UsedMonthly1] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly2] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly3] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly4] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly5] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly6] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly7] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly8] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly9] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly10] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly11] [decimal](18, 2) NULL,
	[CreditBalance_UsedMonthly12] [decimal](18, 2) NULL,
	[CreditBalance_RemainCredit] [decimal](18, 2) NULL,
	[CreditBalance_OverdraftCredit] [decimal](18, 2) NULL,
	[CreditBalance_ConsumedCredit] [decimal](18, 2) NULL,
	[CreditBalance_CumulativeCharges] [decimal](18, 2) NULL,
	[CreditBalance_AppDate] [date] NULL,
    */
}
