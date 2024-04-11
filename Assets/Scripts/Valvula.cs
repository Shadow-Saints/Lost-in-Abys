using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Valvula : MonoBehaviour
{
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
        if (!_active && !multiply)
        {
            GameController.instance.SomarPresaoo(_pressionDiference);
            _active = true;
        }
        else if (_active && !multiply && _subtract) 
        {
            _active = false;
            GameController.instance.SomarPresaoo(-_pressionDiference);
        }else if (!_active && multiply)
        {
            GameController.instance.MultiplicarPresaoo(_pressionDiference);
            _active = true;
        }
        else if (_active && multiply && _subtract)
        {
            GameController.instance.DividirPresaoo(_pressionDiference);
            _active = false;
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
