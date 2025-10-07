using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using WaterMeter_id.BLL;

namespace WaterMeter_id
{
    public class DAL_CompanyData
    {
        Database db = new Database();

        #region Select Company Data
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
                String sql = "SELECT * FROM CompanyData";
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

        #region Update Company Data
        public bool Update(BLL_CompanyData p)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "UPDATE CompanyData SET Name='"+p.CompanyName+ "',LogoImage='"+p.image+ "',Phone='"+p.phone+ "',Address='"+p.address+ "',Email='"+p.email+ "',Website='"+p.website+ "' WHERE id='"+p.id+"'";
                SqlCommand cmd = new SqlCommand(sql, conn);



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

        #region Search for
        /*public DataTable SearchId(string cardTypeCode)
        {
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM CardType WHERE CardType_Code = '" + cardTypeCode + "'";
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
        }*/
        #endregion

        #region Insert Company Data in DB
        public bool Insert(BLL_CompanyData p)
        {
            bool isSuccess = false;

            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            try
            {
                String sql = "INSERT INTO CompanyData (Name,LogoImage,Phone,Address,Email,Website) VALUES (@name,@logo,@phone,@address,@email,@website)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", p.CompanyName);
                cmd.Parameters.AddWithValue("@logo", p.image);
                cmd.Parameters.AddWithValue("@phone", p.phone);
                cmd.Parameters.AddWithValue("@address", p.address);
                cmd.Parameters.AddWithValue("@email", p.email);
                cmd.Parameters.AddWithValue("@website", p.website);



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
    }
}
