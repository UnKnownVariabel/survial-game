using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewWorld()
    {
        WorldGeneration.isCreating = true;
        SceneManager.LoadScene("World Scene");
    }
    
    public void LoadWorld()
    {
        WorldGeneration.isCreating = false;
        SceneManager.LoadScene("World Scene");
    }
}
