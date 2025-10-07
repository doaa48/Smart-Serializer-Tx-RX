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
    public  class DAL_ClientManagment
    {

        //  static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        Database db = new Database();
        #region search Client from Database by Keyword
        public DataTable Search_Client(string Keyword)
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT * FROM Client WHERE  Client_FullName  LIKE '%" + Keyword + "%' OR Client_Number LIKE '%" + Keyword + "%' OR Client_NationID LIKE '%" + Keyword + "%' OR Client_phone LIKE '%" + Keyword + "%' OR Client_Email LIKE '%" + Keyword + "%' ";
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

        #region search ClientManagement  from Database by Keyword
        public DataTable Search_ClientManagment(string Keyword,string unitTypename,DateTime issuesDate)
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();


            string formattedDate = issuesDate.ToString("yyyy-MM-dd"); // Format the date as "yyyy-MM-dd"


            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT                            " +
                            "Client.Client_FullName           AS 'Client Name',     " +
                            "Client.Client_Number             AS 'Client IDm ',     " +
                            "Client.Client_NationID           AS 'Client Nation',   " +
                            "Client.Client_phone              AS 'Client Phone ',   " +
                            "Client.Client_Email              AS 'Client Email ',   " +
                            "ClientInfo.ClientInfo_IssueDate  AS 'Issue Date',      " +
                            "Meter.Meter_MeterNum             AS 'Meter IDm',       " +
                            "Meter.Meter_Diameter             AS 'Meter Dim',       " +
                            "Meter.Meter_Origin               AS 'Meter Origin',    " +
                            "Meter.Meter_Model                AS 'Meter Model',     " +
                            "Meter.Meter_Man                  AS 'Meter MAn' ,      " +
                            "Meter.Meter_ChargeMode           AS 'Charge Mode',     " +
                            "Card.Card_CardNum                AS 'Card IDc',        " +
                            "CardType.CardType_Code           AS 'Card Type',       " +
                            "PriceSchedule.PriceSchedule_Name AS 'PriceSchduler' ,  " +
                            "OFFTime.OFFTime_Name             AS 'OFFTime',         " +
                            "ClientInfo.ClientInfo_NumOFUnit  AS 'Unit Num',        " +
                            "UnitType.UnitType_Name           AS 'Unit Type',       " +
                            "ClientInfo.ClientInfo_Address    AS 'Unit Address',    " +
                            "ClientInfo.ClientInfo_Category   AS 'Category'         " +
                          

                         "FROM ClientInfo " +
                         "INNER JOIN Client ON ClientInfo.ClientInfo_ClientID = Client.Client_ID " +
                         "INNER JOIN UnitType ON ClientInfo.ClientInfo_UnityTypeID = UnitType.UnitType_ID   " +
                         "INNER JOIN Meter ON ClientInfo.ClientInfo_MeterID = Meter.Meter_ID    " +
                         "INNER JOIN Card ON ClientInfo.ClientInfo_CardID = Card.Card_ID        " +
                         "INNER JOIN CardType ON Card.Card_CardTypeID = CardType.CardType_ID   " +
                         "INNER JOIN PriceSchedule ON ClientInfo.ClientInfo_PriceScheduleID = PriceSchedule.PriceSchedule_ID " +
                         "INNER JOIN OFFTime ON ClientInfo.ClientInfo_OFFTimeID = OFFTime.OFFTime_ID " +
                         "WHERE " +
                         "Client.Client_FullName               LIKE '%" + Keyword + "%' " +
                         "OR Client.Client_Number              LIKE '%" + Keyword + "%' " +
                         "OR Client.Client_NationID            LIKE '%" + Keyword + "%' " +
                         "OR Client.Client_phone               LIKE '%" + Keyword + "%' " +
                         "OR Client.Client_Email               LIKE '%" + Keyword + "%' " +
                         "OR ClientInfo.ClientInfo_IssueDate   LIKE '%" + Keyword + "%' " +
                         "OR Meter.Meter_MeterNum              LIKE '%" + Keyword + "%' " +
                         "OR Meter.Meter_Diameter              LIKE '%" + Keyword + "%' " +
                         "OR Meter.Meter_Origin                LIKE '%" + Keyword + "%' " +
                         "OR Meter.Meter_Model                 LIKE '%" + Keyword + "%' " +
                         "OR Meter.Meter_Man                   LIKE '%" + Keyword + "%' " +
                         "OR Card.Card_CardNum                 LIKE '%" + Keyword + "%' " +
                         "OR CardType.CardType_Code            LIKE '%" + Keyword + "%' " +
                         "OR PriceSchedule.PriceSchedule_Name  LIKE '%" + Keyword + "%' " +
                         "OR OFFTime.OFFTime_Name              LIKE '%" + Keyword + "%' " +
                         " AND ClientInfo.ClientInfo_IssueDate >= '" + formattedDate + "' ";
                        
                        if (unitTypename !="ALL Unites")
                        {
                            sql += "  AND UnitType.UnitType_Name           = '" + unitTypename + "' ";
                        }

                      




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


        #region select  UnitType Name from Database by Keyword
        
       public string[] SelectUnitTypeNames()
        {
            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            string[] names = null;



            // ToolBar hold the data from db

            DataTable dt = new DataTable();
            try
            {
                // SQL Query to Get data from db
                String sql = "SELECT   UnitType.UnitType_Name      " +
                             "FROM ClientInfo " +
                             "INNER JOIN UnitType ON ClientInfo.ClientInfo_UnityTypeID = UnitType.UnitType_ID   ";
                       

                    //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //db connection open

                //fill data in dataTable
                adapter.Fill(dt);
                int index = 0;
                names = new string[dt.Rows.Count + 1];
                names[index++] = "ALL Unites";

                if (dt.Rows.Count > 0)
                {
                   
                    foreach (DataRow row in dt.Rows)
                    {
                        names[index++]=row["UnitType_Name"].ToString();
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
            return names;
        }
        #endregion

        #region select min date time issues
       public DateTime Select_MinIssueDate() {

            // Static Method to connect db
            //SqlConnection conn = new SqlConnection(myconnstrng);
            SqlConnection conn = db.Connect();

            DateTime minDateTime = new DateTime(2020, 1, 1);



            // ToolBar hold the data from db

            DataTable dt = new DataTable();

            try
            {
                // SQL Query to Get data from db
                string sql = "SELECT MIN(ClientInfo_IssueDate) FROM ClientInfo  WHERE ClientInfo_IssueDate IS NOT NULL  AND ClientInfo_IssueDate <> '2020-01-01'";


                //For executing Command
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from db
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //db connection open

                //fill data in dataTable
                adapter.Fill(dt);

                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    minDateTime = Convert.ToDateTime(dt.Rows[0][0]);
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
            return minDateTime;

        }
       #endregion
    }
}
