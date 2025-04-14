using System;
using System.IO;
using Newtonsoft.Json;
using WH_Panel;
using System.Windows.Forms;
public static class SettingsManager
{
    private static string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
    public static void SaveSettings(AppSettings settings)
    {
        try
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(settingsFilePath, json);
            //MessageBox.Show("Settings saved successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save settings: {ex.Message}");
        }
    }
    public static AppSettings LoadSettings()
    {
        try
        {
            // Print the current directory for debugging purposes
            //MessageBox.Show($"Current Directory: {Directory.GetCurrentDirectory()}");
            //MessageBox.Show($"Settings File Path: {settingsFilePath}");
            if (!File.Exists(settingsFilePath))
            {
                MessageBox.Show($"File not found: {settingsFilePath}");
                return new AppSettings(); // Return default settings if file does not exist
            }
            string json = File.ReadAllText(settingsFilePath);
            //MessageBox.Show($"Settings JSON: {json}");
            var settings = JsonConvert.DeserializeObject<AppSettings>(json);
            if (settings == null)
            {
                MessageBox.Show("Failed to deserialize settings.");
                return new AppSettings(); // Return default settings if deserialization fails
            }
            return settings;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load settings: {ex.Message}");
            return new AppSettings(); // Return default settings if an error occurs
        }
    }
}
