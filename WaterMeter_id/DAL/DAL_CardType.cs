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
    class DAL_CardType
    {
        //  static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        Database db = new Database();
        #region Select CardType from Database
        public DataTable Select()
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM CardType";
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
        #region Insert CardType in DB
        public bool Insert(BLL_CardType p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn =db.Connect();

            try
            {
                String sql = "INSERT INTO CardType (CardType_Code, CardType_ManfName, CardType_Manfversion, CardType_Desc) VALUES (@Code, @ManfName, @Manfversion, @Desc)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Code",        p.Code);
                cmd.Parameters.AddWithValue("@ManfName",    p.ManfName);
                cmd.Parameters.AddWithValue("@Manfversion", p.Manfversion);
                cmd.Parameters.AddWithValue("@Desc",        p.Desc);
               

             

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
        #region update CardType in db
        public bool Update(BLL_CardType p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.Code);

            if (table.Rows.Count >= 1)
            {
                p.id = Convert.ToInt32(table.Rows[0]["CardType_ID"]);

            }
            else
            {
                return isSuccess;
            }
            SqlConnection conn =db.Connect();
            try
            {
                string sql = "UPDATE CardType SET CardType_Code=@Code, CardType_ManfName=@ManfName, CardType_Manfversion=@Manfversion, CardType_Desc=@Desc WHERE CardType_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Code",        p.Code);
                cmd.Parameters.AddWithValue("@ManfName",    p.ManfName);
                cmd.Parameters.AddWithValue("@Manfversion", p.Manfversion);
                cmd.Parameters.AddWithValue("@Desc",        p.Desc);
                cmd.Parameters.AddWithValue("@id",          p.id);

                

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
        public bool Delete(BLL_CardType p)
        {
            bool isSuccess = false;
            DataTable table = SearchId(p.Code);

              if ( table.Rows.Count > 0)
              {
                      p.id = Convert.ToInt32(table.Rows[0]["CardType_ID"]);
                  
              }else
               {
                    return isSuccess ;
               }

            SqlConnection conn =db.Connect();
            try
            {
                string sql = "DELETE FROM CardType WHERE CardType_ID=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", p.id);

                

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
        #region Search CardType on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
           // SqlConnection conn = new SqlConnection(myconnstrng);
           SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM CardType WHERE CardType_Manfversion LIKE '%" + keywords + "%' OR CardType_Code LIKE '%" + keywords + "%' OR CardType_ManfName LIKE '%" + keywords + "%' ";
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
        public DataTable SearchId(string cardTypeCode)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM CardType WHERE CardType_Code = '" + cardTypeCode+"'" ;
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
