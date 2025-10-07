using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaterMeter_id
{
    class DAL_Operator
    {
        //static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        Database db = new Database();
        private object column;

        public bool checkPassWord (ref BLL_Operator operatorData)
        {
            
            bool isSuccess = false;

             SqlConnection conn =db.Connect();
            if (conn == null)
            {
                MessageBox.Show("error when try connecting to server");

            }
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Operator WHERE OPerator_UaserName=@username AND Operator_Passwoed=@password";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@username", operatorData.UaserName);
                cmd.Parameters.AddWithValue("@password", operatorData.Passwoed);
                // cmd.Parameters.AddWithValue("@Privilage", l.->);


                    // Add a parameter to prevent SQL injection
                


              SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                adapter.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    isSuccess = true;
                 
                    operatorData.id =  Convert.ToInt32(dt.Rows[0]["Operator_id"]);
                          
                    operatorData.Name = dt.Rows[0]["Operator_Name"].ToString();
                    operatorData.UaserName = dt.Rows[0]["OPerator_UaserName"].ToString();
                    operatorData.Passwoed = dt.Rows[0]["Operator_Passwoed"].ToString();
                    operatorData.State = dt.Rows[0]["Operator_State"].ToString();
                    operatorData.Position = dt.Rows[0]["Operator_Position"].ToString();
                    operatorData.Privilage = Convert.ToInt32(dt.Rows[0]["Operator_Privilage"].ToString());
                    operatorData.Date = Convert.ToDateTime(dt.Rows[0]["Operator_Date"].ToString());
                        
                    
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
        #region Select Operator from Database
        public DataTable Select()
        {
            // Static Method to connect db
             SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Operator";
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
        #region Insert Operator in DB
        public bool Insert(BLL_Operator p)
        {
            bool isSuccess = false;

             SqlConnection conn =db.Connect();

            try
            {
                String sql = "INSERT INTO Operator (Operator_Name, Operator_Position, Operator_Passwoed, OPerator_UaserName,Operator_Privilage,Operator_State, Operator_Date) VALUES (@Name, @Position, @Passwoed, @UaserName,@Privilage,@State, @date)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name",           p.Name);
                cmd.Parameters.AddWithValue("@Position",       p.Position);
                cmd.Parameters.AddWithValue("@Passwoed",       p.Passwoed);
                cmd.Parameters.AddWithValue("@UaserName",      p.UaserName);
                cmd.Parameters.AddWithValue("@Privilage",      p.Privilage);
                cmd.Parameters.AddWithValue("@State",          p.State);
                cmd.Parameters.AddWithValue("@date",           p.Date);
                

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
        #region update Operator in db
        public bool Update(BLL_Operator p)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {
                string sql = "UPDATE Operator SET Operator_Name=@Name, Operator_Passwoed=@Passwoed, OPerator_UaserName=@UaserName, Operator_Privilage=@Privilage, Operator_State=@State, Operator_Date=@date WHERE Operator_id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name",           p.Name);
                cmd.Parameters.AddWithValue("@Position",       p.Position);
                cmd.Parameters.AddWithValue("@Passwoed",       p.Passwoed);
                cmd.Parameters.AddWithValue("@UaserName",      p.UaserName);
                cmd.Parameters.AddWithValue("@Privilage",      p.Privilage);
                cmd.Parameters.AddWithValue("@State",          p.State);
                cmd.Parameters.AddWithValue("@id",             p.id);
                cmd.Parameters.AddWithValue("@date",           p.Date);

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
        #region delete data from db
        public bool Delete(BLL_Operator p)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {
                string sql = "DELETE FROM Operator WHERE Operator_id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.id);

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
        #region Search Operator on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
             SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Operator WHERE Operator_Name LIKE '%" + keywords + "%' OR Operator_Position LIKE '%" + keywords + "%' OR Operator_State LIKE '%" + keywords + "%' ";
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
    }
}
