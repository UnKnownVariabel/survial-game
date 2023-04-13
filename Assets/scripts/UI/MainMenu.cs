using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private OpenSave _worldSelection;
    public OpenSave worldSelection
    {
        get { return _worldSelection; }
        set
        {
            if(_worldSelection != null)
            {
                _worldSelection.selected = false;
            }
            _worldSelection = value;
            options.SetActive(value != null);
            if (_worldSelection != null)
            {
                _worldSelection.selected = true;
            }
        }
    }

    [SerializeField] private GameObject loadMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject createMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private OpenSave buttonPref;
    [SerializeField] private TMP_InputField field;
    [SerializeField] private Transform saves;
    [SerializeField] private GameObject options;
    [SerializeField] private TMP_Text warningText;

    private void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
        var info = new DirectoryInfo(Application.persistentDataPath + "/saves");
        var fileInfo = info.GetFiles();
        foreach(FileInfo file in fileInfo)
        {
            string name = file.Name;
            name = name.Remove(name.Length - 5);
            OpenSave button = Instantiate(buttonPref, new Vector3(0, 0, 0), Quaternion.identity, saves);
            button.text.text = name;
            button.path = file.ToString();
            button.mainMenu = this;
            //Debug.Log(name);
        }
    }
    public void NewWorld()
    {
        createMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void CreateWorld()
    {
        // checking length of name
        if(field.text.Length > 9)
        {
            warningText.enabled = true;
            warningText.text = "name has to be less than 10 characters";
            return;
        }
        if(field.text.Length < 1)
        {
            warningText.enabled = true;
            warningText.text = "name field is empty";
            return;
        }

        // checking if name is already used in for other world
        var info = new DirectoryInfo(Application.persistentDataPath + "/saves");
        var fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
        {
            string name = file.Name;
            name = name.Remove(name.Length - 5);
            if (name == field.text)
            {
                warningText.enabled = true;
                warningText.text = "name is already used";
                return;
            }
        }

        WorldGeneration.isCreating = true;
        WorldData.current_name = field.text;
        SceneManager.LoadScene("World Scene");
    }
    
    public void LoadWorld()
    {
        loadMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void Back()
    {
        createMenu.SetActive(false);
        loadMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    
    public void Open()
    {
        if(worldSelection != null)
        {
            worldSelection.Open();
        }
    }

    public void Delete()
    {
        if (worldSelection != null)
        {
            worldSelection.Delete();
        }
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
