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
    public class DAL_MeterIssues
    {

        Database db = new Database();
        #region Select meterIssues from Database
        public DataTable Select()
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Meter";
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

        #region SelectTheMeters
        public DataTable SelectTheMeters()
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT Meter_MeterNum, Meter_PK, MeterType_ManfName, MeterManf_Name, Meter_IssueDate," +
                    " Meter_Diameter, Meter_Origin, Meter_Model, Meter_Man, Meter_ChargeMode, Meter_Satatus FROM Meter " +
                    "INNER JOIN MeterType ON Meter.Meter_MeterTypeID = MeterType.MeterType_ID " +
                    "INNER JOIN MeterManf ON Meter.Meter_MeterManfID = MeterManf.MeterManf_ID";
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
        #endregion SelectThe Meters

        #region Insert Meter in DB
        public bool Insert(BLL_MeterIssues p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO Meter (Meter_PK, Meter_MeterTypeID, Meter_MeterManfID, Meter_CertMeterManf_TO_Meter,Meter_SerialNumber,Meter_IssueDate,Meter_MeterNum,Meter_Diameter,Meter_Origin,Meter_Model,Meter_Man,Meter_ChargeMode,Meter_Satatus) VALUES (@Meter_PK, @Meter_MeterTypeID, @Meter_MeterManfID, @Meter_CertMeterManf_TO_Meter,@Meter_SerialNumber,@Meter_IssueDate,@Meter_MeterNum,@Meter_Diameter,@Meter_Origin,@Meter_Model,@Meter_Man,@Meter_ChargeMode,@Meter_Satatus)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Meter_PK", p.Meter_PK);
                cmd.Parameters.AddWithValue("@Meter_MeterTypeID", p.Meter_MeterTypeID);
                cmd.Parameters.AddWithValue("@Meter_MeterManfID", p.Meter_MeterManfID);
                cmd.Parameters.AddWithValue("@Meter_CertMeterManf_TO_Meter", p.Meter_CertMeterManf_TO_Meter);
              //  cmd.Parameters.AddWithValue("@Meter_WaterCompID", p.Meter_WaterCompID);
                //cmd.Parameters.AddWithValue("@Meter_CertWaterComp_TO_Meter", p.Meter_CertWaterComp_TO_Meter);
                cmd.Parameters.AddWithValue("@Meter_SerialNumber", p.Meter_SerialNumber);
                cmd.Parameters.AddWithValue("@Meter_IssueDate", p.Meter_IssueDate);
                cmd.Parameters.AddWithValue("@Meter_MeterNum", p.Meter_MeterNum);
                cmd.Parameters.AddWithValue("@Meter_Diameter", p.Meter_Diameter);
                cmd.Parameters.AddWithValue("@Meter_Origin", p.Meter_Origin);
                cmd.Parameters.AddWithValue("@Meter_Model", p.Meter_Model);
                cmd.Parameters.AddWithValue("@Meter_Man", p.Meter_Man);
                cmd.Parameters.AddWithValue("@Meter_ChargeMode", p.Meter_ChargeMode);
                cmd.Parameters.AddWithValue("@Meter_Satatus", p.Meter_Satatus);




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
        #region update CardType in db
        public bool Update(BLL_MeterIssues p)
        {
            bool isSuccess = false;
         

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE Meter SET Meter_PK=@Meter_PK,Meter_MeterTypeID=@Meter_MeterTypeID, Meter_MeterManfID=@Meter_MeterManfID, Meter_CertMeterManf_TO_Meter=@Meter_CertMeterManf_TO_Meter,Meter_WaterCompID=@Meter_WaterCompID,Meter_CertWaterComp_TO_Meter=@Meter_CertWaterComp_TO_Meter,Meter_SerialNumber=@Meter_SerialNumber,Meter_IssueDate=@Meter_IssueDate,Meter_MeterNum=@Meter_MeterNum,Meter_Diameter=@Meter_Diameter,Meter_Origin=@Meter_Origin,Meter_Model=@Meter_Model,Meter_Man=@Meter_Man,Meter_ChargeMode=@Meter_ChargeMode,Meter_Satatus=@Meter_Satatus WHERE Meter_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Meter_PK", p.Meter_PK);
                cmd.Parameters.AddWithValue("@Meter_MeterTypeID", p.Meter_MeterTypeID);
                cmd.Parameters.AddWithValue("@Meter_MeterManfID", p.Meter_MeterManfID);
                cmd.Parameters.AddWithValue("@Meter_CertMeterManf_TO_Meter", p.Meter_CertMeterManf_TO_Meter);
                cmd.Parameters.AddWithValue("@Meter_WaterCompID", p.Meter_WaterCompID);
                cmd.Parameters.AddWithValue("@Meter_CertWaterComp_TO_Meter", p.Meter_CertWaterComp_TO_Meter);
                cmd.Parameters.AddWithValue("@Meter_SerialNumber", p.Meter_SerialNumber);
                cmd.Parameters.AddWithValue("@Meter_IssueDate", p.Meter_IssueDate);
                cmd.Parameters.AddWithValue("@Meter_MeterNum", p.Meter_MeterNum);
                cmd.Parameters.AddWithValue("@Meter_Diameter", p.Meter_Diameter);
                cmd.Parameters.AddWithValue("@Meter_Origin", p.Meter_Origin);
                cmd.Parameters.AddWithValue("@Meter_Model", p.Meter_Model);
                cmd.Parameters.AddWithValue("@Meter_Man", p.Meter_Man);
                cmd.Parameters.AddWithValue("@Meter_ChargeMode", p.Meter_ChargeMode);
                cmd.Parameters.AddWithValue("@Meter_Satatus", p.Meter_Satatus);
                cmd.Parameters.AddWithValue("@id", p.Meter_ID);



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
        public bool Delete(BLL_MeterIssues p)
        {
            bool isSuccess = false;
        

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM Meter WHERE Meter_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.Meter_ID);



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
        #region Search Meter on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Meter WHERE Meter_MeterNum = '" + keywords + "'";
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
        #region TheSelectedMeter
        public DataTable SelectedMeter (int keywords)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT Meter_MeterNum, Meter_PK, MeterType_ManfName, MeterManf_Name, Meter_IssueDate," +
                    " Meter_Diameter, Meter_Origin, Meter_Model, Meter_Man, Meter_ChargeMode, Meter_Satatus FROM Meter " +
                    "INNER JOIN MeterType ON Meter.Meter_MeterTypeID = MeterType.MeterType_ID " +
                    "INNER JOIN MeterManf ON Meter.Meter_MeterManfID = MeterManf.MeterManf_ID WHERE Meter_MeterNum =" + keywords ;
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
        #endregion TheSelectedMeter

        #region get me type by code 
        public string[] SelectMeterNumbersNotInClientInfo()
        {
            List<string> meterNumbers = new List<string>();
            SqlConnection conn = db.Connect();

            try
            {
                // SQL Query to get Meter_MeterNum where Meter_ID not found in ClientInfo
                String sql = @"
            SELECT Meter_MeterNum 
            FROM Meter 
            WHERE NOT EXISTS (
                SELECT 1 
                FROM ClientInfo 
                WHERE ClientInfo.ClientInfo_MeterID = Meter.Meter_ID
            )";

                // For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Getting data from db
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string meterNum = Convert.ToInt32(reader["Meter_MeterNum"]).ToString(); ;
                    meterNumbers.Add(meterNum);
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

            return meterNumbers.ToArray();
        }
        #endregion

        #region get me type by code 
        public DataTable SelectMeteTableNotInClientInfo()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = db.Connect();

            try
            {
                // SQL Query to get Meter_MeterNum where Meter_ID not found in ClientInfo
                String sql = @"
            SELECT * 
            FROM Meter 
            WHERE NOT EXISTS (
                SELECT 1 
                FROM ClientInfo 
                WHERE ClientInfo.Meter_ID = Meter.Meter_ID
            )";

                // For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

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

        #region get GetMeterIDByMeterNum type by num 
        public int GetMeterIDByMeterNum(int meterNum)
        {
            int meterID = -1; // Default value in case the meterNum is not found

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get Meter_ID
                    string sql = "SELECT Meter_ID FROM Meter WHERE Meter_MeterNum=@meterNum";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@meterNum", meterNum);

                        // Execute the query and retrieve the Meter_ID
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            meterID = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return meterID;
        }
        #endregion
        #region get GetMeterIDByMeterNum type by num 
        public BLL_MeterIssues GetDataByMeterNum(int meterNum)
        {
            BLL_MeterIssues MeterIssues_Data = new BLL_MeterIssues();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get Meter_ID
                    string sql = "SELECT * FROM Meter WHERE Meter_MeterNum=@meterNum";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@meterNum", meterNum);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MeterIssues_Data.Meter_ID = Convert.ToInt32(reader["Meter_ID"]);
                                MeterIssues_Data.Meter_PK =reader["Meter_PK"].ToString(); 
                                MeterIssues_Data.Meter_MeterTypeID = Convert.ToInt32(reader["Meter_MeterTypeID"]);
                                MeterIssues_Data.Meter_MeterManfID = Convert.ToInt32(reader["Meter_MeterManfID"]);
                                MeterIssues_Data.Meter_CertMeterManf_TO_Meter =reader["Meter_CertMeterManf_TO_Meter"].ToString();
                                MeterIssues_Data.Meter_WaterCompID = Convert.ToInt32(reader["Meter_WaterCompID"]);
                                MeterIssues_Data.Meter_CertWaterComp_TO_Meter = reader["Meter_CertWaterComp_TO_Meter"].ToString();
                                MeterIssues_Data.Meter_SerialNumber = reader["Meter_SerialNumber"].ToString();
                                MeterIssues_Data.Meter_IssueDate = Convert.ToDateTime(reader["Meter_IssueDate"]);
                                MeterIssues_Data.Meter_MeterNum = Convert.ToInt32(reader["Meter_MeterNum"]);
                                MeterIssues_Data.Meter_Diameter = Convert.ToInt32(reader["Meter_Diameter"]);
                                MeterIssues_Data.Meter_Origin = Convert.ToInt32(reader["Meter_Origin"]);
                                MeterIssues_Data.Meter_Model = reader["Meter_Model"].ToString();
                                MeterIssues_Data.Meter_Man = reader["Meter_Man"].ToString();
                                MeterIssues_Data.Meter_ChargeMode = Convert.ToInt32(reader["Meter_ChargeMode"]);
                                MeterIssues_Data.Meter_Satatus = Convert.ToInt32(reader["Meter_Satatus"]);
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
            return MeterIssues_Data;
        }

        #endregion
    }
}
