using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterMeter_id.SEL;


namespace WaterMeter_id
{
    public class DAL_Configurations
    {
        Database db = new Database();

        #region SelectData

        #endregion

        #region Insert Data
        public bool Insert(BLL_Configurations configuration)
        {
            bool Status = false;
            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to insert configuration data
                    string sql = @"INSERT INTO Configurations 
                           (Confuguration_TryToRemoveTheMeterCover,
                            Confuguration_PlaceTheMeterInMagneticField,
                            Confuguration_RemoveTheBattery,
                            Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit,
                            Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening,
                            Confuguration_OfftimeID,
                            Confuguration_PriceschdulerID,
                            Confuguration_ClientMeterAction,
                            Confuguration_ClientMaxOverDraft,
                            Confuguration_ClientOffWarningLimit,
                            Confuguration_EnableVavePeriod)
                           VALUES 
                           (@TryToRemoveTheMeterCover,
                            @PlaceTheMeterInMagneticField,
                            @RemoveTheBattery,
                            @InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit,
                            @InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening,
                            @OfftimeID,
                            @PriceschdulerID,
                            @ClientMeterAction,
                            @ClientMaxOverDraft,
                            @ClientOffWarningLimit,
                            @EnableVavePeriod)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Set parameters
                        cmd.Parameters.AddWithValue("@TryToRemoveTheMeterCover", configuration.Confuguration_TryToRemoveTheMeterCover);
                        cmd.Parameters.AddWithValue("@PlaceTheMeterInMagneticField", configuration.Confuguration_PlaceTheMeterInMagneticField);
                        cmd.Parameters.AddWithValue("@RemoveTheBattery", configuration.Confuguration_RemoveTheBattery);
                        cmd.Parameters.AddWithValue("@InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit", configuration.Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit);
                        cmd.Parameters.AddWithValue("@InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening", configuration.Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening);
                        cmd.Parameters.AddWithValue("@OfftimeID", configuration.Confuguration_OfftimeID);
                        cmd.Parameters.AddWithValue("@PriceschdulerID", configuration.Confuguration_PriceschdulerID);
                        cmd.Parameters.AddWithValue("@ClientMeterAction", configuration.Confuguration_ClientMeterAction);
                        cmd.Parameters.AddWithValue("@ClientMaxOverDraft", configuration.Confuguration_ClientMaxOverDraft);
                        cmd.Parameters.AddWithValue("@ClientOffWarningLimit", configuration.Confuguration_ClientOffWarningLimit);
                        cmd.Parameters.AddWithValue("@EnableVavePeriod", configuration.Confuguration_EnableVavePeriod);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                        Status = true;
                    }
                }
                catch (Exception ex)
                {
                    Status = false;
                    MessageBox.Show(ex.Message);
                    throw; // Re-throw the exception for higher-level handling
                }
            }
            return Status;
        }

        #endregion

        #region Update Data
        public bool Update(BLL_Configurations configuration)
        {
            bool success = false;
            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to update configuration data
                    string sql = @"UPDATE Configurations SET 
                           Confuguration_TryToRemoveTheMeterCover = @TryToRemoveTheMeterCover,
                           Confuguration_PlaceTheMeterInMagneticField = @PlaceTheMeterInMagneticField,
                           Confuguration_RemoveTheBattery = @RemoveTheBattery,
                           Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit = @InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit,
                           Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening = @InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening,
                           Confuguration_OfftimeID = @OfftimeID,
                           Confuguration_PriceschdulerID = @PriceschdulerID,
                           Confuguration_ClientMeterAction = @ClientMeterAction,
                           Confuguration_ClientMaxOverDraft = @ClientMaxOverDraft,
                           Confuguration_ClientOffWarningLimit = @ClientOffWarningLimit,
                           Confuguration_EnableVavePeriod = @EnableVavePeriod
                           WHERE Configuration_ID = @Configuration_ID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Set parameters
                        cmd.Parameters.AddWithValue("@TryToRemoveTheMeterCover", configuration.Confuguration_TryToRemoveTheMeterCover);
                        cmd.Parameters.AddWithValue("@PlaceTheMeterInMagneticField", configuration.Confuguration_PlaceTheMeterInMagneticField);
                        cmd.Parameters.AddWithValue("@RemoveTheBattery", configuration.Confuguration_RemoveTheBattery);
                        cmd.Parameters.AddWithValue("@InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit", configuration.Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit);
                        cmd.Parameters.AddWithValue("@InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening", configuration.Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening);
                        cmd.Parameters.AddWithValue("@OfftimeID", configuration.Confuguration_OfftimeID);
                        cmd.Parameters.AddWithValue("@PriceschdulerID", configuration.Confuguration_PriceschdulerID);
                        cmd.Parameters.AddWithValue("@ClientMeterAction", configuration.Confuguration_ClientMeterAction);
                        cmd.Parameters.AddWithValue("@ClientMaxOverDraft", configuration.Confuguration_ClientMaxOverDraft);
                        cmd.Parameters.AddWithValue("@ClientOffWarningLimit", configuration.Confuguration_ClientOffWarningLimit);
                        cmd.Parameters.AddWithValue("@EnableVavePeriod", configuration.Confuguration_EnableVavePeriod);
                        cmd.Parameters.AddWithValue("@Configuration_ID", configuration.Configuration_ID);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();
                        success = (rowsAffected > 0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    // You may handle the exception here if needed
                }
            }
            return success;
        }

        #endregion


        public BLL_Configurations Select()
        {
            BLL_Configurations configurationData = new BLL_Configurations();

            using (SqlConnection conn = db.Connect())
            {
                try
                {
                    // SQL Query to get configuration data
                    string sql = @"SELECT 
                                        Configuration_ID,
                                        Confuguration_TryToRemoveTheMeterCover,
                                        Confuguration_PlaceTheMeterInMagneticField,
                                        Confuguration_RemoveTheBattery,
                                        Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit,
                                        Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening,
                                        Confuguration_OfftimeID,
                                        Confuguration_PriceschdulerID,
                                        Confuguration_ClientMeterAction,
                                        Confuguration_ClientMaxOverDraft,
                                        Confuguration_ClientOffWarningLimit,
                                        Confuguration_EnableVavePeriod,
                                        PriceSchedule.PriceSchedule_Name,
                                        OFFTime.OFFTime_Name
                           FROM Configurations
                                   INNER JOIN PriceSchedule ON Configurations.Confuguration_PriceschdulerID=PriceSchedule.PriceSchedule_ID 
                                   INNER JOIN OFFTime ON Configurations.Confuguration_OfftimeID=OFFTime.OFFTime_ID 
                                      ";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Execute the query and retrieve the result set
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                configurationData.Configuration_ID = Convert.ToInt32(reader["Configuration_ID"]);
                                configurationData.Confuguration_TryToRemoveTheMeterCover = Convert.ToInt32(reader["Confuguration_TryToRemoveTheMeterCover"]);
                                configurationData.Confuguration_PlaceTheMeterInMagneticField = Convert.ToInt32(reader["Confuguration_PlaceTheMeterInMagneticField"]);
                                configurationData.Confuguration_RemoveTheBattery = Convert.ToInt32(reader["Confuguration_RemoveTheBattery"]);
                                configurationData.Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit = Convert.ToInt32(reader["Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit"]);
                                configurationData.Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening = Convert.ToInt32(reader["Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening"]);
                                configurationData.Confuguration_OfftimeID = Convert.ToInt32(reader["Confuguration_OfftimeID"]);
                                configurationData.Confuguration_PriceschdulerID = Convert.ToInt32(reader["Confuguration_PriceschdulerID"]);
                                configurationData.Confuguration_ClientMeterAction = Convert.ToInt32(reader["Confuguration_ClientMeterAction"]);
                                configurationData.Confuguration_ClientMaxOverDraft = Convert.ToInt32(reader["Confuguration_ClientMaxOverDraft"]);
                                configurationData.Confuguration_ClientOffWarningLimit = Convert.ToInt32(reader["Confuguration_ClientOffWarningLimit"]);
                                configurationData.Confuguration_EnableVavePeriod = Convert.ToInt32(reader["Confuguration_EnableVavePeriod"]);
                                
                                configurationData.Confuguration_PriceschdulerName   = reader["PriceSchedule_Name"].ToString();
                                configurationData.Confuguration_OfftimeName         = reader["OFFTime_Name"].ToString();

                                return configurationData;
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

            return null;
        }

    }
}
