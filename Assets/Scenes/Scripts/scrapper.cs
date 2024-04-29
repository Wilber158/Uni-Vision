using UnityEngine;
using System.Collections.Generic;
using System.IO;
using SQLite;
using System;   

public class SeleniumExample : MonoBehaviour
{
    [SerializeField] private string dbDirectory = "db"; // Folder name under Resources

    private string GetDatabasePath(string targetName)
    {
        string databasePath;
        #if UNITY_ANDROID && !UNITY_EDITOR
            // On Android, you need to use WWW or UnityWebRequest to extract the file into a writable location
            string originPath = Path.Combine(Application.streamingAssetsPath, $"{dbDirectory}/{targetName}.db");
            string outputPath = Path.Combine(Application.persistentDataPath, $"{targetName}.db");

            if (!File.Exists(outputPath))
            {
                WWW reader = new WWW(originPath);
                while (!reader.isDone) { }

                if (string.IsNullOrEmpty(reader.error))
                {
                    File.WriteAllBytes(outputPath, reader.bytes);
                    Debug.Log($"Database copied to: {outputPath}");
                    databasePath = outputPath;
                }
                else
                {
                    Debug.LogError("Failed to load database: " + reader.error);
                    return null;
                }
            }
            else
            {
                databasePath = outputPath;
            }
        #else
            // For other platforms, directly use the file from StreamingAssets
            databasePath = Path.Combine(Application.streamingAssetsPath, $"{dbDirectory}/{targetName}.db");
            if (!File.Exists(databasePath))
            {   
                Debug.LogError("Database file does not exist: " + databasePath);
                return null;
            }
        #endif
            return databasePath;
    }



    public Dictionary<string, List<string>> RetrieveEventDataFromDatabase(string targetName)
    {
        Dictionary<string, List<string>> eventData = new Dictionary<string, List<string>>();
        string currentDay = DateTime.Now.DayOfWeek.ToString();
        string databasePath = GetDatabasePath(targetName);

        if (!File.Exists(databasePath))
        {
            Debug.LogError("Database file does not exist: " + databasePath);
            return null;
        }

        string connectionString = $"Data Source={databasePath};Version=3;";
        using (var connection = new Mono.Data.Sqlite.SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT * FROM {currentDay}";

            using (var command = new Mono.Data.Sqlite.SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Debug.Log("No data available for today.");
                        return null;
                    }

                    while (reader.Read())
                    {
                        string className = reader["class"].ToString();
                        string eventDataValue1 = reader["start_date"].ToString();
                        string eventDataValue2 = reader["end_date"].ToString();
                        
                        if (!eventData.ContainsKey(className))
                        {
                            eventData[className] = new List<string>();
                        }

                        eventData[className].Add($"{eventDataValue1} - {eventDataValue2}");
                    }
                }
            }
        }

        return eventData;
    }
}
