using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public enum disjutorPosition
{
    first,
    second,
    third,
    fourth,
    fifth,
    sixth
}
public class EnergyScreen : MonoBehaviour
{
    #region Variables
    private bool _isIniciated;

    public delegate void EnergyPuzzle();

    public static event EnergyPuzzle OnEnergyPuzzle;

    public delegate void PuzzleReset();

    public static event PuzzleReset OnPuzzleReset;

    private float _timeToEnd;

    private disjutorPosition _position;

    private void Start()
    {
       OnEnergyPuzzle += puzzle;
        OnPuzzleReset += decrement;
        _position = disjutorPosition.first;
    }

    public bool returnPuzleState()
    {
        return _isIniciated;
    }

    public void StartEnergyPuzzle()
    {
        OnEnergyPuzzle();
    }

    public void StopEnergyPuzzle()
    {
        OnPuzzleReset();
        Debug.Log("Reset");
    }

    public bool checkDisjuntorPosition(disjutorPosition id)
    {
        if (id == _position)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator PuzzleTime()
    {
        _isIniciated = true;

        yield return new WaitForSeconds(_timeToEnd);

        _isIniciated = false;
        OnPuzzleReset(); // resetando o puzzle
    }

    private void puzzle()
    {
        StartCoroutine(PuzzleTime());
    }

    public void Increment()
    {
        if (_position == disjutorPosition.sixth)
        {
            Debug.Log("Sucess");
            return;
        }
        _position++;
        Debug.Log(_position.ToString());
    }

    private void decrement()
    {
        _position = disjutorPosition.first;
    }

    #endregion
}
