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
    public class DAL_Client

    {
        //  static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        Database db = new Database();
        #region Select Client from Database
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
                String sql = "SELECT * FROM Client";
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
        #region Insert Client in DB
        public bool Insert(BLL_Client p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO Client (Client_FullName, Client_Number, Client_NationID,Client_phone,Client_Email) VALUES (@Client_FullName, @Client_Number, @Client_NationID,@Client_phone,@Client_Email)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Client_FullName", p.Client_FullName);
                cmd.Parameters.AddWithValue("@Client_Number"  , p.Client_Number);
                cmd.Parameters.AddWithValue("@Client_NationID", p.Client_NationID);
                cmd.Parameters.AddWithValue("@Client_phone"   , p.Client_phone);
                cmd.Parameters.AddWithValue("@Client_Email"   , p.Client_Email);

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
        #region update Client in db
        public bool Update(BLL_Client p)
        {
            bool isSuccess = false;

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE Client SET Client_FullName=@Client_FullName, Client_Number=@Client_Number, Client_NationID=@Client_NationID , Client_phone=@Client_phone , Client_Email=@Client_Email WHERE Client_ID=@Client_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Client_FullName", p.Client_FullName);
                cmd.Parameters.AddWithValue("@Client_Number", p.Client_Number);
                cmd.Parameters.AddWithValue("@Client_NationID", p.Client_NationID);
                cmd.Parameters.AddWithValue("@Client_phone", p.Client_phone);
                cmd.Parameters.AddWithValue("@Client_Email", p.Client_Email);

                cmd.Parameters.AddWithValue("@Client_ID", p.Client_ID);

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
        public bool Delete(BLL_Client p)
        {
            bool isSuccess = false;


            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM Client WHERE Client_NationID=@Client_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Client_ID", p.Client_NationID);



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
        #region Search UnitType on db usingKeywords
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
                String sql = "SELECT * FROM Client WHERE Client_FullName = '" + keywords + "' ";
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
        #region get card type by code 
        public DataTable SearchId(int ClientID)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Client WHERE Client_ID = '" + ClientID + "'";
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
        #region Select max id
        public int GetMaxClientId()
        {
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            int maxClientId = 0; // Initialize with a default value, in case no records are found

            try
            {
                String sql = "SELECT MAX(Client_ID) FROM Client";
                SqlCommand cmd = new SqlCommand(sql, conn);
               

                // ExecuteScalar is used to retrieve a single value from a query
                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    maxClientId = Convert.ToInt32(result);
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

            return maxClientId;
        }
        #endregion
        #region GetClientID UnitType on db usingKeywords
        public int GetClientID(int clientNumber)
        {
            int clientID = -1; // Default value in case the clientNumber is not found

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get Client_ID
                    string sql = "SELECT Client_ID FROM Client WHERE Client_Number=@clientNumber";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@clientNumber", clientNumber);

                        // Execute the query and retrieve the Client_ID
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            clientID = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return clientID;
        }
        #endregion

        #region select client by clint id 
        public BLL_Client GetClientDataBYID(int clientid)
        {
            BLL_Client Data = new BLL_Client();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get specific columns for the given Client_ID
                    string sql = "SELECT [Client_ID], [Client_FullName], [Client_Number], [Client_NationID], [Client_phone], [Client_Email] FROM Client WHERE Client_ID=@clientid";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@clientid", clientid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if there are rows returned
                            if (reader.Read())
                            {
                                // Map the database columns to the BLL_Client properties
                                Data.Client_ID = reader.GetInt32(0);
                                Data.Client_FullName = reader.IsDBNull(1) ? null : reader.GetString(1);
                                Data.Client_Number = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                                Data.Client_NationID = reader.IsDBNull(3) ? null : reader.GetString(3);
                                Data.Client_phone = reader.IsDBNull(4) ? null : reader.GetString(4);
                                Data.Client_Email = reader.IsDBNull(5) ? null : reader.GetString(5);
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

            return Data;
        }

        #endregion


        #region select client by clint Num 
        public BLL_Client GetClientDataBYClientNum(int clientNum)
        {
            BLL_Client Data = new BLL_Client();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get specific columns for the given Client_ID
                    string sql = "SELECT [Client_ID], [Client_FullName], [Client_Number], [Client_NationID], [Client_phone], [Client_Email] FROM Client WHERE Client_Number=@clientNum";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@clientNum", clientNum);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if there are rows returned
                            if (reader.Read())
                            {
                                // Map the database columns to the BLL_Client properties
                                Data.Client_ID = reader.GetInt32(0);
                                Data.Client_FullName = reader.IsDBNull(1) ? null : reader.GetString(1);
                                Data.Client_Number = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                                Data.Client_NationID = reader.IsDBNull(3) ? null : reader.GetString(3);
                                Data.Client_phone = reader.IsDBNull(4) ? null : reader.GetString(4);
                                Data.Client_Email = reader.IsDBNull(5) ? null : reader.GetString(5);
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

            return Data;
        }

        #endregion

        public  int GetMINClientNum()
        {
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            int MINClientNum = 0; // Initialize with a default value, in case no records are found

            try
            {
                String sql = "SELECT MIN(Client_Number) FROM Client";
                SqlCommand cmd = new SqlCommand(sql, conn);


                // ExecuteScalar is used to retrieve a single value from a query
                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    MINClientNum = Convert.ToInt32(result);
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

            return MINClientNum;
        }
        public int GetMAXClientNum()
        {
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            int maxClientNum = 0; // Initialize with a default value, in case no records are found

            try
            {
                String sql = "SELECT MAX(Client_Number) FROM Client";
                SqlCommand cmd = new SqlCommand(sql, conn);


                // ExecuteScalar is used to retrieve a single value from a query
                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    maxClientNum = Convert.ToInt32(result);
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

            return maxClientNum;
        }

    }
}
