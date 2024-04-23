using System.Collections;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public delegate void saveGame();
    
    public static event saveGame OnsavedGame;

    public delegate void ChangeMenu();

    public static event ChangeMenu OnChangeMenu;

    public delegate void LoadMenu();

    public static event LoadMenu OnLoadMenu;

    public static GameController instance;

    [SerializeField] private float _fov;

    [SerializeField] private float sense;

    [SerializeField] private TextMeshProUGUI _barText;

    [SerializeField] private TextMeshProUGUI[] _tutorialText;

    [SerializeField] float barUnit;

    [SerializeField] private string sceneName;


    #region UnityCallBacks
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null) 
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        LoadPression();
        OnsavedGame += SalvarPressao;
        OnChangeMenu += SaveFov;
        OnChangeMenu += SaveSense;
        OnLoadMenu += LoadFov;
        OnLoadMenu += LoadSense;

        LoadFov();
        LoadSense();

    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.L)) 
        {
            if(OnsavedGame != null)
            {
                OnsavedGame();
            }
        }
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

    private void SalvarPressao()
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

    private void LoadPression()
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
        _barText = FindFirstObjectByType<TextMeshProUGUI>();
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

    public void changeFov()
    {
        Slider fovSlider = GameObject.FindGameObjectWithTag("Fov Menu").GetComponent<Slider>();
        _fov = fovSlider.value;
        if (_fov > 100f) 
        {
            _fov = 100f;
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

        

        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (player != null)
        {
            player.ChangeSense(sense);
        }
        else
        {
            Debug.Log("Player has not founded");
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
    #endregion
}
