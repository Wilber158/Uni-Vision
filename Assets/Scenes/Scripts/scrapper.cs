using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;  // Ensure you include this for regex supportusing TMPro;
using TMPro;


public class SeleniumExample : MonoBehaviour
{
     [SerializeField]
    private TMP_Dropdown classDropdown;
    [SerializeField] private string dbPath = "db"; // Path to the centralized database under Resources or StreamingAssets

    private string GetDatabasePath()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string originPath = Path.Combine(Application.streamingAssetsPath, $"{dbPath}/university_schedule.db");
        string outputPath = Path.Combine(Application.persistentDataPath, "university_schedule.db");

        if (!File.Exists(outputPath))
        {
            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            if (string.IsNullOrEmpty(reader.error))
            {
                File.WriteAllBytes(outputPath, reader.bytes);
                Debug.Log($"Database copied to: {outputPath}");
                return outputPath;
            }
            else
            {
                Debug.LogError("Failed to load database: " + reader.error);
                return null;
            }
        }
        else
        {
            return outputPath;
        }
#else
        string databasePath = Path.Combine(Application.streamingAssetsPath, $"{dbPath}/university_schedule.db");
        if (!File.Exists(databasePath))
        {   
            Debug.LogError("Database file does not exist: " + databasePath);
            return null;
        }
        return databasePath;
#endif
    }

    private (string, string) ParseClassroomName(string classroomName)
    {
        var match = Regex.Match(classroomName, @"([a-zA-Z]+)(\d+[a-zA-Z]*)");
        if (!match.Success)
        {
            Debug.LogError("Failed to parse classroom name: " + classroomName);
            return (null, null);
        }
        return (match.Groups[1].Value, match.Groups[2].Value);
    }

    public Dictionary<string, List<string>> RetrieveEventDataFromDatabase(string targetName)
    {
        Dictionary<string, List<string>> eventData = new Dictionary<string, List<string>>();
        string currentDay = DateTime.Now.DayOfWeek.ToString();
        string databasePath = GetDatabasePath();

        if (!File.Exists(databasePath))
        {
            Debug.LogError("Database file does not exist: " + databasePath);
            return null;
        }

        string connectionString = $"Data Source={databasePath};Version=3;";
        using (var connection = new Mono.Data.Sqlite.SqliteConnection(connectionString))
        {
            connection.Open();
            var (building, roomNumber) = ParseClassroomName(targetName);
            if (building == null || roomNumber == null)
            {
                return null; // Early return if parsing fails
            }

           string query = $"SELECT Course.dept_id, Course.course_id, CourseTimeSlot.start_time, CourseTimeSlot.end_time " +
               $"FROM CourseTimeSlot " +
               $"JOIN Course ON Course.course_id = CourseTimeSlot.course_id AND Course.dept_id = CourseTimeSlot.dept_id " +
               $"WHERE CourseTimeSlot.building = '{building}' AND CourseTimeSlot.room_number = '{roomNumber}' " +
               $"AND CourseTimeSlot.day_of_week = '{currentDay}'";


            using (var command = new Mono.Data.Sqlite.SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Debug.Log("No data available for today in classroom " + targetName + ".");
                        return null;
                    }

                    while (reader.Read())
                    {
                        string className = $"{reader["dept_id"].ToString()} {reader["course_id"].ToString()}";
                        string eventDataValue1 = reader["start_time"].ToString();
                        string eventDataValue2 = reader["end_time"].ToString();

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

    private void PopulateClassDropdown()
    {
        List<string> classes = GetClassesFromDatabase();
        if (classes == null) return; // If no classes were found or an error occurred

        classDropdown.ClearOptions(); // Clear existing options
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (var className in classes)
        {
            options.Add(new TMP_Dropdown.OptionData(className));
        }

        classDropdown.AddOptions(options); // Add new options to the TMP Dropdown
    }

    private List<string> GetClassesFromDatabase()
    {
        List<string> classes = new List<string>();
        string databasePath = GetDatabasePath();
        if (databasePath == null) return null;

        string connectionString = $"Data Source={databasePath};Version=3;";
        using (var connection = new Mono.Data.Sqlite.SqliteConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT DISTINCT dept_id || ' ' || course_id AS class FROM Course";
            using (var command = new Mono.Data.Sqlite.SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string className = reader["class"].ToString();
                        classes.Add(className);
                    }
                }
            }
        }
        return classes;
    }
}
