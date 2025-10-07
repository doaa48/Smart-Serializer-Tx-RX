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
    public class DAL_ClientInfo

    {
        Database db = new Database();
        #region select ClientInfo
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
                String sql = "SELECT * FROM ClientInfo";
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

#region insert data
        public bool Insert(BLL_ClientInfo p)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO ClientInfo " +
                             "(ClientInfo_ClientID, ClientInfo_IssueDate, ClientInfo_MeterID, ClientInfo_CardID, " +
                             "ClientInfo_PriceScheduleID, ClientInfo_OFFTimeID, ClientInfo_Activity, ClientInfo_SwGServices, " +
                             "ClientInfo_NumOFUnit, ClientInfo_UnityTypeID, ClientInfo_Category, ClientInfo_Address, " +
                             "ClientInfo_Desc, ClientInfo_SubscriberID, ClientInfo_CertWaterCompToMeterSubscriber) " +
                             "VALUES " +
                             "(@ClientInfo_ClientID, @ClientInfo_IssueDate, @ClientInfo_MeterID, @ClientInfo_CardID, " +
                             "@ClientInfo_PriceScheduleID, @ClientInfo_OFFTimeID, @ClientInfo_Activity, @ClientInfo_SwGServices, " +
                             "@ClientInfo_NumOFUnit, @ClientInfo_UnityTypeID, @ClientInfo_Category, @ClientInfo_Address, " +
                             "@ClientInfo_Desc, @ClientInfo_SubscriberID, @ClientInfo_CertWaterCompToMeterSubscriber)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ClientInfo_ClientID", p.ClientInfo_ClientID);
                cmd.Parameters.AddWithValue("@ClientInfo_IssueDate", p.ClientInfo_IssueDate);
                cmd.Parameters.AddWithValue("@ClientInfo_MeterID", p.ClientInfo_MeterID);
                cmd.Parameters.AddWithValue("@ClientInfo_CardID", p.ClientInfo_CardID);
                cmd.Parameters.AddWithValue("@ClientInfo_PriceScheduleID", p.ClientInfo_PriceScheduleID);
                cmd.Parameters.AddWithValue("@ClientInfo_OFFTimeID", p.ClientInfo_OFFTimeID);
                cmd.Parameters.AddWithValue("@ClientInfo_Activity", p.ClientInfo_Activity);
                cmd.Parameters.AddWithValue("@ClientInfo_SwGServices", p.ClientInfo_SwGServices);
                cmd.Parameters.AddWithValue("@ClientInfo_NumOFUnit", p.ClientInfo_NumOFUnit);
                cmd.Parameters.AddWithValue("@ClientInfo_UnityTypeID", p.ClientInfo_UnityTypeID);
                cmd.Parameters.AddWithValue("@ClientInfo_Category", p.ClientInfo_Category);
                cmd.Parameters.AddWithValue("@ClientInfo_Address", p.ClientInfo_Address);
                cmd.Parameters.AddWithValue("@ClientInfo_Desc", p.ClientInfo_Desc);
                cmd.Parameters.AddWithValue("@ClientInfo_SubscriberID", p.ClientInfo_SubscriberID);
                cmd.Parameters.AddWithValue("@ClientInfo_CertWaterCompToMeterSubscriber", p.CertWaterCompToMeterSubscriber);

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
        #region GetDataBySubscriberID
        public BLL_ClientInfo GetDataBySubscriberID(int subscriberID)
        {
            BLL_ClientInfo clientInfoData = new BLL_ClientInfo();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get data by ClientInfo_SubscriberID
                    string sql = "SELECT * FROM ClientInfo WHERE ClientInfo_SubscriberID=@subscriberID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@subscriberID", subscriberID);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfoData.ClientInfo_ID = Convert.ToInt32(reader["ClientInfo_ID"]);
                                clientInfoData.ClientInfo_ClientID = Convert.ToInt32(reader["ClientInfo_ClientID"]);
                                clientInfoData.ClientInfo_IssueDate = Convert.ToDateTime(reader["ClientInfo_IssueDate"]);
                                clientInfoData.ClientInfo_MeterID = Convert.ToInt32(reader["ClientInfo_MeterID"]);
                                clientInfoData.ClientInfo_CardID = reader["ClientInfo_CardID"] != DBNull.Value ? Convert.ToInt32(reader["ClientInfo_CardID"]) : 0;

                                clientInfoData.ClientInfo_PriceScheduleID = Convert.ToInt32(reader["ClientInfo_PriceScheduleID"]);
                                clientInfoData.ClientInfo_OFFTimeID = Convert.ToInt32(reader["ClientInfo_OFFTimeID"]);
                                clientInfoData.ClientInfo_Activity = Convert.ToInt32(reader["ClientInfo_Activity"]);
                                clientInfoData.ClientInfo_SwGServices = Convert.ToInt32(reader["ClientInfo_SwGServices"]);
                                clientInfoData.ClientInfo_NumOFUnit = Convert.ToInt32(reader["ClientInfo_NumOFUnit"]);
                                clientInfoData.ClientInfo_UnityTypeID = Convert.ToInt32(reader["ClientInfo_UnityTypeID"]);
                                clientInfoData.ClientInfo_Category = Convert.ToInt32(reader["ClientInfo_Category"]);
                                clientInfoData.ClientInfo_Address = reader["ClientInfo_Address"].ToString();
                                clientInfoData.ClientInfo_Desc = reader["ClientInfo_Desc"].ToString();
                                clientInfoData.ClientInfo_SubscriberID = Convert.ToInt32(reader["ClientInfo_SubscriberID"]);
                                clientInfoData.CertWaterCompToMeterSubscriber = reader["ClientInfo_CertWaterCompToMeterSubscriber"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                 //   MessageBox.Show(ex.Message);
                  //  throw; // Re-throw the exception for higher-level handling
                }
            }

            return clientInfoData;
        }
        #endregion
        public BLL_ClientInfo GetDataByClientinfoID(int clientinfoid)
        {
            BLL_ClientInfo clientInfoData = new BLL_ClientInfo();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get data by ClientInfo_CardID
                    string sql = "SELECT * FROM ClientInfo WHERE ClientInfo_ID=@clientinfoid";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@clientinfoid", clientinfoid);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfoData.ClientInfo_ID = Convert.ToInt32(reader["ClientInfo_ID"]);
                                clientInfoData.ClientInfo_ClientID = Convert.ToInt32(reader["ClientInfo_ClientID"]);
                                clientInfoData.ClientInfo_IssueDate = Convert.ToDateTime(reader["ClientInfo_IssueDate"]);
                                clientInfoData.ClientInfo_MeterID = Convert.ToInt32(reader["ClientInfo_MeterID"]);
                                clientInfoData.ClientInfo_CardID = Convert.ToInt32(reader["ClientInfo_CardID"]);
                                clientInfoData.ClientInfo_PriceScheduleID = Convert.ToInt32(reader["ClientInfo_PriceScheduleID"]);
                                clientInfoData.ClientInfo_OFFTimeID = Convert.ToInt32(reader["ClientInfo_OFFTimeID"]);
                                clientInfoData.ClientInfo_Activity = Convert.ToInt32(reader["ClientInfo_Activity"]);
                                clientInfoData.ClientInfo_SwGServices = Convert.ToInt32(reader["ClientInfo_SwGServices"]);
                                clientInfoData.ClientInfo_NumOFUnit = Convert.ToInt32(reader["ClientInfo_NumOFUnit"]);
                                clientInfoData.ClientInfo_UnityTypeID = Convert.ToInt32(reader["ClientInfo_UnityTypeID"]);
                                clientInfoData.ClientInfo_Category = Convert.ToInt32(reader["ClientInfo_Category"]);
                                clientInfoData.ClientInfo_Address = reader["ClientInfo_Address"].ToString();
                                clientInfoData.ClientInfo_Desc = reader["ClientInfo_Desc"].ToString();
                                clientInfoData.ClientInfo_SubscriberID = Convert.ToInt32(reader["ClientInfo_SubscriberID"]);
                                clientInfoData.CertWaterCompToMeterSubscriber = reader["CertWaterCompToMeterSubscriber"].ToString();
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

            return clientInfoData;
        }
        #region selectGetDataByCardID
        public BLL_ClientInfo GetDataByCardID(int cardID)
        {
            BLL_ClientInfo clientInfoData = new BLL_ClientInfo();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get data by ClientInfo_CardID
                    string sql = "SELECT * FROM ClientInfo WHERE ClientInfo_CardID=@cardID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@cardID", cardID);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfoData.ClientInfo_ID = Convert.ToInt32(reader["ClientInfo_ID"]);
                                clientInfoData.ClientInfo_ClientID = Convert.ToInt32(reader["ClientInfo_ClientID"]);
                                clientInfoData.ClientInfo_IssueDate = Convert.ToDateTime(reader["ClientInfo_IssueDate"]);
                                clientInfoData.ClientInfo_MeterID = Convert.ToInt32(reader["ClientInfo_MeterID"]);
                                clientInfoData.ClientInfo_CardID = Convert.ToInt32(reader["ClientInfo_CardID"]);
                                clientInfoData.ClientInfo_PriceScheduleID = Convert.ToInt32(reader["ClientInfo_PriceScheduleID"]);
                                clientInfoData.ClientInfo_OFFTimeID = Convert.ToInt32(reader["ClientInfo_OFFTimeID"]);
                                clientInfoData.ClientInfo_Activity = Convert.ToInt32(reader["ClientInfo_Activity"]);
                                clientInfoData.ClientInfo_SwGServices = Convert.ToInt32(reader["ClientInfo_SwGServices"]);
                                clientInfoData.ClientInfo_NumOFUnit = Convert.ToInt32(reader["ClientInfo_NumOFUnit"]);
                                clientInfoData.ClientInfo_UnityTypeID = Convert.ToInt32(reader["ClientInfo_UnityTypeID"]);
                                clientInfoData.ClientInfo_Category = Convert.ToInt32(reader["ClientInfo_Category"]);
                                clientInfoData.ClientInfo_Address = reader["ClientInfo_Address"].ToString();
                                clientInfoData.ClientInfo_Desc = reader["ClientInfo_Desc"].ToString();
                                clientInfoData.ClientInfo_SubscriberID = Convert.ToInt32(reader["ClientInfo_SubscriberID"]);
                                clientInfoData.CertWaterCompToMeterSubscriber = reader["CertWaterCompToMeterSubscriber"].ToString();
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

            return clientInfoData;
        }
        #endregion

        #region UpdateDataByClientInfoID
        public bool UpdateDataByClientInfoID(BLL_ClientInfo p)
        {
            bool isSuccess = false;

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to update data by ClientInfo_ID
                    string sql = "UPDATE ClientInfo SET " +
                                 "ClientInfo_ClientID = @ClientInfo_ClientID, " +
                                 "ClientInfo_IssueDate = @ClientInfo_IssueDate, " +
                                 "ClientInfo_MeterID = @ClientInfo_MeterID, " +
                                 "ClientInfo_CardID = @ClientInfo_CardID, " +
                                 "ClientInfo_PriceScheduleID = @ClientInfo_PriceScheduleID, " +
                                 "ClientInfo_OFFTimeID = @ClientInfo_OFFTimeID, " +
                                 "ClientInfo_Activity = @ClientInfo_Activity, " +
                                 "ClientInfo_SwGServices = @ClientInfo_SwGServices, " +
                                 "ClientInfo_NumOFUnit = @ClientInfo_NumOFUnit, " +
                                 "ClientInfo_UnityTypeID = @ClientInfo_UnityTypeID, " +
                                 "ClientInfo_Category = @ClientInfo_Category, " +
                                 "ClientInfo_Address = @ClientInfo_Address, " +
                                 "ClientInfo_Desc = @ClientInfo_Desc, " +
                                 "ClientInfo_SubscriberID = @ClientInfo_SubscriberID, " +
                                 "CertWaterCompToMeterSubscriber = @CertWaterCompToMeterSubscriber " +
                                 "WHERE ClientInfo_ID = @ClientInfo_ID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@ClientInfo_ClientID", p.ClientInfo_ClientID);
                        cmd.Parameters.AddWithValue("@ClientInfo_IssueDate", p.ClientInfo_IssueDate);
                        cmd.Parameters.AddWithValue("@ClientInfo_MeterID", p.ClientInfo_MeterID);
                        cmd.Parameters.AddWithValue("@ClientInfo_CardID", p.ClientInfo_CardID);
                        cmd.Parameters.AddWithValue("@ClientInfo_PriceScheduleID", p.ClientInfo_PriceScheduleID);
                        cmd.Parameters.AddWithValue("@ClientInfo_OFFTimeID", p.ClientInfo_OFFTimeID);
                        cmd.Parameters.AddWithValue("@ClientInfo_Activity", p.ClientInfo_Activity);
                        cmd.Parameters.AddWithValue("@ClientInfo_SwGServices", p.ClientInfo_SwGServices);
                        cmd.Parameters.AddWithValue("@ClientInfo_NumOFUnit", p.ClientInfo_NumOFUnit);
                        cmd.Parameters.AddWithValue("@ClientInfo_UnityTypeID", p.ClientInfo_UnityTypeID);
                        cmd.Parameters.AddWithValue("@ClientInfo_Category", p.ClientInfo_Category);
                        cmd.Parameters.AddWithValue("@ClientInfo_Address", p.ClientInfo_Address);
                        cmd.Parameters.AddWithValue("@ClientInfo_Desc", p.ClientInfo_Desc);
                        cmd.Parameters.AddWithValue("@ClientInfo_SubscriberID", p.ClientInfo_SubscriberID);
                        cmd.Parameters.AddWithValue("@CertWaterCompToMeterSubscriber", p.CertWaterCompToMeterSubscriber);
                        cmd.Parameters.AddWithValue("@ClientInfo_ID", p.ClientInfo_ID);

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return isSuccess;
        }
        #endregion
        #region select max client info_id
        public int GetMaxClientInfoID()
        {
            int maxClientInfoID = 0;
            SqlConnection conn = db.Connect();
            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get the overall maximum ClientInfo_ID
                string sql = "SELECT MAX(ClientInfo_ID) AS MaxClientInfoID FROM ClientInfo";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0 && dt.Rows[0]["MaxClientInfoID"] != DBNull.Value)
                {
                    // Get the maximum ClientInfo_ID as an integer value
                    maxClientInfoID = Convert.ToInt32(dt.Rows[0]["MaxClientInfoID"]);
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

            return maxClientInfoID;
        }
        #endregion

        #region select max subscriber ID 
        public int GetMaxSubscriberID()
        {
            int maxClientInfoID = 0;
            SqlConnection conn = db.Connect();
            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get the overall maximum ClientInfo_ID
                string sql = "SELECT MAX(ClientInfo_SubscriberID) AS MaxSubscriberID FROM ClientInfo";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0 && dt.Rows[0]["MaxSubscriberID"] != DBNull.Value)
                {
                    // Get the maximum ClientInfo_ID as an integer value
                    maxClientInfoID = Convert.ToInt32(dt.Rows[0]["MaxSubscriberID"]);
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

            return maxClientInfoID;
        }



        #endregion

        public int GetMINSubscriberID()
        {
            int minClientInfoID = 0;
            SqlConnection conn = db.Connect();
            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get the overall maximum ClientInfo_ID
                string sql = "SELECT MIN(ClientInfo_SubscriberID) AS MaxSubscriberID FROM ClientInfo";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0 && dt.Rows[0]["MaxSubscriberID"] != DBNull.Value)
                {
                    // Get the maximum ClientInfo_ID as an integer value
                    minClientInfoID = Convert.ToInt32(dt.Rows[0]["MaxSubscriberID"]);
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

            return minClientInfoID;
        }

    }
}

