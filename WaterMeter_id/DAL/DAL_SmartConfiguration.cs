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
    public class DAL_SmartConfiguration
    {
        Database db = new Database();

        #region SelectData
        public DataTable Select()
        {
            DataTable ConfigTable = null;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM SmartConfigurations ";
                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //For executing Command
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                ///condition
                if (adapter != null)
                {
                    ConfigTable = new DataTable();
                    //Getting data from db
                    adapter.Fill(ConfigTable);

                    if (ConfigTable.Rows.Count < 1)
                    {
                        ConfigTable = null;
                    }
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
            return ConfigTable;
        }
        #endregion
        #region SelectData1
        public BLL_SmartConfiguration SelectData()
        {
            BLL_SmartConfiguration BLL_SmartConfiguration_Data = null;

            // Using statement ensures proper disposal of resources
            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    string sql = "SELECT * FROM SmartConfigurations";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable ConfigTable = new DataTable();

                    adapter.Fill(ConfigTable);

                    // Check if any rows are returned
                    if (ConfigTable.Rows.Count > 0)
                    {
                        DataRow row = ConfigTable.Rows[0]; // Assuming you only need the first row
                        BLL_SmartConfiguration_Data = new BLL_SmartConfiguration();
                        // Map data to BLL_SmartConfiguration object properties
                        BLL_SmartConfiguration_Data.SmartConfigurations_ID = Convert.ToInt32(row["SmartConfigurations_ID"]);
                        BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable = Convert.ToInt32(row["SmartConfigurations_SecurityEnable"]);
                        BLL_SmartConfiguration_Data.SmartConfigurations_TimeOut = Convert.ToInt32(row["SmartConfigurations_TimeOut"]);
                        BLL_SmartConfiguration_Data.SmartConfigurations_Frequency = Convert.ToInt32(row["SmartConfigurations_Frequency"]);
                    }
                    else
                    {
                        BLL_SmartConfiguration_Data = null; // No data found, return null
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message); // Consider logging the error instead of displaying a message box
                }
            }

            return BLL_SmartConfiguration_Data;
        }

        #endregion
        #region Insert Data
        public bool Insert()
        {
            bool isSuccess = false;
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                // SQL Query to Get data from db
                String sql = "INSERT INTO SmartConfigurations (SmartConfigurations_SecurityEnable," +
                            "SmartConfigurations_TimeOut,SmartConfigurations_Frequency) VALUES ('"+0+"','"+0+"','"+0+"')";
                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                    //Getting data from db

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

        #region Update Data
        public bool Update(BLL_SmartConfiguration SmartTable)
        {
            bool isSuccess = false;

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE SmartConfigurations SET SmartConfigurations_SecurityEnable=@SecurityEnable, SmartConfigurations_TimeOut=@TimeOut, SmartConfigurations_Frequency=@Frequency WHERE SmartConfigurations_ID=@ID";


                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@SecurityEnable", SmartTable.SmartConfigurations_SecurityEnable);
                cmd.Parameters.AddWithValue("@TimeOut", SmartTable.SmartConfigurations_TimeOut);
                cmd.Parameters.AddWithValue("@Frequency", SmartTable.SmartConfigurations_Frequency);

                cmd.Parameters.AddWithValue("@ID", SmartTable.SmartConfigurations_ID);

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
    }
}
