using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenSave : MonoBehaviour
{
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
            selectionImage.enabled = value;
        }
    }

    public TMP_Text text;
    public string path;
    public MainMenu mainMenu;

    [SerializeField] private Image selectionImage;

    public void Open()
    {
        Save.loadPath = path;
        WorldGeneration.isCreating = false;
        SceneManager.LoadScene("World Scene");
    }

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
    public void Delete()
    {
        System.IO.File.Delete(path);
        Destroy(gameObject);
    }
}
