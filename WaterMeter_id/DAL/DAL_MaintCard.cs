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

namespace WaterMeter_id
{
    public class DAL_MaintCard
    {
        Database db = new Database();

        #region Select MaintCard from database
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
                String sql = "SELECT * FROM MaintCard";
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

        #region Insert MaintCard in DB
        public bool Insert(BLL_MaintCard p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO MaintCard (MaintCard_CardNID,MaintCard_IssueDate,MaintCard_MeterAction,MaintCard_ClientCateg,MaintCard_ActionDate,MaintCard_CategToapply,MaintCard_SwegToAPPly,MaintCard_TimeEffective,MaintCard_StartConsumerID,MaintCard_EndConsumerID,MaintCard_OFFTimeNum,MaintCard_PriceScheduleNum) " +
                                             "VALUES (@CardNID,@IssueDate,@MeterAction,@ClientCateg,@ActionDate,@CategToapply,@SwegToAPPly,@TimeEffective,@StartConsumerID,@EndConsumerID,@OFFTimeNum,@PriceScheduleNum)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CardNID", p.MaintCard_CardNID);
                cmd.Parameters.AddWithValue("@IssueDate", p.MaintCard_IssueDate);
                cmd.Parameters.AddWithValue("@MeterAction", p.MaintCard_MeterAction);
                cmd.Parameters.AddWithValue("@ClientCateg", p.MaintCard_ClientCateg);
                cmd.Parameters.AddWithValue("@ActionDate", p.MaintCard_ActionDate);
                cmd.Parameters.AddWithValue("@CategToapply", p.MaintCard_CategToapply);
                cmd.Parameters.AddWithValue("@SwegToAPPly", p.MaintCard_SwegToAPPly);
                cmd.Parameters.AddWithValue("@TimeEffective", p.MaintCard_TimeEffective);
                cmd.Parameters.AddWithValue("@StartConsumerID", p.MaintCard_StartConsumerID);
                cmd.Parameters.AddWithValue("@EndConsumerID", p.MaintCard_EndConsumerID);
                cmd.Parameters.AddWithValue("@OFFTimeNum", p.MaintCard_OFFTimeNum);
                cmd.Parameters.AddWithValue("@PriceScheduleNum", p.MaintCard_PriceScheduleNum);




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

        #region update MaintCard in db
        public bool Update(BLL_MaintCard p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.MaintCard_CardNID);

            if (table.Rows.Count >= 1)
            {
                p.MaintCard_ID = Convert.ToInt32(table.Rows[0]["MaintCard_ID"]);

            }
            else
            {
                return isSuccess;
            }
            SqlConnection conn = db.Connect();
            try
            {
                //@CardNID,@IssueDate,@MeterAction,@ClientCateg,@ActionDate,@CategToapply,@SwegToAPPly,@TimeEffective,@StartConsumerID,@EndConsumerID,@OFFTimeNum,@PriceScheduleNum
                string sql = "UPDATE MaintCard SET MaintCard_CardNID=@CardNID,MaintCard_IssueDate=@IssueDate,MaintCard_MeterAction=@MeterAction,MaintCard_ClientCateg=@ClientCateg,MaintCard_ActionDate=@ActionDate,MaintCard_CategToapply=@CategToapply,MaintCard_SwegToAPPly=@SwegToAPPly,MaintCard_TimeEffective=@TimeEffective,MaintCard_StartConsumerID=@StartConsumerID,MaintCard_EndConsumerID=@EndConsumerID,MaintCard_OFFTimeNum=@OFFTimeNum,MaintCard_PriceScheduleNum=@PriceScheduleNum WHERE MaintCard_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CardNID", p.MaintCard_CardNID);
                cmd.Parameters.AddWithValue("@IssueDate", p.MaintCard_IssueDate);
                cmd.Parameters.AddWithValue("@MeterAction", p.MaintCard_MeterAction);
                cmd.Parameters.AddWithValue("@ClientCateg", p.MaintCard_ClientCateg);
                cmd.Parameters.AddWithValue("@ActionDate", p.MaintCard_ActionDate);
                cmd.Parameters.AddWithValue("@CategToapply", p.MaintCard_CategToapply);
                cmd.Parameters.AddWithValue("@SwegToAPPly", p.MaintCard_SwegToAPPly);
                cmd.Parameters.AddWithValue("@TimeEffective", p.MaintCard_TimeEffective);
                cmd.Parameters.AddWithValue("@StartConsumerID", p.MaintCard_StartConsumerID);
                cmd.Parameters.AddWithValue("@EndConsumerID", p.MaintCard_EndConsumerID);
                cmd.Parameters.AddWithValue("@OFFTimeNum", p.MaintCard_OFFTimeNum);
                cmd.Parameters.AddWithValue("@PriceScheduleNum", p.MaintCard_PriceScheduleNum);
                cmd.Parameters.AddWithValue("@id", p.MaintCard_ID);

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
        public bool Delete(BLL_MaintCard p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.MaintCard_CardNID);

            if (table.Rows.Count > 0)
            {
                p.MaintCard_ID = Convert.ToInt32(table.Rows[0]["MaintCard_ID"]);

            }
            else
            {
                return isSuccess;
            }

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM MaintCard WHERE MaintCard_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.MaintCard_ID);



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

        #region get MaintCard by CardNID 
        public DataTable SearchId(int CardNID)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM MaintCard WHERE MaintCard_CardNID = '" + CardNID + "'";
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
