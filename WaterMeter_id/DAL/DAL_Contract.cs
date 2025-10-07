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
using WaterMeter_id.BLL;

namespace WaterMeter_id.DAL
{
    class DAL_Contract
    {
        Database db = new Database();

        #region Select Contract from database
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
                String sql = "SELECT * FROM Contract";
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
        public bool Insert(BLL_Contract p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO Contract (Contract_ClientInfoID,Contract_StartDate,Contract_EndDate,Contract_Activity,Contract_TotalPrice,Contract_MeterID,Contract_CardID,Contract_OperatorID) " +
                                             "VALUES (@ClientInfoID,@StartDate,@EndDate,@Activity,@TotalPrice,@MeterID,@CardID,@OperatorID)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ClientInfoID", p.Contract_ClientInfoID);
                cmd.Parameters.AddWithValue("@StartDate", p.Contract_StartDate);
                cmd.Parameters.AddWithValue("@EndDate", p.Contract_EndDate);
                cmd.Parameters.AddWithValue("@Activity", p.Contract_Activity);
                cmd.Parameters.AddWithValue("@TotalPrice", p.Contract_TotalPrice);
                cmd.Parameters.AddWithValue("@MeterID", p.Contract_MeterID);
                cmd.Parameters.AddWithValue("@CardID", p.Contract_CardID);
                cmd.Parameters.AddWithValue("@OperatorID", p.Contract_OperatorID);



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

        #region update Contract in db
        public bool Update(BLL_Contract p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.Contract_ClientInfoID);

            if (table.Rows.Count >= 1)
            {
                p.Contract_ID = Convert.ToInt32(table.Rows[0]["Contract_ID"]);

            }
            else
            {
                return isSuccess;
            }
            SqlConnection conn = db.Connect();
            try
            {
                //@CardId, @IssueDate, @MeterAction, @RestDate, @TimeEffective, @startCustId, @endCustId
                string sql = "UPDATE Contract SET Contract_ClientInfoID=@ClientInfoID,Contract_StartDate=@StartDate,Contract_EndDate=@EndDate,Contract_Activity=@Activity,Contract_TotalPrice=@TotalPrice,Contract_MeterID=@MeterID,Contract_CardID=@CardID,Contract_OperatorID=@OperatorID WHERE Contract_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ClientInfoID", p.Contract_ClientInfoID);
                cmd.Parameters.AddWithValue("@StartDate", p.Contract_StartDate);
                cmd.Parameters.AddWithValue("@EndDate", p.Contract_EndDate);
                cmd.Parameters.AddWithValue("@Activity", p.Contract_Activity);
                cmd.Parameters.AddWithValue("@TotalPrice", p.Contract_TotalPrice);
                cmd.Parameters.AddWithValue("@MeterID", p.Contract_MeterID);
                cmd.Parameters.AddWithValue("@CardID", p.Contract_CardID);
                cmd.Parameters.AddWithValue("@OperatorID", p.Contract_OperatorID);
                cmd.Parameters.AddWithValue("@id", p.Contract_ID);


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
        public bool Delete(BLL_Contract p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.Contract_ClientInfoID);

            if (table.Rows.Count > 0)
            {
                p.Contract_ID = Convert.ToInt32(table.Rows[0]["Contract_ID"]);

            }
            else
            {
                return isSuccess;
            }

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM Contract WHERE Contract_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.Contract_ID);



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
                String sql = "SELECT * FROM Contract WHERE Contract_ClientInfoID = '" + ClientInfoID + "'";
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
