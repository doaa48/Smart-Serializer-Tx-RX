using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_PriceSchduler
    {
        DAL_PriceSchduler DAL_PriceSchduler_Obj = new DAL_PriceSchduler();
        public BLL_PriceScheduler PriceScheduler_Data = new BLL_PriceScheduler();
        public string UnitTypeName = "";
        public string[] GetUnitsTypeName()
         {
           DAL_UnitType UnitType_Obj = new DAL_UnitType();
           DataTable table = UnitType_Obj.Select();
            string[] data = new string[table.Rows.Count];

            for (int index = 0; index < table.Rows.Count; index++)
            {
                data[index] = table.Rows[index]["UnitType_Name"].ToString();
            }
            return data;
        }


        public DataTable GetTable()
        {
            return DAL_PriceSchduler_Obj.Select();
        }
        public bool AddNewPriceSchduleData()
        {
            bool Status = false;

            DAL_UnitType UnitType_Obj = new DAL_UnitType();
            DataTable table = UnitType_Obj.Search(UnitTypeName);

            if (table.Rows.Count >0)
            {
                PriceScheduler_Data.PriceSchedule_UnitTypeID = (int)table.Rows[0]["UnitType_ID"];
              Status = DAL_PriceSchduler_Obj.Insert(PriceScheduler_Data);
            }
        
     

            return Status;

        }

        public bool DeletePriceSchduleData()
        {
            bool status = false;
            status = DAL_PriceSchduler_Obj.Delete(PriceScheduler_Data);
            return status;
        }

        public bool UpdatePriceSchduleData()
        {
            bool Status = false;
            DAL_UnitType UnitType_Obj = new DAL_UnitType();
            DataTable table = UnitType_Obj.Search(UnitTypeName);

            if (table.Rows.Count > 0)
            {
                PriceScheduler_Data.PriceSchedule_UnitTypeID = (int)table.Rows[0]["UnitType_ID"];
                Status = DAL_PriceSchduler_Obj.Update(PriceScheduler_Data);
            }

            return Status;

        }
        public bool GetPriceSchduleData(string name)
        {
            bool Status = false;
            PriceScheduler_Data = DAL_PriceSchduler_Obj.GetPriceSchedulerDataByName(name);
            if(PriceScheduler_Data != null)
            {

                Status = true;
            }
         return Status;

        }

        public BLL_PriceScheduler GetSelectedPriceSchedule(string priceScheduleName)
        {
            return DAL_PriceSchduler_Obj.GetPriceSchedulerDataByName(priceScheduleName);
        }

        public DataTable GetPriceSchedulePrinted()
        {
            return DAL_PriceSchduler_Obj.PrintPriceSchedule();
        }

    }
}




/*if (table.Rows.Count > 0)
        {
            DataRow row = table.Rows[0];
            PriceScheduler_Data.PriceSchedule_ID               =Convert.ToInt32(row["PriceSchedule_ID"]);
            PriceScheduler_Data.PriceSchedule_Name             =row["PriceSchedule_Name"].ToString();
            PriceScheduler_Data.PriceSchedule_Code             =row["PriceSchedule_Code"].ToString();
            PriceScheduler_Data.PriceSchedule_ISSueDate        = (DateTime)row["PriceSchedule_ISSueDate"];
            PriceScheduler_Data.PriceSchedule_UnitTypeID       =Convert.ToInt32(row["PriceSchedule_UnitTypeID"]);
            PriceScheduler_Data.PriceSchedule_MonthFee1        =Convert.ToInt32(row["PriceSchedule_MonthFee1"]);
            PriceScheduler_Data.PriceSchedule_MonthFee2        =Convert.ToInt32(row["PriceSchedule_MonthFee2"]);
            PriceScheduler_Data.PriceSchedule_MonthFeeOption   =Convert.ToInt32(row["PriceSchedule_MonthFeeOption"]);
            PriceScheduler_Data.PriceSchedule_PerMeterFee      =Convert.ToInt32(row["PriceSchedule_PerMeterFee"]);
            PriceScheduler_Data.PriceSchedule_Pricing          =Convert.ToInt32(row["PriceSchedule_Pricing"]);
            PriceScheduler_Data.PriceSchedule_SWGPrice         =Convert.ToInt32(row["PriceSchedule_SWGPrice"]);
            PriceScheduler_Data.PriceSchedule_SWGPercent       =Convert.ToInt32(row["PriceSchedule_SWGPercent"]);
            PriceScheduler_Data.PriceSchedule_APPDate          = (DateTime)row["PriceSchedule_APPDate"];
            PriceScheduler_Data.PriceSchedule_NoOFUintsCalc    =Convert.ToInt32(row["PriceSchedule_NoOFUintsCalc"]);
            PriceScheduler_Data.PriceSchedule_LevelNum         =Convert.ToInt32(row["PriceSchedule_LevelNum"]);
            PriceScheduler_Data.PriceSchedule_Level1_StepMax   =Convert.ToInt32(row["PriceSchedule_Level1_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level1_Price     =Convert.ToInt32(row["PriceSchedule_Level1_Price"]);
            PriceScheduler_Data.PriceSchedule_Level1_Fee       =Convert.ToInt32(row["PriceSchedule_Level1_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level2_StepMax   =Convert.ToInt32(row["PriceSchedule_Level2_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level2_Price     =Convert.ToInt32(row["PriceSchedule_Level2_Price"]);
            PriceScheduler_Data.PriceSchedule_Level2_Fee       =Convert.ToInt32(row["PriceSchedule_Level2_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level3_StepMax   =Convert.ToInt32(row["PriceSchedule_Level3_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level3_Price     =Convert.ToInt32(row["PriceSchedule_Level3_Price"]);
            PriceScheduler_Data.PriceSchedule_Level3_Fee       =Convert.ToInt32(row["PriceSchedule_Level3_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level4_StepMax   =Convert.ToInt32(row["PriceSchedule_Level4_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level4_Price     =Convert.ToInt32(row["PriceSchedule_Level4_Price"]);
            PriceScheduler_Data.PriceSchedule_Level4_Fee       =Convert.ToInt32(row["PriceSchedule_Level4_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level5_StepMax   =Convert.ToInt32(row["PriceSchedule_Level5_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level5_Price     =Convert.ToInt32(row["PriceSchedule_Level5_Price"]);
            PriceScheduler_Data.PriceSchedule_Level5_Fee       =Convert.ToInt32(row["PriceSchedule_Level5_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level6_StepMax   =Convert.ToInt32(row["PriceSchedule_Level6_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level6_Price     =Convert.ToInt32(row["PriceSchedule_Level6_Price"]);
            PriceScheduler_Data.PriceSchedule_Level6_Fee       =Convert.ToInt32(row["PriceSchedule_Level6_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level7_StepMax   =Convert.ToInt32(row["PriceSchedule_Level7_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level7_Price     =Convert.ToInt32(row["PriceSchedule_Level7_Price"]);
            PriceScheduler_Data.PriceSchedule_Level7_Fee       =Convert.ToInt32(row["PriceSchedule_Level7_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level8_StepMax   =Convert.ToInt32(row["PriceSchedule_Level8_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level8_Price     =Convert.ToInt32(row["PriceSchedule_Level8_Price"]);
            PriceScheduler_Data.PriceSchedule_Level8_Fee       =Convert.ToInt32(row["PriceSchedule_Level8_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level9_StepMax   =Convert.ToInt32(row["PriceSchedule_Level9_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level9_Price     =Convert.ToInt32(row["PriceSchedule_Level9_Price"]);
            PriceScheduler_Data.PriceSchedule_Level9_Fee       =Convert.ToInt32(row["PriceSchedule_Level9_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level10_StepMax  =Convert.ToInt32(row["PriceSchedule_Level10_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level10_Price    =Convert.ToInt32(row["PriceSchedule_Level10_Price"]);
            PriceScheduler_Data.PriceSchedule_Level10_Fee      =Convert.ToInt32(row["PriceSchedule_Level10_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level11_StepMax  =Convert.ToInt32(row["PriceSchedule_Level11_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level11_Price    =Convert.ToInt32(row["PriceSchedule_Level11_Price"]);
            PriceScheduler_Data.PriceSchedule_Level11_Fee      =Convert.ToInt32(row["PriceSchedule_Level11_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level12_StepMax  =Convert.ToInt32(row["PriceSchedule_Level12_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level12_Price    =Convert.ToInt32(row["PriceSchedule_Level12_Price"]);
            PriceScheduler_Data.PriceSchedule_Level12_Fee      =Convert.ToInt32(row["PriceSchedule_Level12_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level13_StepMax  =Convert.ToInt32(row["PriceSchedule_Level13_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level13_Price    =Convert.ToInt32(row["PriceSchedule_Level13_Price"]);
            PriceScheduler_Data.PriceSchedule_Level13_Fee      =Convert.ToInt32(row["PriceSchedule_Level13_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level14_StepMax  =Convert.ToInt32(row["PriceSchedule_Level14_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level14_Price    =Convert.ToInt32(row["PriceSchedule_Level14_Price"]);
            PriceScheduler_Data.PriceSchedule_Level14_Fee      =Convert.ToInt32(row["PriceSchedule_Level14_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level15_StepMax  =Convert.ToInt32(row["PriceSchedule_Level15_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level15_Price    =Convert.ToInt32(row["PriceSchedule_Level15_Price"]);
            PriceScheduler_Data.PriceSchedule_Level15_Fee      =Convert.ToInt32(row["PriceSchedule_Level15_Fee"]);
            PriceScheduler_Data.PriceSchedule_Level16_StepMax  =Convert.ToInt32(row["PriceSchedule_Level16_StepMax"]);
            PriceScheduler_Data.PriceSchedule_Level16_Price    =Convert.ToInt32(row["PriceSchedule_Level16_Price"]);
            PriceScheduler_Data.PriceSchedule_Level16_Fee      =Convert.ToInt32(row["PriceSchedule_Level16_Fee"]);

            DAL_UnitType UnitType_Obj = new DAL_UnitType();
             table = UnitType_Obj.SearchId(PriceScheduler_Data.PriceSchedule_UnitTypeID);

            if (table.Rows.Count > 0)
            {
            UnitTypeName= (string)table.Rows[0]["UnitType_Name"];

            }

            Status = true;

        }
       */