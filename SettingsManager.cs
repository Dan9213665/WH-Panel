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
        string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(settingsFilePath, json);
    }

    public static AppSettings LoadSettings()
    {
        // Print the current directory for debugging purposes
       // MessageBox.Show($"Current Directory: {Directory.GetCurrentDirectory()}");
      //  MessageBox.Show($"Settings File Path: {settingsFilePath}");

        if (!File.Exists(settingsFilePath))
        {
            MessageBox.Show($"File not found: {settingsFilePath}");
            return new AppSettings(); // Return default settings if file does not exist
        }

        string json = File.ReadAllText(settingsFilePath);

        //MessageBox.Show(json);
        return JsonConvert.DeserializeObject<AppSettings>(json);
    }
}