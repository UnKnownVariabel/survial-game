using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// An instance of OpenSave is attached to each button which represents a save file.
public class OpenSave : MonoBehaviour
{
    // If button selected.
    private bool _selected;

    public bool selected
    {
        get
        {
            return _selected;
        }
        set
        {
            _selected = value;
            if (value)
            {
                image.color = selectedColor;
            }
            else
            {
                image.color = color;
            }
        }
    }

    public TMP_Text text;
    public string path;
    public MainMenu mainMenu;

    [SerializeField] private Image image;
    [SerializeField] private Color color;
    [SerializeField] private Color selectedColor;

    // Open this buttons savefile.
    public void Open()
    {
        Save.loadPath = path;
        WorldGeneration.isCreating = false;
        SceneManager.LoadScene("World Scene");
    }

    // Is called if button is clicked.
    public void Clicked()
    {
        if (selected)
        {
            Open();
        }
        else
        {
            mainMenu.worldSelection = this;
        }
    }

    // Deletes save file and button.
    public void Delete()
    {
        System.IO.File.Delete(path);
        Destroy(gameObject);
    }
}
