using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaterMeter_id
{
    public class DAL_SchedulerMeter
    {
        Database db = new Database();

        #region Select Scheduler meter from Database
        public DataTable Select()
        {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM SchedularMeter";
                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //db connection open
                //conn.Open();
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

        #region Insert Scheduler Meter in DB
        public bool Insert(BLL_SchedulerMeter p)
        {
            bool isSuccess = false;

            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO SchedularMeter (SchedularMeter_MeterID, SchedularMeter_AggregationID, SchedularMeter_PayLoad, SchedularMeter_TimeIssue, SchedularMeter_TimeLastReading, SchedularMeter_Status, SchedularMeter_EnableMeter, SchedularMeter_SmartType)" +
                             " VALUES (@MeterID,@AggregationID,@PayLoad,@TimeIssue,@TimeLastReading,@Status,@EnableMeter,@SmartType)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MeterID", p.SchedularMeter_MeterID);
                cmd.Parameters.AddWithValue("@AggregationID", p.SchedularMeter_AggregationID);
                cmd.Parameters.AddWithValue("@PayLoad", p.SchedularMeter_PayLoad);
                cmd.Parameters.AddWithValue("@TimeIssue", p.SchedularMeter_TimeIssue);
                cmd.Parameters.AddWithValue("@TimeLastReading", p.SchedularMeter_TimeLastReading);
                cmd.Parameters.AddWithValue("@Status", p.SchedularMeter_Status);
                cmd.Parameters.AddWithValue("@EnableMeter", p.SchedularMeter_EnableMeter);
                cmd.Parameters.AddWithValue("@SmartType", p.SchedularMeter_SmartType);


                //conn.Open();

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

        #region update Sceduler Meter in db
        public bool Update(BLL_SchedulerMeter p)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE SchedularMeter SET SchedularMeter_MeterID=@MeterID, SchedularMeter_AggregationID=@AggregationID, SchedularMeter_PayLoad=@PayLoad," +
                    " SchedularMeter_TimeIssue=@TimeIssue, SchedularMeter_TimeLastReading=@TimeLastReading, SchedularMeter_Status=@Status, SchedularMeter_EnableMeter=@EnableMeter," +
                    " SchedularMeter_SmartType=@SmartType WHERE SchedularMeter_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MeterID", p.SchedularMeter_MeterID);
                cmd.Parameters.AddWithValue("@AggregationID", p.SchedularMeter_AggregationID);
                cmd.Parameters.AddWithValue("@PayLoad", p.SchedularMeter_PayLoad);
                cmd.Parameters.AddWithValue("@TimeIssue", p.SchedularMeter_TimeIssue);
                cmd.Parameters.AddWithValue("@TimeLastReading", p.SchedularMeter_TimeLastReading);
                cmd.Parameters.AddWithValue("@Status", p.SchedularMeter_Status);
                cmd.Parameters.AddWithValue("@EnableMeter", p.SchedularMeter_EnableMeter);
                cmd.Parameters.AddWithValue("@SmartType", p.SchedularMeter_SmartType);
                cmd.Parameters.AddWithValue("@id", p.SchedularMeter_ID);

                //conn.Open();

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
               // MessageBox.Show(ex.Message);
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

        #region delete data from db
        public bool Delete(BLL_SchedulerMeter p)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM SchedularMeter WHERE SchedularMeter_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.SchedularMeter_ID);

                //conn.Open();

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

        #region Get Scheduler Meter from Num
        public BLL_SchedulerMeter getSchedulerMeterData(int num)
        {
            BLL_SchedulerMeter bLL_SchedularMeter = new BLL_SchedulerMeter();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    string SqL = "SELECT * FROM SchedularMeter WHERE SchedularMeter_ID=@num";

                    using (SqlCommand cmd = new SqlCommand(SqL, conn))
                    {
                        cmd.Parameters.AddWithValue("@num", num); // Add parameter to the query

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bLL_SchedularMeter.SchedularMeter_ID = Convert.ToInt32(reader["SchedularMeter_ID"]);
                                bLL_SchedularMeter.SchedularMeter_MeterID = Convert.ToInt32(reader["SchedularMeter_MeterID"]);
                                bLL_SchedularMeter.SchedularMeter_AggregationID = Convert.ToInt32(reader["SchedularMeter_AggregationID"]);
                                bLL_SchedularMeter.SchedularMeter_PayLoad = reader["SchedularMeter_PayLoad"].ToString();
                                bLL_SchedularMeter.SchedularMeter_TimeIssue = Convert.ToDateTime(reader["SchedularMeter_TimeIssue"]);
                                bLL_SchedularMeter.SchedularMeter_TimeLastReading = Convert.ToDateTime(reader["SchedularMeter_TimeLastReading"]);
                                bLL_SchedularMeter.SchedularMeter_Status = Convert.ToInt32(reader["SchedularMeter_Status"]);
                                bLL_SchedularMeter.SchedularMeter_EnableMeter = Convert.ToInt32(reader["SchedularMeter_EnableMeter"]);
                                bLL_SchedularMeter.SchedularMeter_SmartType = reader["SchedularMeter_SmartType"].ToString();
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return bLL_SchedularMeter;
        }
        #endregion

        #region Get Datatable of specific row
        public DataTable SelectMeterList(int num)
        {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM SchedularMeter Where SchedularMeter_ID = @id";
                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", num);

                //Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //db connection open
                //conn.Open();
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

        public List<BLL_SchedulerMeter> Getschduler(int getwaynum, DateTime TimeStart, string SmartType)
        {
            List<BLL_SchedulerMeter> schedulerList = new List<BLL_SchedulerMeter>();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    string sql = @"SELECT [SchedularMeter_ID]
                                  ,[SchedularMeter_MeterID]
                                  ,[SchedularMeter_AggregationID]
                                  ,[SchedularMeter_PayLoad]
                                  ,[SchedularMeter_TimeIssue]
                                  ,[SchedularMeter_TimeLastReading]
                                  ,[SchedularMeter_Status]
                                  ,[SchedularMeter_EnableMeter]
                                  ,[SchedularMeter_SmartType]
                                  , Meter.Meter_MeterNum
                                  , Aggregation.Aggregation_Number
                             FROM SchedularMeter
                             INNER JOIN Meter ON SchedularMeter.SchedularMeter_MeterID = Meter.Meter_ID
                             INNER JOIN Aggregation ON SchedularMeter.SchedularMeter_AggregationID = Aggregation.Aggregation_ID
                             INNER JOIN Getway ON Aggregation.Aggregation_GetwayID = Getway.Getway_ID
                             WHERE Getway.Getway_Number = @Getway_Number 
                             AND SchedularMeter_Status = '0' 
                             AND SchedularMeter_EnableMeter = '1' 
                             AND SchedularMeter_TimeLastReading < @startTime;";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Getway_Number", getwaynum);
                        cmd.Parameters.AddWithValue("@startTime", TimeStart);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BLL_SchedulerMeter bLL_SchedularMeter = new BLL_SchedulerMeter();
                                bLL_SchedularMeter.SchedularMeter_ID = Convert.ToInt32(reader["SchedularMeter_ID"]);
                                bLL_SchedularMeter.SchedularMeter_MeterID = Convert.ToInt32(reader["SchedularMeter_MeterID"]);
                                bLL_SchedularMeter.SchedularMeter_AggregationID = Convert.ToInt32(reader["SchedularMeter_AggregationID"]);
                                bLL_SchedularMeter.SchedularMeter_PayLoad = reader["SchedularMeter_PayLoad"].ToString();
                                bLL_SchedularMeter.SchedularMeter_TimeIssue = Convert.ToDateTime(reader["SchedularMeter_TimeIssue"]);
                                bLL_SchedularMeter.SchedularMeter_TimeLastReading = Convert.ToDateTime(reader["SchedularMeter_TimeLastReading"]);
                                bLL_SchedularMeter.SchedularMeter_Status = Convert.ToInt32(reader["SchedularMeter_Status"]);
                                bLL_SchedularMeter.SchedularMeter_EnableMeter = Convert.ToInt32(reader["SchedularMeter_EnableMeter"]);
                                bLL_SchedularMeter.SchedularMeter_SmartType = reader["SchedularMeter_SmartType"].ToString();
                                bLL_SchedularMeter.MeterNum = Convert.ToInt32(reader["Meter_MeterNum"]);
                                bLL_SchedularMeter.AggregationNum = Convert.ToInt32(reader["Aggregation_Number"]);

                                schedulerList.Add(bLL_SchedularMeter);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                   // MessageBox.Show(ex.Message);
                }
            }
            return schedulerList;
        }



    }
}
