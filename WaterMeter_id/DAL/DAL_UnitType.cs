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
    public class DAL_UnitType
    {
        //  static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        Database db = new Database();
        #region Select UnitType from Database
        public DataTable Select()
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            using (SqlConnection conn = db.Connect()) {
                try
                {
                    // SQL Query to Get data from db
                    String sql = "SELECT * FROM UnitType";
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
            }
            return dt;
        }
        #endregion
        #region Insert UnitType in DB
        public bool Insert(BLL_UnitType p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO UnitType (UnitType_Name, UnitType_Code, UnitType_Description) VALUES (@UnitType_Name, @UnitType_Code, @UnitType_Description)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@UnitType_Name", p.UnitType_Name);
                cmd.Parameters.AddWithValue("@UnitType_Code", p.UnitType_Code);
                cmd.Parameters.AddWithValue("@UnitType_Description", p.UnitType_Description);
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
        #region update UnitType in db
        public bool Update(BLL_UnitType p)
        {
            bool isSuccess = false;
          
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE UnitType SET UnitType_Name=@UnitType_Name, UnitType_Code=@UnitType_Code, UnitType_Description=@UnitType_Description  WHERE UnitType_ID=@UnitType_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@UnitType_Name", p.UnitType_Name);
                cmd.Parameters.AddWithValue("@UnitType_Code", p.UnitType_Code);
                cmd.Parameters.AddWithValue("@UnitType_Description", p.UnitType_Description);
                cmd.Parameters.AddWithValue("@UnitType_ID", p.UnitType_ID);



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
        public bool Delete(BLL_UnitType p)
        {
            bool isSuccess = false;
        

            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM UnitType WHERE UnitType_ID=@UnitType_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@UnitType_ID", p.UnitType_ID);



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
        #region Search UnitType on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM UnitType WHERE UnitType_Name = '" + keywords + "' ";
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
        #region get card type by code 
        public DataTable SearchId(int unitID)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM UnitType WHERE UnitType_ID = '" + unitID + "'";
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

        #region select SelectUnitTypeNames by code 
        public  string[]  SelectUnitTypeNames()
        {
            List<string> unitTypeNames = new List<string>();
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT UnitType_Name FROM UnitType";

                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string unitName = reader["UnitType_Name"].ToString();
                    unitTypeNames.Add(unitName);
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

            return unitTypeNames.ToArray();
        }
        #endregion

        #region select get unitID  by name 
        public int GetUnitTypeIDByUnitTypeName(string unitTypeName)
        {
            int unitTypeID = -1; // Default value in case the unitTypeName is not found

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get UnitType_ID
                    string sql = "SELECT UnitType_ID FROM UnitType WHERE UnitType_Name=@unitTypeName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@unitTypeName", unitTypeName);

                        // Execute the query and retrieve the UnitType_ID
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            unitTypeID = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return unitTypeID;
        }

        #endregion
    }
}
