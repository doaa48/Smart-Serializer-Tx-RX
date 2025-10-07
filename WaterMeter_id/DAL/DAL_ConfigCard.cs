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
    public class DAL_ConfigCard
    {
        Database db = new Database();

        #region Select CanfigCard from database
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
                String sql = "SELECT * FROM ConfigCard";
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

        #region Insert ConfigCard in DB
        public bool Insert(BLL_ConfigCard p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO ConfigCard (ConfigCard_CardID, ConfigCard_IssueDate, ConfigCard_MeterAction, ConfigCard_RestDate, ConfigCard_TimeEffective, ConfigCard_StartConsumerID, ConfigCard_EndConsumerID) " +
                                             "VALUES (@CardId, @IssueDate, @MeterAction, @RestDate, @TimeEffective, @startCustId, @endCustId)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CardId", p.ConfigCard_CardID);
                cmd.Parameters.AddWithValue("@IssueDate", p.ConfigCard_IssueDate);
                cmd.Parameters.AddWithValue("@MeterAction", p.ConfigCard_MeterAction);
                cmd.Parameters.AddWithValue("@RestDate", p.ConfigCard_RestDate);
                cmd.Parameters.AddWithValue("@TimeEffective", p.ConfigCard_TimeEffective);
                cmd.Parameters.AddWithValue("@startCustId", p.ConfigCard_StartConsumerID);
                cmd.Parameters.AddWithValue("@endCustId", p.ConfigCard_EndConsumerID);




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

        #region update ConfigCard in db
        public bool Update(BLL_ConfigCard p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.ConfigCard_CardID);

            if (table.Rows.Count >= 1)
            {
                p.ConfigCard_ID = Convert.ToInt32(table.Rows[0]["ConfigCard_ID"]);

            }
            else
            {
                return isSuccess;
            }
            SqlConnection conn = db.Connect();
            try
            {
                //@CardId, @IssueDate, @MeterAction, @RestDate, @TimeEffective, @startCustId, @endCustId
                string sql = "UPDATE ConfigCard SET ConfigCard_CardID=@CardId, ConfigCard_IssueDate=@IssueDate, ConfigCard_MeterAction=@MeterAction, ConfigCard_RestDate=@RestDate, ConfigCard_TimeEffective=@TimeEffective, ConfigCard_StartConsumerID=@startCustId, ConfigCard_EndConsumerID=@endCustId WHERE ConfigCard_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CardId", p.ConfigCard_CardID);
                cmd.Parameters.AddWithValue("@IssueDate", p.ConfigCard_IssueDate);
                cmd.Parameters.AddWithValue("@MeterAction", p.ConfigCard_MeterAction);
                cmd.Parameters.AddWithValue("@RestDate", p.ConfigCard_RestDate);
                cmd.Parameters.AddWithValue("@TimeEffective", p.ConfigCard_TimeEffective);
                cmd.Parameters.AddWithValue("@startCustId", p.ConfigCard_StartConsumerID);
                cmd.Parameters.AddWithValue("@endCustId", p.ConfigCard_EndConsumerID);
                cmd.Parameters.AddWithValue("@id", p.ConfigCard_ID);


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
        public bool Delete(BLL_ConfigCard p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.ConfigCard_CardID);

            if (table.Rows.Count > 0)
            {
                p.ConfigCard_ID = Convert.ToInt32(table.Rows[0]["ConfigCard_ID"]);

            }
            else
            {
                return isSuccess;
            }

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM ConfigCard WHERE ConfigCard_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.ConfigCard_ID);



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

        #region get ConfigCard by cardId 
        public DataTable SearchId(int ConfigCardID)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM ConfigCard WHERE ConfigCard_CardID = '" + ConfigCardID + "'";
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
