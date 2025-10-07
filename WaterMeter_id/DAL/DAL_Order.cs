using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using UnifyWaterCard.Entities;

namespace WaterMeter_id
{
    public class DAL_Order
    {
        Database db = new Database();
        public BLL_Orders Select(int orderID)
        {
            BLL_Orders BLL_Orders_Data = new BLL_Orders();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    string sql = @"SELECT [Orders_ID]
                                ,[Orders_CommandType]
                                ,[Orders_GetwayID]
                                ,[Orders_SchedularID]
                                ,[Orders_Payload]
                                ,[Orders_TimeOut]
                                ,[Orders_RetransmitNo]
                                ,[Orders_Status]
                                ,[Orders_ErrorMessege]
                                ,[Orders_IssueDate]
                            FROM [Orders] 
                            WHERE [Orders_ID] = @orderID"; // Filter by cardNum

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderID", orderID); // Add parameter to the query

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                BLL_Orders_Data.Orders_ID = (int)reader["Orders_ID"];
                                BLL_Orders_Data.Orders_CommandType = (int)reader["Orders_CommandType"];
                                BLL_Orders_Data.Orders_GetwayID = reader["Orders_GetwayID"] == DBNull.Value ? 0 : (int)reader["Orders_GetwayID"];
                                BLL_Orders_Data.Orders_SchedularID = reader["Orders_SchedularID"] == DBNull.Value ? 0 : (int)reader["Orders_SchedularID"];
                                BLL_Orders_Data.Orders_Payload = reader["Orders_Payload"].ToString();
                                BLL_Orders_Data.Orders_TimeOut = reader["Orders_TimeOut"] == DBNull.Value ? 0 : (int)reader["Orders_TimeOut"];
                                BLL_Orders_Data.Orders_RetransmitNo = (int)reader["Orders_RetransmitNo"];
                                BLL_Orders_Data.Orders_Status = reader["Orders_Status"] == DBNull.Value ? 0 : (int)reader["Orders_Status"];
                                BLL_Orders_Data.Orders_ErrorMessege = reader["Orders_ErrorMessege"].ToString();
                                BLL_Orders_Data.Orders_IssueDate = reader["Orders_IssueDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["Orders_IssueDate"];
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
            return BLL_Orders_Data;
        }
        public List<BLL_Orders> Select()
        {
            List<BLL_Orders> ordersList = new List<BLL_Orders>();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    string sql = @"SELECT [Orders_ID]
                                ,[Orders_CommandType]
                                ,[Orders_GetwayID]
                                ,[Orders_SchedularID]
                                ,[Orders_Payload]
                                ,[Orders_TimeOut]
                                ,[Orders_RetransmitNo]
                                ,[Orders_Status]
                                ,[Orders_ErrorMessege]
                                ,[Orders_IssueDate]
                            FROM [Orders] WHERE [Orders_Status] =0 ";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BLL_Orders BLL_Orders_Data = new BLL_Orders();
                                BLL_Orders_Data.Orders_ID = (int)reader["Orders_ID"];
                                BLL_Orders_Data.Orders_CommandType = (int)reader["Orders_CommandType"];
                                BLL_Orders_Data.Orders_GetwayID = reader["Orders_GetwayID"] == DBNull.Value ? 0 : (int)reader["Orders_GetwayID"];
                                BLL_Orders_Data.Orders_SchedularID = reader["Orders_SchedularID"] == DBNull.Value ? 0 : (int)reader["Orders_SchedularID"];
                                BLL_Orders_Data.Orders_Payload = reader["Orders_Payload"].ToString();
                                BLL_Orders_Data.Orders_TimeOut = reader["Orders_TimeOut"] == DBNull.Value ? 0 : (int)reader["Orders_TimeOut"];
                                BLL_Orders_Data.Orders_RetransmitNo = (int)reader["Orders_RetransmitNo"];
                                BLL_Orders_Data.Orders_Status = reader["Orders_Status"] == DBNull.Value ? 0 : (int)reader["Orders_Status"];
                                BLL_Orders_Data.Orders_ErrorMessege = reader["Orders_ErrorMessege"].ToString();
                                BLL_Orders_Data.Orders_IssueDate = reader["Orders_IssueDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["Orders_IssueDate"];

                                ordersList.Add(BLL_Orders_Data);
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
            return ordersList;
        }
        public List<BLL_Orders> GetOrdersFromGatwayNum(int Getway_Number)
        {
            List<BLL_Orders> ordersList = new List<BLL_Orders>();

            try
            {
                using (SqlConnection conn = db.Connect())
                {
                    string sql = @"SELECT 
                                    [Orders_ID],
                                    [Orders_CommandType],
                                    [Orders_GetwayID],
                                    [Orders_SchedularID],                          
	                                [Meter_MeterNum],
	                                Aggregation.Aggregation_Number,
                                    [Orders_Payload],
                                    [Orders_TimeOut],
                                    [Orders_RetransmitNo],
                                    [Orders_Status],
                                    [Orders_ErrorMessege],
                                    [Orders_IssueDate]
                                    FROM [eoip-wmm].[dbo].[Orders]
                                    INNER JOIN [Getway] ON [Orders_GetwayID] = [Getway_ID]
                                    LEFT JOIN [SchedularMeter] ON [Orders_SchedularID] = [SchedularMeter_ID]
                                    LEFT JOIN Aggregation on SchedularMeter.SchedularMeter_AggregationID=Aggregation.Aggregation_ID
                                    LEFT JOIN Meter on [SchedularMeter_MeterID]=meter.Meter_ID
                                    WHERE [Getway_Number]=@Getway_Number  AND   [Orders_Status]  !=1
                                ";

                  ///  [Getway_Number],
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                       cmd.Parameters.AddWithValue("@Getway_Number", Getway_Number);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BLL_Orders BLL_Orders_Data = new BLL_Orders();
                                try
                                {
                                    BLL_Orders_Data.Orders_ID = reader.GetInt32(reader.GetOrdinal("Orders_ID"));
                                    string datacommand = reader.GetString(reader.GetOrdinal("Orders_CommandType"));
                                    if(datacommand == "1")
                                    {
                                        BLL_Orders_Data.Orders_CommandType = 1;
                                    }
                                    else
                                    {
                                        BLL_Orders_Data.Orders_CommandType = 0;
                                    }
                                 
                                    BLL_Orders_Data.Orders_GetwayID = reader.IsDBNull(reader.GetOrdinal("Orders_GetwayID")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orders_GetwayID"));
                                    BLL_Orders_Data.Orders_SchedularID = reader.IsDBNull(reader.GetOrdinal("Orders_SchedularID")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orders_SchedularID"));

                                    if (BLL_Orders_Data.Orders_SchedularID > 0)
                                    {
                                        BLL_Orders_Data.Meter_Number = reader.GetInt32(reader.GetOrdinal("Meter_MeterNum"));
                                        BLL_Orders_Data.Aggergation_Number = reader.GetInt32(reader.GetOrdinal("Aggregation_Number"));
                                    }

                                    BLL_Orders_Data.Orders_Payload = reader.IsDBNull(reader.GetOrdinal("Orders_Payload")) ? "" : reader.GetString(reader.GetOrdinal("Orders_Payload"));
                                    BLL_Orders_Data.Orders_TimeOut = reader.IsDBNull(reader.GetOrdinal("Orders_TimeOut")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orders_TimeOut"));
                                    BLL_Orders_Data.Orders_RetransmitNo = reader.GetInt32(reader.GetOrdinal("Orders_RetransmitNo"));
                                    BLL_Orders_Data.Orders_Status = reader.IsDBNull(reader.GetOrdinal("Orders_Status")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orders_Status"));
                                    BLL_Orders_Data.Orders_ErrorMessege = reader.IsDBNull(reader.GetOrdinal("Orders_ErrorMessege")) ? "" : reader.GetString(reader.GetOrdinal("Orders_ErrorMessege"));
                                    BLL_Orders_Data.Orders_IssueDate = reader.IsDBNull(reader.GetOrdinal("Orders_IssueDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Orders_IssueDate"));

                                    ordersList.Add(BLL_Orders_Data);
                                }
                                catch (Exception ex)
                                {
                                 //   Console.WriteLine("Error processing row: " + ex.Message);
                                    // Optionally, you can log the entire row here for further analysis
                                    // Console.WriteLine("Row data: " + string.Join(", ", Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i) + "=" + reader.GetValue(i))));
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, for example, log it
                Console.WriteLine(ex.Message);
                // You might want to throw the exception again if you cannot handle it here
               // throw;
            }

            return ordersList;
        }

        public int Insert(BLL_Orders order)
        {
            int orderId = -1; // Default value in case insertion fails

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    string sql = @"INSERT INTO [Orders] ([Orders_CommandType], [Orders_GetwayID], [Orders_SchedularID], [Orders_Payload], [Orders_TimeOut], [Orders_RetransmitNo], [Orders_Status], [Orders_ErrorMessege], [Orders_IssueDate])
                           VALUES (@Orders_CommandType, @Orders_GetwayID, @Orders_SchedularID, @Orders_Payload, @Orders_TimeOut, @Orders_RetransmitNo, @Orders_Status, @Orders_ErrorMessege, @Orders_IssueDate);
                           SELECT SCOPE_IDENTITY();"; // Fetch the last inserted ID

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Orders_CommandType", order.Orders_CommandType);
                        cmd.Parameters.AddWithValue("@Orders_GetwayID", order.Orders_GetwayID);
                        cmd.Parameters.AddWithValue("@Orders_SchedularID", order.Orders_SchedularID);
                        cmd.Parameters.AddWithValue("@Orders_Payload", order.Orders_Payload);
                        cmd.Parameters.AddWithValue("@Orders_TimeOut", order.Orders_TimeOut);
                        cmd.Parameters.AddWithValue("@Orders_RetransmitNo", order.Orders_RetransmitNo);
                        cmd.Parameters.AddWithValue("@Orders_Status", order.Orders_Status);
                        cmd.Parameters.AddWithValue("@Orders_ErrorMessege", order.Orders_ErrorMessege);
                        cmd.Parameters.AddWithValue("@Orders_IssueDate", order.Orders_IssueDate);

                        // Execute scalar to get the inserted ID
                        orderId = Convert.ToInt32(cmd.ExecuteScalar());
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
            return orderId;
        }

        public bool Update(BLL_Orders order)
        {
            bool success = false; // Default value to indicate failure

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    string sql = @"UPDATE [Orders] 
                           SET 
                              [Orders_Payload] = @Orders_Payload, 
                                 [Orders_Status] = @Orders_Status, 
                               [Orders_ErrorMessege] = @Orders_ErrorMessege 
                               
                           WHERE [Orders_ID] = @Orders_ID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add parameters

                       
                        
                        cmd.Parameters.AddWithValue("@Orders_Payload", order.Orders_Payload);
                       
                        cmd.Parameters.AddWithValue("@Orders_Status", order.Orders_Status);
                        cmd.Parameters.AddWithValue("@Orders_ErrorMessege", order.Orders_ErrorMessege);
                        cmd.Parameters.AddWithValue("@Orders_ID", order.Orders_ID);

                        // Execute the update
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Set success to true if at least one row was affected
                        success = rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately, for example, log it
                    Console.WriteLine(ex.Message);
                    // You might want to throw the exception again if you cannot handle it here
                   // throw;
                }
            }
            return success;
        }

    }
}
