using System;
using System.IO;
using Newtonsoft.Json;


namespace WaterMeter_id
{
    public class JsonSettingsManager
    {

        // Get the current directory path
        private string CurrentDirectory => Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        // Combine it with the folder and file name when requested
        public string SettingsFilePath => Path.Combine(CurrentDirectory, "JsonConfigr", "ConfigParam.json");

        public void SaveSettings(BLL_JsonSettings settings)
        {

            // Get the directory path without the file name
            string directoryPath = Path.GetDirectoryName(SettingsFilePath);

            // Check if the directory exists, and create it if not
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Serialize and save settings
            string jsonString = JsonConvert.SerializeObject(settings);
            File.WriteAllText(SettingsFilePath, jsonString);
            
        }

        public BLL_JsonSettings LoadSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                string jsonString = File.ReadAllText(SettingsFilePath);
                return JsonConvert.DeserializeObject<BLL_JsonSettings>(jsonString);
            }

            // If the file doesn't exist, return a default settings object or handle it accordingly
            return new BLL_JsonSettings();
        }
    }

}

