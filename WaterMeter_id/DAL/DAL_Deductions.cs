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
    public class DAL_Deductions
    {
        //  static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        Database db = new Database();
        #region Select Deductions from Database
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
                String sql = "SELECT * FROM Deductions";
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
        #region Insert Deductions in DB
        public bool Insert(BLL_Deductions p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO Deductions (Deductions_ClientInfoID,Deductions_Date,Deductions_MonthFees,Deductions_AppDate,Deductions_Month) VALUES (@Deductions_ClientInfoID,@Deductions_Date,@Deductions_MonthFees,@Deductions_AppDate,@Deductions_Month)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Deductions_ClientInfoID", p.Deductions_ClientInfoID);
                cmd.Parameters.AddWithValue("@Deductions_Date", p.Deductions_Date);
                cmd.Parameters.AddWithValue("@Deductions_MonthFees", p.Deductions_MonthFees);
                cmd.Parameters.AddWithValue("@Deductions_AppDate", p.Deductions_AppDate);
                cmd.Parameters.AddWithValue("@Deductions_Month", p.Deductions_Month);

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
        #region update Deductions in db
        public bool Update(BLL_Deductions p)
        {
            bool isSuccess = false;

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE Deductions SET Deductions_ClientInfoID=@Deductions_ClientInfoID,Deductions_Date=@Deductions_Date,Deductions_MonthFees=@Deductions_MonthFees,Deductions_AppDate=@Deductions_AppDate,Deductions_Month=@Deductions_Month WHERE Deductions_ID=@Deductions_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
              
                cmd.Parameters.AddWithValue("@Deductions_ClientInfoID", p.Deductions_ClientInfoID);
                cmd.Parameters.AddWithValue("@Deductions_Date", p.Deductions_Date);
                cmd.Parameters.AddWithValue("@Deductions_MonthFees", p.Deductions_MonthFees);
                cmd.Parameters.AddWithValue("@Deductions_AppDate", p.Deductions_AppDate);
                cmd.Parameters.AddWithValue("@Deductions_Month", p.Deductions_Month);

                cmd.Parameters.AddWithValue("@Deductions_ID", p.Deductions_ID);

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
        public bool Delete(BLL_Deductions p)
        {
            bool isSuccess = false;


            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM Deductions WHERE Deductions_ID=@Deductions_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Deductions_ID", p.Deductions_ID);



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
                String sql = "SELECT * FROM Deductions WHERE Client_FullName = '" + keywords + "' ";
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
                String sql = "SELECT * FROM Deductions WHERE Deductions_ID = '" + ClientID + "'";
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
                String sql = "SELECT MAX(Deductions_ID) FROM Deductions";
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
