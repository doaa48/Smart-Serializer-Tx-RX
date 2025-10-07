using System;
using System.IO;
using System.Text.Json;
namespace JsonConfigrations
{

    public class JsonConfig
    {
        public class AppSettingsConfig
        {
            public string ComPort { get; set; }
            public int UART_BaudRate { get; set; }
            public string UART_Parity { get; set; }
            public int UART_DataBits { get; set; }
            public int UART_StopBits { get; set; }
            public int GatewayNumber { get; set; }
            public string TxLog { get; set; }
            public string RxLog { get; set; }
        }

        public JsonConfig()
        { }

        private const string SettingsFilePath = "C:\Users\user\Desktop\Doaa\ConfigParam.json";

        public void SaveSettings(AppSettings settings)
        {
            string jsonString = JsonSerializer.Serialize(settings);
            File.WriteAllText(SettingsFilePath, jsonString);
        }

        public AppSettings LoadSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                string jsonString = File.ReadAllText(SettingsFilePath);
                return JsonSerializer.Deserialize<AppSettings>(jsonString);
            }

            // If the file doesn't exist, return a default settings object or handle it accordingly
            return new AppSettings();
        }
    }

}
/*

class Program
{
    static void Main()
    {
        // Set initial values
        AppSettings initialSettings = new AppSettings
        {
            ComPort = "COM1",
            UART_BaudRate = 9600,
            UART_Parity = "None",
            UART_DataBits = 8,
            UART_StopBits = 1,
            GatewayNumber = 0
        };

        // Save initial settings
        JsonConfig.SaveSettings(initialSettings);

        // Read settings
        JsonConfig.AppSettings loadedSettings = JsonConfig.LoadSettings();

        // Now you can use the loaded settings
        Console.WriteLine($"ComPort: {loadedSettings.ComPort}");
        Console.WriteLine($"BaudRate: {loadedSettings.UART_BaudRate}");
        // ... and so on
    }
}
*/