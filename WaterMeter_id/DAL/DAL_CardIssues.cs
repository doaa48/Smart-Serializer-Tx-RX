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
    class DAL_CardIssues
    {
      public Database db = new Database();
        #region Select Card from Database
        public DataTable Select()
        {
            // Static Method to connect db
            SqlConnection conn =db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Card";
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
        #region Insert Card in DB
        public bool Insert(BLL_CardIssues P)
        {
            bool isSuccess = false;

           SqlConnection conn =db.Connect();

            try
            {
                String sql = "INSERT INTO Card (Card_PK, Card_CardTypeID, Card_CardProdID, Card_CertCardProd_TO_Card,Card_CertWaterComp_TO_Card,Card_CardNum,Card_Func,Card_Mode,Card_IssueDate,Card_LastTranscationDate,Card_ExpiratonDate,Card_LastUploadBy) VALUES (@PK, @CardTypeID, @CardProdID, @CertCardProd_TO_Card,@CertWaterComp_TO_Card,@CardNum,@Func,@Mode,@IssueDate,@LastTranscationDate,@ExpiratonDate,@LastUploadBy)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                P.CertWaterComp_TO_Card = "";

                cmd.Parameters.AddWithValue("@PK"                   ,   P.PK                   );  
				cmd.Parameters.AddWithValue("@CardTypeID"           ,   P.CardTypeID           );
				cmd.Parameters.AddWithValue("@CardProdID"           ,   P.CardProdID           );
				cmd.Parameters.AddWithValue("@CertCardProd_TO_Card" ,   P.CertCardProd_TO_Card );
				//cmd.Parameters.AddWithValue("@WaterCompID"          ,   P.WaterCompID          );
				cmd.Parameters.AddWithValue("@CertWaterComp_TO_Card",   P.CertWaterComp_TO_Card);
				cmd.Parameters.AddWithValue("@CardNum"              ,   P.CardNum              );
				cmd.Parameters.AddWithValue("@Func"                 ,   P.Func                 );
				cmd.Parameters.AddWithValue("@Mode"                 ,   P.Mode                 );
				cmd.Parameters.AddWithValue("@IssueDate"            ,   P.IssueDate            );
				cmd.Parameters.AddWithValue("@LastTranscationDate"  ,   P.LastTranscationDate  );
				cmd.Parameters.AddWithValue("@ExpiratonDate"        ,   P.ExpiratonDate        );
				cmd.Parameters.AddWithValue("@LastUploadBy"         ,   P.LastUploadBy         );
            

                

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
        #region update CardIssues in db
        public bool Update(BLL_CardIssues P)
        {
            bool isSuccess = false;
          SqlConnection conn =db.Connect();
            try
            {
                string sql = "UPDATE Card SET Card_PK=@PK, Card_CardTypeID=@CardTypeID, Card_CardProdID=@CardProdID, Card_CertCardProd_TO_Card=@CertCardProd_TO_Card,Card_WaterCompID=@WaterCompID,Card_CertWaterComp_TO_Card=@CertWaterComp_TO_Card,Card_CardNum=@CardNum,Card_Func=@Func,Card_Mode=@Mode,Card_IssueDate=@IssueDate,Card_LastTranscationDate=@LastTranscationDate,Card_ExpiratonDate=@ExpiratonDate,Card_LastUploadBy=@LastUploadBy WHERE Card_ID=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
              
                cmd.Parameters.AddWithValue("@PK"                   ,   P.PK                   );  
				cmd.Parameters.AddWithValue("@CardTypeID"           ,   P.CardTypeID           );
				cmd.Parameters.AddWithValue("@CardProdID"           ,   P.CardProdID           );
				cmd.Parameters.AddWithValue("@CertCardProd_TO_Card" ,   P.CertCardProd_TO_Card );
				cmd.Parameters.AddWithValue("@WaterCompID"          ,   P.WaterCompID          );
				cmd.Parameters.AddWithValue("@CertWaterComp_TO_Card",   P.CertWaterComp_TO_Card);
				cmd.Parameters.AddWithValue("@CardNum"              ,   P.CardNum              );
				cmd.Parameters.AddWithValue("@Func"                 ,   P.Func                 );
				cmd.Parameters.AddWithValue("@Mode"                 ,   P.Mode                 );
				cmd.Parameters.AddWithValue("@IssueDate"            ,   P.IssueDate            );
				cmd.Parameters.AddWithValue("@LastTranscationDate"  ,   P.LastTranscationDate  );
				cmd.Parameters.AddWithValue("@ExpiratonDate"        ,   P.ExpiratonDate        );
				cmd.Parameters.AddWithValue("@LastUploadBy"         ,   P.LastUploadBy         );
                cmd.Parameters.AddWithValue("@id",          P.id);

                

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
        public bool Delete(BLL_CardIssues p)
        {
            bool isSuccess = false;
          SqlConnection conn =db.Connect();
            try
            {
                string sql = "DELETE FROM Card WHERE Card_ID=@id";
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
        #region Search CardIssues on db usingKeywords
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
                String sql = "SELECT * FROM Card WHERE Card_CardTypeID LIKE '%" + keywords + "%' OR Card_Func LIKE '%" + keywords + "%' OR Card_Mode LIKE '%" + keywords + "%' ";
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
        #region select last id in database
        public uint GetLastID ()
        {
            uint maxCardID = 0;
            // Static Method to connect db
            // SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT MAX(Card_ID) AS MaxCardID FROM Card";
                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
               
                
                //fill data in dataTable
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    // Get the maximum Card_ID as an integer value
                     maxCardID = (uint)Convert.ToInt32(dt.Rows[0]["MaxCardID"]);

                    // Now, maxCardID contains the maximum Card_ID from the Card table.
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
            return maxCardID;
        }
        #endregion

        #region SelectSpecificCardVariables
        public DataTable SelectSpecificCardVariables()
        {
            using (SqlConnection conn = db.Connect())
            {
                DataTable dt = new DataTable();
                try
                {
                    string sql = @"SELECT Card_CardNum, Card_PK, CardType_Code, CardProd_name, 
                                  Card_IssueDate, Card_LastTranscationDate, Card_ExpiratonDate
                           FROM Card
                           INNER JOIN CardType ON Card.Card_CardTypeID = CardType.CardType_ID
                           INNER JOIN CardProd ON Card.Card_CardProdID = CardProd.CardProd_ID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Getting data from db
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        // Open db connection and fill data in dataTable
                       
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return dt;
            }
        }
        #endregion SelectSpecificCardVariables

        #region Select CardIssue By CardNum
        public DataTable GetCardIssuanceByNum (int cardNum)
        {
            DataTable CardIssue_Data = new DataTable();

            using(SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get MeterManf data by name
                    string sql = @"SELECT Card_CardNum, Card_PK, CardType_Code, CardProd_name, 
                                 WaterComp_Name, Card_Mode, CardProd_name, Card_Func, Card_IssueDate,
                                 Card_ExpiratonDate, Card_LastUploadBy, Card_LastTranscationDate
                                 FROM Card
                                 INNER JOIN CardType ON Card.Card_CardTypeID = CardType.CardType_ID
                                 INNER JOIN CardProd ON Card.Card_CardProdID = CardProd.CardProd_ID
                                 INNER JOIN WaterComp ON Card.Card_WaterCompID = WaterComp.WaterComp_ID
                                 WHERE Card_CardNum=" + cardNum;

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Getting data from db
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        // Open db connection and fill data in dataTable

                        adapter.Fill(CardIssue_Data);
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return CardIssue_Data;
        }
        #endregion
        #region select last id in database
        public int GetCardIDByCardNum(int cardNum)
        {
            int cardID = 0;
            SqlConnection conn = db.Connect();
            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db based on Card_CardNum
                string sql = "SELECT Card_ID FROM Card WHERE Card_CardNum = @CardNum";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CardNum", cardNum);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                if (dt.Rows.Count > 0 && dt.Rows[0]["Card_ID"] != DBNull.Value)
                {
                    // Get the Card_ID as an integer value
                    cardID = Convert.ToInt32(dt.Rows[0]["Card_ID"]);
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

            return cardID;
        }

        #endregion

        #region Select CardIssue By Number
        public BLL_CardIssues CardIssueDataSelection(int cardNum)
        {
            BLL_CardIssues CardData = new BLL_CardIssues();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    string sql = @"SELECT 
                                Card.Card_ID, 
                                Card.Card_PK, 
                                Card.Card_CardTypeID, 
                                Card.Card_CardProdID, 
                                Card.Card_CertCardProd_TO_Card, 
                                Card.Card_WaterCompID, 
                                Card.Card_CertWaterComp_TO_Card, 
                                Card.Card_CardNum, 
                                Card.Card_Func,
                                Card.Card_Mode, 
                                Card.Card_IssueDate, 
                                Card.Card_LastTranscationDate,
                                Card.Card_ExpiratonDate, 
                                Card.Card_LastUploadBy,
                                CardProd.CardProd_KUCP,
                                WaterComp.WaterComp_KUW ,
                                WaterComp.WaterComp_KPW ,
                                CardType_Code,
                                 CardProd_name, 
                                 WaterComp_Name
                            FROM 
                                Card
                           
                               INNER JOIN  CardType ON Card.Card_CardTypeID = CardType.CardType_ID
                                INNER JOIN  CardProd ON Card.Card_CardProdID = CardProd.CardProd_ID
                            LEFT JOIN 
                                WaterComp ON Card.Card_WaterCompID = WaterComp.WaterComp_ID
                            WHERE 
                                Card.Card_CardNum = @cardNum"; // Use parameterized query to prevent SQL injection

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cardNum", cardNum); // Add parameter to the query

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CardData.id = Convert.ToInt32(reader["Card_ID"]);
                                CardData.PK = reader["Card_PK"].ToString();
                                CardData.CardTypeID = reader["Card_CardTypeID"] != DBNull.Value ? Convert.ToInt32(reader["Card_CardTypeID"]) : 0;
                                CardData.CardProdID = reader["Card_CardProdID"] != DBNull.Value ? Convert.ToInt32(reader["Card_CardProdID"]) : 0;
                                CardData.CertCardProd_TO_Card = reader["Card_CertCardProd_TO_Card"].ToString();
                                CardData.WaterCompID = reader["Card_WaterCompID"] != DBNull.Value ? Convert.ToInt32(reader["Card_WaterCompID"]) : 0;
                                CardData.CertWaterComp_TO_Card = reader["Card_CertWaterComp_TO_Card"].ToString();
                                CardData.CardNum = reader["Card_CardNum"] != DBNull.Value ? Convert.ToInt32(reader["Card_CardNum"]) : 0;
                                CardData.Func = reader["Card_Func"] != DBNull.Value ? Convert.ToByte(reader["Card_Func"]) : (byte)0;
                                CardData.Mode = reader["Card_Mode"] != DBNull.Value ? Convert.ToByte(reader["Card_Mode"]) : (byte)0;
                                CardData.IssueDate = Convert.ToDateTime(reader["Card_IssueDate"]);
                                CardData.ExpiratonDate = Convert.ToDateTime(reader["Card_ExpiratonDate"]);
                                CardData.LastUploadBy = reader["Card_LastUploadBy"] != DBNull.Value ? Convert.ToByte(reader["Card_LastUploadBy"]) : (byte)0;
                                CardData.LastTranscationDate = Convert.ToDateTime(reader["Card_LastTranscationDate"]);
                                CardData.CardProd_KUCP = reader["CardProd_KUCP"].ToString();
                                CardData.WaterComp_KUW = reader["WaterComp_KUW"].ToString();
                                CardData.WaterComp_KPW = reader["WaterComp_KPW"].ToString();
                               
                                CardData.CardType_Code = reader["CardType_Code"].ToString();
                                CardData.CardProd_name = reader["CardProd_name"].ToString();
                                CardData.WaterComp_Name = reader["WaterComp_Name"].ToString();
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately, for example, log it
                    Console.WriteLine(ex.Message);
                    // You might want to throw the exception again if you cannot handle it here
                    throw;
                }
            }
            return CardData;
        }



        #endregion Select CardIssue By Number
    }
}
