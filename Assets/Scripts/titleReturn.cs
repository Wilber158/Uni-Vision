using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    public GameObject targetGameObject; // GameObject variable to hold the reference
    private Camera mainCamera; // Reference to the main camera

    void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            string title = GetTitle();
            Debug.Log("Title of the object: " + title); // Log the title to the console
            if (targetGameObject != null)
            {
                // Make the main camera point at the collider
                mainCamera.transform.LookAt(targetGameObject.transform.position);

                // Execute the script attached to the targetGameObject if it has one
                scrapper scriptToRun = targetGameObject.GetComponent<scrapper>();
                if (scriptToRun != null)
                {
                    scriptToRun.Execute();
                }
            }
        }
    }

    private string GetTitle()
    {
        return gameObject.name;
    }
}

public class scrapper : MonoBehaviour
{
    public void Execute()
    {
        NotificationManager.SendNotification("Target Detected", "Target is pointed at!");
    }
}

public static class NotificationManager
{
    // This is a placeholder method for sending notifications.
    public static void SendNotification(string title, string message)
    {
        // Replace this with actual code to send notifications to the user.
        Debug.Log("Notification: " + title + " - " + message);
    }
}

