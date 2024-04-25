using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using SQLite; // Assuming you are using a plugin that supports SQLite directly with Unity

public class SeleniumExample : MonoBehaviour
{
    [SerializeField] private string dbDirectory = "Resources/db"; // Directory where databases are stored relative to Application.dataPath

    public event Action OnDataProcessedSuccessfully;
    public event Action<string> OnDataProcessingFailed;

    public void TriggerDatabaseData(string targetName, Action onComplete)
    {
        Dictionary<string, List<string>> eventData = RetrieveEventDataFromDatabase(targetName);
        if (eventData != null)
        {
            // Ensure the targetName is also passed to the WriteEventDataToFile method
            if (WriteEventDataToFile(eventData, targetName)) // Check if writing is successful
            {
                onComplete?.Invoke(); // Call the onComplete action if it's not null
                OnDataProcessedSuccessfully?.Invoke(); // Trigger the successful processing event
            }
            else
            {
                Debug.LogError("Failed to write data to file.");
                OnDataProcessingFailed?.Invoke("Failed to write data to file.");
            }
        }
        else
        {
            Debug.LogError("Failed to retrieve data from database.");
            OnDataProcessingFailed?.Invoke("Failed to retrieve data from database.");
        }
    }




    private Dictionary<string, List<string>> RetrieveEventDataFromDatabase(string targetName)
    {
        Dictionary<string, List<string>> eventData = new Dictionary<string, List<string>>();
        string currentDay = DateTime.Now.DayOfWeek.ToString();
        string databasePath = Path.Combine(Application.dataPath, dbDirectory, $"{targetName}.db");

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

    private bool WriteEventDataToFile(Dictionary<string, List<string>> eventData, string targetName)
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", $"{targetName}_schedule.txt");
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var entry in eventData)
                {
                    writer.WriteLine($"Class: {entry.Key}");
                    foreach (var time in entry.Value)
                    {
                        writer.WriteLine($"Event Time: {time}");
                    }
                    writer.WriteLine();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error writing to file: " + ex.Message);
            return false;
        }
    }
}
