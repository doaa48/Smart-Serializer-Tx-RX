using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WaterMeter_id
{
    class DAL_Login
    {
        //static string to connect db
       Database db=new Database();
      
        public bool loginCheck(BLL_Login l)
        {
            bool isSuccess = false;

             SqlConnection conn =db.Connect();
            if (conn == null)
            {
                MessageBox.Show("error when try connecting to server");

            }
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Operator WHERE OPerator_UaserName=@username AND Operator_Passwoed=@password ";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@username",  l.username);
                cmd.Parameters.AddWithValue("@password",  l.password);
               // cmd.Parameters.AddWithValue("@Privilage", l.->);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                adapter.Fill(dt);
                if(dt.Rows.Count > 0)
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
    }
}
