using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public delegate void saveGame();
    
    public static event saveGame OnsavedGame;

    public static GameController instance;

    [SerializeField] private TextMeshProUGUI _barText;

    [SerializeField] private TextMeshProUGUI[] _tutorialText;

    [SerializeField] float barUnit;


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
        _barText.text = barUnit.ToString();
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
}
