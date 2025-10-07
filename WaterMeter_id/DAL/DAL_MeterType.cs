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
    class DAL_MeterType
    {
        //static string method for db connection string 
        //static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        Database db = new Database();
        #region Select MeterType
        public DataTable Select()
        {
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT* FROM MeterType";       
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
               
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
        #region Insert new MeterType
        public bool Insert(BLL_MeterType c)
        {
            bool isSuccess = false;
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "INSERT INTO MeterType (MeterType_ManfName, MeterType_Code, MeterType_Model, MeterType_MeterVersionNum,MeterType_Desc) VALUES (@ManfName, @Code, @Model, @MeterVersionNum , @Desc)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManfName",             c.ManfName);
                cmd.Parameters.AddWithValue("@Code",                   c.Code);
                cmd.Parameters.AddWithValue("@Model",                 c.Model);
                cmd.Parameters.AddWithValue("@MeterVersionNum",       c.MeterVersionNum);
                cmd.Parameters.AddWithValue("@Desc",                  c.Desc);
               
                int rows = cmd.ExecuteNonQuery();
                //if the query is excute successfully then the value to rows will be greater than 0 else it will be less than 0
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
        #region update MeterType
        public bool Update(BLL_MeterType c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {


                string sql = "UPDATE MeterType SET MeterType_ManfName=@ManfName, MeterType_Code=@Code,MeterType_Model=@Model , MeterType_MeterVersionNum=@MeterVersionNum, MeterType_Desc=@Desc WHERE MeterType_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ManfName",       c.ManfName);
                cmd.Parameters.AddWithValue("@Code", c.Code);
                cmd.Parameters.AddWithValue("@Model",  c.Model);
                cmd.Parameters.AddWithValue("@MeterVersionNum",    c.MeterVersionNum);
                 cmd.Parameters.AddWithValue("@Desc",    c.Desc);
                cmd.Parameters.AddWithValue("@id",          c.id);

               

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
        public bool Delete(BLL_MeterType c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {
                string sql = "DELETE FROM MeterType WHERE MeterType_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", c.id);

               

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
        #region Search MeterType on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
             SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM MeterType WHERE  MeterType_Code = '" + keywords + "'  ";
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
