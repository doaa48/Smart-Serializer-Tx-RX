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
    public class DAL_ChargeBasicInf
    {
        //  static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        Database db = new Database();
        public DataTable SelectSpecificTable(string StartDateDataTimePiker, string EndDateDataTimePicker)
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT " +
                                "C.Client_FullName, " +
                                "CI.ClientInfo_Category, " +
                                "CI.ClientInfo_NumOFUnit, " +
                                "CI.ClientInfo_SubscriberID, " +
                                "CB.ChargeBasicInf_Date, " +
                                "CB.ChargeBasicInf_ChargeAmount, " +
                                "CB.ChargeBasicInf_ChargeNo, " +
                                "CB.ChargeBasicInf_CutoffWarningLimit, " +
                                "CB.ChargeBasicInf_MaxOverdraftCredit, " +
                                "CB.ChargeBasicInf_ChargeDate " +
                            "FROM " +
                                "ChargeBasicInf CB " +
                            "INNER JOIN " +
                                "ClientInfo CI ON CB.ChargeBasicInf_ClientInfoID = CI.ClientInfo_ClientID " +
                            "INNER JOIN " +
                                "Client C ON CI.ClientInfo_ClientID = C.Client_ID ";
                sql += " WHERE CB.ChargeBasicInf_Date BETWEEN '" + StartDateDataTimePiker + "' AND '" + EndDateDataTimePicker + "'";

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
        #region Select ChargeBasicInf from Database
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
                String sql = "SELECT * FROM ChargeBasicInf";
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
        public DataTable Select(int ClientinfoID)
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM ChargeBasicInf WHERE ChargeBasicInf_ClientInfoID = '"+ ClientinfoID+"' ";
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
        #region Insert ChargeBasicInf in DB
        public bool Insert(BLL_ChargeBasicInf p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO ChargeBasicInf (ChargeBasicInf_ClientInfoID  , ChargeBasicInf_Date	, ChargeBasicInf_ChargeAmount , ChargeBasicInf_ChargeNo	,  ChargeBasicInf_CutoffWarningLimit, ChargeBasicInf_MaxOverdraftCredit ,  ChargeBasicInf_ChargeDate,  ChargeBasicInf_EnabledValvePeriod) VALUES ( "+
                                                        "@ChargeBasicInf_ClientInfoID  , @ChargeBasicInf_Date	, @ChargeBasicInf_ChargeAmount , @ChargeBasicInf_ChargeNo	,  @ChargeBasicInf_CutoffWarningLimit, @ChargeBasicInf_MaxOverdraftCredit ,  @ChargeBasicInf_ChargeDate,  @ChargeBasicInf_EnabledValvePeriod) ";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ChargeBasicInf_ClientInfoID", p.ChargeBasicInf_ClientInfoID       );
                cmd.Parameters.AddWithValue("@ChargeBasicInf_Date", p.ChargeBasicInf_Date				);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_ChargeAmount", p.ChargeBasicInf_ChargeAmount		);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_ChargeNo", p.ChargeBasicInf_ChargeNo			);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_CutoffWarningLimit", p.ChargeBasicInf_CutoffWarningLimit );
                cmd.Parameters.AddWithValue("@ChargeBasicInf_MaxOverdraftCredit", p.ChargeBasicInf_MaxOverdraftCredit );
                cmd.Parameters.AddWithValue("@ChargeBasicInf_ChargeDate",  p.ChargeBasicInf_ChargeDate		    ); 
                cmd.Parameters.AddWithValue("@ChargeBasicInf_EnabledValvePeriod", p.ChargeBasicInf_EnabledValvePeriod );

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
        public bool Update(BLL_ChargeBasicInf p)
        {
            bool isSuccess = false;

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE ChargeBasicInf SET  "+
                             "ChargeBasicInf_ClientInfoID        =@ChargeBasicInf_ClientInfoID       "+
                             "ChargeBasicInf_Date				 =@ChargeBasicInf_Date				 " +
                             "ChargeBasicInf_ChargeAmount		 =@ChargeBasicInf_ChargeAmount		 " +
                             "ChargeBasicInf_ChargeNo			 =@ChargeBasicInf_ChargeNo			 " +
                             "ChargeBasicInf_CutoffWarningLimit  =@ChargeBasicInf_CutoffWarningLimit " +
                             "ChargeBasicInf_MaxOverdraftCredit  =@ChargeBasicInf_MaxOverdraftCredit " +
                             "ChargeBasicInf_ChargeDate		     =@ChargeBasicInf_ChargeDate		 " +
                             "ChargeBasicInf_EnabledValvePeriod  =@ChargeBasicInf_EnabledValvePeriod " +
                             " WHERE ChargeBasicInf_ID=@ChargeBasicInf_ID ";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ChargeBasicInf_ClientInfoID", p.ChargeBasicInf_ClientInfoID);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_Date", p.ChargeBasicInf_Date);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_ChargeAmount", p.ChargeBasicInf_ChargeAmount);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_ChargeNo", p.ChargeBasicInf_ChargeNo);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_CutoffWarningLimit", p.ChargeBasicInf_CutoffWarningLimit);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_MaxOverdraftCredit", p.ChargeBasicInf_MaxOverdraftCredit);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_ChargeDate", p.ChargeBasicInf_ChargeDate);
                cmd.Parameters.AddWithValue("@ChargeBasicInf_EnabledValvePeriod", p.ChargeBasicInf_EnabledValvePeriod);


                cmd.Parameters.AddWithValue("@ChargeBasicInf_ID", p.ChargeBasicInf_ID);

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
        public bool Delete(BLL_ChargeBasicInf p)
        {
            bool isSuccess = false;


            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM ChargeBasicInf WHERE ChargeBasicInf_ID=@ChargeBasicInf_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ChargeBasicInf_ID", p.ChargeBasicInf_ID);



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
                String sql = "SELECT * FROM ChargeBasicInf WHERE Client_FullName = '" + keywords + "' ";
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
                String sql = "SELECT * FROM ChargeBasicInf WHERE ChargeBasicInf_ID = '" + ClientID + "'";
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
                String sql = "SELECT MAX(ChargeBasicInf_ID) FROM ChargeBasicInf";
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
    }
}
