using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using SQLite;

public class SeleniumExample : MonoBehaviour
{
    private AddTextToTextMeshPro addTextToTextMeshPro;


    public void TriggerDatabaseData(string targetName)
    {
        // Call a method to retrieve event data from the database
        Dictionary<string, List<string>> eventData = RetrieveEventDataFromDatabase(targetName);
        // Write the retrieved event data to a text file
        WriteEventDataToFile(eventData);
    }

    public Dictionary<string, List<string>> RetrieveEventDataFromDatabase(string targetName)
    {
        // Initialize dictionary to store event data
        Dictionary<string, List<string>> eventData = new Dictionary<string, List<string>>();

        // Get the current day of the week
        string currentDay = DateTime.Now.DayOfWeek.ToString();

        // Specify the full path to the SQLite database file
        string databasePath = Path.Combine(Application.dataPath, "Resources", "db", targetName + ".db");

        // Check if the file exists
        if (File.Exists(databasePath))
        {
            // Create the connection string
            string connectionString = $"Data Source={databasePath};Version=3;";

            // Open the connection
            using (Mono.Data.Sqlite.SqliteConnection connection = new Mono.Data.Sqlite.SqliteConnection(connectionString))
            {
                connection.Open();

                // Define your query to retrieve event data from a specific table (replace 'YourTableName' with the actual table name)
                string query = $"SELECT * FROM {currentDay}";

                // Create a command to execute the query
                using (Mono.Data.Sqlite.SqliteCommand command = new Mono.Data.Sqlite.SqliteCommand(query, connection))
                {
                    // Execute the query and obtain a reader
                    using (Mono.Data.Sqlite.SqliteDataReader reader = command.ExecuteReader())
                    {
                        // Check if the reader has any rows
                        while (reader.Read())
                        {
                            // Assuming your table has a column named 'EventName'
                            string eventName = reader["class"].ToString();

                            // Assuming your table has a column named 'EventData'
                            string eventDataValue1 = reader["start_date"].ToString();
                            string eventDataValue2 = reader["end_date"].ToString();
                            // Add the event data to the dictionary
                            if (!eventData.ContainsKey(eventName))
                            {
                                eventData[eventName] = new List<string>();
                            }
                            eventData[eventName].Add(eventDataValue1 + " - " + eventDataValue2);
                        }
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Database file does not exist.");
        }

        return eventData;
    }

    void WriteEventDataToFile(Dictionary<string, List<string>> eventData)
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", "schedule.txt");
        if (File.Exists(filePath))
        {
            // Delete the file if it exists
            File.Delete(filePath);
            Debug.Log("Deleted existing file: " + filePath);
        }
        using (StreamWriter writer = new StreamWriter(filePath))
        
                foreach (KeyValuePair<string, List<string>> kvp in eventData)
                {
                    writer.WriteLine("Class: " + kvp.Key);
                    string eventTimes = string.Join(", ", kvp.Value);
                    writer.WriteLine("Event Time: " + eventTimes);
                    writer.WriteLine();
                }

    }
}

