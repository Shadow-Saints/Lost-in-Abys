using System;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    #region Variables
    public delegate void saveGame();

    public static event saveGame OnsavedGame;

    public delegate void ChangeMenu();

    public static event ChangeMenu OnChangeMenu;

    public delegate void LoadMenu();

    public static event LoadMenu OnLoadMenu;

    public static GameController instance;

    [SerializeField] private float _fov;

    [SerializeField] private float sense;

    [SerializeField] private TextMeshPro _barText;

    [SerializeField] private TextMeshProUGUI[] _tutorialText;

    [SerializeField] float barUnit;

    [SerializeField] private string sceneName;

    private GameObject config;

    private GameObject InicialText;

    #endregion

    #region UnityCallBacks
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        GetPression();
        OnsavedGame += SavePression;
        OnChangeMenu += SaveFov;
        OnChangeMenu += SaveSense;
        OnLoadMenu += LoadFov;
        OnLoadMenu += LoadSense;
        OnsavedGame += SavePlayerPosition;

        LoadFov();
        LoadSense();

    }

    #endregion

    #region Positions 
    private void SavePlayerPosition()
    {
        Player player = FindAnyObjectByType<Player>();
        Vector3 playerPosition = player.transform.position;
        float x = playerPosition.x;
        float y = playerPosition.y;
        float z = playerPosition.z;
        Save save = FindAnyObjectByType<Save>();
        save.UpdatePossitionValues(0, x, y, z);
    }

    public Vector3 GetPlayerPosition()
    {
        Vector3 positions = new Vector3(0, 6.251f, 0);
        Save save = GetComponent<Save>();
        IDataReader read = save.ReadPossition(0);
        while (read.Read())
        {
            positions.x = read.GetFloat(1);
            positions.y = read.GetFloat(2);
            positions.z = read.GetFloat(3);
        }
        
        return positions;
        
    }
    #endregion

    #region Pression
    public void SomarPresaoo(float atm)
    {
        barUnit += atm;
        _barText.text = barUnit.ToString();
    }

    public void MultiplicarPresaoo(float atm)
    {
        barUnit *= atm;
        _barText.text = barUnit.ToString();
    }
    public void DividirPresaoo(float atm)
    {
        barUnit /= atm;
        _barText.text = barUnit.ToString();
    }

    private void SavePression()
    {
        Save save = GetComponent<Save>();
        if (save != null)
        {
            save.UpdateHudValues(0, barUnit, true);
            save.Close();
        }
        else
        {
            Debug.LogWarning("FUDEU!");
        }
    }

    public void GetPression()
    {
        Save save = GetComponent<Save>();
        IDataReader read = save.ReadHudValues(0);
        while (read.Read())
        {
            barUnit = read.GetFloat(1);
        }
        save.Close();

        if (_barText != null)
        {
            _barText.text = barUnit.ToString();
        }
    }

    public void SaveGame()
    {
        OnsavedGame();
    }

    #endregion

    #region Tutorial
    public void ActiveTutorial(int tutorialCode)
    {
        _tutorialText[tutorialCode].gameObject.SetActive(true);
    }
    public void DisableTutorial(int tutorialCode)
    {
        _tutorialText[tutorialCode].gameObject.SetActive(false);
    }
    #endregion

    #region setings
    public void Play()
    {
        SceneManager.LoadScene(sceneName);

        HealthAndVariables health = FindFirstObjectByType<HealthAndVariables>();
        health.GetSliders();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void changeSense()
    {
        Slider slider = GameObject.FindGameObjectWithTag("Sense Menu").GetComponent<Slider>();

        sense = slider.value;
        if (sense > 200f)
        {
            sense = 200f;
        }
    }

    public void changeFov()
    {
        Slider fovSlider = GameObject.FindGameObjectWithTag("Fov Menu").GetComponent<Slider>();
        _fov = fovSlider.value;
        if (_fov > 100f)
        {
            _fov = 100f;
        }
    }

    #region Save and Load
    private void SaveSense()
    {
        Save save = GetComponent<Save>();
        if (save != null)
        {
            save.UpdateHudValues(2, sense, true);
            save.Close();
        }
        else
        {
            Debug.LogWarning("FUDEU!");
        }
    }

    private void SaveFov()
    {
        Save save = GetComponent<Save>();
        if (save != null)
        {
            save.UpdateHudValues(3, _fov, true);
            save.Close();
        }
        else
        {
            Debug.LogWarning("FUDEU!");
        }

    }




    private void LoadFov()
    {
        Save save = GetComponent<Save>();
        IDataReader read = save.ReadHudValues(3);
        while (read.Read())
        {
            _fov = read.GetFloat(1);
            Debug.Log("Fov: " + _fov);
        }
        save.Close();


        Camera mcam = Camera.main;
        if (mcam != null)
        {
            mcam.fieldOfView = _fov;
            Debug.Log(mcam);
            Debug.Log(mcam.fieldOfView);
        }
        else
        {
            Debug.LogError("Camera has not founded");
        }


    }

    private void LoadSense()
    {
        Save save = GetComponent<Save>();
        IDataReader read = save.ReadHudValues(2);
        while (read.Read())
        {
            sense = read.GetFloat(1);
            Debug.Log("Sense: " + sense);
        }
        save.Close();


        if (SceneManager.GetActiveScene().name == "Jogo")
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            if (player != null)
            {
                player.ChangeSense(sense);
            }
            else
            {
                Debug.LogError("Player has not founded");
            }
        }


    }

    public void changeMenuState()
    {
        OnChangeMenu();
    }

    private IEnumerator whait1mili()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public void loadMenuState()
    {
        OnLoadMenu();
    }

    public void activeSettings()
    {
        Slider sliderS = GameObject.FindGameObjectWithTag("Sense Menu").GetComponent<Slider>();

        if (sliderS != null)
        {
            sliderS.value = sense;
        }
        else
        {
            Debug.LogError("Seu Inutil!");
        }

        Slider sliderF = GameObject.FindGameObjectWithTag("Fov Menu").GetComponent<Slider>();

        if (sliderF != null)
        {
            sliderF.value = _fov;
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        if (config == null) { Debug.LogError("FUDEUDEVEZ"); }
        config.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        config.SetActive(false);
        Time.timeScale = 1f;
    }

    public void getBarText()
    {
        _barText = FindFirstObjectByType<TextMeshPro>(); 
        Debug.Log(_barText);
    }

    public void getMenuConfig()
    {
        config = GameObject.FindGameObjectWithTag("Pause").GameObject();
        if (config != null)
        {
            config.SetActive(false);
        }
    }

    public void GetInitialText()
    {
        InicialText = GameObject.FindWithTag("TUTORIAL").GameObject();
        if (InicialText != null)
        {
            InicialText.SetActive(true);
            StartCoroutine(whait15());
        }
    }

    private IEnumerator whait15()
    {
        yield return new WaitForSeconds(10f);
        InicialText.SetActive(false);
    }

    #endregion
    #endregion
}