using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifyWaterCard.Entities;



namespace WaterMeter_id
{
    class DAL_CreditBalance
    {
        Database db = new Database();

        #region Select CreditBalance from database
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
                String sql = "SELECT * FROM CreditBalance";
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

        #region Insert Contract in DB
        public bool Insert(BLL_CreditBalance p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO CreditBalance (CreditBalance_ClientInfoID,CreditBalance_Date,CreditBalance_UsedMonthly1,CreditBalance_UsedMonthly2,CreditBalance_UsedMonthly3,CreditBalance_UsedMonthly4,CreditBalance_UsedMonthly5,CreditBalance_UsedMonthly6,CreditBalance_UsedMonthly7,CreditBalance_UsedMonthly8,CreditBalance_UsedMonthly9,CreditBalance_UsedMonthly10,CreditBalance_UsedMonthly11,CreditBalance_UsedMonthly12,CreditBalance_RemainCredit,CreditBalance_OverdraftCredit,CreditBalance_ConsumedCredit,CreditBalance_CumulativeCharges,CreditBalance_AppDate) " +
                                             "VALUES (@ClientInfoID,@Date,@UsedMonthly1,@UsedMonthly2,@UsedMonthly3,@UsedMonthly4,@UsedMonthly5,@UsedMonthly6,@UsedMonthly7,@UsedMonthly8,@UsedMonthly9,@UsedMonthly10,@UsedMonthly11,@UsedMonthly12,@RemainCredit,@OverdraftCredit,@ConsumedCredit,@CumulativeCharges,@AppDate)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                //@ClientInfoID,@Date,@ReadingBy,@UsedMonthly1,@UsedMonthly2,@UsedMonthly3,@UsedMonthly4,@UsedMonthly5,@UsedMonthly6,@UsedMonthly7,@UsedMonthly8,@UsedMonthly9,@UsedMonthly10,@UsedMonthly11,@UsedMonthly12,@RemainCredit,@OverdraftCredit,@ConsumedCredit,@CumulativeCharges,@AppDate
                cmd.Parameters.AddWithValue("@ClientInfoID", p.CreditBalance_ClientInfoID);
                cmd.Parameters.AddWithValue("@Date", p.CreditBalance_Date);
                cmd.Parameters.AddWithValue("@UsedMonthly1", p.CreditBalance_UsedMonthly1);
                cmd.Parameters.AddWithValue("@UsedMonthly2", p.CreditBalance_UsedMonthly2);
                cmd.Parameters.AddWithValue("@UsedMonthly3", p.CreditBalance_UsedMonthly3);
                cmd.Parameters.AddWithValue("@UsedMonthly4", p.CreditBalance_UsedMonthly4);
                cmd.Parameters.AddWithValue("@UsedMonthly5", p.CreditBalance_UsedMonthly5);
                cmd.Parameters.AddWithValue("@UsedMonthly6", p.CreditBalance_UsedMonthly6);
                cmd.Parameters.AddWithValue("@UsedMonthly7", p.CreditBalance_UsedMonthly7);
                cmd.Parameters.AddWithValue("@UsedMonthly8", p.CreditBalance_UsedMonthly8);
                cmd.Parameters.AddWithValue("@UsedMonthly9", p.CreditBalance_UsedMonthly9);
                cmd.Parameters.AddWithValue("@UsedMonthly10", p.CreditBalance_UsedMonthly10);
                cmd.Parameters.AddWithValue("@UsedMonthly11", p.CreditBalance_UsedMonthly11);
                cmd.Parameters.AddWithValue("@UsedMonthly12", p.CreditBalance_UsedMonthly12);
                cmd.Parameters.AddWithValue("@RemainCredit", p.CreditBalance_RemainCredit);
                cmd.Parameters.AddWithValue("@OverdraftCredit", p.CreditBalance_OverdraftCredit);
                cmd.Parameters.AddWithValue("@ConsumedCredit", p.CreditBalance_ConsumedCredit);
                cmd.Parameters.AddWithValue("@CumulativeCharges", p.CreditBalance_CumulativeCharges);
                cmd.Parameters.AddWithValue("@AppDate", p.CreditBalance_AppDate);



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
                //MessageBox.Show(ex.Message);
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

        #region update CreditBalance in db
        public bool Update(BLL_CreditBalance p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.CreditBalance_ClientInfoID);

            if (table.Rows.Count >= 1)
            {
                p.CreditBalance_ID = Convert.ToInt32(table.Rows[0]["CreditBalance_ID"]);

            }
            else
            {
                return isSuccess;
            }
            SqlConnection conn = db.Connect();
            try
            {
                //@ClientInfoID,@Date,@ReadingBy,@UsedMonthly1,@UsedMonthly2,@UsedMonthly3,@UsedMonthly4,@UsedMonthly5,@UsedMonthly6,@UsedMonthly7,@UsedMonthly8,@UsedMonthly9,@UsedMonthly10,@UsedMonthly11,@UsedMonthly12,@RemainCredit,@OverdraftCredit,@ConsumedCredit,@CumulativeCharges,@AppDate
                string sql = "UPDATE CreditBalance SET CreditBalance_ClientInfoID=@ClientInfoID,CreditBalance_Date=@Date,CreditBalance_ReadingBy=@ReadingBy,CreditBalance_UsedMonthly1=@UsedMonthly1,CreditBalance_UsedMonthly2=@UsedMonthly2,CreditBalance_UsedMonthly3=@UsedMonthly3,CreditBalance_UsedMonthly4=@UsedMonthly4,CreditBalance_UsedMonthly5=@UsedMonthly5,CreditBalance_UsedMonthly6=@UsedMonthly6,CreditBalance_UsedMonthly7=@UsedMonthly7,CreditBalance_UsedMonthly8=@UsedMonthly8,CreditBalance_UsedMonthly9=@UsedMonthly9,CreditBalance_UsedMonthly10=@UsedMonthly10,CreditBalance_UsedMonthly11=@UsedMonthly11,CreditBalance_UsedMonthly12=@UsedMonthly12,CreditBalance_RemainCredit=@RemainCredit,CreditBalance_OverdraftCredit=@OverdraftCredit,CreditBalance_ConsumedCredit=@ConsumedCredit,CreditBalance_CumulativeCharges=@CumulativeCharges,CreditBalance_AppDate=@AppDate WHERE CreditBalance_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ClientInfoID", p.CreditBalance_ClientInfoID);
                cmd.Parameters.AddWithValue("@Date", p.CreditBalance_Date);
                cmd.Parameters.AddWithValue("@ReadingBy", p.CreditBalance_ReadingBy);
                cmd.Parameters.AddWithValue("@UsedMonthly1", p.CreditBalance_UsedMonthly1);
                cmd.Parameters.AddWithValue("@UsedMonthly2", p.CreditBalance_UsedMonthly2);
                cmd.Parameters.AddWithValue("@UsedMonthly3", p.CreditBalance_UsedMonthly3);
                cmd.Parameters.AddWithValue("@UsedMonthly4", p.CreditBalance_UsedMonthly4);
                cmd.Parameters.AddWithValue("@UsedMonthly5", p.CreditBalance_UsedMonthly5);
                cmd.Parameters.AddWithValue("@UsedMonthly6", p.CreditBalance_UsedMonthly6);
                cmd.Parameters.AddWithValue("@UsedMonthly7", p.CreditBalance_UsedMonthly7);
                cmd.Parameters.AddWithValue("@UsedMonthly8", p.CreditBalance_UsedMonthly8);
                cmd.Parameters.AddWithValue("@UsedMonthly9", p.CreditBalance_UsedMonthly9);
                cmd.Parameters.AddWithValue("@UsedMonthly10", p.CreditBalance_UsedMonthly10);
                cmd.Parameters.AddWithValue("@UsedMonthly11", p.CreditBalance_UsedMonthly11);
                cmd.Parameters.AddWithValue("@UsedMonthly12", p.CreditBalance_UsedMonthly12);
                cmd.Parameters.AddWithValue("@RemainCredit", p.CreditBalance_RemainCredit);
                cmd.Parameters.AddWithValue("@OverdraftCredit", p.CreditBalance_OverdraftCredit);
                cmd.Parameters.AddWithValue("@ConsumedCredit", p.CreditBalance_ConsumedCredit);
                cmd.Parameters.AddWithValue("@CumulativeCharges", p.CreditBalance_CumulativeCharges);
                cmd.Parameters.AddWithValue("@AppDate", p.CreditBalance_AppDate);
                cmd.Parameters.AddWithValue("@id", p.CreditBalance_ID);


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
        public bool Delete(BLL_CreditBalance p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.CreditBalance_ClientInfoID);

            if (table.Rows.Count > 0)
            {
                p.CreditBalance_ID = Convert.ToInt32(table.Rows[0]["CreditBalance_ID"]);

            }
            else
            {
                return isSuccess;
            }

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM CreditBalance WHERE CreditBalance_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.CreditBalance_ID);



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

        #region get Contract by ClientInfoID 
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
                String sql = "SELECT * FROM CreditBalance WHERE CreditBalance_ClientInfoID = '" + ClientInfoID + "'";
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
