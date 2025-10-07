using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WaterMeter_id
{
    public class DAL_PriceSchduler
    {
        Database db = new Database();
        #region Select PriceSchedule
        public DataTable Select()
        {
            //creating db connection
            SqlConnection conn = db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT* FROM PriceSchedule";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
        #endregion
        #region Insert new Price Schedule
        public bool Insert(BLL_PriceScheduler c)
        {
            bool isSuccess = false;
            //creating db connection
            SqlConnection conn = db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "INSERT INTO PriceSchedule (" +
                             "PriceSchedule_Name            ," +
                             "PriceSchedule_Code            ," +
                             "PriceSchedule_ISSueDate       ," +
                             "PriceSchedule_UnitTypeID      ," +
                             "PriceSchedule_MonthFee1       ," +
                             "PriceSchedule_MonthFee2       ," +
                             "PriceSchedule_MonthFeeOption  ," +
                             "PriceSchedule_PerMeterFee     ," +
                             "PriceSchedule_Pricing         ," +
                             "PriceSchedule_SWGPrice        ," +
                             "PriceSchedule_SWGPercent      ," +
                             "PriceSchedule_APPDate         ," +
                             "PriceSchedule_NoOFUintsCalc   ," +
                             "PriceSchedule_LevelNum        ," +
                             "PriceSchedule_Level1_StepMax  ," +
                             "PriceSchedule_Level1_Price    ," +
                             "PriceSchedule_Level1_Fee      ," +
                             "PriceSchedule_Level2_StepMax  ," +
                             "PriceSchedule_Level2_Price    ," +
                             "PriceSchedule_Level2_Fee      ," +
                             "PriceSchedule_Level3_StepMax  ," +
                             "PriceSchedule_Level3_Price    ," +
                             "PriceSchedule_Level3_Fee      ," +
                             "PriceSchedule_Level4_StepMax  ," +
                             "PriceSchedule_Level4_Price    ," +
                             "PriceSchedule_Level4_Fee      ," +
                             "PriceSchedule_Level5_StepMax  ," +
                             "PriceSchedule_Level5_Price    ," +
                             "PriceSchedule_Level5_Fee      ," +
                             "PriceSchedule_Level6_StepMax  ," +
                             "PriceSchedule_Level6_Price    ," +
                             "PriceSchedule_Level6_Fee      ," +
                             "PriceSchedule_Level7_StepMax  ," +
                             "PriceSchedule_Level7_Price    ," +
                             "PriceSchedule_Level7_Fee      ," +
                             "PriceSchedule_Level8_StepMax  ," +
                             "PriceSchedule_Level8_Price    ," +
                             "PriceSchedule_Level8_Fee      ," +
                             "PriceSchedule_Level9_StepMax  ," +
                             "PriceSchedule_Level9_Price    ," +
                             "PriceSchedule_Level9_Fee      ," +
                             "PriceSchedule_Level10_StepMax ," +
                             "PriceSchedule_Level10_Price   ," +
                             "PriceSchedule_Level10_Fee     ," +
                             "PriceSchedule_Level11_StepMax ," +
                             "PriceSchedule_Level11_Price   ," +
                             "PriceSchedule_Level11_Fee     ," +
                             "PriceSchedule_Level12_StepMax ," +
                             "PriceSchedule_Level12_Price   ," +
                             "PriceSchedule_Level12_Fee     ," +
                             "PriceSchedule_Level13_StepMax ," +
                             "PriceSchedule_Level13_Price   ," +
                             "PriceSchedule_Level13_Fee     ," +
                             "PriceSchedule_Level14_StepMax ," +
                             "PriceSchedule_Level14_Price   ," +
                             "PriceSchedule_Level14_Fee     ," +
                             "PriceSchedule_Level15_StepMax ," +
                             "PriceSchedule_Level15_Price   ," +
                             "PriceSchedule_Level15_Fee     ," +
                             "PriceSchedule_Level16_StepMax ," +
                             "PriceSchedule_Level16_Price   ," +
                             "PriceSchedule_Level16_Fee      " +
                             ")   VALUES   (	                " +
                             "@PriceSchedule_Name            ," +
                             "@PriceSchedule_Code            ," +
                             "@PriceSchedule_ISSueDate       ," +
                             "@PriceSchedule_UnitTypeID      ," +
                             "@PriceSchedule_MonthFee1       ," +
                             "@PriceSchedule_MonthFee2       ," +
                             "@PriceSchedule_MonthFeeOption  ," +
                             "@PriceSchedule_PerMeterFee     ," +
                             "@PriceSchedule_Pricing         ," +
                             "@PriceSchedule_SWGPrice        ," +
                             "@PriceSchedule_SWGPercent      ," +
                             "@PriceSchedule_APPDate         ," +
                             "@PriceSchedule_NoOFUintsCalc   ," +
                             "@PriceSchedule_LevelNum        ," +
                             "@PriceSchedule_Level1_StepMax  ," +
                             "@PriceSchedule_Level1_Price    ," +
                             "@PriceSchedule_Level1_Fee      ," +
                             "@PriceSchedule_Level2_StepMax  ," +
                             "@PriceSchedule_Level2_Price    ," +
                             "@PriceSchedule_Level2_Fee      ," +
                             "@PriceSchedule_Level3_StepMax  ," +
                             "@PriceSchedule_Level3_Price    ," +
                             "@PriceSchedule_Level3_Fee      ," +
                             "@PriceSchedule_Level4_StepMax  ," +
                             "@PriceSchedule_Level4_Price    ," +
                             "@PriceSchedule_Level4_Fee      ," +
                             "@PriceSchedule_Level5_StepMax  ," +
                             "@PriceSchedule_Level5_Price    ," +
                             "@PriceSchedule_Level5_Fee      ," +
                             "@PriceSchedule_Level6_StepMax  ," +
                             "@PriceSchedule_Level6_Price    ," +
                             "@PriceSchedule_Level6_Fee      ," +
                             "@PriceSchedule_Level7_StepMax  ," +
                             "@PriceSchedule_Level7_Price    ," +
                             "@PriceSchedule_Level7_Fee      ," +
                             "@PriceSchedule_Level8_StepMax  ," +
                             "@PriceSchedule_Level8_Price    ," +
                             "@PriceSchedule_Level8_Fee      ," +
                             "@PriceSchedule_Level9_StepMax  ," +
                             "@PriceSchedule_Level9_Price    ," +
                             "@PriceSchedule_Level9_Fee      ," +
                             "@PriceSchedule_Level10_StepMax ," +
                             "@PriceSchedule_Level10_Price   ," +
                             "@PriceSchedule_Level10_Fee     ," +
                             "@PriceSchedule_Level11_StepMax ," +
                             "@PriceSchedule_Level11_Price   ," +
                             "@PriceSchedule_Level11_Fee     ," +
                             "@PriceSchedule_Level12_StepMax ," +
                             "@PriceSchedule_Level12_Price   ," +
                             "@PriceSchedule_Level12_Fee     ," +
                             "@PriceSchedule_Level13_StepMax ," +
                             "@PriceSchedule_Level13_Price   ," +
                             "@PriceSchedule_Level13_Fee     ," +
                             "@PriceSchedule_Level14_StepMax ," +
                             "@PriceSchedule_Level14_Price   ," +
                             "@PriceSchedule_Level14_Fee     ," +
                             "@PriceSchedule_Level15_StepMax ," +
                             "@PriceSchedule_Level15_Price   ," +
                             "@PriceSchedule_Level15_Fee     ," +
                             "@PriceSchedule_Level16_StepMax ," +
                             "@PriceSchedule_Level16_Price   ," +
                             "@PriceSchedule_Level16_Fee      )";
                SqlCommand cmd = new SqlCommand(sql, conn);


                cmd.Parameters.AddWithValue("@PriceSchedule_Name ", c.PriceSchedule_Name);
                cmd.Parameters.AddWithValue("@PriceSchedule_Code ", c.PriceSchedule_Code);
                cmd.Parameters.AddWithValue("@PriceSchedule_ISSueDate ", c.PriceSchedule_ISSueDate);
                cmd.Parameters.AddWithValue("@PriceSchedule_UnitTypeID ", c.PriceSchedule_UnitTypeID);
                cmd.Parameters.AddWithValue("@PriceSchedule_MonthFee1 ", c.PriceSchedule_MonthFee1);
                cmd.Parameters.AddWithValue("@PriceSchedule_MonthFee2 ", c.PriceSchedule_MonthFee2);
                cmd.Parameters.AddWithValue("@PriceSchedule_MonthFeeOption ", c.PriceSchedule_MonthFeeOption);
                cmd.Parameters.AddWithValue("@PriceSchedule_PerMeterFee ", c.PriceSchedule_PerMeterFee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Pricing ", c.PriceSchedule_Pricing);
                cmd.Parameters.AddWithValue("@PriceSchedule_SWGPrice ", c.PriceSchedule_SWGPrice);
                cmd.Parameters.AddWithValue("@PriceSchedule_SWGPercent ", c.PriceSchedule_SWGPercent);
                cmd.Parameters.AddWithValue("@PriceSchedule_APPDate ", c.PriceSchedule_APPDate);
                cmd.Parameters.AddWithValue("@PriceSchedule_NoOFUintsCalc ", c.PriceSchedule_NoOFUintsCalc);
                cmd.Parameters.AddWithValue("@PriceSchedule_LevelNum ", c.PriceSchedule_LevelNum);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level1_StepMax", c.PriceSchedule_Level1_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level1_Price ", c.PriceSchedule_Level1_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level1_Fee ", c.PriceSchedule_Level1_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level2_StepMax", c.PriceSchedule_Level2_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level2_Price ", c.PriceSchedule_Level2_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level2_Fee ", c.PriceSchedule_Level2_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level3_StepMax", c.PriceSchedule_Level3_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level3_Price ", c.PriceSchedule_Level3_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level3_Fee ", c.PriceSchedule_Level3_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level4_StepMax", c.PriceSchedule_Level4_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level4_Price ", c.PriceSchedule_Level4_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level4_Fee ", c.PriceSchedule_Level4_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level5_StepMax", c.PriceSchedule_Level5_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level5_Price ", c.PriceSchedule_Level5_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level5_Fee ", c.PriceSchedule_Level5_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level6_StepMax", c.PriceSchedule_Level6_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level6_Price ", c.PriceSchedule_Level6_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level6_Fee ", c.PriceSchedule_Level6_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level7_StepMax", c.PriceSchedule_Level7_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level7_Price ", c.PriceSchedule_Level7_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level7_Fee ", c.PriceSchedule_Level7_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level8_StepMax", c.PriceSchedule_Level8_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level8_Price ", c.PriceSchedule_Level8_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level8_Fee ", c.PriceSchedule_Level8_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level9_StepMax", c.PriceSchedule_Level9_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level9_Price ", c.PriceSchedule_Level9_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level9_Fee ", c.PriceSchedule_Level9_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level10_StepMax", c.PriceSchedule_Level10_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level10_Price ", c.PriceSchedule_Level10_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level10_Fee ", c.PriceSchedule_Level10_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level11_StepMax", c.PriceSchedule_Level11_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level11_Price ", c.PriceSchedule_Level11_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level11_Fee ", c.PriceSchedule_Level11_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level12_StepMax", c.PriceSchedule_Level12_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level12_Price ", c.PriceSchedule_Level12_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level12_Fee ", c.PriceSchedule_Level12_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level13_StepMax", c.PriceSchedule_Level13_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level13_Price ", c.PriceSchedule_Level13_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level13_Fee ", c.PriceSchedule_Level13_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level14_StepMax", c.PriceSchedule_Level14_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level14_Price ", c.PriceSchedule_Level14_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level14_Fee ", c.PriceSchedule_Level14_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level15_StepMax", c.PriceSchedule_Level15_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level15_Price ", c.PriceSchedule_Level15_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level15_Fee ", c.PriceSchedule_Level15_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level16_StepMax", c.PriceSchedule_Level16_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level16_Price ", c.PriceSchedule_Level16_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level16_Fee ", c.PriceSchedule_Level16_Fee);


                int rows = cmd.ExecuteNonQuery();
                //if the query is excute successfully then the value to rows will be greater than 0 else it will be less than 0
                if (rows > 0)
                {
                    //Query Successfull
                    isSuccess = true;
                }
                else
                {
                    //query failed
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;

        }
        #endregion
        #region update MeterType
        public bool Update(BLL_PriceScheduler c)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {


                string sql = "UPDATE PriceSchedule SET  " + 
                    "PriceSchedule_Name            =@PriceSchedule_Name            ," +
                    "PriceSchedule_Code            =@PriceSchedule_Code            ," +
                    "PriceSchedule_ISSueDate       =@PriceSchedule_ISSueDate       ," +
                    "PriceSchedule_UnitTypeID      =@PriceSchedule_UnitTypeID      ," +
                    "PriceSchedule_MonthFee1       =@PriceSchedule_MonthFee1       ," +
                    "PriceSchedule_MonthFee2       =@PriceSchedule_MonthFee2       ," +
                    "PriceSchedule_MonthFeeOption  =@PriceSchedule_MonthFeeOption  ," +
                    "PriceSchedule_PerMeterFee     =@PriceSchedule_PerMeterFee     ," +
                    "PriceSchedule_Pricing         =@PriceSchedule_Pricing         ," +
                    "PriceSchedule_SWGPrice        =@PriceSchedule_SWGPrice        ," +
                    "PriceSchedule_SWGPercent      =@PriceSchedule_SWGPercent      ," +
                    "PriceSchedule_APPDate         =@PriceSchedule_APPDate         ," +
                    "PriceSchedule_NoOFUintsCalc   =@PriceSchedule_NoOFUintsCalc   ," +
                    "PriceSchedule_LevelNum        =@PriceSchedule_LevelNum        ," +
                    "PriceSchedule_Level1_StepMax  =@PriceSchedule_Level1_StepMax  ," +
                    "PriceSchedule_Level1_Price    =@PriceSchedule_Level1_Price    ," +
                    "PriceSchedule_Level1_Fee      =@PriceSchedule_Level1_Fee      ," +
                    "PriceSchedule_Level2_StepMax  =@PriceSchedule_Level2_StepMax  ," +
                    "PriceSchedule_Level2_Price    =@PriceSchedule_Level2_Price    ," +
                    "PriceSchedule_Level2_Fee      =@PriceSchedule_Level2_Fee      ," +
                    "PriceSchedule_Level3_StepMax  =@PriceSchedule_Level3_StepMax  ," +
                    "PriceSchedule_Level3_Price    =@PriceSchedule_Level3_Price    ," +
                    "PriceSchedule_Level3_Fee      =@PriceSchedule_Level3_Fee      ," +
                    "PriceSchedule_Level4_StepMax  =@PriceSchedule_Level4_StepMax  ," +
                    "PriceSchedule_Level4_Price    =@PriceSchedule_Level4_Price    ," +
                    "PriceSchedule_Level4_Fee      =@PriceSchedule_Level4_Fee      ," +
                    "PriceSchedule_Level5_StepMax  =@PriceSchedule_Level5_StepMax  ," +
                    "PriceSchedule_Level5_Price    =@PriceSchedule_Level5_Price    ," +
                    "PriceSchedule_Level5_Fee      =@PriceSchedule_Level5_Fee      ," +
                    "PriceSchedule_Level6_StepMax  =@PriceSchedule_Level6_StepMax  ," +
                    "PriceSchedule_Level6_Price    =@PriceSchedule_Level6_Price    ," +
                    "PriceSchedule_Level6_Fee      =@PriceSchedule_Level6_Fee      ," +
                    "PriceSchedule_Level7_StepMax  =@PriceSchedule_Level7_StepMax  ," +
                    "PriceSchedule_Level7_Price    =@PriceSchedule_Level7_Price    ," +
                    "PriceSchedule_Level7_Fee      =@PriceSchedule_Level7_Fee      ," +
                    "PriceSchedule_Level8_StepMax  =@PriceSchedule_Level8_StepMax  ," +
                    "PriceSchedule_Level8_Price    =@PriceSchedule_Level8_Price    ," +
                    "PriceSchedule_Level8_Fee      =@PriceSchedule_Level8_Fee      ," +
                    "PriceSchedule_Level9_StepMax  =@PriceSchedule_Level9_StepMax  ," +
                    "PriceSchedule_Level9_Price    =@PriceSchedule_Level9_Price    ," +
                    "PriceSchedule_Level9_Fee      =@PriceSchedule_Level9_Fee      ," +
                    "PriceSchedule_Level10_StepMax =@PriceSchedule_Level10_StepMax ," +
                    "PriceSchedule_Level10_Price   =@PriceSchedule_Level10_Price   ," +
                    "PriceSchedule_Level10_Fee     =@PriceSchedule_Level10_Fee     ," +
                    "PriceSchedule_Level11_StepMax =@PriceSchedule_Level11_StepMax ," +
                    "PriceSchedule_Level11_Price   =@PriceSchedule_Level11_Price   ," +
                    "PriceSchedule_Level11_Fee     =@PriceSchedule_Level11_Fee     ," +
                    "PriceSchedule_Level12_StepMax =@PriceSchedule_Level12_StepMax ," +
                    "PriceSchedule_Level12_Price   =@PriceSchedule_Level12_Price   ," +
                    "PriceSchedule_Level12_Fee     =@PriceSchedule_Level12_Fee     ," +
                    "PriceSchedule_Level13_StepMax =@PriceSchedule_Level13_StepMax ," +
                    "PriceSchedule_Level13_Price   =@PriceSchedule_Level13_Price   ," +
                    "PriceSchedule_Level13_Fee     =@PriceSchedule_Level13_Fee     ," +
                    "PriceSchedule_Level14_StepMax =@PriceSchedule_Level14_StepMax ," +
                    "PriceSchedule_Level14_Price   =@PriceSchedule_Level14_Price   ," +
                    "PriceSchedule_Level14_Fee     =@PriceSchedule_Level14_Fee     ," +
                    "PriceSchedule_Level15_StepMax =@PriceSchedule_Level15_StepMax ," +
                    "PriceSchedule_Level15_Price   =@PriceSchedule_Level15_Price   ," +
                    "PriceSchedule_Level15_Fee     =@PriceSchedule_Level15_Fee     ," +
                    "PriceSchedule_Level16_StepMax =@PriceSchedule_Level16_StepMax ," +
                    "PriceSchedule_Level16_Price   =@PriceSchedule_Level16_Price   ," +
                    "PriceSchedule_Level16_Fee     =@PriceSchedule_Level16_Fee      " +

                    " WHERE PriceSchedule_ID=@PriceSchedule_ID ";


                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@PriceSchedule_Name ", c.PriceSchedule_Name);
                cmd.Parameters.AddWithValue("@PriceSchedule_Code ", c.PriceSchedule_Code);
                cmd.Parameters.AddWithValue("@PriceSchedule_ISSueDate ", c.PriceSchedule_ISSueDate);
                cmd.Parameters.AddWithValue("@PriceSchedule_UnitTypeID ", c.PriceSchedule_UnitTypeID);
                cmd.Parameters.AddWithValue("@PriceSchedule_MonthFee1 ", c.PriceSchedule_MonthFee1);
                cmd.Parameters.AddWithValue("@PriceSchedule_MonthFee2 ", c.PriceSchedule_MonthFee2);
                cmd.Parameters.AddWithValue("@PriceSchedule_MonthFeeOption ", c.PriceSchedule_MonthFeeOption);
                cmd.Parameters.AddWithValue("@PriceSchedule_PerMeterFee ", c.PriceSchedule_PerMeterFee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Pricing ", c.PriceSchedule_Pricing);
                cmd.Parameters.AddWithValue("@PriceSchedule_SWGPrice ", c.PriceSchedule_SWGPrice);
                cmd.Parameters.AddWithValue("@PriceSchedule_SWGPercent ", c.PriceSchedule_SWGPercent);
                cmd.Parameters.AddWithValue("@PriceSchedule_APPDate ", c.PriceSchedule_APPDate);
                cmd.Parameters.AddWithValue("@PriceSchedule_NoOFUintsCalc ", c.PriceSchedule_NoOFUintsCalc);
                cmd.Parameters.AddWithValue("@PriceSchedule_LevelNum ", c.PriceSchedule_LevelNum);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level1_StepMax", c.PriceSchedule_Level1_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level1_Price ", c.PriceSchedule_Level1_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level1_Fee ", c.PriceSchedule_Level1_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level2_StepMax", c.PriceSchedule_Level2_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level2_Price ", c.PriceSchedule_Level2_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level2_Fee ", c.PriceSchedule_Level2_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level3_StepMax", c.PriceSchedule_Level3_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level3_Price ", c.PriceSchedule_Level3_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level3_Fee ", c.PriceSchedule_Level3_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level4_StepMax", c.PriceSchedule_Level4_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level4_Price ", c.PriceSchedule_Level4_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level4_Fee ", c.PriceSchedule_Level4_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level5_StepMax", c.PriceSchedule_Level5_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level5_Price ", c.PriceSchedule_Level5_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level5_Fee ", c.PriceSchedule_Level5_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level6_StepMax", c.PriceSchedule_Level6_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level6_Price ", c.PriceSchedule_Level6_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level6_Fee ", c.PriceSchedule_Level6_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level7_StepMax", c.PriceSchedule_Level7_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level7_Price ", c.PriceSchedule_Level7_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level7_Fee ", c.PriceSchedule_Level7_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level8_StepMax", c.PriceSchedule_Level8_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level8_Price ", c.PriceSchedule_Level8_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level8_Fee ", c.PriceSchedule_Level8_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level9_StepMax", c.PriceSchedule_Level9_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level9_Price ", c.PriceSchedule_Level9_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level9_Fee ", c.PriceSchedule_Level9_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level10_StepMax", c.PriceSchedule_Level10_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level10_Price ", c.PriceSchedule_Level10_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level10_Fee ", c.PriceSchedule_Level10_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level11_StepMax", c.PriceSchedule_Level11_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level11_Price ", c.PriceSchedule_Level11_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level11_Fee ", c.PriceSchedule_Level11_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level12_StepMax", c.PriceSchedule_Level12_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level12_Price ", c.PriceSchedule_Level12_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level12_Fee ", c.PriceSchedule_Level12_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level13_StepMax", c.PriceSchedule_Level13_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level13_Price ", c.PriceSchedule_Level13_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level13_Fee ", c.PriceSchedule_Level13_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level14_StepMax", c.PriceSchedule_Level14_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level14_Price ", c.PriceSchedule_Level14_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level14_Fee ", c.PriceSchedule_Level14_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level15_StepMax", c.PriceSchedule_Level15_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level15_Price ", c.PriceSchedule_Level15_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level15_Fee ", c.PriceSchedule_Level15_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level16_StepMax", c.PriceSchedule_Level16_StepMax);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level16_Price ", c.PriceSchedule_Level16_Price);
                cmd.Parameters.AddWithValue("@PriceSchedule_Level16_Fee ", c.PriceSchedule_Level16_Fee);
                cmd.Parameters.AddWithValue("@PriceSchedule_ID", c.PriceSchedule_ID);



                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    //Query Successfull
                    isSuccess = true;
                }
                else
                {
                    //query failed
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        #endregion
        #region delete data from db
        public bool Delete(BLL_PriceScheduler c)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM PriceSchedule WHERE PriceSchedule_ID=@PriceSchedule_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@PriceSchedule_ID", c.PriceSchedule_ID);



                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    //Query Successfull
                    isSuccess = true;
                }
                else
                {
                    //query failed
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        #endregion
        #region Search PriceSchedule on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM PriceSchedule WHERE  PriceSchedule_Name = '" + keywords + "'  ";
                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //db connection open

                //fill data in dataTable
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        #endregion

        #region select PriceSchedule by code 
        public string[] SelectPriceScheduleNames()
        {
            List<string> PriceScheduleNames = new List<string>();
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT PriceSchedule_Name FROM PriceSchedule";

                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string unitName = reader["PriceSchedule_Name"].ToString();
                    PriceScheduleNames.Add(unitName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return PriceScheduleNames.ToArray();
        }
        #endregion


        #region select PriceSchedule by code 
        public string[] SelectPriceScheduleNames(string UnitName)
        {
            List<string> PriceScheduleNames = new List<string>();
            SqlConnection conn = db.Connect();

            try
            {
                // SQL Query to Get data from db with a WHERE clause to filter by UnitName
                String sql = "SELECT PriceSchedule_Name FROM PriceSchedule " +
                             "INNER JOIN UnitType ON PriceSchedule.PriceSchedule_UnitTypeID = UnitType.UnitType_ID " +
                             "WHERE UnitType.UnitType_Name = @UnitName";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UnitName", UnitName);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string priceScheduleName = reader["PriceSchedule_Name"].ToString();
                    PriceScheduleNames.Add(priceScheduleName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return PriceScheduleNames.ToArray();
        }

        #endregion

        #region select PriceSchedule by code 

        public int GetPriceSchedulerID(string priceScheduleName)
        {
            int priceScheduleID = -1; // Default value in case the priceScheduleName is not found

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get PriceSchedule_ID
                    string sql = "SELECT PriceSchedule_ID FROM PriceSchedule WHERE PriceSchedule_Name=@priceScheduleName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@priceScheduleName", priceScheduleName);

                        // Execute the query and retrieve the PriceSchedule_ID
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            priceScheduleID = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return priceScheduleID;
        }
        #endregion
        public int GetMaxPriceScheduleID()
        {
            int priceScheduleID = -1; // Default value in case no PriceSchedule_ID is found

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get the maximum PriceSchedule_ID from the PriceSchedule table
                    string sql = "SELECT MAX(PriceSchedule_ID) FROM PriceSchedule";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Execute the query and retrieve the maximum PriceSchedule_ID
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            priceScheduleID = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return priceScheduleID;
        }

        #region select PriceSchedule by code 

        public BLL_PriceScheduler GetPriceSchedulerDataByName(string priceScheduleName)
        {
            BLL_PriceScheduler PriceScheduler_Data = new BLL_PriceScheduler(); // Default value in case the priceScheduleName is not found

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get PriceSchedule data
                    string sql = "SELECT PriceSchedule_ID,PriceSchedule_Name,PriceSchedule_Code,PriceSchedule_ISSueDate," +
                        "PriceSchedule_UnitTypeID,PriceSchedule_MonthFee1,PriceSchedule_MonthFee2,PriceSchedule_MonthFeeOption," +
                        "PriceSchedule_PerMeterFee,PriceSchedule_Pricing,PriceSchedule_SWGPrice,PriceSchedule_SWGPercent," +
                        "PriceSchedule_APPDate,PriceSchedule_NoOFUintsCalc,PriceSchedule_LevelNum,PriceSchedule_Level1_StepMax," +
                        "PriceSchedule_Level1_Price,PriceSchedule_Level1_Fee,PriceSchedule_Level2_StepMax,PriceSchedule_Level2_Price," +
                        "PriceSchedule_Level2_Fee,PriceSchedule_Level3_StepMax,PriceSchedule_Level3_Price,PriceSchedule_Level3_Fee," +
                        "PriceSchedule_Level4_StepMax,PriceSchedule_Level4_Price,PriceSchedule_Level4_Fee,PriceSchedule_Level5_StepMax," +
                        "PriceSchedule_Level5_Price,PriceSchedule_Level5_Fee,PriceSchedule_Level6_StepMax,PriceSchedule_Level6_Price," +
                        "PriceSchedule_Level6_Fee,PriceSchedule_Level7_StepMax,PriceSchedule_Level7_Price,PriceSchedule_Level7_Fee," +
                        "PriceSchedule_Level8_StepMax,PriceSchedule_Level8_Price,PriceSchedule_Level8_Fee,PriceSchedule_Level9_StepMax," +
                        "PriceSchedule_Level9_Price,PriceSchedule_Level9_Fee,PriceSchedule_Level10_StepMax,PriceSchedule_Level10_Price," +
                        "PriceSchedule_Level10_Fee,PriceSchedule_Level11_StepMax,PriceSchedule_Level11_Price,PriceSchedule_Level11_Fee," +
                        "PriceSchedule_Level12_StepMax,PriceSchedule_Level12_Price,PriceSchedule_Level12_Fee,PriceSchedule_Level13_StepMax," +
                        "PriceSchedule_Level13_Price,PriceSchedule_Level13_Fee,PriceSchedule_Level14_StepMax,PriceSchedule_Level14_Price," +
                        "PriceSchedule_Level14_Fee,PriceSchedule_Level15_StepMax,PriceSchedule_Level15_Price,PriceSchedule_Level15_Fee," +
                        "PriceSchedule_Level16_StepMax,PriceSchedule_Level16_Price,PriceSchedule_Level16_Fee,UnitType_Name FROM PriceSchedule " +
                        "INNER JOIN UnitType ON PriceSchedule.PriceSchedule_UnitTypeID = UnitType.UnitType_ID WHERE PriceSchedule_Name = '"+priceScheduleName+"'";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@priceScheduleName", priceScheduleName);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of PriceScheduler_Data
                                PriceScheduler_Data.PriceSchedule_ID = Convert.ToInt32(reader["PriceSchedule_ID"]);
                                PriceScheduler_Data.PriceSchedule_Name = reader["PriceSchedule_Name"].ToString();
                                // Continue with the rest of the properties...

                                  PriceScheduler_Data.PriceSchedule_ID                  = Convert.ToInt32(reader["PriceSchedule_ID"]);
                                  PriceScheduler_Data.PriceSchedule_Name                = reader["PriceSchedule_Name"].ToString();
                                  PriceScheduler_Data.PriceSchedule_Code                = reader["PriceSchedule_Code"].ToString();
                                  PriceScheduler_Data.PriceSchedule_ISSueDate           = Convert.ToDateTime(reader["PriceSchedule_ISSueDate"]);
                                  PriceScheduler_Data.PriceSchedule_UnitTypeID          = Convert.ToInt32(reader["PriceSchedule_UnitTypeID"]);
                                  PriceScheduler_Data.PriceSchedule_MonthFee1           = Convert.ToInt32(reader["PriceSchedule_MonthFee1"]);
                                  PriceScheduler_Data.PriceSchedule_MonthFee2           = Convert.ToInt32(reader["PriceSchedule_MonthFee2"]);
                                  PriceScheduler_Data.PriceSchedule_MonthFeeOption      = Convert.ToInt32(reader["PriceSchedule_MonthFeeOption"]);
                                  PriceScheduler_Data.PriceSchedule_PerMeterFee         = Convert.ToInt32(reader["PriceSchedule_PerMeterFee"]);
                                  PriceScheduler_Data.PriceSchedule_Pricing             = Convert.ToInt32(reader["PriceSchedule_Pricing"]);
                                  PriceScheduler_Data.PriceSchedule_SWGPrice            = Convert.ToInt32(reader["PriceSchedule_SWGPrice"]);
                                  PriceScheduler_Data.PriceSchedule_SWGPercent          = Convert.ToInt32(reader["PriceSchedule_SWGPercent"]);
                                  PriceScheduler_Data.PriceSchedule_APPDate             = Convert.ToDateTime(reader["PriceSchedule_APPDate"]);
                                  PriceScheduler_Data.PriceSchedule_NoOFUintsCalc       = Convert.ToInt32(reader["PriceSchedule_NoOFUintsCalc"]);
                                  PriceScheduler_Data.PriceSchedule_LevelNum            = Convert.ToInt32(reader["PriceSchedule_LevelNum"]);
                                  PriceScheduler_Data.PriceSchedule_Level1_StepMax      = Convert.ToInt32(reader["PriceSchedule_Level1_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level1_Price        = Convert.ToInt32(reader["PriceSchedule_Level1_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level1_Fee          = Convert.ToInt32(reader["PriceSchedule_Level1_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level2_StepMax      = Convert.ToInt32(reader["PriceSchedule_Level2_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level2_Price        = Convert.ToInt32(reader["PriceSchedule_Level2_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level2_Fee          = Convert.ToInt32(reader["PriceSchedule_Level2_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level3_StepMax      = Convert.ToInt32(reader["PriceSchedule_Level3_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level3_Price        = Convert.ToInt32(reader["PriceSchedule_Level3_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level3_Fee          = Convert.ToInt32(reader["PriceSchedule_Level3_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level4_StepMax      = Convert.ToInt32(reader["PriceSchedule_Level4_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level4_Price        = Convert.ToInt32(reader["PriceSchedule_Level4_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level4_Fee          = Convert.ToInt32(reader["PriceSchedule_Level4_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level5_StepMax      = Convert.ToInt32(reader["PriceSchedule_Level5_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level5_Price        = Convert.ToInt32(reader["PriceSchedule_Level5_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level5_Fee          = Convert.ToInt32(reader["PriceSchedule_Level5_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level6_StepMax      = Convert.ToInt32(reader["PriceSchedule_Level6_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level6_Price        = Convert.ToInt32(reader["PriceSchedule_Level6_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level6_Fee          = Convert.ToInt32(reader["PriceSchedule_Level6_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level7_StepMax      = Convert.ToInt32(reader["PriceSchedule_Level7_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level7_Price        = Convert.ToInt32(reader["PriceSchedule_Level7_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level7_Fee          = Convert.ToInt32(reader["PriceSchedule_Level7_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level8_StepMax      = Convert.ToInt32(reader["PriceSchedule_Level8_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level8_Price        = Convert.ToInt32(reader["PriceSchedule_Level8_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level8_Fee          = Convert.ToInt32(reader["PriceSchedule_Level8_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level9_StepMax      = Convert.ToInt32(reader["PriceSchedule_Level9_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level9_Price        = Convert.ToInt32(reader["PriceSchedule_Level9_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level9_Fee          = Convert.ToInt32(reader["PriceSchedule_Level9_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level10_StepMax     = Convert.ToInt32(reader["PriceSchedule_Level10_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level10_Price       = Convert.ToInt32(reader["PriceSchedule_Level10_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level10_Fee         = Convert.ToInt32(reader["PriceSchedule_Level10_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level11_StepMax     = Convert.ToInt32(reader["PriceSchedule_Level11_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level11_Price       = Convert.ToInt32(reader["PriceSchedule_Level11_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level11_Fee         = Convert.ToInt32(reader["PriceSchedule_Level11_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level12_StepMax     = Convert.ToInt32(reader["PriceSchedule_Level12_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level12_Price       = Convert.ToInt32(reader["PriceSchedule_Level12_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level12_Fee         = Convert.ToInt32(reader["PriceSchedule_Level12_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level13_StepMax     = Convert.ToInt32(reader["PriceSchedule_Level13_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level13_Price       = Convert.ToInt32(reader["PriceSchedule_Level13_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level13_Fee         = Convert.ToInt32(reader["PriceSchedule_Level13_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level14_StepMax     = Convert.ToInt32(reader["PriceSchedule_Level14_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level14_Price       = Convert.ToInt32(reader["PriceSchedule_Level14_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level14_Fee         = Convert.ToInt32(reader["PriceSchedule_Level14_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level15_StepMax     = Convert.ToInt32(reader["PriceSchedule_Level15_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level15_Price       = Convert.ToInt32(reader["PriceSchedule_Level15_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level15_Fee         = Convert.ToInt32(reader["PriceSchedule_Level15_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_Level16_StepMax     = Convert.ToInt32(reader["PriceSchedule_Level16_StepMax"]);
                                  PriceScheduler_Data.PriceSchedule_Level16_Price       = Convert.ToInt32(reader["PriceSchedule_Level16_Price"]);
                                  PriceScheduler_Data.PriceSchedule_Level16_Fee         = Convert.ToInt32(reader["PriceSchedule_Level16_Fee"]);
                                  PriceScheduler_Data.PriceSchedule_TypeName            = reader["UnitType_Name"].ToString();


                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return PriceScheduler_Data;
        }

        #endregion

        #region Print Price Schedule
        public DataTable PrintPriceSchedule()
        {
            //creating db connection
            SqlConnection conn = db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT PriceSchedule_Name,PriceSchedule_Code,PriceSchedule_ISSueDate," +
                    " PriceSchedule_UnitTypeID,PriceSchedule_MonthFee1, PriceSchedule_MonthFee2," +
                    " PriceSchedule_MonthFeeOption, PriceSchedule_PerMeterFee, PriceSchedule_Pricing," +
                    " PriceSchedule_SWGPrice, PriceSchedule_SWGPercent, PriceSchedule_APPDate FROM PriceSchedule";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
        #endregion
    }
}
