using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text loadText;
    public void NewWorld()
    {
        SceneManager.LoadScene("World Scene");
    }
    
    public void LoadWorld()
    {
        loadText.text = "LOL";
    }
}
