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
    public class DAL_AddMeter
    {
        Database db = new Database();

        #region get MeterID from its num
        public int meterid(int meterNum)
        {

            int getwayID = 0;
            SqlConnection conn = db.Connect();

            try
            {
                string Sql = "SELECT Meter_ID FROM Meter WHERE Meter_MeterNum=@num";

                SqlCommand cmd = new SqlCommand(Sql, conn);

                cmd.Parameters.AddWithValue("@num", meterNum);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        getwayID = Convert.ToInt32(reader["Meter_ID"]);
                    }
                    else
                    {
                        getwayID = 0;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            return getwayID;
        }
        #endregion

        #region Get aggregation Id from its num 
        public int aggregationid(int aggregationNum)
        {

            int getwayID = 0;
            SqlConnection conn = db.Connect();

            try
            {
                string Sql = "SELECT Aggregation_ID FROM Aggregation WHERE Aggregation_Number=@num";

                SqlCommand cmd = new SqlCommand(Sql, conn);

                cmd.Parameters.AddWithValue("@num", aggregationNum);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        getwayID = Convert.ToInt32(reader["Aggregation_ID"]);
                    }
                    else
                    {
                        getwayID = 0;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            return getwayID;
        }
        #endregion

        #region get Meter Data
        public DataTable GetMeterList(int aggID)
        {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT Meter.Meter_MeterNum, Aggregation.Aggregation_Number, MeterList.MeterList_SecuirityEnable, " +
                             "MeterList.MeterList_Status, MeterList.MeterList_TimeIssue, MeterList.MeterList_LastTimeReading FROM MeterList " +
                             "RIGHT JOIN Meter ON Meter.Meter_ID=MeterList.MeterList_MeterID " +
                             "RIGHT JOIN Aggregation ON Aggregation.Aggregation_ID=MeterList.MeterList_AggregationID " +
                             "WHERE MeterList.MeterList_AggregationID = " + aggID;
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

        #region get this Aggregation
        public DataTable GetThisAgg(int AggNum)
        {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT Aggregation.Aggregation_GetwayID,Aggregation.Aggregation_ID, Aggregation.Aggregation_Number, Aggregation.Aggregation_Serial,Getway.Getway_Number, Aggregation.Aggregation_Description FROM Aggregation INNER JOIN Getway ON Getway.Getway_ID=Aggregation.Aggregation_GetwayID WHERE Aggregation.Aggregation_Number=" + AggNum;
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

        #region Add Meter in MeterList
        public bool InsertMeter(BLL_MeterList meterData)
        {
            bool isSuccess = false;

            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO MeterList (MeterList_MeterID,MeterList_AggregationID,MeterList_SecuirityEnable,MeterList_Status,MeterList_TimeIssue,MeterList_LastTimeReading) " +
                             "VALUES (@MeterID,@AggregationID,@SecuirityEnable,@Status,@TimeIssue,@LastTimeReading)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MeterID", meterData.MeterList_MeterID);
                cmd.Parameters.AddWithValue("@AggregationID", meterData.MeterList_AggregationID);
                cmd.Parameters.AddWithValue("@SecuirityEnable", meterData.MeterList_SecuirityEnable);
                cmd.Parameters.AddWithValue("@Status", meterData.MeterList_Status);
                cmd.Parameters.AddWithValue("@TimeIssue", meterData.MeterList_TimeIssue);
                cmd.Parameters.AddWithValue("@LastTimeReading", meterData.MeterList_LastTimeReading);



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

        #region Get Meter Table 
        public DataTable SelectTheMeters(int aggid)
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT Meter.Meter_MeterNum, Meter.Meter_PK, MeterType.MeterType_ManfName, MeterManf.MeterManf_Name, Meter.Meter_IssueDate," +
                    " Meter.Meter_Diameter, Meter.Meter_Origin, Meter.Meter_Model, Meter.Meter_Man, Meter.Meter_ChargeMode, Meter.Meter_Satatus FROM Meter " +
                    "INNER JOIN MeterType ON Meter.Meter_MeterTypeID = MeterType.MeterType_ID " +
                    "INNER JOIN MeterManf ON Meter.Meter_MeterManfID = MeterManf.MeterManf_ID " +
                    "WHERE Meter.Meter_ID NOT IN (SELECT MeterList_MeterID FROM MeterList WHERE MeterList_AggregationID=@aggID)";
                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@aggID", aggid);

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
    }
}
