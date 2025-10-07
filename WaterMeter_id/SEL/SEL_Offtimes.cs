using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_Offtimes
    {
        public BLL_Offtimes Offtimes_Data = new BLL_Offtimes();
        DAL_Offtimes DAL_Offtimes_Obj = new DAL_Offtimes();
        
      public  DataTable GetTable()
      {
            return DAL_Offtimes_Obj.Select();
      }
       public bool AddNewOFFtimesDAta()
       {
            bool Status = false;
            Status = DAL_Offtimes_Obj.Insert(Offtimes_Data);
            return Status;

        }
        
        public bool UpdateOFFtimesData()
        {
            bool Status = false;
            Status = DAL_Offtimes_Obj.Update(Offtimes_Data);
            return Status;

        }
        public bool GetOFFtimesData(string name)
        {
            bool Status = false;
            Offtimes_Data = DAL_Offtimes_Obj.GetOFFTimeData(name);
         
            if(Offtimes_Data != null)
            {
                Status = true;
            }
            return Status;

        }
    }
}


/*if (table.Rows.Count > 0)
          {
              DataRow row = table.Rows[0];
              Offtimes_Data.OFFTime_ID = Convert.ToInt32(row["OFFTime_ID"]);
              Offtimes_Data.OFFTime_Name = row["OFFTime_Name"].ToString();
              Offtimes_Data.OFFTime_Code = row["OFFTime_Code"].ToString();
              Offtimes_Data.OFFTime_IssueDate = Convert.ToDateTime(row["OFFTime_IssueDate"]);
              Offtimes_Data.OFFTime_StartDate = Convert.ToDateTime(row["OFFTime_StartDate"]);
              Offtimes_Data.OFFTime_WorkingDays = Convert.ToInt32(row["OFFTime_WorkingDays"]);
              Offtimes_Data.OFFTime_CutOffTime = Convert.ToInt32(row["OFFTime_CutOffTime"]);
              Offtimes_Data.OFFTime_GracePeriod = Convert.ToInt32(row["OFFTime_GracePeriod"]);
              Offtimes_Data.OFFTime_WorkStart = Convert.ToInt32(row["OFFTime_WorkStart"]);
              Offtimes_Data.OFFTime_WorkEnd = Convert.ToInt32(row["OFFTime_WorkEnd"]);
              Offtimes_Data.OFFTime_Holiday1_Month = Convert.ToInt32(row["OFFTime_Holiday1_Month"]);
              Offtimes_Data.OFFTime_Holiday1_Day = Convert.ToInt32(row["OFFTime_Holiday1_Day"]);
              Offtimes_Data.OFFTime_Holiday2_Month = Convert.ToInt32(row["OFFTime_Holiday2_Month"]);
              Offtimes_Data.OFFTime_Holiday2_Day = Convert.ToInt32(row["OFFTime_Holiday2_Day"]);
              Offtimes_Data.OFFTime_Holiday3_Month = Convert.ToInt32(row["OFFTime_Holiday3_Month"]);
              Offtimes_Data.OFFTime_Holiday3_Day = Convert.ToInt32(row["OFFTime_Holiday3_Day"]);
              Offtimes_Data.OFFTime_Holiday4_Month = Convert.ToInt32(row["OFFTime_Holiday4_Month"]);
              Offtimes_Data.OFFTime_Holiday4_Day = Convert.ToInt32(row["OFFTime_Holiday4_Day"]);
              Offtimes_Data.OFFTime_Holiday5_Month = Convert.ToInt32(row["OFFTime_Holiday5_Month"]);
              Offtimes_Data.OFFTime_Holiday5_Day = Convert.ToInt32(row["OFFTime_Holiday5_Day"]);
              Offtimes_Data.OFFTime_Holiday6_Month = Convert.ToInt32(row["OFFTime_Holiday6_Month"]);
              Offtimes_Data.OFFTime_Holiday6_Day = Convert.ToInt32(row["OFFTime_Holiday6_Day"]);
              Offtimes_Data.OFFTime_Holiday7_Month = Convert.ToInt32(row["OFFTime_Holiday7_Month"]);
              Offtimes_Data.OFFTime_Holiday7_Day = Convert.ToInt32(row["OFFTime_Holiday7_Day"]);
              Offtimes_Data.OFFTime_Holiday8_Month = Convert.ToInt32(row["OFFTime_Holiday8_Month"]);
              Offtimes_Data.OFFTime_Holiday8_Day = Convert.ToInt32(row["OFFTime_Holiday8_Day"]);
              Offtimes_Data.OFFTime_Holiday9_Month = Convert.ToInt32(row["OFFTime_Holiday9_Month"]);
              Offtimes_Data.OFFTime_Holiday9_Day = Convert.ToInt32(row["OFFTime_Holiday9_Day"]);
              Offtimes_Data.OFFTime_Holiday10_Month = Convert.ToInt32(row["OFFTime_Holiday10_Month"]);
              Offtimes_Data.OFFTime_Holiday10_Day = Convert.ToInt32(row["OFFTime_Holiday10_Day"]);
              Offtimes_Data.OFFTime_Holiday11_Month = Convert.ToInt32(row["OFFTime_Holiday11_Month"]);
              Offtimes_Data.OFFTime_Holiday11_Day = Convert.ToInt32(row["OFFTime_Holiday11_Day"]);
              Offtimes_Data.OFFTime_Holiday12_Month = Convert.ToInt32(row["OFFTime_Holiday12_Month"]);
              Offtimes_Data.OFFTime_Holiday12_Day = Convert.ToInt32(row["OFFTime_Holiday12_Day"]);
              Offtimes_Data.OFFTime_Holiday13_Month = Convert.ToInt32(row["OFFTime_Holiday13_Month"]);
              Offtimes_Data.OFFTime_Holiday13_Day = Convert.ToInt32(row["OFFTime_Holiday13_Day"]);
              Offtimes_Data.OFFTime_Holiday14_Month = Convert.ToInt32(row["OFFTime_Holiday14_Month"]);
              Offtimes_Data.OFFTime_Holiday14_Day = Convert.ToInt32(row["OFFTime_Holiday14_Day"]);
              Offtimes_Data.OFFTime_Holiday15_Month = Convert.ToInt32(row["OFFTime_Holiday15_Month"]);
              Offtimes_Data.OFFTime_Holiday15_Day = Convert.ToInt32(row["OFFTime_Holiday15_Day"]);
              Offtimes_Data.OFFTime_Holiday16_Month = Convert.ToInt32(row["OFFTime_Holiday16_Month"]);
              Offtimes_Data.OFFTime_Holiday16_Day = Convert.ToInt32(row["OFFTime_Holiday16_Day"]);
              Offtimes_Data.OFFTime_Holiday17_Month = Convert.ToInt32(row["OFFTime_Holiday17_Month"]);
              Offtimes_Data.OFFTime_Holiday17_Day = Convert.ToInt32(row["OFFTime_Holiday17_Day"]);
              Offtimes_Data.OFFTime_Holiday18_Month = Convert.ToInt32(row["OFFTime_Holiday18_Month"]);
              Offtimes_Data.OFFTime_Holiday18_Day = Convert.ToInt32(row["OFFTime_Holiday18_Day"]);
              Offtimes_Data.OFFTime_Holiday19_Month = Convert.ToInt32(row["OFFTime_Holiday19_Month"]);
              Offtimes_Data.OFFTime_Holiday19_Day = Convert.ToInt32(row["OFFTime_Holiday19_Day"]);
              Offtimes_Data.OFFTime_Holiday20_Month = Convert.ToInt32(row["OFFTime_Holiday20_Month"]);
              Offtimes_Data.OFFTime_Holiday20_Day = Convert.ToInt32(row["OFFTime_Holiday20_Day"]);
              Offtimes_Data.OFFTime_Holiday21_Month = Convert.ToInt32(row["OFFTime_Holiday21_Month"]);
              Offtimes_Data.OFFTime_Holiday21_Day = Convert.ToInt32(row["OFFTime_Holiday21_Day"]);
              Offtimes_Data.OFFTime_Holiday22_Month = Convert.ToInt32(row["OFFTime_Holiday22_Month"]);
              Offtimes_Data.OFFTime_Holiday22_Day = Convert.ToInt32(row["OFFTime_Holiday22_Day"]);
              Offtimes_Data.OFFTime_Holiday23_Month = Convert.ToInt32(row["OFFTime_Holiday23_Month"]);
              Offtimes_Data.OFFTime_Holiday23_Day = Convert.ToInt32(row["OFFTime_Holiday23_Day"]);
              Offtimes_Data.OFFTime_Holiday24_Month = Convert.ToInt32(row["OFFTime_Holiday24_Month"]);
              Offtimes_Data.OFFTime_Holiday24_Day = Convert.ToInt32(row["OFFTime_Holiday24_Day"]);
              Offtimes_Data.OFFTime_Holiday25_Month = Convert.ToInt32(row["OFFTime_Holiday25_Month"]);
              Offtimes_Data.OFFTime_Holiday25_Day = Convert.ToInt32(row["OFFTime_Holiday25_Day"]);
              Status = true;

          }

          */