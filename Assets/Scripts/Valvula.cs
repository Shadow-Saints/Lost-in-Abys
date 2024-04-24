using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

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

    [SerializeField]private Animator _animator;

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
            _animator.SetBool("Abre", true);
            _animator.SetBool("Fecha", false);
            StartCoroutine(whait1second());
        }
        else
        {
            _active = false;
            _animator.SetBool("Fecha", true);
            _animator.SetBool("Abre", false);
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

    #region Anim

    private IEnumerator whait1second() 
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("Abre", false);
        _animator.SetBool("Fecha", false);
    }


    #endregion
}
