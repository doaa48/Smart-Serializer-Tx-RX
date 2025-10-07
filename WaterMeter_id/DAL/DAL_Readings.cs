using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using WaterMeter_id.BLL;

namespace WaterMeter_id
{
    public class DAL_Readings
    {
        Database db = new Database();

        #region Select Data From ClientInfo 
        public BLL_ReadingCardInfo ReadCardData(int ClinetNum,int MeterNum,int CardNUM)
        {

            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();

            BLL_ReadingCardInfo bLL_ReadingCardInfo_data = new BLL_ReadingCardInfo();

            try
            {
                //create BLL for the following

                // SQL Query to Get data from db
                string sql = "SELECT ClientInfo.ClientInfo_ID, Meter.Meter_MeterNum, Meter.Meter_Origin, Meter.Meter_Man, Meter.Meter_ChargeMode, Meter.Meter_Diameter, " +
                             "ClientInfo.ClientInfo_Activity, ClientInfo.ClientInfo_NumOFUnit, ClientInfo.ClientInfo_Category, ClientInfo.ClientInfo_SwGServices, " +
                             "PriceSchedule.PriceSchedule_Name, OFFTime.OFFTime_Name, Client.Client_FullName, Client.Client_Number FROM ClientInfo " +
                             " INNER JOIN Meter ON ClientInfo.ClientInfo_MeterID=Meter.Meter_ID " +
                             " INNER JOIN PriceSchedule ON ClientInfo.ClientInfo_PriceScheduleID=PriceSchedule.PriceSchedule_ID " +
                             " INNER JOIN OFFTime ON ClientInfo.ClientInfo_OFFTimeID=OFFTime.OFFTime_ID " +
                             " INNER JOIN Client ON ClientInfo.ClientInfo_ClientID=Client.Client_ID" +
                             " INNER JOIN Card ON ClientInfo.ClientInfo_CardID=Card.Card_ID" +
                             " WHERE Meter.Meter_MeterNum = '" + MeterNum + "' AND Client.Client_Number = '" + ClinetNum + "' AND Card.Card_CardNum = '" + CardNUM + "'";

                //For executing Command
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            //Fill the data in BLL_ReadingCardInfo
                            bLL_ReadingCardInfo_data.ClientInfo_ID = Convert.ToInt32(reader["ClientInfo_ID"]);
                            bLL_ReadingCardInfo_data.Meter_MeterNum = Convert.ToInt32(reader["Meter_MeterNum"]);
                            bLL_ReadingCardInfo_data.Meter_Origin = Convert.ToInt32(reader["Meter_Origin"]);
                            bLL_ReadingCardInfo_data.Meter_Man = reader["Meter_Man"].ToString();
                            bLL_ReadingCardInfo_data.Meter_ChargeMode = Convert.ToInt32(reader["Meter_ChargeMode"]);
                            bLL_ReadingCardInfo_data.Meter_Diameter = Convert.ToInt32(reader["Meter_Diameter"]);
                            bLL_ReadingCardInfo_data.ClientInfo_Activity = Convert.ToInt32(reader["ClientInfo_Activity"]);
                            bLL_ReadingCardInfo_data.ClientInfo_NumOFUnit = Convert.ToInt32(reader["ClientInfo_NumOFUnit"]);
                            bLL_ReadingCardInfo_data.ClientInfo_Category = Convert.ToInt32(reader["ClientInfo_Category"]);
                            bLL_ReadingCardInfo_data.ClientInfo_SwGServices = Convert.ToInt32(reader["ClientInfo_SwGServices"]);
                            bLL_ReadingCardInfo_data.PriceSchedule_Name = reader["PriceSchedule_Name"].ToString();
                            bLL_ReadingCardInfo_data.OFFTime_Name = reader["OFFTime_Name"].ToString();
                            bLL_ReadingCardInfo_data.Client_FullName = reader["Client_FullName"].ToString();
                            bLL_ReadingCardInfo_data.Client_Number = Convert.ToInt32(reader["Client_Number"]);
                        }
                    }
                }

                /*//Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //db connection open

                //fill data in dataTable
                adapter.Fill(dt);*/

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return bLL_ReadingCardInfo_data;
        }
        #endregion
        #region Select Data From ClientInfo 
        public BLL_ReadingCardInfo ReadCardData(int subscriberID)
        {

            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();

            BLL_ReadingCardInfo bLL_ReadingCardInfo_data = new BLL_ReadingCardInfo();

            try
            {
                //create BLL for the following

                // SQL Query to Get data from db
                String sql = "SELECT ClientInfo.ClientInfo_ID, Meter.Meter_MeterNum, Meter.Meter_Origin, Meter.Meter_Man, Meter.Meter_ChargeMode, Meter.Meter_Diameter, " +
                             "ClientInfo.ClientInfo_Activity, ClientInfo.ClientInfo_NumOFUnit, ClientInfo.ClientInfo_Category, ClientInfo.ClientInfo_SwGServices, " +
                             "PriceSchedule.PriceSchedule_Name, OFFTime.OFFTime_Name, Client.Client_FullName, Client.Client_Number FROM ClientInfo " +
                             " INNER JOIN Meter ON ClientInfo.ClientInfo_MeterID=Meter.Meter_ID " +
                             " INNER JOIN PriceSchedule ON ClientInfo.ClientInfo_PriceScheduleID=PriceSchedule.PriceSchedule_ID " +
                             " INNER JOIN OFFTime ON ClientInfo.ClientInfo_OFFTimeID=OFFTime.OFFTime_ID " +
                             " INNER JOIN Client ON ClientInfo.ClientInfo_ClientID=Client.Client_ID" +
                             " INNER JOIN Card ON ClientInfo.ClientInfo_CardID=Card.Card_ID" +
                             " WHERE ClientInfo_SubscriberID = '" + subscriberID + "'";

                //For executing Command
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //Fill the data in BLL_ReadingCardInfo
                            bLL_ReadingCardInfo_data.ClientInfo_ID = Convert.ToInt32(reader["ClientInfo_ID"]);
                            bLL_ReadingCardInfo_data.Meter_MeterNum = Convert.ToInt32(reader["Meter_MeterNum"]);
                            bLL_ReadingCardInfo_data.Meter_Origin = Convert.ToInt32(reader["Meter_Origin"]);
                            bLL_ReadingCardInfo_data.Meter_Man = reader["Meter_Man"].ToString();
                            bLL_ReadingCardInfo_data.Meter_ChargeMode = Convert.ToInt32(reader["Meter_ChargeMode"]);
                            bLL_ReadingCardInfo_data.Meter_Diameter = Convert.ToInt32(reader["Meter_Diameter"]);
                            bLL_ReadingCardInfo_data.ClientInfo_Activity = Convert.ToInt32(reader["ClientInfo_Activity"]);
                            bLL_ReadingCardInfo_data.ClientInfo_NumOFUnit = Convert.ToInt32(reader["ClientInfo_NumOFUnit"]);
                            bLL_ReadingCardInfo_data.ClientInfo_Category = Convert.ToInt32(reader["ClientInfo_Category"]);
                            bLL_ReadingCardInfo_data.ClientInfo_SwGServices = Convert.ToInt32(reader["ClientInfo_SwGServices"]);
                            bLL_ReadingCardInfo_data.PriceSchedule_Name = reader["PriceSchedule_Name"].ToString();
                            bLL_ReadingCardInfo_data.OFFTime_Name = reader["OFFTime_Name"].ToString();
                            bLL_ReadingCardInfo_data.Client_FullName = reader["Client_FullName"].ToString();
                            bLL_ReadingCardInfo_data.Client_Number = Convert.ToInt32(reader["Client_Number"]);
                        }
                    }
                }

                /*//Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //db connection open

                //fill data in dataTable
                adapter.Fill(dt);*/


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return bLL_ReadingCardInfo_data;
        }
        #endregion
        #region Insert Deductions Data
        public bool InsertDeduction(BLL_Deductions bLL_Deduction)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();
            try
            {
                String sql = "Insert INTO Deductions (Deductions_ClientInfoID, Deductions_Date, Deductions_MonthFees, Deductions_AppDate, Deductions_Month) " +
                             "VALUES (@ClientInfoID, @Date, @MonthFees, @AppDate, @Month)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ClientInfoID", bLL_Deduction.Deductions_ClientInfoID);
                cmd.Parameters.AddWithValue("@Date", bLL_Deduction.Deductions_Date);
                cmd.Parameters.AddWithValue("@MonthFees", bLL_Deduction.Deductions_MonthFees);
                cmd.Parameters.AddWithValue("@AppDate", bLL_Deduction.Deductions_AppDate);
                cmd.Parameters.AddWithValue("@Month", bLL_Deduction.Deductions_Month);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
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

        #region Insert ChargeBasicInfo Data
        public bool InsertChargeBasicInfo(BLL_ChargeBasicInf bLL_ChargeBasicInfo)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();
            try
            {
                String sql = "Insert INTO ChargeBasicInf (ChargeBasicInf_ClientInfoID, ChargeBasicInf_Date, ChargeBasicInf_ChargeAmount, " +
                             "ChargeBasicInf_ChargeNo, ChargeBasicInf_CutoffWarningLimit, ChargeBasicInf_MaxOverdraftCredit, ChargeBasicInf_ChargeDate, " +
                             "ChargeBasicInf_EnabledValvePeriod, ChargeBasicInf_MeterAction) " +
                             " VALUES (@ClientInfoID,@Date,@ChargeAmount,@ChargeNo, @CutoffWarningLimit,@MaxOverdraftCredit,@ChargeDate,@EnabledValvePeriod,@MeterAction)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ClientInfoID", bLL_ChargeBasicInfo.ChargeBasicInf_ClientInfoID);
                cmd.Parameters.AddWithValue("@Date", bLL_ChargeBasicInfo.ChargeBasicInf_Date);
                cmd.Parameters.AddWithValue("@ChargeAmount", bLL_ChargeBasicInfo.ChargeBasicInf_ChargeAmount);
                cmd.Parameters.AddWithValue("@ChargeNo", bLL_ChargeBasicInfo.ChargeBasicInf_ChargeNo);
                cmd.Parameters.AddWithValue("@CutoffWarningLimit", bLL_ChargeBasicInfo.ChargeBasicInf_CutoffWarningLimit);
                cmd.Parameters.AddWithValue("@MaxOverdraftCredit", bLL_ChargeBasicInfo.ChargeBasicInf_MaxOverdraftCredit);
                cmd.Parameters.AddWithValue("@ChargeDate", bLL_ChargeBasicInfo.ChargeBasicInf_ChargeDate);
                cmd.Parameters.AddWithValue("@EnabledValvePeriod", bLL_ChargeBasicInfo.ChargeBasicInf_EnabledValvePeriod);
                cmd.Parameters.AddWithValue("@MeterAction", bLL_ChargeBasicInfo.ChargeBasicInf_MeterAction);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
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

        #region Insert CreditBalance Data
        public bool InsertCreditBalance(BLL_CreditBalance p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO CreditBalance (CreditBalance_ClientInfoID,CreditBalance_Date,CreditBalance_ReadingBy,CreditBalance_UsedMonthly1,CreditBalance_UsedMonthly2,CreditBalance_UsedMonthly3,CreditBalance_UsedMonthly4,CreditBalance_UsedMonthly5,CreditBalance_UsedMonthly6,CreditBalance_UsedMonthly7,CreditBalance_UsedMonthly8,CreditBalance_UsedMonthly9,CreditBalance_UsedMonthly10,CreditBalance_UsedMonthly11,CreditBalance_UsedMonthly12,CreditBalance_RemainCredit,CreditBalance_OverdraftCredit,CreditBalance_ConsumedCredit,CreditBalance_CumulativeCharges,CreditBalance_AppDate) " +
                                             " VALUES (@ClientInfoID,@Date,@ReadingBy,@UsedMonthly1,@UsedMonthly2,@UsedMonthly3,@UsedMonthly4,@UsedMonthly5,@UsedMonthly6,@UsedMonthly7,@UsedMonthly8,@UsedMonthly9,@UsedMonthly10,@UsedMonthly11,@UsedMonthly12,@RemainCredit,@OverdraftCredit,@ConsumedCredit,@CumulativeCharges,@AppDate)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                //@ClientInfoID,@Date,@ReadingBy,@UsedMonthly1,@UsedMonthly2,@UsedMonthly3,@UsedMonthly4,@UsedMonthly5,@UsedMonthly6,@UsedMonthly7,@UsedMonthly8,@UsedMonthly9,@UsedMonthly10,@UsedMonthly11,@UsedMonthly12,@RemainCredit,@OverdraftCredit,@ConsumedCredit,@CumulativeCharges,@AppDate
                cmd.Parameters.AddWithValue("@ClientInfoID", p.CreditBalance_ClientInfoID);
                cmd.Parameters.AddWithValue("@Date", p.CreditBalance_Date);
                cmd.Parameters.AddWithValue("@ReadingBy", p.CreditBalance_ReadingBy);
                cmd.Parameters.AddWithValue("@UsedMonthly1", p.CreditBalance_UsedMonthly1);
                cmd.Parameters.AddWithValue("@UsedMonthly2", p.CreditBalance_UsedMonthly2);
                cmd.Parameters.AddWithValue("@UsedMonthly3", p.CreditBalance_UsedMonthly3);
                cmd.Parameters.AddWithValue("@UsedMonthly4", p.CreditBalance_UsedMonthly4);
                cmd.Parameters.AddWithValue("@UsedMonthly5", p.CreditBalance_UsedMonthly5);
                cmd.Parameters.AddWithValue("@UsedMonthly6", p.CreditBalance_UsedMonthly6);
                cmd.Parameters.AddWithValue("@UsedMonthly7", p.CreditBalance_UsedMonthly7);
                cmd.Parameters.AddWithValue("@UsedMonthly8", p.CreditBalance_UsedMonthly8);
                cmd.Parameters.AddWithValue("@UsedMonthly9", p.CreditBalance_UsedMonthly9);
                cmd.Parameters.AddWithValue("@UsedMonthly10", p.CreditBalance_UsedMonthly10);
                cmd.Parameters.AddWithValue("@UsedMonthly11", p.CreditBalance_UsedMonthly11);
                cmd.Parameters.AddWithValue("@UsedMonthly12", p.CreditBalance_UsedMonthly12);
                cmd.Parameters.AddWithValue("@RemainCredit", p.CreditBalance_RemainCredit);
                cmd.Parameters.AddWithValue("@OverdraftCredit", p.CreditBalance_OverdraftCredit);
                cmd.Parameters.AddWithValue("@ConsumedCredit", p.CreditBalance_ConsumedCredit);
                cmd.Parameters.AddWithValue("@CumulativeCharges", p.CreditBalance_CumulativeCharges);
                cmd.Parameters.AddWithValue("@AppDate", p.CreditBalance_AppDate);



                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
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

        #region Insert Reading Data
        public bool InsertReading(BLL_Readings bLL_Reading)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = " INSERT INTO Readings (Readings_ClientInfoID, Readings_Date, Readings_MonthConsumption1, " +
                             " Readings_MonthConsumption2, Readings_MonthConsumption3, Readings_MonthConsumption4, Readings_MonthConsumption5, " +
                             " Readings_MonthConsumption6, Readings_MonthConsumption7, Readings_MonthConsumption8, Readings_MonthConsumption9, " +
                             " Readings_MonthConsumption10, Readings_MonthConsumption11, Readings_MonthConsumption12, Readings_QuantityTotalNegative," +
                             " Readings_CurrentMonthConsumption, Readings_Reading) " +
                             " VALUES (@ClientInfoID, @Date, @MonthConsumption1, @MonthConsumption2, @MonthConsumption3, @MonthConsumption4, " +
                             " @MonthConsumption5, @MonthConsumption6, @MonthConsumption7, @MonthConsumption8, @MonthConsumption9, @MonthConsumption10, " +
                             " @MonthConsumption11, @MonthConsumption12, @QuantityTotalNegative, @CurrentMonthConsumption, @Reading)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                cmd.Parameters.AddWithValue("@ClientInfoID"           , bLL_Reading.Readings_ClientInfoID);
                cmd.Parameters.AddWithValue("@Date"                   , bLL_Reading.Readings_Date);
                cmd.Parameters.AddWithValue("@MonthConsumption1"      , bLL_Reading.Readings_MonthConsumption1);
                cmd.Parameters.AddWithValue("@MonthConsumption2"      , bLL_Reading.Readings_MonthConsumption2);
                cmd.Parameters.AddWithValue("@MonthConsumption3"      , bLL_Reading.Readings_MonthConsumption3);
                cmd.Parameters.AddWithValue("@MonthConsumption4"      , bLL_Reading.Readings_MonthConsumption4);
                cmd.Parameters.AddWithValue("@MonthConsumption5"      , bLL_Reading.Readings_MonthConsumption5);
                cmd.Parameters.AddWithValue("@MonthConsumption6"      , bLL_Reading.Readings_MonthConsumption6);
                cmd.Parameters.AddWithValue("@MonthConsumption7"      , bLL_Reading.Readings_MonthConsumption7);
                cmd.Parameters.AddWithValue("@MonthConsumption8"      , bLL_Reading.Readings_MonthConsumption8);
                cmd.Parameters.AddWithValue("@MonthConsumption9"      , bLL_Reading.Readings_MonthConsumption9);
                cmd.Parameters.AddWithValue("@MonthConsumption10"     , bLL_Reading.Readings_MonthConsumption10);
                cmd.Parameters.AddWithValue("@MonthConsumption11"     , bLL_Reading.Readings_MonthConsumption11);
                cmd.Parameters.AddWithValue("@MonthConsumption12"     , bLL_Reading.Readings_MonthConsumption12);
                cmd.Parameters.AddWithValue("@QuantityTotalNegative"  , bLL_Reading.Readings_QuantityTotalNegative);
                cmd.Parameters.AddWithValue("@CurrentMonthConsumption", bLL_Reading.Readings_CurrentMonthConsumption);
                cmd.Parameters.AddWithValue("@Reading"                , bLL_Reading.Readings_Reading);
                



                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return isSuccess;
        }
        #endregion

        #region Insert Meter State Data
        public bool InsertMeterState(BLL_MeterState p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO MeterState (MeterState_ClientInfoID,MeterState_Date,MeterState_ReadingBy," +
                             "MeterState_Malfun1_Month,MeterState_Malfun1_Day,MeterState_Malfun1_Count,MeterState_Malfun2_Month," +
                             "MeterState_Malfun2_Day,MeterState_Malfun2_Count,MeterState_Malfun3_Month,MeterState_Malfun3_Day," +
                             "MeterState_Malfun3_Count,MeterState_Malfun4_Month,MeterState_Malfun4_Day,MeterState_Malfun4_Count," +
                             "MeterState_Malfun5_Month,MeterState_Malfun5_Day,MeterState_Malfun5_Count,MeterState_MeterStateDate1," +
                             "MeterState_MeterStateDate2,MeterState_MeterStateDate3,MeterState_MeterStateDate4,MeterState_MeterErrors) " +
                             " VALUES (@ClientInfoID,@Date,@ReadingBy,@Malfun1_Month,@Malfun1_Day,@Malfun1_Count,@Malfun2_Month," +
                             "@Malfun2_Day,@Malfun2_Count,@Malfun3_Month,@Malfun3_Day,@Malfun3_Count,@Malfun4_Month,@Malfun4_Day," +
                             "@Malfun4_Count,@Malfun5_Month,@Malfun5_Day,@Malfun5_Count,@MeterStateDate1,@MeterStateDate2,@MeterStateDate3," +
                             "@MeterStateDate4,@MeterErrors)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ClientInfoID", p.MeterState_ClientInfoID);
                cmd.Parameters.AddWithValue("@Date", p.MeterState_Date);
                cmd.Parameters.AddWithValue("@ReadingBy", p.MeterState_ReadingBy);
                cmd.Parameters.AddWithValue("@Malfun1_Month", p.MeterState_Malfun1_Month);
                cmd.Parameters.AddWithValue("@Malfun1_Day", p.MeterState_Malfun1_Day);
                cmd.Parameters.AddWithValue("@Malfun1_Count", p.MeterState_Malfun1_Count);
                cmd.Parameters.AddWithValue("@Malfun2_Month", p.MeterState_Malfun2_Month);
                cmd.Parameters.AddWithValue("@Malfun2_Day", p.MeterState_Malfun2_Day);
                cmd.Parameters.AddWithValue("@Malfun2_Count", p.MeterState_Malfun2_Count);
                cmd.Parameters.AddWithValue("@Malfun3_Month", p.MeterState_Malfun3_Month);
                cmd.Parameters.AddWithValue("@Malfun3_Day", p.MeterState_Malfun3_Day);
                cmd.Parameters.AddWithValue("@Malfun3_Count", p.MeterState_Malfun3_Count);
                cmd.Parameters.AddWithValue("@Malfun4_Month", p.MeterState_Malfun4_Month);
                cmd.Parameters.AddWithValue("@Malfun4_Day", p.MeterState_Malfun4_Day);
                cmd.Parameters.AddWithValue("@Malfun4_Count", p.MeterState_Malfun4_Count);
                cmd.Parameters.AddWithValue("@Malfun5_Month", p.MeterState_Malfun5_Month);
                cmd.Parameters.AddWithValue("@Malfun5_Day", p.MeterState_Malfun5_Day);
                cmd.Parameters.AddWithValue("@Malfun5_Count", p.MeterState_Malfun5_Count);
                cmd.Parameters.AddWithValue("@MeterStateDate1", p.MeterState_MeterStateDate1);
                cmd.Parameters.AddWithValue("@MeterStateDate2", p.MeterState_MeterStateDate2);
                cmd.Parameters.AddWithValue("@MeterStateDate3", p.MeterState_MeterStateDate3);
                cmd.Parameters.AddWithValue("@MeterStateDate4", p.MeterState_MeterStateDate4);
                cmd.Parameters.AddWithValue("@MeterErrors", p.MeterState_MeterErrors);





                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
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

        #region Check the Last Transaction Date
        public bool CheckReadingDate(BLL_CardIssues bLL_CardIssues)
        {
            bool isExists = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "SELECT * FROM Card WHERE Card_LastTranscationDate=@lastTransactionDate"; //Check if this card was read before or not

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@lastTransactionDate", bLL_CardIssues.LastTranscationDate);



                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    //Query Successfull
                    isExists = true;
                }
                else
                {
                    //query failed
                    isExists = false;
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

            return isExists;
        }
        #endregion

        #region Update The Card 
        public bool UpdateCard(BLL_CardIssues bll_cardIssue)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {


                string sql = "UPDATE Card SET Card_LastTranscationDate = @lastTransactionDate ,Card_Func=@Card_Func,Card_Mode=@Card_Mode,Card_LastUploadBy=@LastUpdateBy WHERE Card_ID='" + bll_cardIssue.id + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@lastTransactionDate", bll_cardIssue.LastTranscationDate);
                cmd.Parameters.AddWithValue("@Card_Func", bll_cardIssue.Func);
                cmd.Parameters.AddWithValue("@Card_Mode", bll_cardIssue.Mode);
                cmd.Parameters.AddWithValue("@LastUpdateBy", bll_cardIssue.LastUploadBy);


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
    }
    #endregion
}