using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifyWaterCard.Entities;
using WaterMeter_id.BLL;

namespace WaterMeter_id
{
   public  class DAL_MeterState
    {
        Database db = new Database();
        public DataTable Select(int ClientIfo_ID)
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM MeterState WHERE MeterState_ClientInfoID ='"+ ClientIfo_ID + "'";
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
        #region Select MeterState from database
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
                String sql = "SELECT * FROM MeterState";
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

        #region Insert MeterState in DB
        public bool Insert(BLL_MeterState p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO MeterState (MeterState_ClientInfoID,MeterState_Date," +
                             "MeterState_Malfun1_Month,MeterState_Malfun1_Day,MeterState_Malfun1_Count,MeterState_Malfun2_Month," +
                             "MeterState_Malfun2_Day,MeterState_Malfun2_Count,MeterState_Malfun3_Month,MeterState_Malfun3_Day," +
                             "MeterState_Malfun3_Count,MeterState_Malfun4_Month,MeterState_Malfun4_Day,MeterState_Malfun4_Count," +
                             "MeterState_Malfun5_Month,MeterState_Malfun5_Day,MeterState_Malfun5_Count,MeterState_MeterStateDate1," +
                             "MeterState_MeterStateDate2,MeterState_MeterStateDate3,MeterState_MeterStateDate4,MeterState_MeterErrors) " +
                             "VALUES (@ClientInfoID,@Date,@Malfun1_Month,@Malfun1_Day,@Malfun1_Count,@Malfun2_Month," +
                             "@Malfun2_Day,@Malfun2_Count,@Malfun3_Month,@Malfun3_Day,@Malfun3_Count,@Malfun4_Month,@Malfun4_Day," +
                             "@Malfun4_Count,@Malfun5_Month,@Malfun5_Day,@Malfun5_Count,@MeterStateDate1,@MeterStateDate2,@MeterStateDate3," +
                             "@MeterStateDate4,@MeterErrors)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ClientInfoID", p.MeterState_ClientInfoID);
                cmd.Parameters.AddWithValue("@Date", p.MeterState_Date);
             
                cmd.Parameters.AddWithValue("@Malfun1_Month", p.MeterState_Malfun1_Month);
                cmd.Parameters.AddWithValue("@Malfun1_Day", p.MeterState_Malfun1_Day);
                cmd.Parameters.AddWithValue("@Malfun1_Count", p.MeterState_Malfun1_Count);
                cmd.Parameters.AddWithValue("@Malfun2_Month", p.MeterState_Malfun2_Month);
                cmd.Parameters.AddWithValue("@Malfun2_Day", p.MeterState_Malfun2_Day);
                cmd.Parameters.AddWithValue("@Malfun2_Count", p.MeterState_Malfun2_Count);
                cmd.Parameters.AddWithValue("@Malfun3_Month", p.MeterState_Malfun3_Month);
                cmd.Parameters.AddWithValue("@Malfun3_Day", p.MeterState_Malfun3_Day);
                cmd.Parameters.AddWithValue("@Malfun3_Count", p.MeterState_Malfun3_Count);
                cmd.Parameters.AddWithValue("@Malfun4_Month", p.MeterState_Malfun4_Month);
                cmd.Parameters.AddWithValue("@Malfun4_Day", p.MeterState_Malfun4_Day);
                cmd.Parameters.AddWithValue("@Malfun4_Count", p.MeterState_Malfun4_Count);
                cmd.Parameters.AddWithValue("@Malfun5_Month", p.MeterState_Malfun5_Month);
                cmd.Parameters.AddWithValue("@Malfun5_Day", p.MeterState_Malfun5_Day);
                cmd.Parameters.AddWithValue("@Malfun5_Count", p.MeterState_Malfun5_Count);
                cmd.Parameters.AddWithValue("@MeterStateDate1", p.MeterState_MeterStateDate1);
                cmd.Parameters.AddWithValue("@MeterStateDate2", p.MeterState_MeterStateDate2);
                cmd.Parameters.AddWithValue("@MeterStateDate3", p.MeterState_MeterStateDate3);
                cmd.Parameters.AddWithValue("@MeterStateDate4", p.MeterState_MeterStateDate4);
                cmd.Parameters.AddWithValue("@MeterErrors", p.MeterState_MeterErrors);





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
              //  MessageBox.Show(ex.Message);
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

        #region update MeterState in db
        public bool Update(BLL_MeterState p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.MeterState_ClientInfoID);

            if (table.Rows.Count >= 1)
            {
                p.MeterState_ID = Convert.ToInt32(table.Rows[0]["MeterState_ID"]);

            }
            else
            {
                return isSuccess;
            }
            SqlConnection conn = db.Connect();
            try
            {
                //(@ClientInfoID,@Date,@ReadingBy,@Malfun1_Month,@Malfun1_Day,@Malfun1_Count,@Malfun2_Month,@Malfun2_Day,@Malfun2_Count,@Malfun3_Month,@Malfun3_Day,@Malfun3_Count,@Malfun4_Month,@Malfun4_Day,@Malfun4_Count,@Malfun5_Month,@Malfun5_Day,@Malfun5_Count,@MeterStateDate1,@MeterStateDate2,@MeterStateDate3,@MeterStateDate4,@MeterErrors
                string sql = "UPDATE MeterState SET MeterState_ClientInfoID=@ClientInfoID,MeterState_Date=@Date,MeterState_ReadingBy=@ReadingBy," +
                             "MeterState_Malfun1_Month=@Malfun1_Month,MeterState_Malfun1_Day=@Malfun1_Day,MeterState_Malfun1_Count=@Malfun1_Count," +
                             "MeterState_Malfun2_Month=@Malfun2_Month,MeterState_Malfun2_Day=@Malfun2_Day,MeterState_Malfun2_Count=@Malfun2_Count," +
                             "MeterState_Malfun3_Month=@Malfun3_Month,MeterState_Malfun3_Day=@Malfun3_Day,MeterState_Malfun3_Count=@Malfun3_Count," +
                             "MeterState_Malfun4_Month=@Malfun4_Month,MeterState_Malfun4_Day=@Malfun4_Day,MeterState_Malfun4_Count=@Malfun4_Count," +
                             "MeterState_Malfun5_Month=@Malfun5_Month,MeterState_Malfun5_Day=@Malfun5_Day,MeterState_Malfun5_Count=@Malfun5_Count," +
                             "MeterState_MeterStateDate1=@MeterStateDate1,MeterState_MeterStateDate2=@MeterStateDate2,MeterState_MeterStateDate3=@MeterStateDate3," +
                             "MeterState_MeterStateDate4=@MeterStateDate4,MeterState_MeterErrors WHERE MeterState_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ClientInfoID", p.MeterState_ClientInfoID);
                cmd.Parameters.AddWithValue("@Date", p.MeterState_Date);
                cmd.Parameters.AddWithValue("@ReadingBy", p.MeterState_ReadingBy);
                cmd.Parameters.AddWithValue("@Malfun1_Month", p.MeterState_Malfun1_Month);
                cmd.Parameters.AddWithValue("@Malfun1_Day", p.MeterState_Malfun1_Day);
                cmd.Parameters.AddWithValue("@Malfun1_Count", p.MeterState_Malfun1_Count);
                cmd.Parameters.AddWithValue("@Malfun2_Month", p.MeterState_Malfun2_Month);
                cmd.Parameters.AddWithValue("@Malfun2_Day", p.MeterState_Malfun2_Day);
                cmd.Parameters.AddWithValue("@Malfun2_Count", p.MeterState_Malfun2_Count);
                cmd.Parameters.AddWithValue("@Malfun3_Month", p.MeterState_Malfun3_Month);
                cmd.Parameters.AddWithValue("@Malfun3_Day", p.MeterState_Malfun3_Day);
                cmd.Parameters.AddWithValue("@Malfun3_Count", p.MeterState_Malfun3_Count);
                cmd.Parameters.AddWithValue("@Malfun4_Month", p.MeterState_Malfun4_Month);
                cmd.Parameters.AddWithValue("@Malfun4_Day", p.MeterState_Malfun4_Day);
                cmd.Parameters.AddWithValue("@Malfun4_Count", p.MeterState_Malfun4_Count);
                cmd.Parameters.AddWithValue("@Malfun5_Month", p.MeterState_Malfun5_Month);
                cmd.Parameters.AddWithValue("@Malfun5_Day", p.MeterState_Malfun5_Day);
                cmd.Parameters.AddWithValue("@Malfun5_Count", p.MeterState_Malfun5_Count);
                cmd.Parameters.AddWithValue("@MeterStateDate1", p.MeterState_MeterStateDate1);
                cmd.Parameters.AddWithValue("@MeterStateDate2", p.MeterState_MeterStateDate2);
                cmd.Parameters.AddWithValue("@MeterStateDate3", p.MeterState_MeterStateDate3);
                cmd.Parameters.AddWithValue("@MeterStateDate4", p.MeterState_MeterStateDate4);
                cmd.Parameters.AddWithValue("@MeterErrors", p.MeterState_MeterErrors);
                cmd.Parameters.AddWithValue("@id", p.MeterState_ID);

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
        public bool Delete(BLL_MeterState p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.MeterState_ClientInfoID);

            if (table.Rows.Count > 0)
            {
                p.MeterState_ID = Convert.ToInt32(table.Rows[0]["MeterState_ID"]);

            }
            else
            {
                return isSuccess;
            }

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM MeterState WHERE MeterState_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.MeterState_ID);



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

        #region get MeterState by ClientInfoID
        public DataTable SearchId(int ClientInfoID)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM MeterState WHERE MeterState_ClientInfoID = '" + ClientInfoID + "'";
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
    }
}
