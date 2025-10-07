using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterMeter_id.BLL;

namespace WaterMeter_id
{
   public  class DAL_RetrivalCard
    {
        Database db = new Database();

        #region Select RetrivalCard from database
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
                String sql = "SELECT * FROM RetrivalCard";
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

        #region Insert RetrivalCard in DB
        public bool Insert(BLL_RetrivalCard p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO RetrivalCard (RetrivalCard_CardID,RetrivalCard_IssueDate,RetrivalCard_RequiredData," +
                             "RetrivalCard_TimeEffective,RetrivalCard_StartConsumerID,RetrivalCard_EndConsumerID) VALUES (@CardID," +
                             "@IssueDate,@RequiredData,@TimeEffective,@StartConsumerID,@EndConsumerID)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CardID", p.RetrivalCard_CardID);
                cmd.Parameters.AddWithValue("@IssueDate", p.RetrivalCard_IssueDate);
                cmd.Parameters.AddWithValue("@RequiredData", p.RetrivalCard_RequiredData);
                cmd.Parameters.AddWithValue("@TimeEffective", p.RetrivalCard_TimeEffective);
                cmd.Parameters.AddWithValue("@StartConsumerID", p.RetrivalCard_StartConsumerID);
                cmd.Parameters.AddWithValue("@EndConsumerID", p.RetrivalCard_EndConsumerID);




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

        #region update RetrivalCard in db
        public bool Update(BLL_RetrivalCard p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.RetrivalCard_CardID);

            if (table.Rows.Count >= 1)
            {
                p.RetrivalCard_ID = Convert.ToInt32(table.Rows[0]["RetrivalCard_ID"]);

            }
            else
            {
                return isSuccess;
            }
            SqlConnection conn = db.Connect();
            try
            {
                //@CardId, @IssueDate, @MeterAction, @RestDate, @TimeEffective, @startCustId, @endCustId
                string sql = "UPDATE RetrivalCard SET RetrivalCard_CardID=@CardID,RetrivalCard_IssueDate=@IssueDate," +
                             "RetrivalCard_RequiredData=@RequiredData,RetrivalCard_TimeEffective=@TimeEffective," +
                             "RetrivalCard_StartConsumerID=@StartConsumerID,RetrivalCard_EndConsumerID=@EndConsumerID WHERE RetrivalCard_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CardID", p.RetrivalCard_CardID);
                cmd.Parameters.AddWithValue("@IssueDate", p.RetrivalCard_IssueDate);
                cmd.Parameters.AddWithValue("@RequiredData", p.RetrivalCard_RequiredData);
                cmd.Parameters.AddWithValue("@TimeEffective", p.RetrivalCard_TimeEffective);
                cmd.Parameters.AddWithValue("@StartConsumerID", p.RetrivalCard_StartConsumerID);
                cmd.Parameters.AddWithValue("@EndConsumerID", p.RetrivalCard_EndConsumerID);
                cmd.Parameters.AddWithValue("@id", p.RetrivalCard_ID);


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
        public bool Delete(BLL_RetrivalCard p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.RetrivalCard_CardID);

            if (table.Rows.Count > 0)
            {
                p.RetrivalCard_ID = Convert.ToInt32(table.Rows[0]["RetrivalCard_ID"]);

            }
            else
            {
                return isSuccess;
            }

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM RetrivalCard WHERE RetrivalCard_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.RetrivalCard_ID);



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

        #region get RetrivalCard by cardId 
        public DataTable SearchId(int RetrivalCardID)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM RetrivalCard WHERE RetrivalCard_ID = '" + RetrivalCardID + "'";
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
