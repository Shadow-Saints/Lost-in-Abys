using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disjuntor : MonoBehaviour
{
    #region Variables
    [Header("States")]
    [SerializeField]private bool _isActive;
    [SerializeField]private Material _material;

    [Header("Time and Order")]
    private bool _isIniciated;
    [SerializeField]private disjutorPosition id;
    #endregion

    [SerializeField]private EnergyScreen _Energyscreen;

    [Header("Mouse")]
    private bool _mouseEnter;

    #region UnityCallbacks

    private void Start()
    {
        if(_Energyscreen == null)
        {
            Debug.LogWarning("Energy screen not founded !");
        }
        
        EnergyScreen.OnPuzzleReset += ResetThis;
    }

    private void OnMouseEnter()
    {
        _mouseEnter = true;
        GameController.instance.ActiveTutorial(1);
    }

    private void OnMouseExit()
    {
        _mouseEnter = false;
        GameController.instance.DisableTutorial(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _mouseEnter)
        {
            ChangeState();
        }
    }
    #endregion

    #region States

    public void ChangeState()
    {
        if (_isActive) // Muda o estado para inativo
        {
            _material.color = Color.red;
            _isActive = false;
        }else // Muda o estado para ativo
        {
            if(isMyTime(id))
            {
                _Energyscreen.Increment();
            }else
            {
                _Energyscreen.StopEnergyPuzzle();
                StartCoroutine(whait());
            }
            _material.color = Color.green;
            _isActive = true;
        }
    }

    #endregion  

    #region Time
    private bool isMyTime(disjutorPosition position)
    {
        if (_Energyscreen.checkDisjuntorPosition(position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ResetThis()
    {
        _material.color = Color.red;
        _isActive = false;
        Debug.Log("Reseted"+ id.ToString());
    }

    IEnumerator whait()
    {
        yield return new WaitForSeconds(0.3f);
        ResetThis();
    }

    #endregion
}
