using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class SceneUIManager : MonoBehaviour
{
    public GameObject canvasPrefab;
    public string[] scenesWithCanvas = { "AccountPage", "Main Screen" }; // Add scene names here

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }




    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!Array.Exists(scenesWithCanvas, element => element == scene.name))
        {
            Instantiate(canvasPrefab);

            // Also ensure there's an EventSystem
            if (FindObjectOfType<EventSystem>() == null)
            {
                Instantiate(new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule)));
            }
        }
        else
        {
            // If the scene IS in the scenesWithCanvas array, destroy the persistent GameObject
            Destroy(gameObject);
        }
    }


    /*

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Array.Exists(scenesWithCanvas, element => element == scene.name))
        {
            // If the GameObject finds itself in a scene listed in the scenesWithCanvas array, destroy it
            Destroy(gameObject);
        }
        else
        {
            Instantiate(canvasPrefab);
            // In other scenes, check for and create Canvas and EventSystem if they don't exist
            if (FindObjectOfType<Canvas>() == null)
            {
                Instantiate(canvasPrefab);
            }
            if (FindObjectOfType<EventSystem>() == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }
    }
    */
}


/*
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class SceneUIManager : MonoBehaviour
{
    public GameObject canvasPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keep this GameObject persistent
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Instantiate(canvasPrefab);
        // Check if Canvas already exists in the scene
        if (FindObjectOfType<Canvas>() == null)
        {
            Instantiate(canvasPrefab);
        }
        if (FindObjectOfType<EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }
}


*/