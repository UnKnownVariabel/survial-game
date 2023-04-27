using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Main menu controles the main menu and some sub menus.
public class MainMenu : MonoBehaviour
{
    // world selection is the save selected in the load world menu.
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
    [SerializeField] private GameObject nameMenu;
    [SerializeField] private GameObject leaderboard;
    [SerializeField] private OpenSave buttonPref;
    [SerializeField] private TMP_InputField field;
    [SerializeField] private Transform saves;
    [SerializeField] private GameObject options;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_Text nameWarningText;
    [SerializeField] private TMP_Text nameText;

    // Start is called before the first frame update.
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

        if (GameManager.playerName == "" || GameManager.playerName == null)
        {
            OpenNameMenu();
        }
        else
        {
            nameText.text = GameManager.playerName;
        }

        StartCoroutine(Leaderboard.LoginRoutine(SystemInfo.deviceUniqueIdentifier));

        if (!PlayerPrefs.HasKey("music volume"))
        {
            PlayerPrefs.SetFloat("music volume", 0.5f);
        }
        if (!PlayerPrefs.HasKey("effects volume"))
        {
            PlayerPrefs.SetFloat("effects volume", 0.5f);
        }
        if(!PlayerPrefs.HasKey("auto save"))
        {
            PlayerPrefs.SetInt("auto save", 1);
        }
    }

    // Opens the new world menu.
    public void NewWorld()
    {
        createMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    // Creates a new world and loads it if name fits criteria.
    public void CreateWorld()
    {
        // Checking length of name.
        if(field.text.Length > 16)
        {
            warningText.enabled = true;
            warningText.text = "name has to be less than 17 characters";
            return;
        }
        if(field.text.Length < 1)
        {
            warningText.enabled = true;
            warningText.text = "name field is empty";
            return;
        }

        // Checking if name is already used in for other world.
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
    
    // Opens load world menu.
    public void LoadWorld()
    {
        loadMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    // Opens settings.
    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    // Goes back to main menu from sub menu.
    public void Back()
    {
        createMenu.SetActive(false);
        loadMenu.SetActive(false);
        settingsMenu.SetActive(false);
        nameMenu.SetActive(false);
        leaderboard.SetActive(false);
        mainMenu.SetActive(true);
    }
    
    // Loads savefile from worldSelection.
    public void Open()
    {
        if(worldSelection != null)
        {
            worldSelection.Open();
        }
    }
    
    // Deletes world selection.
    public void Delete()
    {
        if (worldSelection != null)
        {
            worldSelection.Delete();
        }
    }

    // Opens tutorial scene.
    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    // Sets name.
    public void SetName()
    {
        if(nameField.text != "")
        {
            GameManager.playerName = nameField.text;
            nameText.text = nameField.text;
            Leaderboard.SetPlayerName();
            nameMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
        else
        {
            nameWarningText.text = "you have to write a name";
            nameWarningText.enabled = true;
        }
    }

    // Opens name menu.
    public void OpenNameMenu()
    {
        nameWarningText.enabled = false;
        nameField.text = GameManager.playerName;
        nameMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    // Open leaderboard.
    public void OpenLeaderboard()
    {
        leaderboard.SetActive(true);
        mainMenu.SetActive(false);
    }
}
