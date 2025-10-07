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
    public class DAL_Offtimes
    {
        Database db = new Database();
        #region Select BLL_Offtimes
        public DataTable Select()
        {
            //creating db connection
            SqlConnection conn = db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT* FROM OFFTime";
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
        public bool Insert(BLL_Offtimes c)
        {
            bool isSuccess = false;
            //creating db connection
            SqlConnection conn = db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "INSERT INTO OFFTime " +
                    "(OFFTime_Name ,OFFTime_Code , OFFTime_IssueDate , OFFTime_StartDate ,OFFTime_WorkingDays ," +
                    "OFFTime_CutOffTime , OFFTime_GracePeriod , OFFTime_WorkStart ,OFFTime_WorkEnd ,OFFTime_Holiday1_Month ," +
                    " OFFTime_Holiday1_Day ,OFFTime_Holiday2_Month , OFFTime_Holiday2_Day , OFFTime_Holiday3_Month ,OFFTime_Holiday3_Day ," +
                    "OFFTime_Holiday4_Month ,OFFTime_Holiday4_Day , OFFTime_Holiday5_Month , OFFTime_Holiday5_Day , OFFTime_Holiday6_Month ," +
                    " OFFTime_Holiday6_Day , OFFTime_Holiday7_Month , OFFTime_Holiday7_Day , OFFTime_Holiday8_Month ,OFFTime_Holiday8_Day ," +
                    " OFFTime_Holiday9_Month , OFFTime_Holiday9_Day , OFFTime_Holiday10_Month ,OFFTime_Holiday10_Day , OFFTime_Holiday11_Month ," +
                    "OFFTime_Holiday11_Day ,OFFTime_Holiday12_Month ,OFFTime_Holiday12_Day ,OFFTime_Holiday13_Month ,OFFTime_Holiday13_Day ," +
                    " OFFTime_Holiday14_Month ,OFFTime_Holiday14_Day , OFFTime_Holiday15_Month ,OFFTime_Holiday15_Day ,OFFTime_Holiday16_Month ," +
                    "OFFTime_Holiday16_Day ,OFFTime_Holiday17_Month ,OFFTime_Holiday17_Day ,OFFTime_Holiday18_Month , OFFTime_Holiday18_Day ," +
                    "OFFTime_Holiday19_Month , OFFTime_Holiday19_Day ,OFFTime_Holiday20_Month ,OFFTime_Holiday20_Day ,OFFTime_Holiday21_Month ," +
                    "OFFTime_Holiday21_Day ,OFFTime_Holiday22_Month ,OFFTime_Holiday22_Day ,OFFTime_Holiday23_Month ,OFFTime_Holiday23_Day ," +
                    "OFFTime_Holiday24_Month , OFFTime_Holiday24_Day , OFFTime_Holiday25_Month , OFFTime_Holiday25_Day ) " +

                    "VALUES  (@OFFTime_Name, @OFFTime_Code, @OFFTime_IssueDate, @OFFTime_StartDate, @OFFTime_WorkingDays," +
                    "@OFFTime_CutOffTime , @OFFTime_GracePeriod , @OFFTime_WorkStart , @OFFTime_WorkEnd , @OFFTime_Holiday1_Month , " +
                    " @OFFTime_Holiday1_Day ,@OFFTime_Holiday2_Month , @OFFTime_Holiday2_Day , @OFFTime_Holiday3_Month ,@OFFTime_Holiday3_Day ," +
                    " @OFFTime_Holiday4_Month , @OFFTime_Holiday4_Day , @OFFTime_Holiday5_Month , @OFFTime_Holiday5_Day , @OFFTime_Holiday6_Month ," +
                    " @OFFTime_Holiday6_Day , @OFFTime_Holiday7_Month , @OFFTime_Holiday7_Day , @OFFTime_Holiday8_Month ,@OFFTime_Holiday8_Day ," +
                    " @OFFTime_Holiday9_Month , @OFFTime_Holiday9_Day , @OFFTime_Holiday10_Month ,@OFFTime_Holiday10_Day , @OFFTime_Holiday11_Month ," +
                    "@OFFTime_Holiday11_Day , @OFFTime_Holiday12_Month , @OFFTime_Holiday12_Day , @OFFTime_Holiday13_Month , @OFFTime_Holiday13_Day ," +
                    "  @OFFTime_Holiday14_Month , @OFFTime_Holiday14_Day ,  @OFFTime_Holiday15_Month , @OFFTime_Holiday15_Day , @OFFTime_Holiday16_Month ," +
                    " @OFFTime_Holiday16_Day , @OFFTime_Holiday17_Month , @OFFTime_Holiday17_Day , @OFFTime_Holiday18_Month ,  @OFFTime_Holiday18_Day ," +
                    " @OFFTime_Holiday19_Month ,  @OFFTime_Holiday19_Day , @OFFTime_Holiday20_Month , @OFFTime_Holiday20_Day , @OFFTime_Holiday21_Month ," +
                    " @OFFTime_Holiday21_Day , @OFFTime_Holiday22_Month , @OFFTime_Holiday22_Day , @OFFTime_Holiday23_Month , @OFFTime_Holiday23_Day ," +
                    " @OFFTime_Holiday24_Month ,  @OFFTime_Holiday24_Day ,  @OFFTime_Holiday25_Month ,  @OFFTime_Holiday25_Day )";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@OFFTime_Name ", c.OFFTime_Name);
                cmd.Parameters.AddWithValue("@OFFTime_Code ", c.OFFTime_Code);
                cmd.Parameters.AddWithValue("@OFFTime_IssueDate ", c.OFFTime_IssueDate);
                cmd.Parameters.AddWithValue("@OFFTime_StartDate ", c.OFFTime_StartDate);
                cmd.Parameters.AddWithValue("@OFFTime_WorkingDays ", c.OFFTime_WorkingDays);
                cmd.Parameters.AddWithValue("@OFFTime_CutOffTime ", c.OFFTime_CutOffTime);
                cmd.Parameters.AddWithValue("@OFFTime_GracePeriod ", c.OFFTime_GracePeriod);
                cmd.Parameters.AddWithValue("@OFFTime_WorkStart ", c.OFFTime_WorkStart);
                cmd.Parameters.AddWithValue("@OFFTime_WorkEnd ", c.OFFTime_WorkEnd);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday1_Month", c.OFFTime_Holiday1_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday1_Day ", c.OFFTime_Holiday1_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday2_Month", c.OFFTime_Holiday2_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday2_Day ", c.OFFTime_Holiday2_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday3_Month", c.OFFTime_Holiday3_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday3_Day ", c.OFFTime_Holiday3_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday4_Month", c.OFFTime_Holiday4_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday4_Day ", c.OFFTime_Holiday4_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday5_Month", c.OFFTime_Holiday5_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday5_Day ", c.OFFTime_Holiday5_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday6_Month", c.OFFTime_Holiday6_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday6_Day ", c.OFFTime_Holiday6_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday7_Month", c.OFFTime_Holiday7_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday7_Day ", c.OFFTime_Holiday7_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday8_Month", c.OFFTime_Holiday8_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday8_Day ", c.OFFTime_Holiday8_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday9_Month", c.OFFTime_Holiday9_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday9_Day ", c.OFFTime_Holiday9_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday10_Month", c.OFFTime_Holiday10_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday10_Day ", c.OFFTime_Holiday10_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday11_Month", c.OFFTime_Holiday11_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday11_Day ", c.OFFTime_Holiday11_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday12_Month", c.OFFTime_Holiday12_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday12_Day ", c.OFFTime_Holiday12_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday13_Month", c.OFFTime_Holiday13_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday13_Day ", c.OFFTime_Holiday13_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday14_Month", c.OFFTime_Holiday14_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday14_Day ", c.OFFTime_Holiday14_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday15_Month", c.OFFTime_Holiday15_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday15_Day ", c.OFFTime_Holiday15_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday16_Month", c.OFFTime_Holiday16_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday16_Day ", c.OFFTime_Holiday16_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday17_Month", c.OFFTime_Holiday17_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday17_Day ", c.OFFTime_Holiday17_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday18_Month", c.OFFTime_Holiday18_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday18_Day ", c.OFFTime_Holiday18_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday19_Month", c.OFFTime_Holiday19_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday19_Day ", c.OFFTime_Holiday19_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday20_Month", c.OFFTime_Holiday20_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday20_Day ", c.OFFTime_Holiday20_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday21_Month", c.OFFTime_Holiday21_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday21_Day ", c.OFFTime_Holiday21_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday22_Month", c.OFFTime_Holiday22_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday22_Day ", c.OFFTime_Holiday22_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday23_Month", c.OFFTime_Holiday23_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday23_Day ", c.OFFTime_Holiday23_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday24_Month", c.OFFTime_Holiday24_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday24_Day ", c.OFFTime_Holiday24_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday25_Month", c.OFFTime_Holiday25_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday25_Day ", c.OFFTime_Holiday25_Day);

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
        public bool Update(BLL_Offtimes c)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {


                string sql = "UPDATE OFFTime SET " +
                 "OFFTime_Name           =@OFFTime_Name           ," +
                "OFFTime_Code            =@OFFTime_Code           ," +
                "OFFTime_IssueDate       =@OFFTime_IssueDate      ," +
                "OFFTime_StartDate       =@OFFTime_StartDate      ," +
                "OFFTime_WorkingDays     =@OFFTime_WorkingDays    ," +
                "OFFTime_CutOffTime      =@OFFTime_CutOffTime     ," +
                "OFFTime_GracePeriod     =@OFFTime_GracePeriod    ," +
                "OFFTime_WorkStart       =@OFFTime_WorkStart      ," +
                "OFFTime_WorkEnd         =@OFFTime_WorkEnd        ," +
                "OFFTime_Holiday1_Month  =@OFFTime_Holiday1_Month ," +
                "OFFTime_Holiday1_Day    =@OFFTime_Holiday1_Day   ," +
                "OFFTime_Holiday2_Month  =@OFFTime_Holiday2_Month ," +
                "OFFTime_Holiday2_Day    =@OFFTime_Holiday2_Day   ," +
                "OFFTime_Holiday3_Month  =@OFFTime_Holiday3_Month ," +
                "OFFTime_Holiday3_Day    =@OFFTime_Holiday3_Day   ," +
                "OFFTime_Holiday4_Month  =@OFFTime_Holiday4_Month ," +
                "OFFTime_Holiday4_Day    =@OFFTime_Holiday4_Day   ," +
                "OFFTime_Holiday5_Month  =@OFFTime_Holiday5_Month ," +
                "OFFTime_Holiday5_Day    =@OFFTime_Holiday5_Day   ," +
                "OFFTime_Holiday6_Month  =@OFFTime_Holiday6_Month ," +
                "OFFTime_Holiday6_Day    =@OFFTime_Holiday6_Day   ," +
                "OFFTime_Holiday7_Month  =@OFFTime_Holiday7_Month ," +
                "OFFTime_Holiday7_Day    =@OFFTime_Holiday7_Day   ," +
                "OFFTime_Holiday8_Month  =@OFFTime_Holiday8_Month ," +
                "OFFTime_Holiday8_Day    =@OFFTime_Holiday8_Day   ," +
                "OFFTime_Holiday9_Month  =@OFFTime_Holiday9_Month ," +
                "OFFTime_Holiday9_Day    =@OFFTime_Holiday9_Day   ," +
                "OFFTime_Holiday10_Month =@OFFTime_Holiday10_Month," +
                "OFFTime_Holiday10_Day   =@OFFTime_Holiday10_Day  ," +
                "OFFTime_Holiday11_Month =@OFFTime_Holiday11_Month," +
                "OFFTime_Holiday11_Day   =@OFFTime_Holiday11_Day  ," +
                "OFFTime_Holiday12_Month =@OFFTime_Holiday12_Month," +
                "OFFTime_Holiday12_Day   =@OFFTime_Holiday12_Day  ," +
                "OFFTime_Holiday13_Month =@OFFTime_Holiday13_Month," +
                "OFFTime_Holiday13_Day   =@OFFTime_Holiday13_Day  ," +
                "OFFTime_Holiday14_Month =@OFFTime_Holiday14_Month," +
                "OFFTime_Holiday14_Day   =@OFFTime_Holiday14_Day  ," +
                "OFFTime_Holiday15_Month =@OFFTime_Holiday15_Month," +
                "OFFTime_Holiday15_Day   =@OFFTime_Holiday15_Day  ," +
                "OFFTime_Holiday16_Month =@OFFTime_Holiday16_Month," +
                "OFFTime_Holiday16_Day   =@OFFTime_Holiday16_Day  ," +
                "OFFTime_Holiday17_Month =@OFFTime_Holiday17_Month," +
                "OFFTime_Holiday17_Day   =@OFFTime_Holiday17_Day  ," +
                "OFFTime_Holiday18_Month =@OFFTime_Holiday18_Month," +
                "OFFTime_Holiday18_Day   =@OFFTime_Holiday18_Day  ," +
                "OFFTime_Holiday19_Month =@OFFTime_Holiday19_Month," +
                "OFFTime_Holiday19_Day   =@OFFTime_Holiday19_Day  ," +
                "OFFTime_Holiday20_Month =@OFFTime_Holiday20_Month," +
                "OFFTime_Holiday20_Day   =@OFFTime_Holiday20_Day  ," +
                "OFFTime_Holiday21_Month =@OFFTime_Holiday21_Month," +
                "OFFTime_Holiday21_Day   =@OFFTime_Holiday21_Day  ," +
                "OFFTime_Holiday22_Month =@OFFTime_Holiday22_Month," +
                "OFFTime_Holiday22_Day   =@OFFTime_Holiday22_Day  ," +
                "OFFTime_Holiday23_Month =@OFFTime_Holiday23_Month," +
                "OFFTime_Holiday23_Day   =@OFFTime_Holiday23_Day  ," +
                "OFFTime_Holiday24_Month =@OFFTime_Holiday24_Month," +
                "OFFTime_Holiday24_Day   =@OFFTime_Holiday24_Day  ," +
                "OFFTime_Holiday25_Month =@OFFTime_Holiday25_Month," +
                "OFFTime_Holiday25_Day   =@OFFTime_Holiday25_Day" +
                " WHERE OFFTime_ID=@OFFTime_ID ";

                
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@OFFTime_Name ", c.OFFTime_Name);
                cmd.Parameters.AddWithValue("@OFFTime_Code ", c.OFFTime_Code);
                cmd.Parameters.AddWithValue("@OFFTime_IssueDate ", c.OFFTime_IssueDate);
                cmd.Parameters.AddWithValue("@OFFTime_StartDate ", c.OFFTime_StartDate);
                cmd.Parameters.AddWithValue("@OFFTime_WorkingDays ", c.OFFTime_WorkingDays);
                cmd.Parameters.AddWithValue("@OFFTime_CutOffTime ", c.OFFTime_CutOffTime);
                cmd.Parameters.AddWithValue("@OFFTime_GracePeriod ", c.OFFTime_GracePeriod);
                cmd.Parameters.AddWithValue("@OFFTime_WorkStart ", c.OFFTime_WorkStart);
                cmd.Parameters.AddWithValue("@OFFTime_WorkEnd ", c.OFFTime_WorkEnd);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday1_Month", c.OFFTime_Holiday1_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday1_Day ", c.OFFTime_Holiday1_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday2_Month", c.OFFTime_Holiday2_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday2_Day ", c.OFFTime_Holiday2_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday3_Month", c.OFFTime_Holiday3_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday3_Day ", c.OFFTime_Holiday3_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday4_Month", c.OFFTime_Holiday4_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday4_Day ", c.OFFTime_Holiday4_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday5_Month", c.OFFTime_Holiday5_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday5_Day ", c.OFFTime_Holiday5_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday6_Month", c.OFFTime_Holiday6_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday6_Day ", c.OFFTime_Holiday6_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday7_Month", c.OFFTime_Holiday7_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday7_Day ", c.OFFTime_Holiday7_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday8_Month", c.OFFTime_Holiday8_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday8_Day ", c.OFFTime_Holiday8_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday9_Month", c.OFFTime_Holiday9_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday9_Day ", c.OFFTime_Holiday9_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday10_Month", c.OFFTime_Holiday10_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday10_Day ", c.OFFTime_Holiday10_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday11_Month", c.OFFTime_Holiday11_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday11_Day ", c.OFFTime_Holiday11_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday12_Month", c.OFFTime_Holiday12_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday12_Day ", c.OFFTime_Holiday12_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday13_Month", c.OFFTime_Holiday13_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday13_Day ", c.OFFTime_Holiday13_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday14_Month", c.OFFTime_Holiday14_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday14_Day ", c.OFFTime_Holiday14_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday15_Month", c.OFFTime_Holiday15_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday15_Day ", c.OFFTime_Holiday15_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday16_Month", c.OFFTime_Holiday16_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday16_Day ", c.OFFTime_Holiday16_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday17_Month", c.OFFTime_Holiday17_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday17_Day ", c.OFFTime_Holiday17_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday18_Month", c.OFFTime_Holiday18_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday18_Day ", c.OFFTime_Holiday18_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday19_Month", c.OFFTime_Holiday19_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday19_Day ", c.OFFTime_Holiday19_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday20_Month", c.OFFTime_Holiday20_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday20_Day ", c.OFFTime_Holiday20_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday21_Month", c.OFFTime_Holiday21_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday21_Day ", c.OFFTime_Holiday21_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday22_Month", c.OFFTime_Holiday22_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday22_Day ", c.OFFTime_Holiday22_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday23_Month", c.OFFTime_Holiday23_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday23_Day ", c.OFFTime_Holiday23_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday24_Month", c.OFFTime_Holiday24_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday24_Day ", c.OFFTime_Holiday24_Day);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday25_Month", c.OFFTime_Holiday25_Month);
                cmd.Parameters.AddWithValue("@OFFTime_Holiday25_Day ", c.OFFTime_Holiday25_Day);
                cmd.Parameters.AddWithValue("@OFFTime_ID", c.OFFTime_ID);



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
        public bool Delete(BLL_Offtimes c)
        {
            bool isSuccess = false;
            SqlConnection conn = db.Connect();
            try
            {
                string sql = "DELETE FROM OFFTime WHERE OFFTime_ID=@OFFTime_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@OFFTime_ID", c.OFFTime_ID);



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
        #region Search OFFTime on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM OFFTime WHERE  OFFTime_Name = '" + keywords + "'  ";
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

        #region select OFFTime_Names by code 
        public string[] SelectOFFTimeNames()
        {
            List<string> OFFTime_Names = new List<string>();
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT OFFTime_Name FROM OFFTime";

                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string unitName = reader["OFFTime_Name"].ToString();
                    OFFTime_Names.Add(unitName);
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

            return OFFTime_Names.ToArray();
        }
        #endregion
        public int GetMaxOFFTimeID()
        {
            int offTimeID = -1; // Default value in case no OFFTime_ID is found

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get the maximum OFFTime_ID from the OFFTime table
                    string sql = "SELECT MAX(OFFTime_ID) FROM OFFTime";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Execute the query and retrieve the maximum OFFTime_ID
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            offTimeID = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return offTimeID;
        }

        #region get offtime id from by using name of offtime 
        public int GetOFFTimeID(string offTimeName)
        {
            int offTimeID = -1; // Default value in case the offTimeName is not found

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get OFFTime_ID
                    string sql = "SELECT OFFTime_ID FROM OFFTime WHERE OFFTime_Name=@offTimeName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@offTimeName", offTimeName);

                        // Execute the query and retrieve the OFFTime_ID
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            offTimeID = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return offTimeID;
        }

        #endregion
        #region get offtime id from by using name of offtime 
        public BLL_Offtimes GetOFFTimeData(string offTimeName)
        {
            BLL_Offtimes Offtimes_Data = new BLL_Offtimes();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get OFFTime data
                    string sql = "SELECT * FROM OFFTime WHERE OFFTime_Name=@offTimeName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@offTimeName", offTimeName);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of Offtimes_Data
                                Offtimes_Data.OFFTime_ID                 = Convert.ToInt32(reader["OFFTime_ID"]);
                                Offtimes_Data.OFFTime_Name               = reader["OFFTime_Name"].ToString();
                                Offtimes_Data.OFFTime_Code               = reader["OFFTime_Code"].ToString();
                                Offtimes_Data.OFFTime_IssueDate          = Convert.ToDateTime(reader["OFFTime_IssueDate"]);
                                Offtimes_Data.OFFTime_StartDate          = Convert.ToDateTime(reader["OFFTime_StartDate"]);
                                Offtimes_Data.OFFTime_WorkingDays        = Convert.ToInt32(reader["OFFTime_WorkingDays"]);
                                Offtimes_Data.OFFTime_CutOffTime         = Convert.ToInt32(reader["OFFTime_CutOffTime"]);
                                Offtimes_Data.OFFTime_GracePeriod        = Convert.ToInt32(reader["OFFTime_GracePeriod"]);
                                Offtimes_Data.OFFTime_WorkStart          = Convert.ToInt32(reader["OFFTime_WorkStart"]);
                                Offtimes_Data.OFFTime_WorkEnd            = Convert.ToInt32(reader["OFFTime_WorkEnd"]);
                                Offtimes_Data.OFFTime_Holiday1_Month     = Convert.ToInt32(reader["OFFTime_Holiday1_Month"]);
                                Offtimes_Data.OFFTime_Holiday1_Day       = Convert.ToInt32(reader["OFFTime_Holiday1_Day"]);
                                Offtimes_Data.OFFTime_Holiday2_Month     = Convert.ToInt32(reader["OFFTime_Holiday2_Month"]);
                                Offtimes_Data.OFFTime_Holiday2_Day       = Convert.ToInt32(reader["OFFTime_Holiday2_Day"]);
                                Offtimes_Data.OFFTime_Holiday3_Month     = Convert.ToInt32(reader["OFFTime_Holiday3_Month"]);
                                Offtimes_Data.OFFTime_Holiday3_Day       = Convert.ToInt32(reader["OFFTime_Holiday3_Day"]);
                                Offtimes_Data.OFFTime_Holiday4_Month     = Convert.ToInt32(reader["OFFTime_Holiday4_Month"]);
                                Offtimes_Data.OFFTime_Holiday4_Day       = Convert.ToInt32(reader["OFFTime_Holiday4_Day"]);
                                Offtimes_Data.OFFTime_Holiday5_Month     = Convert.ToInt32(reader["OFFTime_Holiday5_Month"]);
                                Offtimes_Data.OFFTime_Holiday5_Day       = Convert.ToInt32(reader["OFFTime_Holiday5_Day"]);
                                Offtimes_Data.OFFTime_Holiday6_Month     = Convert.ToInt32(reader["OFFTime_Holiday6_Month"]);
                                Offtimes_Data.OFFTime_Holiday6_Day       = Convert.ToInt32(reader["OFFTime_Holiday6_Day"]);
                                Offtimes_Data.OFFTime_Holiday7_Month     = Convert.ToInt32(reader["OFFTime_Holiday7_Month"]);
                                Offtimes_Data.OFFTime_Holiday7_Day       = Convert.ToInt32(reader["OFFTime_Holiday7_Day"]);
                                Offtimes_Data.OFFTime_Holiday8_Month     = Convert.ToInt32(reader["OFFTime_Holiday8_Month"]);
                                Offtimes_Data.OFFTime_Holiday8_Day       = Convert.ToInt32(reader["OFFTime_Holiday8_Day"]);
                                Offtimes_Data.OFFTime_Holiday9_Month     = Convert.ToInt32(reader["OFFTime_Holiday9_Month"]);
                                Offtimes_Data.OFFTime_Holiday9_Day       = Convert.ToInt32(reader["OFFTime_Holiday9_Day"]);
                                Offtimes_Data.OFFTime_Holiday10_Month    = Convert.ToInt32(reader["OFFTime_Holiday10_Month"]);
                                Offtimes_Data.OFFTime_Holiday10_Day      = Convert.ToInt32(reader["OFFTime_Holiday10_Day"]);
                                Offtimes_Data.OFFTime_Holiday11_Month    = Convert.ToInt32(reader["OFFTime_Holiday11_Month"]);
                                Offtimes_Data.OFFTime_Holiday11_Day      = Convert.ToInt32(reader["OFFTime_Holiday11_Day"]);
                                Offtimes_Data.OFFTime_Holiday12_Month    = Convert.ToInt32(reader["OFFTime_Holiday12_Month"]);
                                Offtimes_Data.OFFTime_Holiday12_Day      = Convert.ToInt32(reader["OFFTime_Holiday12_Day"]);
                                Offtimes_Data.OFFTime_Holiday13_Month    = Convert.ToInt32(reader["OFFTime_Holiday13_Month"]);
                                Offtimes_Data.OFFTime_Holiday13_Day      = Convert.ToInt32(reader["OFFTime_Holiday13_Day"]);
                                Offtimes_Data.OFFTime_Holiday14_Month    = Convert.ToInt32(reader["OFFTime_Holiday14_Month"]);
                                Offtimes_Data.OFFTime_Holiday14_Day      = Convert.ToInt32(reader["OFFTime_Holiday14_Day"]);
                                Offtimes_Data.OFFTime_Holiday15_Month    = Convert.ToInt32(reader["OFFTime_Holiday15_Month"]);
                                Offtimes_Data.OFFTime_Holiday15_Day      = Convert.ToInt32(reader["OFFTime_Holiday15_Day"]);
                                Offtimes_Data.OFFTime_Holiday16_Month    = Convert.ToInt32(reader["OFFTime_Holiday16_Month"]);
                                Offtimes_Data.OFFTime_Holiday16_Day      = Convert.ToInt32(reader["OFFTime_Holiday16_Day"]);
                                Offtimes_Data.OFFTime_Holiday17_Month    = Convert.ToInt32(reader["OFFTime_Holiday17_Month"]);
                                Offtimes_Data.OFFTime_Holiday17_Day      = Convert.ToInt32(reader["OFFTime_Holiday17_Day"]);
                                Offtimes_Data.OFFTime_Holiday18_Month    = Convert.ToInt32(reader["OFFTime_Holiday18_Month"]);
                                Offtimes_Data.OFFTime_Holiday18_Day      = Convert.ToInt32(reader["OFFTime_Holiday18_Day"]);
                                Offtimes_Data.OFFTime_Holiday19_Month    = Convert.ToInt32(reader["OFFTime_Holiday19_Month"]);
                                Offtimes_Data.OFFTime_Holiday19_Day      = Convert.ToInt32(reader["OFFTime_Holiday19_Day"]);
                                Offtimes_Data.OFFTime_Holiday20_Month    = Convert.ToInt32(reader["OFFTime_Holiday20_Month"]);
                                Offtimes_Data.OFFTime_Holiday20_Day      = Convert.ToInt32(reader["OFFTime_Holiday20_Day"]);
                                Offtimes_Data.OFFTime_Holiday21_Month    = Convert.ToInt32(reader["OFFTime_Holiday21_Month"]);
                                Offtimes_Data.OFFTime_Holiday21_Day      = Convert.ToInt32(reader["OFFTime_Holiday21_Day"]);
                                Offtimes_Data.OFFTime_Holiday22_Month    = Convert.ToInt32(reader["OFFTime_Holiday22_Month"]);
                                Offtimes_Data.OFFTime_Holiday22_Day      = Convert.ToInt32(reader["OFFTime_Holiday22_Day"]);
                                Offtimes_Data.OFFTime_Holiday23_Month    = Convert.ToInt32(reader["OFFTime_Holiday23_Month"]);
                                Offtimes_Data.OFFTime_Holiday23_Day      = Convert.ToInt32(reader["OFFTime_Holiday23_Day"]);
                                Offtimes_Data.OFFTime_Holiday24_Month    = Convert.ToInt32(reader["OFFTime_Holiday24_Month"]);
                                Offtimes_Data.OFFTime_Holiday24_Day      = Convert.ToInt32(reader["OFFTime_Holiday24_Day"]);
                                Offtimes_Data.OFFTime_Holiday25_Month    = Convert.ToInt32(reader["OFFTime_Holiday25_Month"]);
                                Offtimes_Data.OFFTime_Holiday25_Day      = Convert.ToInt32(reader["OFFTime_Holiday25_Day"]);


                                // Continue with the rest of the properties...
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }

            return Offtimes_Data;
        }


        #endregion
    }
}
