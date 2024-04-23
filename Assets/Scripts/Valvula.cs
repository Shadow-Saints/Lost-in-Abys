using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Valvula : MonoBehaviour
{
    #region Variables

    [Header("ID")]
    [SerializeField] private int valveID;

    [Header("Pression Atributes")]
    [SerializeField] private float _pressionDiference;

    [SerializeField] private bool _active;

    [SerializeField] private bool _subtract;

    [SerializeField] private bool multiply;

    [Header("Mouse")]
    private bool _mouseEnter;

    [Header("Save")]
    private Save save;

    [Header("Others")]
    [SerializeField] private Valvula[] _othersActive;
    [SerializeField] private Valvula[] _othersDisable;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        save = FindAnyObjectByType<Save>();
        if (save != null)
        {
           IDataReader read =  save.CheckValveValues(valveID);
            while (read.Read())
            {
                _active = read.GetBoolean(1);
                multiply = read.GetBoolean(2);
            }
            save.Close();
            //LoadGame();
            GameController.OnsavedGame += SaveValve;
        }
        else
        {
            Debug.LogWarning("Fudeu");
        }
    
    }


    private void OnMouseEnter()
    {
        _mouseEnter = true;
        GameController.instance.ActiveTutorial(0);
    }

    private void OnMouseExit()
    {
        _mouseEnter = false;
        GameController.instance.DisableTutorial(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _mouseEnter)
        {
            ChangeState();

        }
    }

    #endregion

    #region States

    private void ChangeState()
    {
        if (!_active)
        {
            _active = true;
        }
        else
        {
            _active = false;
        }
        CheckOttersActive();
    }

    public bool returnState()
    {
        return _active;
    }

    private void CheckOttersActive()
    {
        for (int i = 0; i < _othersActive.Length; i++)
        {
            if (!_othersActive[i].returnState())
            {
                Debug.Log("Inactive");
               return;
            }
        }
        if (_active)
        {
            GameController.instance.SomarPresaoo(_pressionDiference);
        }
        else
        {
            GameController.instance.SomarPresaoo(-_pressionDiference);
        }
        
    }

    private void CheckOttersDiabled()
    {
        for (int i = 0; i < _othersActive.Length; i++)
        {
            if (_othersActive[i].returnState())
            {
                Debug.Log("active");
                return;
            }
        }
        if (_active)
        {
            GameController.instance.SomarPresaoo(_pressionDiference);
        }
        else
        {
            GameController.instance.SomarPresaoo(-_pressionDiference);
        }
    }

    #endregion

    #region Save

    private void SaveValve() 
    {
        save.UpdateValveValues(valveID, _active, multiply); 
    }


    private void LoadGame()
    {
        if (_active && !multiply)
        {
            GameController.instance.SomarPresaoo(_pressionDiference);
        }
    }

    #endregion
}
