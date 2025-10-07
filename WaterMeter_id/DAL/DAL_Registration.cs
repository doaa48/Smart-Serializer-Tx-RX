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
    class DAL_HoldingComp
    {
        //static string method for db connection string 
        //static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
          Database db = new Database();
        #region Select HoldingComp
        public DataTable Select()
        {
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM HoldingComp";       
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
        #region Insert new HoldingComp
        public bool Insert(BLL_HoldingComp c)
        {
            bool isSuccess = false;
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "INSERT INTO HoldingComp (Holding_Comp_Name, Holding_Comp_KUH, Holding_Comp_KPH) VALUES (@Holding_Comp_Name, @Holding_Comp_KUH, @Holding_Comp_KPH)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Holding_Comp_Name",             c.Holding_Comp_Name);
                cmd.Parameters.AddWithValue("@Holding_Comp_KUH",                   c.Holding_Comp_KUH);
                cmd.Parameters.AddWithValue("@Holding_Comp_KPH",                 c.Holding_Comp_KPH);
                
               
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
        #region update HoldingComp
        public bool Update(BLL_HoldingComp c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {


                string sql = "UPDATE HoldingComp SET Holding_Comp_Name=@Holding_Comp_Name, Holding_Comp_KUH=@Holding_Comp_KUH,Holding_Comp_KPH=@Holding_Comp_KPH ,  WHERE HoldingComp_ID=@HoldingComp_Id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Holding_Comp_Name",       c.Holding_Comp_Name);
                cmd.Parameters.AddWithValue("@Holding_Comp_KUH", c.Holding_Comp_KUH);
                cmd.Parameters.AddWithValue("@Holding_Comp_KPH",  c.Holding_Comp_KPH);
             
                cmd.Parameters.AddWithValue("@HoldingComp_Id",          c.HoldingComp_Id);

               

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
        public bool Delete(BLL_HoldingComp c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {
                string sql = "DELETE FROM HoldingComp WHERE HoldingComp_ID=@iHoldingComp_Idd";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@HoldingComp_Id", c.HoldingComp_Id);

               

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
        #region Search HoldingComp_Id on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
             SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM HoldingComp WHERE HoldingComp_ID LIKE '%" + keywords + "%' OR Holding_Comp_Name LIKE '%" + keywords +  "%' ";
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

        #region HoldingComp CompanyData
        public BLL_HoldingComp GetHoldingCompData()
        {
            BLL_HoldingComp HoldingComp_Data = new BLL_HoldingComp();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get HoldingComp data
                    string sql = "SELECT * FROM HoldingComp";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of HoldingComp_Data
                                HoldingComp_Data.HoldingComp_Id = Convert.ToInt32(reader["HoldingComp_Id"]);
                                HoldingComp_Data.Holding_Comp_Name = reader["Holding_Comp_Name"].ToString();
                                HoldingComp_Data.Holding_Comp_KUH = reader["Holding_Comp_KUH"].ToString();
                                HoldingComp_Data.Holding_Comp_KPH = reader["Holding_Comp_KPH"].ToString();
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

            return HoldingComp_Data;
        }

        #endregion



    }
    class DAL_MeterManf
    {
        //static string method for db connection string 
        //static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
          Database db= new Database();
        #region Select MeterManf
        public DataTable Select()
        {
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT* FROM MeterManf";       
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
        #region Insert new MeterManf
        public bool Insert(BLL_MeterManf c)
        {
            bool isSuccess = false;
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "INSERT INTO MeterManf (MeterManf_Name, MeterManf_KUF, MeterManf_KPF, MeterManf_CKUF,MeterManf_HoldingCompID) VALUES (@MeterManf_Name, @MeterManf_KUF, @MeterManf_KPF, @MeterManf_CKUF,@MeterManf_HoldingCompId)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MeterManf_Name",             c.MeterManf_Name);
                cmd.Parameters.AddWithValue("@MeterManf_KUF",                   c.MeterManf_KUF);
                cmd.Parameters.AddWithValue("@MeterManf_KPF",                 c.MeterManf_KPF);
                cmd.Parameters.AddWithValue("@MeterManf_CKUF",       c.MeterManf_CKUF);
               cmd.Parameters.AddWithValue("@MeterManf_HoldingCompId",       c.MeterManf_HoldingCompId);
               
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
        #region update MeterManf
        public bool Update(BLL_MeterManf c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {


                string sql = "UPDATE MeterManf SET MeterManf_Name=@MeterManf_Name, MeterManf_KUF=@MeterManf_KUF,MeterManf_KPF=@MeterManf_KPF , MeterManf_CKUF=@MeterManf_CKUF, MeterManf_HoldingCompID=@MeterManf_HoldingCompId WHERE MeterManf_ID=@MeterManf_Id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                
                cmd.Parameters.AddWithValue("@MeterManf_Name", c.MeterManf_Name);
                cmd.Parameters.AddWithValue("@MeterManf_KUF",  c.MeterManf_KUF);
                cmd.Parameters.AddWithValue("@MeterManf_KPF",    c.MeterManf_KPF);
                 cmd.Parameters.AddWithValue("@MeterManf_CKUF",    c.MeterManf_CKUF);
                  cmd.Parameters.AddWithValue("@MeterManf_HoldingCompId",    c.MeterManf_HoldingCompId);
                cmd.Parameters.AddWithValue("@MeterManf_Id",          c.MeterManf_Id);

               

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
        public bool Delete(BLL_MeterManf c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {
                string sql = "DELETE FROM MeterManf WHERE MeterManf_ID=@MeterManf_Id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MeterManf_Id", c.MeterManf_Id);

               

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
        #region Search MeterManf on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
             SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM MeterManf WHERE MeterManf_Name = '" + keywords + "'  ";
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

        #region Select metermanfByID
        public BLL_MeterManf GetMeterManfData(int docID)
        {
            BLL_MeterManf MeterManf_Data = new BLL_MeterManf();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get MeterManf data
                    string sql = "SELECT * FROM MeterManf WHERE MeterManf_Id='"+docID.ToString()+"'";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of MeterManf_Data
                                MeterManf_Data.MeterManf_Id = Convert.ToInt32(reader["MeterManf_Id"]);
                                MeterManf_Data.MeterManf_Name = reader["MeterManf_Name"].ToString();
                                MeterManf_Data.MeterManf_KUF = reader["MeterManf_KUF"].ToString();
                                MeterManf_Data.MeterManf_KPF = reader["MeterManf_KPF"].ToString();
                                MeterManf_Data.MeterManf_CKUF = reader["MeterManf_CKUF"].ToString();
                                MeterManf_Data.MeterManf_HoldingCompId = Convert.ToInt32(reader["MeterManf_HoldingCompId"]);
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

            return MeterManf_Data;
        }
        #endregion

        #region select metermanf when name 
        public BLL_MeterManf GetMeterManfData(string MeterManfName)
        {
            BLL_MeterManf MeterManf_Data = new BLL_MeterManf();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get MeterManf data by name
                    string sql = "SELECT * FROM MeterManf WHERE MeterManf_Name=@MeterManfName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@MeterManfName", MeterManfName);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of MeterManf_Data
                                MeterManf_Data.MeterManf_Id = Convert.ToInt32(reader["MeterManf_Id"]);
                                MeterManf_Data.MeterManf_Name = reader["MeterManf_Name"].ToString();
                                MeterManf_Data.MeterManf_KUF = reader["MeterManf_KUF"].ToString();
                                MeterManf_Data.MeterManf_KPF = reader["MeterManf_KPF"].ToString();
                                MeterManf_Data.MeterManf_CKUF = reader["MeterManf_CKUF"].ToString();
                                MeterManf_Data.MeterManf_HoldingCompId = Convert.ToInt32(reader["MeterManf_HoldingCompId"]);
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

            return MeterManf_Data;
        }

        #endregion 

    }
    class DAL_CardProd
    {
        //static string method for db connection string 
        //static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        Database db = new Database();
        #region Select CardProd
        public DataTable Select()
        {
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT* FROM CardProd";       
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
        #region Insert new CardProd
        public bool Insert(BLL_CardProd c)
        {
            bool isSuccess = false;
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "INSERT INTO CardProd (CardProd_name, CardProd_KUCP, CardProd_KPCP, CardProd_CKUCP,CardProd_HoldingCompID) VALUES (@CardProd_name, @CardProd_KUCP, @CardProd_KPCP, @CardProd_CKUCP , @CardProd_HoldingCompId)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CardProd_name",             c.CardProd_name);
                cmd.Parameters.AddWithValue("@CardProd_KUCP",                   c.CardProd_KUCP);
                cmd.Parameters.AddWithValue("@CardProd_KPCP",                 c.CardProd_KPCP);
                cmd.Parameters.AddWithValue("@CardProd_CKUCP",       c.CardProd_CKUCP);
                cmd.Parameters.AddWithValue("@CardProd_HoldingCompId",                  c.CardProd_HoldingCompId);
               
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
        #region update CardProd
        public bool Update(BLL_CardProd c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {


                string sql = "UPDATE CardProd SET CardProd_name=@CardProd_name, CardProd_KUCP=@CardProd_KUCP,CardProd_KPCP=@CardProd_KPCP , CardProd_CKUCP=@CardProd_CKUCP, CardProd_HoldingCompId=@CardProd_HoldingCompId WHERE CardProd_ID=@CardProd_Id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CardProd_name",       c.CardProd_name);
                cmd.Parameters.AddWithValue("@CardProd_KUCP", c.CardProd_KUCP);
                cmd.Parameters.AddWithValue("@CardProd_KPCP",  c.CardProd_KPCP);
                cmd.Parameters.AddWithValue("@CardProd_CKUCP",    c.CardProd_CKUCP);
                 cmd.Parameters.AddWithValue("@CardProd_HoldingCompId",    c.CardProd_HoldingCompId);
                cmd.Parameters.AddWithValue("@CardProd_Id",          c.CardProd_Id);

               

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
        public bool Delete(BLL_CardProd c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {
                string sql = "DELETE FROM CardProd WHERE CardProd_ID=@CardProd_Id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CardProd_Id", c.CardProd_Id);

               

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
        #region Search CardProd on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
             SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM CardProd WHERE CardProd_name LIKE '%" + keywords + "%' OR CardProd_HoldingCompID LIKE '%" + keywords + "%' OR CardProd_ID LIKE '%" + keywords + "%' ";
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

        #region select cardproducer 
        public BLL_CardProd GetCardProdData()
        {
            BLL_CardProd CardProd_Data = new BLL_CardProd();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get CardProd data by CardProd_Name
                    string sql = "SELECT * FROM CardProd ";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
 

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of CardProd_Data
                                CardProd_Data.CardProd_Id = Convert.ToInt32(reader["CardProd_Id"]);
                                CardProd_Data.CardProd_name = reader["CardProd_name"].ToString();
                                CardProd_Data.CardProd_KUCP = reader["CardProd_KUCP"].ToString();
                                CardProd_Data.CardProd_KPCP = reader["CardProd_KPCP"].ToString();
                                CardProd_Data.CardProd_CKUCP = reader["CardProd_CKUCP"].ToString();
                                CardProd_Data.CardProd_HoldingCompId = Convert.ToInt32(reader["CardProd_HoldingCompId"]);
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

            return CardProd_Data;
        }

        #endregion
        #region select cardproducer by id
        public BLL_CardProd GetCardProdData(int cardProdId)
        {
            BLL_CardProd CardProd_Data = new BLL_CardProd();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get CardProd data by CardProd_Id
                    string sql = "SELECT * FROM CardProd WHERE CardProd_Id=@cardProdId";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@cardProdId", cardProdId);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of CardProd_Data
                                CardProd_Data.CardProd_Id = Convert.ToInt32(reader["CardProd_Id"]);
                                CardProd_Data.CardProd_name = reader["CardProd_name"].ToString();
                                CardProd_Data.CardProd_KUCP = reader["CardProd_KUCP"].ToString();
                                CardProd_Data.CardProd_KPCP = reader["CardProd_KPCP"].ToString();
                                CardProd_Data.CardProd_CKUCP = reader["CardProd_CKUCP"].ToString();
                                CardProd_Data.CardProd_HoldingCompId = Convert.ToInt32(reader["CardProd_HoldingCompId"]);
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

            return CardProd_Data;
        }

        #endregion

        #region select cardproducer when name

        public BLL_CardProd GetCardProdDataByName(string cardProdName)
        {
            BLL_CardProd CardProd_Data = new BLL_CardProd();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get CardProd data by CardProd_Name
                    string sql = "SELECT * FROM CardProd WHERE CardProd_name=@cardProdName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@cardProdName", cardProdName);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of CardProd_Data
                                CardProd_Data.CardProd_Id = Convert.ToInt32(reader["CardProd_Id"]);
                                CardProd_Data.CardProd_name = reader["CardProd_name"].ToString();
                                CardProd_Data.CardProd_KUCP = reader["CardProd_KUCP"].ToString();
                                CardProd_Data.CardProd_KPCP = reader["CardProd_KPCP"].ToString();
                                CardProd_Data.CardProd_CKUCP = reader["CardProd_CKUCP"].ToString();
                                CardProd_Data.CardProd_HoldingCompId = Convert.ToInt32(reader["CardProd_HoldingCompId"]);
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

            return CardProd_Data;
        }

        #endregion

    }
    class DAL_WaterComp
    {
        //static string method for db connection string 
        //static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
          Database db= new Database();
        #region Select WaterComp
        public DataTable Select()
        {
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM WaterComp";       
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
        #region Insert new WaterComp
        public bool Insert(BLL_WaterComp c)
        {
            bool isSuccess = false;
            //creating db connection
             SqlConnection conn =db.Connect();

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                String sql = "INSERT INTO WaterComp (WaterComp_Name, WaterComp_KUW, WaterComp_KPW, WaterComp_HoldingCompID,WaterComp_CertHoldingComp_TO_Watercomp) VALUES (@WaterComp_Name, @WaterComp_KUW, @WaterComp_KPW, @WaterComp_HoldingCompId , @WaterComp_CertHoldingComp_TO_Watercomp)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@WaterComp_Name",             c.WaterComp_Name);
                cmd.Parameters.AddWithValue("@WaterComp_KUW",                   c.WaterComp_KUW);
                cmd.Parameters.AddWithValue("@WaterComp_KPW",                 c.WaterComp_KPW);
                cmd.Parameters.AddWithValue("@WaterComp_HoldingCompId",       c.WaterComp_HoldingCompId);
                cmd.Parameters.AddWithValue("@WaterComp_CertHoldingComp_TO_Watercomp",                  c.WaterComp_CertHoldingComp_TO_Watercomp);
               
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
        #region update WaterComp
        public bool Update(BLL_WaterComp c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {


                string sql = "UPDATE WaterComp SET WaterComp_Name=@WaterComp_Name, WaterComp_KUW=@WaterComp_KUW,WaterComp_KPW=@WaterComp_KPW , WaterComp_HoldingCompID=@WaterComp_HoldingCompId, WaterComp_CertHoldingComp_TO_Watercomp=@WaterComp_CertHoldingComp_TO_Watercomp WHERE WaterComp_ID=@WaterComp_Id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                 cmd.Parameters.AddWithValue("@WaterComp_Name",             c.WaterComp_Name);
                cmd.Parameters.AddWithValue("@WaterComp_KUW",                   c.WaterComp_KUW);
                cmd.Parameters.AddWithValue("@WaterComp_KPW",                 c.WaterComp_KPW);
                cmd.Parameters.AddWithValue("@WaterComp_HoldingCompId",       c.WaterComp_HoldingCompId);
                cmd.Parameters.AddWithValue("@WaterComp_CertHoldingComp_TO_Watercomp",                  c.WaterComp_CertHoldingComp_TO_Watercomp);
               
                cmd.Parameters.AddWithValue("@WaterComp_Id",          c.WaterComp_Id);

               

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
        public bool Delete(BLL_WaterComp c)
        {
            bool isSuccess = false;
             SqlConnection conn =db.Connect();
            try
            {
                string sql = "DELETE FROM WaterComp WHERE WaterComp_ID=@WaterComp_Id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@WaterComp_Id", c.WaterComp_Id);

               

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
        #region Search Registration on db usingKeywords
        public DataTable Search(string keywords)
        {
            // Static Method to connect db
             SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM WaterComp WHERE WaterComp_Name LIKE '%" + keywords + "%' OR WaterComp_CardProdID LIKE '%" + keywords + "%' OR WaterComp_MeterManfID LIKE '%" + keywords + "%' ";
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
        #region SelectWater CompanyData ByID
        public BLL_WaterComp GetWaterCompDataByID(int waterCompId)
        {
            BLL_WaterComp WaterComp_Data = new BLL_WaterComp();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get WaterComp data
                    string sql = "SELECT * FROM WaterComp WHERE WaterComp_Id="+waterCompId;

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of WaterComp_Data

                                WaterComp_Data.WaterComp_Id = Convert.ToInt32(reader["WaterComp_Id"]);
                                WaterComp_Data.WaterComp_Name = reader["WaterComp_Name"].ToString();
                                WaterComp_Data.WaterComp_KUW = reader["WaterComp_KUW"].ToString();
                                WaterComp_Data.WaterComp_KPW = reader["WaterComp_KPW"].ToString();
                                WaterComp_Data.WaterComp_HoldingCompId = Convert.ToInt32(reader["WaterComp_HoldingCompId"]);
                                WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp = reader["WaterComp_CertHoldingComp_TO_Watercomp"].ToString();
                               // WaterComp_Data.WaterComp_CardProdId = Convert.ToInt32(reader["WaterComp_CardProdId"]);
                            //    WaterComp_Data.WaterComp_MeterManfId = Convert.ToInt32(reader["WaterComp_MeterManfId"]);

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

            return WaterComp_Data;
        }
        #endregion

        #region SelectWater WaterCompanyData By Name
        public BLL_WaterComp GetWaterCompDataByName(string waterCompanyName)
        {
            BLL_WaterComp WaterComp_Data = new BLL_WaterComp();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get WaterComp data
                    string sql = "SELECT * FROM WaterComp WHERE WaterComp_Name=@waterCompanyName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add a parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@waterCompanyName", waterCompanyName);

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of WaterComp_Data

                                WaterComp_Data.WaterComp_Id = Convert.ToInt32(reader["WaterComp_Id"]);
                                WaterComp_Data.WaterComp_Name = reader["WaterComp_Name"].ToString();
                                WaterComp_Data.WaterComp_KUW = reader["WaterComp_KUW"].ToString();
                                WaterComp_Data.WaterComp_KPW = reader["WaterComp_KPW"].ToString();
                                WaterComp_Data.WaterComp_HoldingCompId = Convert.ToInt32(reader["WaterComp_HoldingCompId"]);
                                WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp = reader["WaterComp_CertHoldingComp_TO_Watercomp"].ToString();
                               

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

            return WaterComp_Data;
        }
        #endregion

        #region SelectWater CompanyData
        public BLL_WaterComp GetWaterCompData()
        {
            BLL_WaterComp WaterComp_Data = new BLL_WaterComp();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get WaterComp data
                    string sql = "SELECT * FROM WaterComp";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                      

                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the properties of WaterComp_Data

                                WaterComp_Data.WaterComp_Id = Convert.ToInt32(reader["WaterComp_Id"]);
                                WaterComp_Data.WaterComp_Name = reader["WaterComp_Name"].ToString();
                                WaterComp_Data.WaterComp_KUW = reader["WaterComp_KUW"].ToString();
                                WaterComp_Data.WaterComp_KPW = reader["WaterComp_KPW"].ToString();
                                WaterComp_Data.WaterComp_HoldingCompId = Convert.ToInt32(reader["WaterComp_HoldingCompId"]);
                                WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp = reader["WaterComp_CertHoldingComp_TO_Watercomp"].ToString();
                              //  WaterComp_Data.WaterComp_CardProdId = Convert.ToInt32(reader["WaterComp_CardProdId"]);
                              //  WaterComp_Data.WaterComp_MeterManfId = Convert.ToInt32(reader["WaterComp_MeterManfId"]);

                                // Continue with the rest of the properties...
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    //throw; // Re-throw the exception for higher-level handling
                }
            }

            return WaterComp_Data;
        }
        #endregion
    }

}
