using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadController : MonoBehaviour
{
    public Door door;
    public string password;
    public int passwordLimit;
    public Text passwordText;



    private void Start()
    {
        passwordText.text = "";
    }

    public void PasswordEntry(string number)
    {
        if (number == "Clear")
        {
            Clear();
            return;
        }
        else if(number == "Enter")
        {
            Enter();
            return;
        }

        int length = passwordText.text.ToString().Length;
        if(length<passwordLimit)
        {
            passwordText.text = passwordText.text + number;
        }
    }

    public void Clear()
    {
        passwordText.text = "";
        passwordText.color = Color.white;
    }

    private void Enter()
    {
        if (passwordText.text == password)
        {
            // Desbloqueia a porta e chama o método para abri-la
            door.lockedByPassword = false;
            door.OpenDoor();

            passwordText.color = Color.green;
            StartCoroutine(waitAndClear());
        }
        else
        {
            passwordText.color = Color.red;
            StartCoroutine(waitAndClear());
        }
    }


    IEnumerator waitAndClear()
    {
        yield return new WaitForSeconds(0.75f);
        Clear();
    }


}
