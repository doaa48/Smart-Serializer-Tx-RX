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
    public class DAL_History
    {
        Database db = new Database();
        public DataTable GetHistory(string StartDateDataTimePiker, string EndDateDataTimePicker)
        {
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT * FROM History WHERE History_Time BETWEEN '"+ StartDateDataTimePiker + "' AND '"+ EndDateDataTimePicker + "'";

                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //db connection open

                //fill data in dataTable
                adapter.Fill(dt);
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
    }
}
