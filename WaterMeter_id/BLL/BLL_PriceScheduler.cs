using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_PriceScheduler
    {
	public int        PriceSchedule_ID                               { get; set; } = 0;
	public string     PriceSchedule_Name                          { get; set; } = "";
	public string     PriceSchedule_Code		                  { get; set; } = "";
	public DateTime   PriceSchedule_ISSueDate	                  { get; set; }    
	public int        PriceSchedule_UnitTypeID		                  { get; set; } = 0;
	public int        PriceSchedule_MonthFee1		                  { get; set; } = 0;
	public int        PriceSchedule_MonthFee2		                  { get; set; } = 0;
	public int        PriceSchedule_MonthFeeOption	                  { get; set; } = 0;
	public int        PriceSchedule_PerMeterFee	                  { get; set; } = 0;
	public int        PriceSchedule_Pricing		                  { get; set; } = 0;
	public int        PriceSchedule_SWGPrice		                  { get; set; } = 0;
	public int        PriceSchedule_SWGPercent		                  { get; set; } = 0;
	public DateTime  PriceSchedule_APPDate	                  { get; set; }  
	public int       PriceSchedule_NoOFUintsCalc	                  { get; set; } = 0;
	public int       PriceSchedule_LevelNum		                  { get; set; } = 0;
	public int       PriceSchedule_Level1_StepMax	                  { get; set; } = 0;
	public int       PriceSchedule_Level1_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level1_Fee		                  { get; set; } = 0;
	public int       PriceSchedule_Level2_StepMax	                  { get; set; } = 0;
	public int       PriceSchedule_Level2_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level2_Fee		                  { get; set; } = 0;
	public int       PriceSchedule_Level3_StepMax	                  { get; set; } = 0;
	public int       PriceSchedule_Level3_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level3_Fee		                  { get; set; } = 0;
	public int       PriceSchedule_Level4_StepMax	                  { get; set; } = 0;
	public int       PriceSchedule_Level4_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level4_Fee		                  { get; set; } = 0;
	public int       PriceSchedule_Level5_StepMax	                  { get; set; } = 0;
	public int       PriceSchedule_Level5_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level5_Fee		                  { get; set; } = 0;
	public int       PriceSchedule_Level6_StepMax	                  { get; set; } = 0;
	public int       PriceSchedule_Level6_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level6_Fee		                  { get; set; } = 0;
	public int       PriceSchedule_Level7_StepMax	                  { get; set; } = 0;
	public int       PriceSchedule_Level7_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level7_Fee		                  { get; set; } = 0;
	public int       PriceSchedule_Level8_StepMax	                  { get; set; } = 0;
	public int       PriceSchedule_Level8_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level8_Fee		                  { get; set; } = 0;
	public int       PriceSchedule_Level9_StepMax	                  { get; set; } = 0;
	public int       PriceSchedule_Level9_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level9_Fee		                  { get; set; } = 0;
	public int       PriceSchedule_Level10_StepMax                  { get; set; } = 0;
	public int       PriceSchedule_Level10_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level10_Fee	                  { get; set; } = 0;
	public int       PriceSchedule_Level11_StepMax                  { get; set; } = 0;
	public int       PriceSchedule_Level11_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level11_Fee	                  { get; set; } = 0;
	public int       PriceSchedule_Level12_StepMax                  { get; set; } = 0;
	public int       PriceSchedule_Level12_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level12_Fee	                  { get; set; } = 0;
	public int       PriceSchedule_Level13_StepMax                  { get; set; } = 0;
	public int       PriceSchedule_Level13_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level13_Fee	                  { get; set; } = 0;
	public int       PriceSchedule_Level14_StepMax                  { get; set; } = 0;
	public int       PriceSchedule_Level14_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level14_Fee	                  { get; set; } = 0;
	public int       PriceSchedule_Level15_StepMax                  { get; set; } = 0;
	public int       PriceSchedule_Level15_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level15_Fee	                  { get; set; } = 0;
	public int       PriceSchedule_Level16_StepMax                  { get; set; } = 0;
	public int       PriceSchedule_Level16_Price	                  { get; set; } = 0;
	public int       PriceSchedule_Level16_Fee                      { get; set; } = 0;

	public string PriceSchedule_TypeName { get; set; } = "";

    }
}
