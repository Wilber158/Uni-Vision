using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class SearchManager : MonoBehaviour
{
    
    public TMP_InputField BuildingChoice;
    public TMP_InputField RoomChoice;
    public Button searchButton;

    void Start()
    {
        searchButton.onClick.AddListener(() => LoadSceneBasedOnSearch());
    }

    void LoadSceneBasedOnSearch()
    {
        string buildingName = BuildingChoice.text;
        string roomName = RoomChoice.text;
        string sceneName = buildingName + roomName; // Assuming scene names are formatted as "BuildingNameRoomName"

        // Check if the scene is in the build settings
        if (SceneIsInBuildSettings(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("Scene not found in build settings: " + sceneName);
            // Optionally, show a message to the user that the scene was not found
        }
    }

    bool SceneIsInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(path);
            if (sceneNameFromPath == sceneName)
                return true;
        }
        return false;
    }
}
