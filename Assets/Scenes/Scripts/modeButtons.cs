using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class modeButtons : MonoBehaviour
{
public void modebuttons(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
