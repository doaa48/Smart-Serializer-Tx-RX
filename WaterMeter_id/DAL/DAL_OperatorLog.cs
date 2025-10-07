using System;
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
   
    class DAL_OperatorLog
    {
        Database db = new Database();
       //OperatorLogForm OperatorLogObj = new OperatorLogForm();
        #region Select OperatorLog from database
        public DataTable Select()
        {
            //Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM OperatorLog";
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
        
            public DataTable OperatorLogSelect()
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                //String sql = "SELECT * FROM OperatorLog";
            string sql = "SELECT  Operator.Operator_Name, OperatorLog.* " +
             "FROM OperatorLog " +
             "INNER JOIN Operator ON OperatorLog.OperatorLog_OperatorID = Operator.Operator_id";



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
        public DataTable SpecificSelect(string StartDateText, string EndDateText, string SelectOperatorNameComboBox, string SelectOperationComboBox)
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT ROW_NUMBER() OVER (ORDER BY OperatorLog.OperatorLog_ID) AS RowIndex, Operator.Operator_Name,OperatorLog_TableName,OperatorLog_Action,OperatorLog_Action_Disc,OperatorLog_Date  FROM OperatorLog   " +
                       "INNER JOIN Operator ON OperatorLog.OperatorLog_OperatorID = Operator.Operator_id WHERE ";
              
                if (SelectOperatorNameComboBox == "All Operator Names" && SelectOperationComboBox != "All Operations")
                {
                    sql += "OperatorLog_Action = '" + SelectOperationComboBox + "' AND " +
                         "OperatorLog_Date BETWEEN '" + StartDateText + "' AND '" + EndDateText + "'";
                }
                else if (SelectOperatorNameComboBox != "All Operator Names" && SelectOperationComboBox == "All Operations")
                {

             

               sql += " OperatorLog.OperatorLog_Date BETWEEN '" + StartDateText + "' AND '" + EndDateText + "' " +
                "AND Operator.Operator_Name = '" + SelectOperatorNameComboBox + "'";

                }
                else if (SelectOperatorNameComboBox == "All Operator Names" && SelectOperationComboBox == "All Operations")
                {

                    sql += "OperatorLog_Date BETWEEN '" + StartDateText + "' AND '" + EndDateText + "'";
                }
                else
                {



                    sql += " OperatorLog.OperatorLog_Action = '" + SelectOperationComboBox + "' " +
                   "AND OperatorLog.OperatorLog_Date BETWEEN '" + StartDateText + "' AND '" + EndDateText + "' " +
                   "AND Operator.Operator_Name = '" + SelectOperatorNameComboBox + "'";
                }
              
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

        #region Insert OperatorLog in DB

        public bool Insert(BLL_OperatorLog p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO OperatorLog (OperatorLog_OperatorID,OperatorLog_TableName,OperatorLog_TableNameID,OperatorLog_Action," +
                             "OperatorLog_Action_Disc, OperatorLog_Date) VALUES (@OperatorID,@TableName,@TableNameID,@Action,@Action_Disc,@Date)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@OperatorID", p.OperatorLog_OperatorID);
                cmd.Parameters.AddWithValue("@TableName", p.OperatorLog_TableName);
                cmd.Parameters.AddWithValue("@TableNameID", p.OperatorLog_TableNameID);
                cmd.Parameters.AddWithValue("@Action", p.OperatorLog_Action);
                cmd.Parameters.AddWithValue("@Action_Disc", p.OperatorLog_Action_Disc);
                cmd.Parameters.AddWithValue("@Date", p.OperatorLog_Date);




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

        #region update OperatorLog in db
        public bool Update(BLL_OperatorLog p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.OperatorLog_OperatorID);

            if (table.Rows.Count >= 1)
            {
                p.OperatorLog_ID = Convert.ToInt32(table.Rows[0]["OperatorLog_ID"]);

            }
            else
            {
                return isSuccess;
            }
            SqlConnection conn = db.Connect();
            try
            {
                //@CardId, @IssueDate, @MeterAction, @RestDate, @TimeEffective, @startCustId, @endCustId
                string sql = "UPDATE OperatorLog SET OperatorLog_OperatorID=@OperatorID,OperatorLog_TableName=@TableName," +
                    "OperatorLog_TableNameID=@TableNameID,OperatorLog_Action=@Action,OperatorLog_Action_Disc=@Action_Disc," +
                    "OperatorLog_Date=@Date WHERE OperatorLog_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@OperatorID", p.OperatorLog_OperatorID);
                cmd.Parameters.AddWithValue("@TableName", p.OperatorLog_TableName);
                cmd.Parameters.AddWithValue("@TableNameID", p.OperatorLog_TableNameID);
                cmd.Parameters.AddWithValue("@Action", p.OperatorLog_Action);
                cmd.Parameters.AddWithValue("@Action_Disc", p.OperatorLog_Action_Disc);
                cmd.Parameters.AddWithValue("@Date", p.OperatorLog_Date);
                cmd.Parameters.AddWithValue("@id", p.OperatorLog_ID);


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
        public bool Delete(BLL_OperatorLog p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.OperatorLog_OperatorID);

            if (table.Rows.Count > 0)
            {
                p.OperatorLog_ID = Convert.ToInt32(table.Rows[0]["OperatorLog_ID"]);

            }
            else
            {
                return isSuccess;
            }

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM OperatorLog WHERE OperatorLog_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.OperatorLog_ID);



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

        #region get OperatorLog by OperatorID 
        public DataTable SearchId(int OperatorID)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM OperatorLog WHERE OperatorLog_OperatorID = '" + OperatorID + "'";
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
