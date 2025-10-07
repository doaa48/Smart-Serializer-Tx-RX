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
    public class DAL_Aggregation
    {
        Database db = new Database();

        #region Select Aggregation from Database
        public DataTable Select()
        {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT Aggregation.Aggregation_GetwayID,Aggregation.Aggregation_ID, Aggregation.Aggregation_Number, Aggregation.Aggregation_Serial,Getway.Getway_Number, Aggregation.Aggregation_Description FROM Aggregation INNER JOIN Getway ON Getway.Getway_ID=Aggregation.Aggregation_GetwayID";
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

        #region Get getway Id from its num 
        public int getwaynum(int getwaynum)
        {

            int getwayID = 0;
            SqlConnection conn = db.Connect();

            try
            {
                string Sql = "SELECT Getway_ID FROM Getway WHERE Getway_Number=@num";

                SqlCommand cmd = new SqlCommand(Sql, conn);

                cmd.Parameters.AddWithValue("@num", getwaynum);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        getwayID = Convert.ToInt32(reader["Getway_ID"]);
                    }
                    else
                    {
                        getwayID = 0;

                    }
                }
            }catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message);
            } finally { conn.Close(); }

            return getwayID;
        }
        #endregion

        #region Get getway Id from its num 
        public int getwayid(int getwayid)
        {

            int getwayID = 0;
            SqlConnection conn = db.Connect();

            try
            {
                string Sql = "SELECT Getway_Number FROM Getway WHERE Getway_ID=@num";

                SqlCommand cmd = new SqlCommand(Sql, conn);

                cmd.Parameters.AddWithValue("@num", getwayid);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        getwayID = Convert.ToInt32(reader["Getway_Number"]);
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

        #region Insert Aggregation in DB
        public bool Insert(BLL_Aggregation p)
        {
            bool isSuccess = false;

            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO Aggregation (Aggregation_GetwayID,Aggregation_Number,Aggregation_Serial,Aggregation_Description) " +
                             "VALUES (@GetwayID,@Number,@Serial,@Des)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                int getwayid = getwaynum(p.Aggregation_GetwayID);
                cmd.Parameters.AddWithValue("@GetwayID", getwayid);
                cmd.Parameters.AddWithValue("@Number", p.Aggregation_Number);
                cmd.Parameters.AddWithValue("@Serial", p.Aggregation_Serial);
                cmd.Parameters.AddWithValue("@Des", p.Aggregation_Description);



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

        #region update Aggregation in db
        public bool Update(BLL_Aggregation p)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE Aggregation SET Aggregation_GetwayID=@GetwayID,Aggregation_Number=@Number,Aggregation_Serial=@Serial,Aggregation_Description=@Des WHERE Aggregation_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                int getwayid = getwaynum(p.Aggregation_GetwayID);
                cmd.Parameters.AddWithValue("@GetwayID", getwayid);
                cmd.Parameters.AddWithValue("@Number", p.Aggregation_Number);
                cmd.Parameters.AddWithValue("@Serial", p.Aggregation_Serial);
                cmd.Parameters.AddWithValue("@Des", p.Aggregation_Description);
                cmd.Parameters.AddWithValue("@id", p.Aggregation_ID);

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
        public bool Delete(BLL_Aggregation p)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM Aggregation WHERE Aggregation_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.Aggregation_ID);

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

        #region Get GetwayID 
        public DataTable GetGetwayID()
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
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        #endregion

        #region Get Aggregation from Num
        public BLL_Aggregation getGetwayData(int num)
        {
            BLL_Aggregation bLL_Aggregation = new BLL_Aggregation();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    string SqL = "SELECT Aggregation.Aggregation_GetwayID,Aggregation.Aggregation_ID, Aggregation.Aggregation_Number, Aggregation.Aggregation_Serial,Getway.Getway_Number, Aggregation.Aggregation_Description FROM Aggregation" +
                        " INNER JOIN Getway ON Getway.Getway_ID=Aggregation.Aggregation_GetwayID" +
                        " WHERE Aggregation_Number=@num";

                    using (SqlCommand cmd = new SqlCommand(SqL, conn))
                    {
                        cmd.Parameters.AddWithValue("@num", num); // Add parameter to the query

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int gitwaynum = getwayid(Convert.ToInt32(reader["Aggregation_GetwayID"]));
                                bLL_Aggregation.Aggregation_ID = Convert.ToInt32(reader["Aggregation_ID"]);
                                bLL_Aggregation.Aggregation_GetwayID = gitwaynum;
                                bLL_Aggregation.Aggregation_Number = Convert.ToInt32(reader["Aggregation_Number"]);
                                bLL_Aggregation.Aggregation_Serial = Convert.ToInt32(reader["Aggregation_Serial"]);
                                bLL_Aggregation.Aggregation_Description = reader["Aggregation_Description"].ToString();
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
            return bLL_Aggregation;
        }

        public int[] getAllAggregationNumbersFromGetwayData(int num)
         {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                string sqL = "SELECT Aggregation.Aggregation_Number FROM Aggregation" +
                                      " INNER JOIN Getway ON Getway.Getway_ID=Aggregation.Aggregation_GetwayID" +
                                      " WHERE Getway_Number=@num";                //For executing Command
                SqlCommand cmd = new SqlCommand(sqL, conn);
                cmd.Parameters.AddWithValue("@num", num);

                //Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //db connection open
                //conn.Open();
                //fill data in dataTable
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
             //   MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            // Extract Aggregation_Numbers from DataTable into an array
            int[] aggregationNumbers = new int[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                aggregationNumbers[i] = Convert.ToInt32(dt.Rows[i]["Aggregation_Number"]);
            }

            return aggregationNumbers;
        }


        #endregion
    }
}
