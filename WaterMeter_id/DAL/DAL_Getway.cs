using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaterMeter_id
{
    public class DAL_Getway
    {
        Database db = new Database();

        #region Select Operator from Database
        public DataTable Select()
        {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Getway";
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
        public DataTable GetwayNumColomnSelect()
        {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT Getway_Number FROM Getway";
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
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return dt;
        }
        #region Insert Operator in DB
        public bool Insert(BLL_Getway p)
        {
            bool isSuccess = false;

            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO Getway (Getway_Number,Getway_Serial,Getway_Description) VALUES (@Number,@Serial,@Des)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Number", p.Getway_Number);
                cmd.Parameters.AddWithValue("@Serial", p.Getway_Serial);
                cmd.Parameters.AddWithValue("@Des", p.Getway_Description);
                


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
        public bool Update(BLL_Getway p)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE Getway SET Getway_Number=@Number,Getway_Serial=@Serial,Getway_Description=@Des WHERE Getway_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Number", p.Getway_Number);
                cmd.Parameters.AddWithValue("@Serial", p.Getway_Serial);
                cmd.Parameters.AddWithValue("@Des", p.Getway_Description);
                cmd.Parameters.AddWithValue("@id", p.Getway_ID);
                
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
        public bool Delete(BLL_Getway p)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM Getway WHERE Getway_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.Getway_ID);

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

        #region Get GetwayData from Num
        public BLL_Getway getGetwayData (int num)
        {
            BLL_Getway bLL_Getway = new BLL_Getway();

            using(SqlConnection conn = db.Connect())
            {
                try
                {
                    string SqL = "SELECT * FROM Getway WHERE Getway_Number=@num";

                    using(SqlCommand cmd = new SqlCommand(SqL, conn))
                    {
                        cmd.Parameters.AddWithValue("@num", num); // Add parameter to the query

                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bLL_Getway.Getway_ID = Convert.ToInt32(reader["Getway_ID"]);
                                bLL_Getway.Getway_Number = Convert.ToInt32(reader["Getway_Number"]);
                                bLL_Getway.Getway_Serial = Convert.ToInt32(reader["Getway_Serial"]);
                                bLL_Getway.Getway_Description = reader["Getway_Description"].ToString();
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
            return bLL_Getway;
        }
        #endregion
    }
}
